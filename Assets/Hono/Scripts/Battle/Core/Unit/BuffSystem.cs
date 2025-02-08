using System;
using System.Collections.Generic;
using Hono.Scripts.Battle.Core.Base;
using UnityEngine;

namespace Hono.Scripts.Battle.Core
{
    public struct Buff
    {
        public bool IsValid;       // 标记当前 Buff 是否有效
        public short LayerCount;   // 当前 Buff 层数
        public int SourceActorUid; // Buff 来源角色 ID
        public int BelongActorUid; // Buff 所属角色 ID
        public float Duration;     // 持续时间
        public BuffData BuffData;  // Buff 数据

        public static bool operator ==(Buff a, Buff b)
        {
            return a.BuffData.id == b.BuffData.id && a.SourceActorUid == b.SourceActorUid && a.BelongActorUid == b.BelongActorUid;
        }

        public static bool operator !=(Buff a, Buff b)
        {
            return !(a == b);
        }
    }

    public class BuffSystem : Singleton<BuffSystem>, IWorldSystemWhenEnterCalled, IWorldSystemWhenTickCalled, IWorldSystemWhenExitCalled
    {
        private Buff[] _buffs;                      // 存储所有 Buff 的数组
        private int _buffCount = 0;                 // 当前有效 Buff 的数量
        private Dictionary<int, List<int>> _lookup; // 辅助字典，用于快速查找 Buff 索引

        public void OnWorldEnter(World world)
        {
            _buffs = new Buff[2048];                    // 初始化数组
            _lookup = new Dictionary<int, List<int>>(); // 初始化字典
        }

        public void OnWorldExit()
        {
            _buffs = Array.Empty<Buff>();
            _lookup.Clear();
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

                buff.Duration += dt;
                if (buff.Duration > buff.BuffData.MaxDuration)
                {
                    RemoveBuffAt(i); // 移除超时的 Buff
                    i--;             // 调整索引以避免跳过下一个元素
                }
            }
        }

        public void AddBuff(int sourceId, int targetId, int buffId,int buffLayer)
        {
            //组成对象
        }

        /// <summary>
        /// 添加一个新的 Buff
        /// </summary>
        public void AddBuff(ref Buff buff)
        {
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

            _buffCount++;
        }

        /// <summary>
        /// 获取指定 Buff 的层数
        /// </summary>
        public int GetBuffLayer(int belongActorUid, int buffId)
        {
            if (!_lookup.TryGetValue(belongActorUid, out var indices)) return 0;

            foreach (var index in indices)
            {
                ref Buff buff = ref _buffs[index];
                if (buff.BuffData.id == buffId && buff.IsValid)
                {
                    return buff.LayerCount;
                }
            }

            return 0;
        }
        
        /// <summary>
        /// 获取指定 Buff 来源
        /// </summary>
        public int GetBuffSource(int belongActorUid, int buffId)
        {
            if (!_lookup.TryGetValue(belongActorUid, out var indices)) return 0;

            foreach (var index in indices)
            {
                ref Buff buff = ref _buffs[index];
                if (buff.BuffData.id == buffId && buff.IsValid)
                {
                    return buff.SourceActorUid;
                }
            }

            return -1;
        }

        /// <summary>
        /// 移除指定的 Buff
        /// </summary>
        public void RemoveBuff(int belongActorUid, int buffId)
        {
            if (!_lookup.TryGetValue(belongActorUid, out var indices)) return;

            for (int i = 0; i < indices.Count; i++)
            {
                int index = indices[i];
                ref Buff buff = ref _buffs[index];
                if (buff.BuffData.id == buffId && buff.IsValid)
                {
                    RemoveBuffAt(index); // 移除 Buff
                    indices.RemoveAt(i); // 从字典中移除索引
                    break;
                }
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
            }

            _buffCount--;
        }

        /// <summary>
        /// 动态扩容数组
        /// </summary>
        private void ResizeBuffs()
        {
            int newSize = _buffs.Length + 4096; // 每次扩容 4096
            Array.Resize(ref _buffs, newSize);
            Debug.Log($"Buff 数组已扩容至 {newSize} 个元素");
        }
    }
}