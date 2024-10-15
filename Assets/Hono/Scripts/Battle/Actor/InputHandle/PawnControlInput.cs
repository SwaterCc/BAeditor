using UnityEngine;
using UnityEngine.InputSystem;

namespace Hono.Scripts.Battle
{
    /// <summary>
    /// 手操
    /// </summary>
    public class PawnControlInput : AutoInput
    {
        public PawnControlInput(ActorLogic logic) : base(logic) { }

        public bool OpenManualControl { get; set; }

        protected override void onInit()
        {
            base.onInit();
            InputManager.Instance.AddMoveCallBack(onMove);
            InputManager.Instance.AddMoveEndCallBack(onMoveEnd);
            OpenManualControl = Logic.Actor.IsPlayerControl;
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
            if (!OpenManualControl)
            {
                base.AutoMove();
            }
        }

        protected override void AutoUseSkill()
        {
            if (!OpenManualControl)
            {
                base.AutoUseSkill();
            }
            else
            {
                if (Logic.CurState() == EActorLogicStateType.Idle)
                {
                    base.AutoUseSkill();
                }
            }
        }
    }
}