using System;
using System.Collections.Generic;
using System.Linq;
using Hono.Scripts.Battle.Core.Base;
using Hono.Scripts.Battle.ObjectPool;
using UnityEngine;

namespace Hono.Scripts.Battle.Core
{
    [JsonUnitCompCtorParams(typeof(BuffComp))]
    public class BuffCompCtorParams : UnitCompCtorParams
    {
        public List<int> BuffList = new();
    }

    /// <summary>
    /// buff容器，管理单位身上的buff
    /// </summary>
    public class BuffComp : UnitComponent, IGPoolObject
    {
        /// <summary>
        /// Uid分配器 全Buff共享
        /// </summary>
        private static readonly IdAllocator UidAllocator = new();
        /// <summary>
        /// 当前buff容量
        /// </summary>
        private Buff[] _buffs = new Buff[30]; // 存储所有 Buff 的数组
        /// <summary>
        /// buff计数
        /// </summary>
        private int _buffCount = 0; // 当前有效 Buff 的数量
        /// <summary>
        /// 快速索引buff uid->index
        /// </summary>
        private readonly Dictionary<int, int> _lookup = new(15);
        /// <summary>
        /// 阻断tag容器
        /// </summary>
        private readonly NameList<int> _blockTags = new(30);
        /// <summary>
        /// 阻挡buffId
        /// </summary>
        private readonly NameList<int> _blockIds = new(10);

        public override void Init() { }

        public override void Ctor(UnitCompCtorParams ctorParams)
        {
            var buffCompCtorParams = (BuffCompCtorParams)ctorParams;
            foreach (var buffId in buffCompCtorParams.BuffList)
            {
                AddBuff(Unit.Uid, buffId, 1);
            }
        }

        protected override void onTick(float dt)
        {
            for (int i = 0; i < _buffCount; i++)
            {
                ref Buff buff = ref _buffs[i];
                if (!buff.IsValid) continue; // 跳过无效 Buff

                buff.TimeLeft -= dt;

                if (buff.IsPermanent)
                    continue;
                if (buff.TimeLeft < 0)
                {
                    RemoveBuffAt(i); // 移除超时的 Buff
                    i--;             // 调整索引以避免跳过下一个元素
                }
            }
        }

        public override void Recycle()
        {
            GPool<BuffComp>.Pool.Recycle(this);
        }

        public void OnRecycle()
        {
            Array.Clear(_buffs, 0, _buffs.Length);
            _lookup.Clear();
            _blockIds.Clear();
            _blockTags.Clear();
        }

        /// <summary>
        /// 添加buff 返回buffUid
        /// 叠层和添加失败都会返回Int32.Min
        /// </summary>
        /// <param name="sourceId">buff来源，给别的对象添加buff的对象</param>
        /// <param name="buffId"></param>
        /// <param name="buffLayer">初始层数</param>
        public int AddBuff(int sourceId, int buffId, int buffLayer)
        {
            if (!AssetManager.Instance.TryGetData<BuffData>(buffId, out var buffData))
            {
                Debug.LogError($"找不到指定Buff:{buffId}数据");
                return Int32.MinValue;
            }

            //阻断判断
            switch (buffData.blockRule)
            {
                case EBuffAddBlockRule.BlockByTags:
                    if (buffData.buffTags.Any(tag => checkInBlockList(EBuffAddBlockRule.BlockByTags, tag)))
                    {
                        return Int32.MinValue;
                    }

                    break;
                case EBuffAddBlockRule.BlockById:
                    if (checkInBlockList(EBuffAddBlockRule.BlockById, buffId))
                    {
                        return Int32.MinValue;
                    }
                    break;
            }

            //先检查buff是否存在，如果不存在无论如何都会添加
            if (GetBuffCount(buffId) == 0)
            {
                return addBuff(sourceId, buffLayer, buffData);
            }

            switch (buffData.addRule)
            {
                case EBuffAddRule.SameSourceOverride:
                {
                    if (tryGetBuffIndex(buffId, sourceId, out int buffIndex))
                    {
                        return overrideBuff(sourceId, buffLayer, buffData, buffIndex);
                    }

                    break;
                }
                case EBuffAddRule.SameSourceLayering:
                {
                    if (tryGetBuffIndex(buffId, sourceId, out int buffIndex))
                    {
                        return layeringBuff(buffLayer, buffData, buffIndex);
                    }

                    break;
                }
                case EBuffAddRule.AllOverride:
                {
                    if (tryGetBuffIndex(buffId, -1, out int buffIndex))
                    {
                        return overrideBuff(sourceId, buffLayer, buffData, buffIndex);
                    }

                    break;
                }
                case EBuffAddRule.AllLayering:
                {
                    if (tryGetBuffIndex(buffId, -1, out int buffIndex))
                    {
                        return layeringBuff(buffLayer, buffData, buffIndex);
                    }

                    break;
                }
            }

            return Int32.MinValue;
        }

