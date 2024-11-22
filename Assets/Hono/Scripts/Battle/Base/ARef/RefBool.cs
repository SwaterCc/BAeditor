#region

using System;

#endregion

namespace Hono.Scripts.Battle.Base {
	[Serializable]
	public class RefBool : IARef,IAPoolObject {
		public bool Value;

		public RefBool(bool initialValue = false) {
			Value = initialValue;
		}
		
		public void OnRecycle() {
			throw new NotImplementedException();
		}

		public IARef DeepCopy() {
			throw new NotImplementedException();
		}
		
		// 隐式转换到 bool
		public static implicit operator bool(RefBool refBool) {
			return refBool.Value;
		}

		// 等于操作符重载
		public static bool operator ==(RefBool a, RefBool b) {
			return a.Value == b.Value;
		}

		// 不等于操作符重载
		public static bool operator !=(RefBool a, RefBool b) {
			return a.Value != b.Value;
		}

		private new bool Equals(object obj) {
			if (!(obj is RefBool)) return false;
			return this == (RefBool)obj;
		}

		public override string ToString() {
			return Value.ToString();
		}
	}
}