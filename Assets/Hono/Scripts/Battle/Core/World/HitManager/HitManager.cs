using System;
using System.Collections.Generic;
using Hono.Scripts.Battle.Tools;

namespace Hono.Scripts.Battle.Core
{
    /// <summary>
    /// 瞬时打击和脱手打击
    /// </summary>
    public class HitManager : Singleton<HitManager>, IWorldSystemWhenExitCalled, IWorldSystemWhenTickCalled
    {
        private struct OffHandInfo
        {
            public AttrCollection.AttrSnapshots AttrSnapshots { get; }
            public HitBoxData HitBoxData { get; }
            public int CurrentHitCount;
            public float WaitTime;
            
            public OffHandInfo(int currentHitCount, float waitTime, AttrCollection.AttrSnapshots attrSnapshots, HitBoxData hitBoxData)
            {
                CurrentHitCount = currentHitCount;
                WaitTime = waitTime;
                AttrSnapshots = attrSnapshots;
                HitBoxData = hitBoxData;
            }
        }

        private readonly List<OffHandInfo> _offHandHitInfos = new(100);

            
        /// <summary>
        /// 
        /// </summary>
        public void CreateHit(HitBoxData hitBoxData, Unit attacker, Unit target)
        {
            if (hitBoxData.isOffHand)
            {
                var info = new OffHandInfo(0, 0, attacker.Attrs.GetAttrSnapShots(null), hitBoxData);
                _offHandHitInfos.Add(info);
            }
            else
            {
                
            }
        }

        private void targetHit()
        {
            
        }

        private void aoeHit()
        {
            
        }
        
        public void OnWorldTick(float dt)
        {
            //处理脱手打击
            for (var index = 0; index < _offHandHitInfos.Count; index++)
            {
                 _offHandHitInfos[index];
            }
        }

        public void OnWorldExit()
        {
            //清理脱手打击
        }

        
    }
}