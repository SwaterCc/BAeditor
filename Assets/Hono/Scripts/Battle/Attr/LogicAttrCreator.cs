using UnityEngine;

namespace Hono.Scripts.Battle
{
    public static class LogicAttrCreator {
        public static IAttr Create(int attrType) {
            switch ((ELogicAttr)attrType) {
                case ELogicAttr.Hp:
                case ELogicAttr.Mp:
                case ELogicAttr.Source:
                case ELogicAttr.Attack:
                    return new Attr<int>((a, b) => a + b);
                case ELogicAttr.Position:
                    return new Attr<Vector3>((a, b) => a + b);
                case ELogicAttr.Rot:
                    return new Attr<Quaternion>((a, b) => b * a);
            }

            return null;
        }
    }
}