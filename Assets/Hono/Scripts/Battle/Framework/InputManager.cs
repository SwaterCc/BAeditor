using System;
using Hono.Scripts.Battle.Tools;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;

namespace Hono.Scripts.Battle
{
    public class InputManager : MonoBehaviour
    {
        private static InputManager _instance;
        public static InputManager Instance {
            get {
                if (_instance == null) {
                    // 尝试查找现有实例
                    _instance = FindObjectOfType<InputManager>();

                    // 如果没有找到，则创建新的 GameObject 并添加该组件
                    if (_instance == null) {
                        GameObject singletonObject = new GameObject(nameof(InputManager));
                        _instance = singletonObject.AddComponent<InputManager>();
                    }
                }

                return _instance;
            }
        }
        
        private PawnInput _pawnInput;
        
        /// <summary>
        /// X左右，Y前后
        /// </summary>
        public Vector2 InputValue;

        public Vector3 InputDirection;

        public bool HasMoveInput;
        public void Awake()
        {
            DontDestroyOnLoad(this.gameObject);

            // 检查是否已经存在另一个实例
            if (_instance != null && _instance != this) {
                Destroy(this.gameObject);
            }
            else {
                _instance = this;
            }
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

        public void AddMoveCallBack(Action<InputAction.CallbackContext> onMove)
        {
            _pawnInput.Pawn.Move.performed += onMove;
           
        }
        
        public void AddMoveEndCallBack(Action<InputAction.CallbackContext> onMoveEnd)
        {
            _pawnInput.Pawn.Move.canceled += onMoveEnd;
        }

        public void AddPressNum1(Action<InputAction.CallbackContext> onPress)
        {
            _pawnInput.Pawn.Skill1.performed += onPress;
        }
        
        public void AddPressNum2(Action<InputAction.CallbackContext> onPress)
        {
            _pawnInput.Pawn.Skill2.performed += onPress;
        }
        
        private void onMove(InputAction.CallbackContext context)
        {
            InputValue = context.ReadValue<Vector2>();
            InputDirection = new Vector3(-InputValue.y, 0, InputValue.x);
            HasMoveInput = true;
        }

        private void onMoveEnd(InputAction.CallbackContext context)
        {
            HasMoveInput = false;
            InputValue = Vector2.zero;
        }
    }
}