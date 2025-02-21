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
        /// 当前帧朝向
        /// </summary>
        public Vector3 Forward { get; private set; } = Vector3.forward;

        /// <summary>
        /// 当前帧Y轴角度
        /// </summary>
        public float YAxisAngle { get; private set; } = 0;

        /// <summary>
        /// 当前帧速度向量,每帧开始前会清空重新计算
        /// </summary>
        public Vector3 FinalVelocity { get; set; }

        public void Calc() {
	        Forward = Rot * Vector3.forward;
	        YAxisAngle = Rot.eulerAngles.y;
	        FinalVelocity = Vector3.zero;
        }
        
        public void Update() {
	        if (FinalVelocity.magnitude == 0) {
		        return;
	        }
	        Pos += FinalVelocity * World.Current.OnceTickTime;
        }
    }
}