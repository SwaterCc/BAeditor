using UnityEngine;
using UnityEngine.InputSystem;

namespace Hono.Scripts.Battle
{
	/// <summary>
	/// 手操
	/// </summary>
    public class ManualControlInput : ActorInput
    {
        public ManualControlInput(ActorLogic logic) : base(logic) {
            InputManager.Instance.AddMoveCallBack(onMove);
            InputManager.Instance.AddMoveEndCallBack(onMoveEnd);
        }
        
        private void onMove(InputAction.CallbackContext context)
        {
            var inputValue = context.ReadValue<Vector2>();
            MoveInputValue = new Vector3(-inputValue.y, 0, inputValue.x);
        }

        private void onMoveEnd(InputAction.CallbackContext context)
        {
	        MoveInputValue = Vector3.zero;
        }

        private void useSkill(int skillId)
        {
            
        }
    }
}