using System.Collections.Generic;
using UnityEngine;

namespace Hono.Scripts.Battle
{
    public static class LogicAttrCreator
    {
        public static IAttr Create(int attrType)
        {
            switch ((ELogicAttr)attrType)
            {
                case ELogicAttr.AttrUid:
                case ELogicAttr.AttrSourceActorUid:
                case ELogicAttr.AttrSourceAbilityConfigId:
                case ELogicAttr.AttrFaction:
                    return new Attr<int>((a, b) => a + b);
                case ELogicAttr.AttrBaseSpeed:
                    return new Attr<float>((a, b) => a + b);
                case ELogicAttr.AttrPosition:
                    return new Attr<Vector3>((a, b) => a + b);
                case ELogicAttr.AttrRot:
                    return new Attr<Quaternion>((a, b) => b * a);
                case ELogicAttr.SourceAbilityType:
                    return new Attr<EAbilityType>(null);
                case ELogicAttr.AttrAttackTargetUid:
                    return new Attr<List<int>>(null);

                #region 数值数据

                case ELogicAttr.AttrEntityLevel:
                case ELogicAttr.AttrHp:
                case ELogicAttr.AttrHpPer:
                case ELogicAttr.AttrMaxHp:
                case ELogicAttr.AttrMp:
                case ELogicAttr.AttrMpPer:
                case ELogicAttr.AttrDefense:
                case ELogicAttr.AttrIgnoreDefense:
                case ELogicAttr.AttrIgnoreDefensePCT:
                case ELogicAttr.AttrCrit:
                case ELogicAttr.AttrCritDamage:
                case ELogicAttr.AttrAttackSpeedPCT:
                case ELogicAttr.AttrAttack:
                case ELogicAttr.AttrDmgRed:
                case ELogicAttr.AttrDmgRedNear:
                case ELogicAttr.AttrDmgRedFar:
                case ELogicAttr.AttrDmgRedBullet:
                case ELogicAttr.AttrDmgRedMelee:
                case ELogicAttr.AttrDmgRedBuff:
                case ELogicAttr.AttrDmgRedVsNormalEnemy:
                case ELogicAttr.AttrDmgRedVsEliteEnemy:
                case ELogicAttr.AttrDmgRedHealthy:
                case ELogicAttr.AttrDmgRedNonHealthy:
                case ELogicAttr.AttrElementPenPCT:
                case ELogicAttr.AttrElementRedPCT:
                case ELogicAttr.AttrElementPhysicalPenPCT:
                case ELogicAttr.AttrElementPhysicalRedPCT:
                case ELogicAttr.AttrElementMagicPenPCT:
                case ELogicAttr.AttrElementMagicRedPCT:
                case ELogicAttr.AttrHealIntensity:
                case ELogicAttr.AttrHeal:
                case ELogicAttr.AttrHealed:
                    return new Attr<int>((a, b) => a + b);

                #endregion
            }

            return null;
        }
    }
}