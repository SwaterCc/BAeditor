using UnityEngine;

namespace Hono.Scripts.Battle {
	public class ShowAttrCreator {
		public static IAttr Create(int attrType) {
			switch ((EShowAttrType)attrType) { }

			return null;
		}
	}

	public static class LogicAttrCreator {
		public static IAttr Create(int attrType) {
			switch ((EAttrType)attrType) {
				case EAttrType.Hp:
				case EAttrType.Mp:
				case EAttrType.Source:
				case EAttrType.Attack:
					return new Attr<int>((a, b) => a + b);
				case EAttrType.Position:
					return new Attr<Vector3>((a, b) => a + b);
				case EAttrType.Rot:
					return new Attr<Quaternion>((a, b) => b * a);
			}

			return null;
		}
	}


	public static class AttrEnumExtensions {
		public static int ToInt(this EAttrType attrType) {
			return (int)attrType;
		}

		public static int ToInt(this EShowAttrType attrType) {
			return (int)attrType;
		}
	}
}