using UnityEngine;

namespace Hono.Scripts.Battle {
	public class VFXObject {
		private VFXSetting _setting;
		public VFXSetting Setting => _setting;
		private float _dt;
		private bool _isExpired;
		public bool IsExpired => _isExpired;
		private int _uid;
		public int Uid => _uid;
		
		public Vector3 Pos;

		public Quaternion Rot;

		public VFXObject(int uid ,VFXSetting setting) {
			_setting = setting;
			_isExpired = false;
			_uid = uid;
		}
		
		public void OnTick(float dt) {
			//if (_isExpired) return;

			if (_setting.Duration > 0 && _setting.Duration < _dt) {
				_isExpired = true;
			}
			
			_dt += dt;
		}
	}
}