#region

using System.Collections.Generic;
using UnityEngine;

#endregion

namespace Hono.Scripts.Battle
{
    public class BarrackLogic : BuildingLogic
    {
        /// <summary>
        /// 最大的士兵数量
        /// </summary>
        private int _maxSoldierCount = 5;

        /// <summary>
        /// 当前的士兵数量
        /// </summary>
        private int _curSoldierCount;

        /// <summary>
        /// 生产士兵的时间
        /// </summary>
        private const float ProduceSoldierTime = 3f;

        /// <summary>
        /// 生产间隔
        /// </summary>
        private float _produceInterval = 0;

        /// <summary>
        /// 士兵uids
        /// </summary>
        private List<int> _soldierUid = new(5);

        /// <summary>
        /// 士兵配置
        /// </summary>
        private int _soliderConfigId;

        protected override void onTick(float dt)
        {
            if (BattleManager.CurBattle.RtInfo.CurRoundState != ERoundState.Running)
            {
                return;
            }

            if (_soliderConfigId <= 0)
            {
                return;
            }

            if (_curSoldierCount == _maxSoldierCount)
            {
                return;
            }

            if (_produceInterval < ProduceSoldierTime)
            {
                _produceInterval += dt;
                return;
            }

            _produceInterval = 0;

            ActorManager.Instance.CreateActor(EActorType.Monster, _soliderConfigId, null, onActorSetup);
        }

        private void onActorSetup(Actor actor)
        {
            ++_curSoldierCount;
            _soldierUid.Add(actor.Uid);

            actor.ExitSceneCallBack = (soldier) =>
            {
                --_curSoldierCount;
                _soldierUid.Remove(soldier.Uid);
            };

            Vector3 randomOffset = new(
                Random.Range(-4f, 4f),
                0,
                Random.Range(-4f, 4f)
            );

            //actor.SetAttr(EAttrType.AttrPosition, Self.Pos + randomOffset, false);
            //actor.SetAttr(EAttrType.AttrOriginPos, Self.Pos + randomOffset, false);
        }

        public void ChangeSoliderConfigId(int soliderConfigId)
        {
            _soliderConfigId = soliderConfigId;
        }

        public override void RecycleLogicObject()
        {
            APool<BarrackLogic>.Pool.Recycle(this);
        }
    }
}