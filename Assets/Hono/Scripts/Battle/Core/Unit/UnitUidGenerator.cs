#region

using System;
using System.Collections.Generic;
using System.Threading;

#endregion

namespace Hono.Scripts.Battle.Core
{
    /// <summary>
    ///     ActorUid分段类型
    /// </summary>
    public enum EUnitUidRangeType
    {
        /// <summary>
        /// 低活跃段，但也不是经常存在，如怪物，建筑，集结点
        /// </summary>
        NormalActor,

        /// <summary>
        /// 极高活跃段（即经常创建，存活周期非常短的单位，经常销毁的Actor 如：子弹，短期召唤物，打击盒子等）
        /// </summary>
        DynamicActor,
    }

    /// <summary>
    ///     ActorUid生成器
    /// </summary>
    public static class UnitUidGenerator
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
                int newId = Interlocked.Increment(ref _idx);

                if (newId > _end)
                {
                    _idx = _start;
                    newId = Interlocked.Increment(ref _idx);
                }

                return newId;
            }

            public bool CheckInRange(int uid)
            {
                return uid > _start && uid < _end;
            }
        }


        private static readonly Dictionary<EUnitUidRangeType, UidRange> UidRanges = new(5)
        {
            { EUnitUidRangeType.DynamicActor, new UidRange(100000, 500000) },
            { EUnitUidRangeType.NormalActor, new UidRange(500000, Int32.MaxValue - 1) },
        };

        public static int GenerateUid(EUnitUidRangeType uidRangeType)
        {
            return UidRanges[uidRangeType].GetUid();
        }

        public static bool CheckSpecialUid(int spUid)
        {
            return !(UidRanges[EUnitUidRangeType.NormalActor].CheckInRange(spUid) ||
                     UidRanges[EUnitUidRangeType.DynamicActor].CheckInRange(spUid));
        }
    }
}