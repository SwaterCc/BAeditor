using System.Collections.Generic;
using Hono.Scripts.Battle.Core.Base;
using UnityEngine;

namespace Hono.Scripts.Battle.Core {
	/// <summary>
	/// 速度修改接口，实现最终速度获取接口
	/// </summary>
	public interface IVelocityModifier {
		public Vector3 GetVelocity();
	}

	[JsonUnitCompCtorParams(typeof(MoveComp))]
	public class MoveControlCtorParams : UnitCompCtorParams {
		public EMoveControllerType ControllerType;
	}

	/// <summary>
	/// 移动控制组件
	/// </summary>
	[JsonUnitComponent]
	public class MoveComp : UnitComponent, IGPoolObject {
		private EMoveControllerType _moveControllerType = EMoveControllerType.Human;
		private MoveControllor _controllor;
		private Dictionary<EMoveControllerType, MoveControllor> _moveControllors;

		public MoveComp() {
			_moveControllors = new Dictionary<EMoveControllerType, MoveControllor>() {
				{ EMoveControllerType.Human, new HumanMoveControllor(this) }, { EMoveControllerType.Building, new BuildingMoveControllor(this) },
			};
		}
		
		public override void Init() {
			
			_controllor = _moveControllors[_moveControllerType];
			_controllor.Init();
		}
		
		protected override void onTick(float dt) {
			Unit.UnitTransform.FinalVelocity += _controllor.GetVelocity();
			Unit.UnitTransform.Rot = _controllor.GetRotation() * Unit.UnitTransform.Rot;
		}

		public override void Recycle() {
			GPool<MoveComp>.Pool.Recycle(this);
		}

		public void OnRecycle() { }
	}
}