using System;
using Sirenix.OdinInspector;

namespace Hono.Scripts.Battle {
	/// <summary>
	/// 速度单位都是m/s
	/// </summary>
	[Serializable]
	public class MotionSetting {
		/// <summary>
		/// 位移类型
		/// </summary>
		[LabelText("位移类型，目前只有直线")]
		public EMotionType MoveType;

		/// <summary>
		/// 反方向移动(默认是向目标移动)
		/// </summary>
		[LabelText("反方向移动(默认是向目标移动)")]
		public bool IsReverse;

		/// <summary>
		/// 移动时朝向目标
		/// </summary>
		[LabelText("移动时朝向目标")]
		public bool MovingNoChangeRot;
		
		/// <summary>
		/// 不响应输入
		/// </summary>
		[LabelText("不响应输入")]
		public bool DisableMoveInput;
		
		/// <summary>
		/// 初速度(环绕模式下是y轴角度)
		/// </summary>
		[LabelText("初速度(环绕模式下是y轴角度)")]
		[HideIf("MoveType",EMotionType.Curve)]
		public float Speed;

		/// <summary>
		/// 加速度(环绕模式下是角加速度)
		/// </summary>
		[LabelText("加速度(环绕模式下是角加速度)")]
		[HideIf("MoveType",EMotionType.Curve)]
		public float Acceleration;

		/// <summary>
		/// 曲线名
		/// </summary>
		[LabelText("曲线名")]
		[ShowIf("MoveType",EMotionType.Curve)]
		public string CurveName;

		/// <summary>
		/// 最大持续时长
		/// </summary>
		[LabelText("持续时长")]
		public float Duration;

		/// <summary>
		/// 会被阻挡
		/// </summary>
		[LabelText("会被阻挡")]
		public bool StopAfterCollision;

		/// <summary>
		/// 关闭事件触发
		/// </summary>
		[LabelText("关闭事件触发")]
		public bool TriggerEventClose;
	}
}