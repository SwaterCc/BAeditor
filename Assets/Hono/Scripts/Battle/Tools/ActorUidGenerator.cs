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
        /// 低活跃段，但也不是经常存在，如怪物，建筑，集结点
        /// </summary>
        NormalActor,

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

            public UidRange(int start, int end)
            {
                _start = start;
                _end = end;
                _idx = _start;
            }

            public int GetUid()
            {
                ++_idx;

                if (_idx >= _end)
                {
                    _idx = _start + 1;
                }

                return _idx;
            }

            public bool CheckInRange(int uid)
            {
                return uid > _start && uid < _end;
            }
        }


        private static readonly Dictionary<EActorUidRangeType, UidRange> UidRanges = new(5)
        {
            { EActorUidRangeType.NormalActor, new UidRange(100000, 500000) },
            { EActorUidRangeType.DynamicActor, new UidRange(500000, Int32.MaxValue - 1) },
        };

        public static int GenerateUid(EActorUidRangeType uidRangeType)
        {
            return UidRanges[uidRangeType].GetUid();
        }

        public static bool CheckSpecialUid(int spUid)
        {
            return !(UidRanges[EActorUidRangeType.NormalActor].CheckInRange(spUid) ||
                     UidRanges[EActorUidRangeType.DynamicActor].CheckInRange(spUid));
        }
    }
}