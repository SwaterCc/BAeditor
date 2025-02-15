using Unity.Mathematics;
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
        /// 四元数转向
        /// </summary>
        public Quaternion Rot { get; set; } = quaternion.identity;

        /// <summary>
        /// 当前朝向
        /// </summary>
        public Vector3 Forward => Rot * Vector3.forward;

        /// <summary>
        /// 当前Y轴角度
        /// </summary>
        public float YAxisAngle => Rot.eulerAngles.y;
    }
}