using System;
using UnityEngine;
using UnityEngine.Serialization;

namespace Hono.Scripts.Battle
{
    public class UOProxyPhysicsHandler : MonoBehaviour
    {
        /// <summary>
        /// 碰撞器
        /// </summary>
        public Collider collider;

        /// <summary>
        /// 物理组件
        /// </summary>
        public Rigidbody rigidbody;

        /// <summary>
        /// 移动组件
        /// </summary>
        public CharacterController characterController;

        public void OnEnable()
        {
            if (!rigidbody && !collider && !characterController)
            {
                Debug.LogError("找不到物理组件");
                return;
            }

            if (rigidbody != null && !rigidbody.isKinematic)
            {
                rigidbody.isKinematic = true;
                Debug.LogError("刚体的 isKinematic 没勾选");
            }
        }

        public void Set(float p1, float p2, float p3)
        {
            if (characterController != null)
            {
                characterController.radius = p1 > 0 ? p1 : characterController.radius;
                characterController.height = p2 > 0 ? p2 : characterController.height;
            }

            if (collider != null)
            {
                switch (collider)
                {
                    case SphereCollider sphereCollider:
                        sphereCollider.radius = p1 > 0 ? p1 : sphereCollider.radius ;
                        break;
                    case CapsuleCollider capsuleCollider:
                        capsuleCollider.radius = p1 > 0 ? p1 : capsuleCollider.radius;
                        capsuleCollider.height = p2 > 0 ? p2 : capsuleCollider.height;
                        break;
                    case BoxCollider boxCollider:
                        p1 = p1 > 0 ? p1 : boxCollider.size.x;
                        p2 = p2 > 0 ? p2 : boxCollider.size.y;
                        p3 = p3 > 0 ? p3 : boxCollider.size.z;
                        boxCollider.size = new Vector3(p1, p2, p3);
                        break;
                }
            }
        }

        public void SetCenter(Vector3 center)
        {
            if (characterController != null)
            {
                characterController.center = center;
            }

            if (collider != null)
            {
                switch (collider)
                {
                    case SphereCollider sphereCollider:
                        sphereCollider.center = center;
                        break;
                    case CapsuleCollider capsuleCollider:
                        capsuleCollider.center = center;
                        break;
                    case BoxCollider boxCollider:
                        boxCollider.center = center;
                        break;
                }
            }
        }

        public bool Move(Vector3 velocity)
        {
            if (characterController != null)
            {
                characterController.SimpleMove(velocity);
                return true;
            }

            if (rigidbody != null)
            {
                rigidbody.linearVelocity = velocity;
                return true;
            }

            return false;
        }
    }
}