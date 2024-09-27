using System;

namespace Hono.Scripts.Battle {
	/// <summary>
	/// 速度单位都是m/s
	/// </summary>
	[Serializable]
	public class MotionSetting {
		/// <summary>
		/// 用于识别位移的id
		/// </summary>
		public int EventId;

		/// <summary>
		/// 位移类型
		/// </summary>
		public EMotionType MoveType;

		/// <summary>
		/// 反方向移动(默认是向目标移动)
		/// </summary>
		public bool IsReverse;

		/// <summary>
		/// 初速度(环绕模式下是y轴角度)
		/// </summary>
		public float Speed;

		/// <summary>
		/// 加速度(环绕模式下是角加速度)
		/// </summary>
		public float Acceleration;

		/// <summary>
		/// 曲线名
		/// </summary>
		public string CurveName;

		/// <summary>
		/// 最大持续时长
		/// </summary>
		public float Duration;

		/// <summary>
		/// 会被阻挡
		/// </summary>
		public bool StopAfterCollision;

		/// <summary>
		/// 关闭事件触发
		/// </summary>
		public bool TriggerEventClose;
	}
}