using System;
using UnityEngine;

namespace Hono.Scripts.Battle
{
    public class UOProxyPhysicsHandler : MonoBehaviour
    {
        /// <summary>
        /// 碰撞器
        /// </summary>
        private Collider _collider;
        
        /// <summary>
        /// 物理组件
        /// </summary>
        private Rigidbody _rigidbody;

        /// <summary>
        /// 移动组件
        /// </summary>
        private CharacterController _characterController;

        public void OnEnable()
        {
            TryGetComponent(out _rigidbody);
            TryGetComponent(out _collider);
            TryGetComponent(out _characterController);

            if (!_rigidbody && !_collider && !_characterController)
            {
                Debug.LogError("找不到物理组件");
                return;
            }

            if (_rigidbody != null && !_rigidbody.isKinematic )
            {
                _rigidbody.isKinematic = true;
                Debug.LogError("刚体的 isKinematic 没勾选");
                return;
            }
        }

        public bool Move(Vector3 velocity)
        {
            if (_characterController != null)
            {
                _characterController.SimpleMove(velocity);
                return true;
            }

            if (_rigidbody != null)
            {
                _rigidbody.linearVelocity = velocity;
                return true;
            }

            return false;
        }
    }
}