        /// <summary>
        /// 检测是否被阻断
        /// </summary>
        /// <param name="blockRule"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        private bool checkInBlockList(EBuffAddBlockRule blockRule, int id)
        {
            NameList<int> blockList = null;
            switch (blockRule)
            {
                case EBuffAddBlockRule.BlockByTags:
                    blockList = _blockTags;
                    break;
                case EBuffAddBlockRule.BlockById:
                    blockList = _blockIds;
                    break;
            }

            if (blockList == null)
            {
                Debug.LogError("buff {id} 错误的屏蔽类型");
                return false;
            }

            if (blockList.Contains(id))
            {
                Debug.Log($"buff {id} 添加 被阻断了");
                return false;
            }

            return true;
        }

        /// <summary>
        /// 添加一个新的 Buff 或者覆盖 某个buff
        /// </summary>
        private int addBuff(int sourceId, int buffLayer, BuffData buffData)
        {
            //组成对象
            var buff = new Buff
            {
                Uid = UidAllocator.Allocate(),
                Id = buffData.id,
                LayerCount = (short)buffLayer,
                SourceUnitUid = sourceId,
                BelongActorUid = Unit.Uid,
                TimeLeft = buffData.duration,
                IsPermanent = buffData.duration < 0
            };

            if (_buffCount >= _buffs.Length)
            {
                ResizeBuffs(); // 动态扩容
            }

            // 将新 Buff 添加到数组中
            _buffs[_buffCount] = buff;

            // 更新字典
            _lookup[buff.Uid] = _buffCount;
            _buffCount++;

            //添加阻断tag
            foreach (var tag in buffData.buffTags)
            {
                _blockTags.Add(tag);
            }

            //添加阻断tag
            foreach (var tag in buffData.blockBuffIds)
            {
                _blockIds.Add(tag);
            }

            Unit.AddAbility(buffData.BuffAbility);
            Unit.ExecuteAbility(buffData.id);

            doAddBehave(buffData);

            return buff.Uid;
        }

        /// <summary>
        /// 覆盖buff
        /// </summary>
        /// <param name="sourceId"></param>
        /// <param name="buffLayer"></param>
        /// <param name="buffData"></param>
        /// <param name="index"></param>
        /// <returns></returns>
        private int overrideBuff(int sourceId, int buffLayer, BuffData buffData, int index)
        {
            var overrideBuff = new Buff
            {
                Uid = UidAllocator.Allocate(),
                Id = buffData.id,
                LayerCount = buffLayer,
                SourceUnitUid = sourceId,
                BelongActorUid = Unit.Uid,
                TimeLeft = buffData.duration,
                IsPermanent = buffData.duration < 0
            };

            _buffs[index] = overrideBuff;
            Unit.ExecuteAbility(buffData.id);

            return overrideBuff.Uid;
        }

        /// <summary>
        /// buff叠层
        /// </summary>
        /// <param name="buffLayer"></param>
        /// <param name="buffData"></param>
        /// <param name="index"></param>
        /// <returns></returns>
        private int layeringBuff(int buffLayer, BuffData buffData, int index)
        {
            ref Buff buff = ref _buffs[index];
            buff.LayerCount = Mathf.Min(_buffs[index].LayerCount + buffLayer, buffData.maxLayerNumber);

            if (buffData.lifeAddWhenLayering)
            {
                buff.TimeLeft += buffData.duration;
            }

            if (buffData.runAgainWhenLayering)
            {
                Unit.ExecuteAbility(buffData.id);
            }

            return Int32.MinValue;
        }

        /// <summary>
        /// 尝试获取指定的buff
        /// </summary>
        /// <param name="buffUid"></param>
        /// <param name="buff"></param>
        /// <returns></returns>
        private bool tryGetBuffByUid(int buffUid, out Buff buff)
        {
            buff = default;

            if (!_lookup.TryGetValue(buffUid, out var index))
                return false;

            buff = ref _buffs[index];

            return false;
        }

        /// <summary>
        /// 尝试获取指定buff的index
        /// </summary>
        /// <param name="buffId"></param>
        /// <param name="sourceUnitUid"></param>
        /// <param name="index"></param>
        /// <returns></returns>
        private bool tryGetBuffIndex(int buffId, int sourceUnitUid, out int index)
        {
            for (index = 0; index < _buffs.Length; index++)
            {
                ref Buff buff = ref _buffs[index];

                if (buff.Id != buffId || !buff.IsValid)
                    continue;

                if (sourceUnitUid == -1)
                {
                    return true;
                }

                if (buff.SourceUnitUid == sourceUnitUid)
                {
                    return true;
                }

                break;
            }

            return false;
        }

