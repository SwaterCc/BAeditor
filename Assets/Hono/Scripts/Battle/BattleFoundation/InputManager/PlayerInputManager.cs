#region

using Hono.Scripts.Battle.Core.Base;
using System;
using UnityEngine;
using UnityEngine.InputSystem;

#endregion

namespace Hono.Scripts.Battle
{
    public class PlayerInputManager : MonoSingleton<PlayerInputManager>
    {
        private PawnInput _pawnInput;

        /// <summary>
        /// X左右，Y前后
        /// </summary>
        public Vector2 InputValue { get; private set; }
        public Vector3 InputDirection { get; private set; }
        public bool HasMoveInput { get; private set; }

        protected override void Awake()
        {
            base.Awake();
            _pawnInput = new PawnInput();
            _pawnInput.Pawn.Move.performed += onMove;
            _pawnInput.Pawn.Move.canceled += onMoveEnd;
        }

        public void OnEnable()
        {
            _pawnInput.Pawn.Enable();
        }

        public void OnDestroy()
        {
            _pawnInput.Pawn.Disable();
        }
        
        private void onMove(InputAction.CallbackContext context)
        {
            InputValue = context.ReadValue<Vector2>();
            InputDirection = new Vector3(InputValue.x, 0, InputValue.y);
            HasMoveInput = true;
        }

        private void onMoveEnd(InputAction.CallbackContext context)
        {
            HasMoveInput = false;
            InputDirection = Vector3.zero;
            InputValue = Vector2.zero;
        }
    }
}