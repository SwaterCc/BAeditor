using UnityEngine;
using UnityEngine.Serialization;

namespace Hono.Scripts.Battle.Core
{
    public class UnitTransform
    {
        /// <summary>
        /// 半径
        /// </summary>
        public float Radius { get; set; } = 0.5f;

        /// <summary>
        /// 当前坐标
        /// </summary>
        public Vector3 Pos { get; set; } = Vector3.zero;

        /// <summary>
        /// 当前朝向
        /// </summary>
        public Vector3 Forward { get; set; } = Vector3.forward;
        
        /// <summary>
        /// 四元数转向
        /// </summary>
        public Quaternion Rot => Quaternion.LookRotation(Forward, Vector3.up);

        /// <summary>
        /// 当前Y轴角度
        /// </summary>
        public float YAxisAngle
        {
            get
            {
                var xz = Forward;
                xz.y = 0;
                return Vector3.SignedAngle(Vector3.forward, xz, Vector3.up);
            }
        }
    }
}