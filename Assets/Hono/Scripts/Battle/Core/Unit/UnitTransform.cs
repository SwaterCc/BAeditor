using UnityEngine;
using UnityEngine.Serialization;

namespace Hono.Scripts.Battle.Core
{
    public struct UnitTransform
    {
        /// <summary>
        /// 当前朝向
        /// </summary>
        public float YAxisAngle;
        /// <summary>
        /// 半径
        /// </summary>
        public float Radius;
        /// <summary>
        /// 当前位置信息
        /// </summary>
        public Vector3 Pos;
        /// <summary>
        /// 单位缩放系数
        /// </summary>
        public Vector3 Scale;
    }
}