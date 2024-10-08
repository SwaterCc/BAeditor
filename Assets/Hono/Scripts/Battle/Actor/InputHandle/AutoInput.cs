using System.Collections.Generic;
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

            List<Vector3> wayPoints = (List<Vector3>)Logic.Actor.Variables.Get("WayPoints");
            
            if (wayPoints is { Count: > 0 })
            {
                var curPos = Logic.Actor.GetAttr<Vector3>(ELogicAttr.AttrPosition);
                MoveInputValue = (wayPoints[0] - curPos).normalized;
                return;
            }

            var hateTargetUid = Logic.Actor.GetAttr<int>(ELogicAttr.AttrHateTargetUid);

            if(_noHateComp) return;
            
            if (hateTargetUid <= 0)
            {
                _hateComp.UpdateHateTarget();
                if (Logic.Actor.ActorType == EActorType.Pawn)
                {
                    //开始返回队伍位置
                }
                return;
            }

            if (!ActorManager.Instance.TryGetActor(hateTargetUid, out var hateTarget)) 
                return;
            
            var curPos1 = Logic.Actor.GetAttr<Vector3>(ELogicAttr.AttrPosition);
            var hateTargetPos = hateTarget.GetAttr<Vector3>(ELogicAttr.AttrPosition);
            MoveInputValue = (hateTargetPos - curPos1).normalized;
        }

        protected virtual void AutoUseSkill()
        {
            if (NoSkillComp) return;

            if(SkillComp.BeforeUsedSkill is { IsExecuting: true }) return;
            
            foreach (var pSkill in SkillComp.Skills)
            {
                var skill = pSkill.Value;
                if (!skill.IsEnable)
                    continue;
                if(skill.Data.SkillType == ESkillType.PassiveSkill) 
                    continue;
                if(skill.Data.SkillType == ESkillType.UltimateSkill && !BattleRoot.Instance.AutoUltimateSkill)
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