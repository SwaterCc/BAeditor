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
        public AutoInput(ActorLogic logic) : base(logic) { }


        private bool _noHateComp;
        private ActorLogic.HateComp _hateComp;
        protected ActorLogic.HateComp HateComp => _hateComp;

        private float _useSkillDt;

        protected override void onInit()
        {
            _noHateComp = !Logic.TryGetComponent(out _hateComp);
        }

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

            var curPos = Logic.Actor.GetAttr<Vector3>(ELogicAttr.AttrPosition);
            var disPrecision = 0.1f;
            
            if (TryGetWayPoint(curPos, out moveTargetPos))
            {
                hasMove = true;
            }
            else if (TryGetHateTarget(out moveTargetPos))
            {
                disPrecision = 3f;
                hasMove = true;
            }
            else if (TryGetOriginPos(out moveTargetPos))
            {
                disPrecision = 0.2f;
                hasMove = true;
            }

            if (hasMove)
            {
                MoveInputValue = Vector3.Distance(moveTargetPos, curPos) > disPrecision ? (moveTargetPos - curPos).normalized : Vector3.zero;
                return;
            }

            MoveInputValue = Vector3.zero;
        }

        /// <summary>
        /// 路点移动
        /// </summary>
        /// <param name="curPos"></param>
        /// <param name="targetPos"></param>
        /// <returns></returns>
        private bool TryGetWayPoint(Vector3 curPos, out Vector3 targetPos)
        {
            targetPos = Vector3.zero;
            List<Vector3> wayPoints = (List<Vector3>)Logic.Actor.Variables.Get("WayPoints");

            if (wayPoints is { Count: > 0 })
            {
                targetPos = wayPoints.First();
                if (Vector3.Distance(curPos, targetPos) < 0.1f)
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
            targetPos = Logic.Actor.GetAttr<Vector3>(ELogicAttr.AttrOriginPos);
            return true;
        }

        protected virtual void AutoUseSkill()
        {
            if (SkillComp == null) return;

            if (SkillComp.BeforeUsedSkill is { IsExecuting: true }) return;

            foreach (var pSkill in SkillComp.Skills)
            {
                var skill = pSkill.Value;
                if (!skill.IsEnable)
                    continue;
                if (skill.Data.SkillType == ESkillType.PassiveSkill)
                    continue;
                if (skill.Data.SkillType == ESkillType.UltimateSkill)
                    continue;
                SkillComp.UseSkill(skill.Id);
            }
        }

        protected override void onTick(float dt)
        {
            AutoMove();

            _useSkillDt += dt;
            if (!(_useSkillDt > 1.5f)) return;

            AutoUseSkill();
            _useSkillDt = 0;
        }
    }
}