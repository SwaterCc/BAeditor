using Hono.Scripts.Battle.Event;
using UnityEngine;

namespace Hono.Scripts.Battle.Core {
	/// <summary>
	/// 移动控制器基类
	/// </summary>
	public abstract class MoveControllor {
		protected MoveComp _moveComp;
		/// <summary>
		/// 当前是否被玩家控制
		/// </summary>
		private bool _isPlayerControl;
		/// <summary>
		/// 禁止移动
		/// </summary>
		private bool _isDisableMoveInput;
		/// <summary>
		/// 玩家控制切换事件监听
		/// </summary>
		private EventListener _onAttrChanged;
		/// <summary>
		/// 输入方向
		/// </summary>
		protected Vector3 InputDirection {
			get {
				if (_isPlayerControl) {
					return PlayerInputManager.Instance.InputDirection;
				}
				else {
					return Vector3.zero;
				}
			} 
		}
		/// <summary>
		/// 属性中获取的移动速度
		/// </summary>
		public float MoveSpeed { get; private set; }
		
		protected MoveControllor(MoveComp moveComp) {
			_moveComp = moveComp;
			_onAttrChanged = new EventListener(EEventType.OnAttrChanged, OnAttrChanged);
		}

		public void Init() {
			MoveSpeed = _moveComp.Unit.GetAttr(EAttrType.AttrMoveSpeedPCT) / 10000f;
			_moveComp.Unit.AddUnitEvtListener(_onAttrChanged);
		}

		public void Clear() {
			_moveComp.Unit.RemoveUnitEvtListener(_onAttrChanged);
		}

		/// <summary>
		/// 属性改变监听
		/// </summary>
		/// <param name="board"></param>
		private void OnAttrChanged(VariableBoard board) {
			EAttrType attrType = board.GetEvtField(AttrChangedEventInfo.AttrType);
			switch (attrType) {
				case EAttrType.AttrIsPlayerCtrl:
					_isPlayerControl = board.GetEvtField(AttrChangedEventInfo.Value) > 0;
					break;
				case EAttrType.DisableInputMove:
					_isDisableMoveInput = board.GetEvtField(AttrChangedEventInfo.Value) > 0;
					break;
				case EAttrType.AttrMoveSpeedPCT:
					MoveSpeed = board.GetEvtField(AttrChangedEventInfo.Value) / 10000f;
					break;
			}
		}
		
		protected abstract Vector3 CalcVelocity();
		protected abstract Quaternion CalcRotation();

		/// <summary>
		/// 获取最终速度
		/// </summary>
		/// <returns></returns>
		public Vector3 GetVelocity() {
			if (_isDisableMoveInput) {
				return Vector3.zero;
			}

			if (InputDirection.magnitude == 0) {
				return Vector3.zero;
			}
			return CalcVelocity();
		}

		/// <summary>
		/// 获取最终旋转
		/// </summary>
		/// <returns></returns>
		public Quaternion GetRotation() {
			if (_isDisableMoveInput) {
				return _moveComp.Unit.UnitTransform.Rot;
			}

			return CalcRotation();
		}
	}
}