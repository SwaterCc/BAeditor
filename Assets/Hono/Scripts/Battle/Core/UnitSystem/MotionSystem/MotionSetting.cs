#region

using Sirenix.OdinInspector;
using System;

#endregion

namespace Hono.Scripts.Battle.Core {
	/// <summary>
	///     速度单位都是m/s
	/// </summary>
	[Serializable]
	public struct MotionSetting {
		/// <summary>
		///     最大持续时长
		/// </summary>
		[LabelText("持续时长")]
		public float duration;
		
		/// <summary>
		///     反方向移动(默认是向目标移动)
		/// </summary>
		[LabelText("反方向移动(默认是向目标移动)")]
		public bool isReverse;
		
		/// <summary>
		///     不响应输入
		/// </summary>
		[LabelText("屏蔽输入")]
		public bool disableMoveInput;

		/// <summary>
		///     初速度(环绕模式下是y轴角度)
		/// </summary>
		[LabelText("初速度")]
		public float speed;

		/// <summary>
		///     加速度(环绕模式下是角加速度)
		/// </summary>
		[LabelText("加速度")]
		public float acceleration;

		/// <summary>
		/// 最大速度
		/// </summary>
		[LabelText("最大速度 -1为无上限")]
		public float maxSpeed;
		
		/// <summary>
		///     会被阻挡
		/// </summary>
		[LabelText("会被阻挡")]
		public bool stopAfterCollision;

		/// <summary>
		///     关闭事件触发
		/// </summary>
		[LabelText("关闭事件触发")]
		public bool disableMotionCollisionEvent;
	}
}