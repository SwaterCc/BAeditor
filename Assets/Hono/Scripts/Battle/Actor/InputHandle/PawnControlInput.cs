#region

using UnityEngine;
using UnityEngine.InputSystem;

#endregion

namespace Hono.Scripts.Battle
{
	/// <summary>
	///     手操
	/// </summary>
	public class PawnControlInput : AutoInput
    {
        public PawnControlInput(ActorLogic logic) : base(logic) { }
        private float _hateRange;

        protected override void onInit()
        {
            base.onInit();
            InputManager.Instance.AddMoveCallBack(onMove);
            InputManager.Instance.AddMoveEndCallBack(onMoveEnd);
            if (Logic is PawnLogic pawnLogic)
            {
                _hateRange = pawnLogic.PawnLogicRow.SearchRadiu;
            }
        }

        private void onMove(InputAction.CallbackContext context)
        {
            var inputValue = context.ReadValue<Vector2>();
            MoveInputValue = new Vector3(inputValue.x, 0, inputValue.y);
        }

        private void onMoveEnd(InputAction.CallbackContext context)
        {
            MoveInputValue = Vector3.zero;
        }

        protected override void AutoMove()
        {
            if (!Logic.Actor.IsPlayerControl || !InputManager.Instance.HasMoveInput)
            {
                PawnAutoMove();
            }
        }

        private void PawnAutoMove()
        {
            if (Logic.CurState() == EActorLogicStateType.Skill) return;

            bool hasMove = false;

            var curPos = Logic.Actor.GetAttr<Vector3>(ELogicAttr.AttrPosition);
            var disPrecision = 0.1f;
            if (TryGetHateTarget(out var moveTargetPos))
            {
                disPrecision = _hateRange > 0 ? _hateRange : 2f;
                hasMove = true;
            }
            else if (TryGetWayPoint(curPos, out moveTargetPos))
            {
                hasMove = true;
            }
            else if (TryGetOriginPos(out moveTargetPos))
            {
                disPrecision = 0.2f;
                hasMove = true;
            }

            if (hasMove)
            {
                MoveInputValue = Vector3.Distance(moveTargetPos, curPos) > disPrecision
                    ? (moveTargetPos - curPos).normalized
                    : Vector3.zero;
                return;
            }

            MoveInputValue = Vector3.zero;

            if (MoveInputValue.magnitude > 0)
            {
                Logic.SetAttr(ELogicAttr.AttrHateTargetUid, -1, false);
            }
        }


        protected override void AutoUseSkill()
        {
            PawnRogueSkillAuto();
            if (!Logic.Actor.IsPlayerControl)
            {
                PawnAutoSkill();
            }
            else
            {
                if (!InputManager.Instance.HasMoveInput)
                {
                    PawnAutoSkill();
                }
            }
        }

        private void PawnRogueSkillAuto()
        {
            foreach (var pSkill in _skillComp.Skills)
            {
                var skill = pSkill.Value;
                if (!skill.IsEnable)
                    continue;
                if (skill.Data.SkillType != ESkillType.RogueSkill)
                    continue;
                _skillComp.TryUseSkill(skill.Id);
            }
        }

        private void PawnAutoSkill()
        {
            if (_skillComp == null) return;

            if (Logic.CurState() == EActorLogicStateType.Skill) return;

            foreach (var pSkill in _skillComp.Skills)
            {
                var skill = pSkill.Value;
                if (!skill.IsEnable)
                    continue;
                if (skill.Data.SkillType == ESkillType.PassiveSkill)
                    continue;
                if (skill.Data.SkillType == ESkillType.RogueSkill)
                    continue;
                if (skill.Data.SkillType == ESkillType.UltimateSkill &&
                    !BattleManager.CurBattle.RtInfo.OpenAutoUlt)
                    continue;
                if (_skillComp.TryUseSkill(skill.Id))
                {
                    return;
                }
            }
        }
    }
}