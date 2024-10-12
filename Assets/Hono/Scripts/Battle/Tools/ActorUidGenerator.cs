using System;
using System.Collections.Generic;

namespace Hono.Scripts.Battle.Tools
{
    /// <summary>
    /// ActorUid分段类型
    /// </summary>
    public enum EActorUidRangeType
    {
        /// <summary>
        /// 生成频率低于活跃段，但也不是经常存在，如怪物，建筑，集结点
        /// </summary>
        NormalActor,
        
        /// <summary>
        /// 玩家角色段，存在时间很长，整个战斗周期基本一直存在
        /// </summary>
        PawnActor,
        
        /// <summary>
        /// 静态段（地图上的静态Actor,Uid在地图Scene中生成）
        /// </summary>
        StaticActor,
        
        /// <summary>
        /// 特殊Actor，此段Uid与表中Id相同，比如Boss，特殊NPC
        /// </summary>
        SpecialActor,
        
        /// <summary>
        /// 活跃段（即经常创建，经常销毁的Actor 如：子弹，短期召唤物，打击盒子等）
        /// </summary>
        DynamicActor,
    }
    
    /// <summary>
    /// ActorUid生成器
    /// </summary>
    public static class ActorUidGenerator
    { 
        ///UidRange(start,end)
        private class UidRange
        {
            private readonly int _start;
            private readonly int _end;
            private int _idx;
            public UidRange(int start,int end)
            {
                _start = start;
                _end = end;
                _idx = _start;
            }

            public virtual int GetUid()
            {
                ++_idx;

                if (_idx >= _end)
                {
                    _idx = _start + 1;
                }

                return _idx;
            }
        }
        
        private class SpecialUidRange : UidRange
        {
            public SpecialUidRange(int start, int end) : base(start, end)
            {
                
            }
            
            
        }
        
        
        private static readonly Dictionary<EActorUidRangeType, UidRange> UidRanges = new(5)
        {
            { EActorUidRangeType.SpecialActor ,new UidRange(0,10000)},
            { EActorUidRangeType.PawnActor ,new UidRange(20000,30000)},
            { EActorUidRangeType.StaticActor ,new UidRange(40000,60000)},
            { EActorUidRangeType.NormalActor ,new UidRange(200000,400000)},
            { EActorUidRangeType.DynamicActor ,new UidRange(500000,Int32.MaxValue-1)},
        };
        
        public static int GenerateId(EActorUidRangeType uidRangeType)
        {
            return UidRanges[uidRangeType].GetUid();
        }
    }
}