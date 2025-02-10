using System;
using System.Collections.Generic;
using System.Linq;
using Hono.Scripts.Battle.Base;
using Hono.Scripts.Battle.Core.Base;
using UnityEngine;
using UnityEngine.Serialization;

namespace Hono.Scripts.Battle.Core
{
    public class BuffSystem : Singleton<BuffSystem>, IWorldSystemWhenEnterCalled, IWorldSystemWhenTickCalled,
        IWorldSystemWhenExitCalled
    {
        /// <summary>
        /// Uid分配器
        /// </summary>
        private IdAllocator _idAllocator = new();
        /// <summary>
        /// 当前buff容量
        /// </summary>
        private Buff[] _buffs; // 存储所有 Buff 的数组
        /// <summary>
        /// buff计数
        /// </summary>
        private int _buffCount = 0; // 当前有效 Buff 的数量
        /// <summary>
        /// 快速索引Unit拥有者
        /// </summary>
        private Dictionary<int, List<int>> _lookup; // 辅助字典，用于快速查找 Buff 索引 uid -> buffId -> index
        /// <summary>
        /// 快速索引buff uid
        /// </summary>
        private Dictionary<int, int> _buffUidSearch;
        /// <summary>
        /// 阻断tag容器
        /// </summary>
        private readonly Dictionary<int, NameList<int>> _blockTags = new(1024);
        /// <summary>
        /// 阻挡buffId
        /// </summary>
        private readonly Dictionary<int, NameList<int>> _blockIds = new(100);

        public void OnWorldEnter(World world)
        {
            _buffs = new Buff[2048];                       // 初始化数组
            _lookup = new Dictionary<int, List<int>>(100); // 初始化字典
        }

        public void OnWorldExit()
        {
            _buffs = Array.Empty<Buff>();
            _lookup.Clear();
            _blockIds.Clear();
            _blockTags.Clear();
        }

        /// <summary>
        /// 更新所有 Buff 的持续时间，并清理无效 Buff
        /// </summary>
        public void OnWorldTick(float dt)
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

        /// <summary>
        /// 添加buff 返回buffUid
        /// 叠层和添加失败都会返回Int32.Min
        /// </summary>
        /// <param name="sourceId">buff来源，给别的对象添加buff的对象</param>
        /// <param name="targetId">被添加buff的目标，被添加buff的对象</param>
        /// <param name="buffId"></param>
        /// <param name="buffLayer">初始层数</param>
        public int AddBuff(int sourceId, int targetId, int buffId, int buffLayer)
        {
            if (!AssetManager.Instance.TryGetData<BuffData>(buffId, out var buffData))
            {
                Debug.LogError($"找不到指定Buff:{buffId}数据");
                return Int32.MinValue;
            }

            if (!World.Searcher.TryGetUnit(targetId, out var target))
            {
                Debug.LogError($"添加Buff:{buffId}的目标不存在");
                return Int32.MinValue;
            }

            //阻断判断
            switch (buffData.blockRule)
            {
                case EBuffAddBlockRule.BlockByTags:
                    if (buffData.buffTags.Any(tag => checkInBlockList(targetId, EBuffAddBlockRule.BlockByTags, tag)))
                    {
                        return Int32.MinValue;
                    }

                    break;
                case EBuffAddBlockRule.BlockById:
                    if (checkInBlockList(targetId, EBuffAddBlockRule.BlockById, buffId))
                    {
                        return Int32.MinValue;
                    }

                    break;
            }

            //先检查buff是否存在，如果不存在无论如何都会添加
            if (GetBuffCount(targetId, buffId) == 0)
            {
                return addBuff(sourceId, target, buffLayer, buffData);
            }

            switch (buffData.addRule)
            {
                case EBuffAddRule.SameSourceOverride:
                {
                    if (tryGetBuff(targetId, sourceId, buffId, out int buffIndex))
                    {
                        return overrideBuff(sourceId, target, buffLayer, buffData, buffIndex);
                    }

                    break;
                }
                case EBuffAddRule.SameSourceLayering:
                {
                    if (tryGetBuff(targetId, sourceId, buffId, out int buffIndex))
                    {
                        return layeringBuff(targetId, buffLayer, buffData, buffIndex);
                    }

                    break;
                }
                case EBuffAddRule.AllOverride:
                {
                    if (tryGetBuff(targetId, buffId, out int buffIndex))
                    {
                        return overrideBuff(sourceId, target, buffLayer, buffData, buffIndex);
                    }

                    break;
                }
                case EBuffAddRule.AllLayering:
                {
                    if (tryGetBuff(targetId, buffId, out int buffIndex))
                    {
                        return layeringBuff(targetId, buffLayer, buffData, buffIndex);
                    }

                    break;
                }
            }

            return Int32.MinValue;
        }

        /// <summary>
        /// 检测是否被阻断
        /// </summary>
        /// <param name="uid"></param>
        /// <param name="blockRule"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        private bool checkInBlockList(int uid, EBuffAddBlockRule blockRule, int id)
        {
            NameList<int> blockList = null;
            switch (blockRule)
            {
                case EBuffAddBlockRule.BlockByTags:
                    if (!_blockTags.TryGetValue(uid, out blockList))
                    {
                        return true;
                    }

                    break;
                case EBuffAddBlockRule.BlockById:
                    if (_blockTags.TryGetValue(uid, out blockList))
                    {
                        return true;
                    }

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
        private int addBuff(int sourceId, Unit target, int buffLayer, BuffData buffData)
        {
            //组成对象
            var buff = new Buff
            {
                Uid = _idAllocator.Allocate(),
                Id = buffData.id,
                LayerCount = (short)buffLayer,
                SourceUnitUid = sourceId,
                BelongActorUid = target.Uid,
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
            if (!_lookup.ContainsKey(buff.BelongActorUid))
            {
                _lookup[buff.BelongActorUid] = new List<int>();
            }

            _lookup[buff.BelongActorUid].Add(_buffCount);
            _buffUidSearch[buff.Uid] = _buffCount;
            _buffCount++;

            if (!_blockTags.TryGetValue(target.Uid, out var tagsBlock))
            {
                tagsBlock = new NameList<int>();
                _blockTags.Add(target.Uid, tagsBlock);
            }

            //添加阻断tag
            foreach (var tag in buffData.buffTags)
            {
                tagsBlock.Add(tag);
            }

            if (!_blockIds.TryGetValue(target.Uid, out var idsBlock))
            {
                idsBlock = new NameList<int>();
                _blockIds.Add(target.Uid, idsBlock);
            }

            //添加阻断tag
            foreach (var tag in buffData.blockBuffIds)
            {
                idsBlock.Add(tag);
            }

            target.ExecuteAbility(buff.Id);

            return buff.Uid;
        }

        /// <summary>
        /// 覆盖buff
        /// </summary>
        /// <param name="sourceId"></param>
        /// <param name="target"></param>
        /// <param name="buffLayer"></param>
        /// <param name="buffData"></param>
        /// <param name="index"></param>
        /// <returns></returns>
        private int overrideBuff(int sourceId, Unit target, int buffLayer, BuffData buffData, int index)
        {
            var overrideBuff = new Buff
            {
                Uid = _idAllocator.Allocate(),
                Id = buffData.id,
                LayerCount = (short)buffLayer,
                SourceUnitUid = sourceId,
                BelongActorUid = target.Uid,
                TimeLeft = buffData.duration,
                IsPermanent = buffData.duration < 0
            };
            //停止之前的buff
            target.StopAbility(overrideBuff.Id);
            //执行新的buff
            _buffs[index] = overrideBuff;
            target.ExecuteAbility(overrideBuff.Id);

            return overrideBuff.Uid;
        }

        /// <summary>
        /// buff叠层
        /// </summary>
        /// <param name="targetUid"></param>
        /// <param name="buffLayer"></param>
        /// <param name="buffData"></param>
        /// <param name="index"></param>
        /// <returns></returns>
        private int layeringBuff(int targetUid, int buffLayer, BuffData buffData, int index)
        {
            ref Buff buff = ref _buffs[index];
            buff.LayerCount = Mathf.Min(_buffs[index].LayerCount + buffLayer, buffData.maxLayerNumber);

            if (buffData.lifeAddWhenLayering)
            {
                buff.TimeLeft += buffData.duration;
            }

            if (buffData.runAgainWhenLayering)
            {
                if (World.Searcher.TryGetUnit(targetUid, out var unit))
                {
                    unit.ExecuteAbility(buff.Id);
                }
            }

            return Int32.MinValue;
        }

        /// <summary>
        /// 尝试获取指定buff
        /// </summary>
        /// <param name="targetUnitUid"></param>
        /// <param name="buffId"></param>
        /// <param name="buff"></param>
        /// <returns></returns>
        private bool tryGetBuff(int targetUnitUid, int buffId, out Buff buff)
        {
            buff = default;

            if (!_lookup.TryGetValue(targetUnitUid, out var indices))
                return false;

            foreach (var index in indices)
            {
                buff = ref _buffs[index];
                if (buff.IsValid && buff.Id == buffId)
                {
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// 尝试获取指定buff
        /// </summary>
        /// <param name="targetUnitUid"></param>
        /// <param name="buffId"></param>
        /// <param name="buffIndex"></param>
        /// <returns></returns>
        private bool tryGetBuff(int targetUnitUid, int buffId, out int buffIndex)
        {
            buffIndex = 0;

            if (!_lookup.TryGetValue(targetUnitUid, out var indices))
                return false;

            foreach (var index in indices)
            {
                ref Buff buff = ref _buffs[index];
                if (buff.IsValid && buff.Id == buffId)
                {
                    buffIndex = index;
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// 尝试获取指定buff
        /// </summary>
        /// <param name="targetUnitUid"></param>
        /// <param name="sourceUnitUid"></param>
        /// <param name="buffId"></param>
        /// <param name="buff"></param>
        /// <returns></returns>
        private bool tryGetBuff(int targetUnitUid, int sourceUnitUid, int buffId, out Buff buff)
        {
            buff = default;

            if (!_lookup.TryGetValue(targetUnitUid, out var indices))
                return false;

            foreach (var index in indices)
            {
                buff = ref _buffs[index];
                if (buff.IsValid && buff.Id == buffId && buff.SourceUnitUid == sourceUnitUid)
                {
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// 尝试获取指定buff
        /// </summary>
        /// <param name="targetUnitUid"></param>
        /// <param name="sourceUnitUid"></param>
        /// <param name="buffId"></param>
        /// <param name="buffIndex"></param>
        /// <returns></returns>
        private bool tryGetBuff(int targetUnitUid, int sourceUnitUid, int buffId, out int buffIndex)
        {
            buffIndex = -1;

            if (!_lookup.TryGetValue(targetUnitUid, out var indices))
                return false;

            foreach (var index in indices)
            {
                ref Buff buff = ref _buffs[index];
                if (buff.IsValid && buff.Id == buffId && buff.SourceUnitUid == sourceUnitUid)
                {
                    buffIndex = index;
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// 获取指定Buff的数量
        /// </summary>
        public int GetBuffCount(int belongUnitUid, int buffId)
        {
            int buffCount = 0;
            if (!_lookup.TryGetValue(belongUnitUid, out var indices))
                return 0;

            foreach (var index in indices)
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
        private void doAddBehave(Unit target, BuffData buffData)
        {
            switch (buffData.addSuccessBehave)
            {
                case EBuffAddSuccessBehave.RemoveBuffsByTag:
                    int tagRemoveCount = 0;
                    foreach (var removeTag in buffData.removeBuffByTags)
                    {
                        foreach (var index in _lookup[target.Uid])
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
                        foreach (var index in _lookup[target.Uid])
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
        public int GetBuffLayer(int belongUnitUid, int sourceUnitUid, int buffId)
        {
            return tryGetBuff(belongUnitUid, sourceUnitUid, buffId, out Buff buff) ? buff.LayerCount : 0;
        }

        /// <summary>
        /// 移除指定的 Buff
        /// </summary>
        public void RemoveBuff(int belongUnitUid, int sourceUnitId, int buffId)
        {
            if (!_lookup.TryGetValue(belongUnitUid, out var indices)) return;

            for (int i = 0; i < indices.Count; i++)
            {
                int index = indices[i];
                ref Buff buff = ref _buffs[index];
                if (buff.Id == buffId && buff.SourceUnitUid == sourceUnitId && buff.IsValid)
                {
                    RemoveBuffAt(index); // 移除 Buff
                    indices.RemoveAt(i); // 从字典中移除索引
                    break;
                }
            }
        }

        /// <summary>
        /// 移除指定的 Buff
        /// </summary>
        public void RemoveBuff(int buffUid)
        {
            if (!_buffUidSearch.TryGetValue(buffUid, out var index))
                return;
            RemoveBuffAt(index);
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
                _buffs[index] = _buffs[_buffCount - 1];

                // 更新字典中的索引
                if (_lookup.TryGetValue(_buffs[index].BelongActorUid, out var indices))
                {
                    for (int i = 0; i < indices.Count; i++)
                    {
                        if (indices[i] == _buffCount - 1)
                        {
                            indices[i] = index;
                            break;
                        }
                    }
                }

                // 更新字典中的索引
                _buffUidSearch[_buffs[index].Uid] = index;
            }

            _buffUidSearch.Remove(buff.Uid);
            _idAllocator.Recycle(buff.Uid);
            _buffCount--;
        }

        /// <summary>
        /// 动态扩容数组
        /// </summary>
        private void ResizeBuffs()
        {
            int newSize = _buffs.Length + 512; // 每次扩容 512
            Array.Resize(ref _buffs, newSize);
            Debug.Log($"Buff 数组已扩容至 {newSize} 个元素");
        }
    }
}