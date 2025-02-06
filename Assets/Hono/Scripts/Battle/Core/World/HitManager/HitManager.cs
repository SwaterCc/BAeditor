using System;
using System.Collections.Generic;
using Hono.Scripts.Battle.Event;
using Hono.Scripts.Battle.Tools;
using UnityEngine;

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
            public HitSetting HitSetting { get; }
            public int CurrentHitCount;
            public float WaitTime;

            public OffHandInfo(int currentHitCount,
                float waitTime,
                AttrCollection.AttrSnapshots attrSnapshots,
                HitSetting hitSetting)
            {
                CurrentHitCount = currentHitCount;
                WaitTime = waitTime;
                AttrSnapshots = attrSnapshots;
                HitSetting = hitSetting;
            }
        }

        private readonly List<OffHandInfo> _offHandHitInfos = new(100);


        /// <summary>
        /// 创建针对目标的打击检测
        /// </summary>
        ///我需要的不是一个属性的快照，而是整个Unit的快照（属性，tag，身上的buff列表）传给lua应该是一个代理，这个代理有读取方法，lua只读，代理可以接受对象，可以接受快照，在lua用完后释放代理
        public void CreateTargetHit(SingleHitSetting aoeHitData, Unit attacker, Unit target)
        {
            //是否为脱手打击点
            if (aoeHitData.isOffHand)
            {
                var info = new OffHandInfo(0, 0, attacker.Attrs.GetAttrSnapShots(null), aoeHitData);
                _offHandHitInfos.Add(info);
            }
            else
            {
                attacker.FireWorldEvent(EEventType.OnHit);
                DamageManager.Instance.MakeDamage();
            }
        }

        public void CreateAoeHit(AoeHitSetting aoeHitSetting, Unit attacker, Vector3 worldPos, Vector3 worldRot)
        {
            BattleManager.World.Searcher.SearchUnits();
        }

        private void targetHit() { }

        private void aoeHit() { }

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