        /// <summary>
        /// 尝试获取指定buff
        /// </summary>
        /// <param name="sourceUnitUid"></param>
        /// <param name="buffId"></param>
        /// <param name="buff"></param>
        /// <returns></returns>
        private bool tryGetBuff(int buffId, int sourceUnitUid, out Buff buff)
        {
            buff = default;

            for (int index = 0; index < _buffs.Length; index++)
            {
                buff = ref _buffs[index];

                if (buff.Id != buffId || !buff.IsValid)
                    continue;

                if (sourceUnitUid == -1)
                {
                    return true;
                }

                if (buff.SourceUnitUid == sourceUnitUid)
                {
                    return true;
                }

                break;
            }

            return false;
        }

        public bool HasBuff(int buffId, int sourceUnitUid)
        {
            for (int index = 0; index < _buffs.Length; index++)
            {
                ref Buff buff = ref _buffs[index];

                if (buff.Id != buffId || !buff.IsValid)
                    continue;

                if (sourceUnitUid == -1)
                {
                    return true;
                }

                if (buff.SourceUnitUid == sourceUnitUid)
                {
                    return true;
                }

                break;
            }

            return false;
        }

        /// <summary>
        /// 获取指定Buff的数量
        /// </summary>
        public int GetBuffCount(int buffId)
        {
            int buffCount = 0;
            if (_buffs.Length == 0)
                return buffCount;

            for (var index = 0; index < _buffs.Length; index++)
            {
                ref Buff buff = ref _buffs[index];
                if (buff.IsValid && buff.Id == buffId)
                {
                    ++buffCount;
                }
            }

            return buffCount;
        }

        /// <summary>
        /// 添加成功后的行为
        /// </summary>
        private void doAddBehave(BuffData buffData)
        {
            switch (buffData.addSuccessBehave)
            {
                case EBuffAddSuccessBehave.RemoveBuffsByTag:
                    int tagRemoveCount = 0;
                    foreach (var removeTag in buffData.removeBuffByTags)
                    {
                        for (int index = 0; index < _buffs.Length; index++)
                        {
                            ref Buff buff = ref _buffs[index];

                            var itemBuffData = AssetManager.Instance.GetData<BuffData>(buff.Id);

                            if (!itemBuffData.buffTags.Contains(removeTag))
                                continue;
                            if (buffData.tagRemoveCount != -1 && buffData.tagRemoveCount <= tagRemoveCount)
                                continue;
                            RemoveBuffAt(index);
                            ++tagRemoveCount;
                        }
                    }

                    break;
                case EBuffAddSuccessBehave.RemoveBuffsById:
                    int idRemoveCount = 0;
                    foreach (var removeId in buffData.removeBuffByIds)
                    {
                        for (int index = 0; index < _buffs.Length; index++)
                        {
                            ref Buff buff = ref _buffs[index];
                            if (buff.Id != removeId)
                                continue;
                            if (buffData.idRemoveCount != -1 && buffData.idRemoveCount <= idRemoveCount)
                                continue;
                            RemoveBuffAt(index);
                        }
                    }

                    break;
            }
        }

        /// <summary>
        /// 获取指定 Buff 的层数
        /// </summary>
        public int GetBuffLayer(int buffId, int sourceUnitUid)
        {
            return tryGetBuff(buffId, sourceUnitUid, out Buff buff) ? buff.LayerCount : 0;
        }

        /// <summary>
        /// 移除指定的 Buff 来源为-1说明不在意来源
        /// </summary>
        public void RemoveBuff(int buffId, int sourceUnitId = -1)
        {
            if (_lookup.Count == 0) return;

            for (int index = 0; index < _buffs.Length; index++)
            {
                ref Buff buff = ref _buffs[index];

                if (buff.Id != buffId || !buff.IsValid)
                    continue;

                if (sourceUnitId == -1)
                {
                    RemoveBuffAt(index);
                }
                else if (buff.SourceUnitUid == sourceUnitId)
                {
                    RemoveBuffAt(index);
                }

                break;
            }
        }

        /// <summary>
        /// 移除指定索引处的 Buff
        /// </summary>
        private void RemoveBuffAt(int index)
        {
            ref Buff buff = ref _buffs[index];
            buff.IsValid = false; // 标记为无效

            // 将最后一个有效 Buff 移动到当前位置
            if (index != _buffCount - 1)
            {
                // 更新字典中的索引
                _lookup[_buffs[_buffCount - 1].Uid] = index;

                _buffs[index] = _buffs[_buffCount - 1];
            }

            _lookup.Remove(buff.Uid);
            _buffCount--;
            Unit.StopAbility(buff.Id);
            UidAllocator.Recycle(buff.Uid);
        }

        /// <summary>
        /// 动态扩容数组
        /// </summary>
        private void ResizeBuffs()
        {
            int newSize = _buffs.Length + 10; // 每次扩容 10
            Array.Resize(ref _buffs, newSize);
            Debug.Log($"Buff 数组已扩容至 {newSize} 个元素");
        }
    }
}