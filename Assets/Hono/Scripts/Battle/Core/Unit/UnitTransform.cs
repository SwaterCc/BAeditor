using UnityEngine;
using UnityEngine.Serialization;

namespace Hono.Scripts.Battle.Core
{
    public class UnitTransform
    {
        /// <summary>
        /// 当前朝向
        /// </summary>
        public float YAxisAngle { get; set; }

        /// <summary>
        /// 半径
        /// </summary>
        public float Radius { get; set; }

        /// <summary>
        /// 当前位置信息
        /// </summary>
        public Vector3 Pos { get; set; }

        /// <summary>
        /// 单位缩放系数
        /// </summary>
        public Vector3 Scale { get; set; }
    }
}