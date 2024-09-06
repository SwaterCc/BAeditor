using UnityEngine;
using UnityEngine.InputSystem;

namespace Hono.Scripts.Battle
{
    public class PawnLeaderInput : IInputHandle
    {
        public Vector3 MoveInputValue => _moveDirection;
        
        private Vector3 _moveDirection;
        
        public PawnLeaderInput() 
        {
            InputManager.Instance.AddMoveCallBack(onMove);
            InputManager.Instance.AddMoveEndCallBack(onMoveEnd);
           
        }
        
        private void onMove(InputAction.CallbackContext context)
        {
            var inputValue = context.ReadValue<Vector2>();
            _moveDirection = new Vector3(-inputValue.y, 0, inputValue.x);
        }

        private void onMoveEnd(InputAction.CallbackContext context)
        {
            _moveDirection = Vector3.zero;
        }

        private void useSkill()
        {
            
        }
    }
}