using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Hono.Scripts.Battle
{
    /// <summary>
    /// 自动输入
    /// </summary>
    public class AutoInput : ActorInput
    {
        public AutoInput(ActorLogic logic) : base(logic)
        {
            _noHateComp = logic.TryGetComponent(out _hateComp);
        }


        private bool _noHateComp;
        private ActorLogic.HateComp _hateComp;
        protected ActorLogic.HateComp HateComp => _hateComp;

        private float _useSkillDt;

        //遍历主动技能
        //如果有可以释放的则释放技能
        //如果没有则向仇恨目标移动

        protected virtual void AutoMove()
        {
            //优先向路点移动
            //其次移动向仇恨目标
            //技能期间不会移动
            //玩家角色除了手操角色以外
            if (Logic.CurState() == EActorState.Battle) return;

            Vector3 moveTargetPos;
            bool hasMove = false;

            if (TryGetWayPoint(out moveTargetPos))
            {
                hasMove = true;
            }
            else if (TryGetHateTarget(out moveTargetPos))
            {
                hasMove = true;
            }
            else if (TryGetOriginPos(out moveTargetPos))
            {
                hasMove = true;
            }

            if (hasMove)
            {
                var curPos = Logic.Actor.GetAttr<Vector3>(ELogicAttr.AttrPosition);
                MoveInputValue = (moveTargetPos - curPos).normalized;
                return;
            }
            
            MoveInputValue = Vector3.zero;
        }

        /// <summary>
        /// 路点移动
        /// </summary>
        /// <param name="targetPos"></param>
        /// <returns></returns>
        private bool TryGetWayPoint(out Vector3 targetPos)
        {
            targetPos = Vector3.zero;
            List<Vector3> wayPoints = (List<Vector3>)Logic.Actor.Variables.Get("WayPoints");

            if (wayPoints is { Count: > 0 })
            {
                var curPos = Logic.Actor.GetAttr<Vector3>(ELogicAttr.AttrPosition);
                targetPos = wayPoints.First();
                if (Vector3.Distance(curPos, targetPos) < 0.5f)
                {
                    wayPoints.RemoveAt(0);
                    if (wayPoints.Count == 0)
                    {
                        return false;
                    }

                    targetPos = wayPoints[0];
                    return true;
                }

                return true;
            }

            return false;
        }

        /// <summary>
        /// 向仇恨目标移动
        /// </summary>
        /// <param name="targetPos"></param>
        /// <returns></returns>
        private bool TryGetHateTarget(out Vector3 targetPos)
        {
            targetPos = Vector3.zero;

            if (_noHateComp) return false;

            var hateTargetUid = Logic.Actor.GetAttr<int>(ELogicAttr.AttrHateTargetUid);

            if (hateTargetUid <= 0)
            {
                _hateComp.UpdateHateTarget();
                return false;
            }

            if (!ActorManager.Instance.TryGetActor(hateTargetUid, out var hateTarget))
                return false;

            targetPos = hateTarget.Pos;
            return true;
        }

        /// <summary>
        /// 尝试回归原点
        /// </summary>
        /// <returns></returns>
        private bool TryGetOriginPos(out Vector3 targetPos)
        {
            //if(Logic == )
        }

        protected virtual void AutoUseSkill()
        {
            if (NoSkillComp) return;

            if (SkillComp.BeforeUsedSkill is { IsExecuting: true }) return;

            foreach (var pSkill in SkillComp.Skills)
            {
                var skill = pSkill.Value;
                if (!skill.IsEnable)
                    continue;
                if (skill.Data.SkillType == ESkillType.PassiveSkill)
                    continue;
                if (skill.Data.SkillType == ESkillType.UltimateSkill && !BattleRoot.BattleLevelControl.AutoUltimateSkill)
                    continue;
                SkillComp.UseSkill(skill.Id);
            }
        }

        protected override void onTick(float dt)
        {
            AutoMove();

            _useSkillDt += dt;
            if (!(_useSkillDt > 0.5f)) return;

            AutoUseSkill();
            _useSkillDt = 0;
        }
    }
}