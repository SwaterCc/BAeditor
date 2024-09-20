using System;

namespace Hono.Scripts.Battle {
	public partial class ActorLogic {
		public enum EMotionType {
			//直线
			Liner,

			//环绕
			Rad,

			//自定义曲线路径
			Curve,
		}
		
		[Serializable]
		public class MotionSetting {
			//反方向移动(默认是向目标移动)
			public bool IsReverse;

			//初速度
			public float Speed;

			//加速度
			public float Acceleration;

			//角速度
			public float AngleSpeed;

			//角加速度
			public float AngleAcceleration;
		}

		public class Motion { }

		public class MotionComp : ALogicComponent {
			public MotionComp(ActorLogic logic) : base(logic) { }

			public override void Init() { }

			public void AddMotion() { }

			protected override void onTick(float dt) { }
		}
	}
}