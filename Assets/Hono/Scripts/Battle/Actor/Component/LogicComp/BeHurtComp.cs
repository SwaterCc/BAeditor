#region

using Hono.Scripts.Battle.Event;
using UnityEngine;

#endregion

namespace Hono.Scripts.Battle
{
    public partial class ActorLogic
    {
        public class BeHurtComp : AComponent
        {
            public BeHurtComp(ActorLogic logic) : base(logic) { }
            public override void Init() { }
            public override void Clear() { }

            public void OnBeHurt(HitDamageInfo hitDamageInfo)
            {
                var damageRow = ConfigManager.Table<DamageTable>().Get(hitDamageInfo.DamageConfigId);

                //处理血量
                progressHp(hitDamageInfo, damageRow);

                //添加受击特效
                progressBeHitVFX(damageRow.BeHitVFXPath);

                if (hitDamageInfo.IsKillTarget)
                {
                    ActorLogic._stateMachine?.SwitchState(EActorStateType.Death);
                }

              //  EventManager.FireEvent(Self.Uid, EBattleEventType.OnBeHit, hitDamageInfo);
                
                //事件发送者
                
            }


            private void progressHp(HitDamageInfo damageInfo, DamageTable.DamageRow damageRow)
            {
                //TODO：当前，伤害是正数，治疗是复数，后续处理掉
                //游戏中的属性数值后续统一成万分比和int，不使用float,属性完全转化为Int，Vairables里完全存储引用类型

                var curHp = Self.GetAttr(EAttrType.AttrHp);
                var curShield = Self.GetAttr(EAttrType.AttrShield);
                var maxShield = Self.GetAttr(EAttrType.AttrMaxHp) * 2;

                switch ((EDamageType)(damageRow.DamageType))
                {
                    case EDamageType.Normal:
                    case EDamageType.Percent:
                    case EDamageType.Dot:
                        if (Self.GetAttr(EAttrType.AttrInvincible) > 0)
                            return;

                        var lastShield = curShield - (int)damageInfo.FinalDamageValue;
                        if (lastShield > 0)
                        {
                            curShield = Mathf.Clamp(lastShield, 0, maxShield);
                        }
                        else
                        {
                            curHp += lastShield;
                            curShield = 0;
                        }

                        break;
                    case EDamageType.Health:
                        var maxHp = Self.GetAttr(EAttrType.AttrMaxHp);
                        curHp -= (int)(damageInfo.FinalDamageValue);
                        curHp = Mathf.Clamp(curHp, 0, maxHp);
                        break;
                }

                Self.SetAttr(EAttrType.AttrHp, curHp, false);
                Self.SetAttr(EAttrType.AttrShield, curShield, false);

                Debug.Log($"当前血量{Self.GetAttr(EAttrType.AttrHp)}");
            }

            private void progressBeHitVFX(string path)
            {
                if (string.IsNullOrEmpty(path))
                {
                    return;
                }

                if (ActorLogic.TryGetComponent<VFXComp>(out var comp))
                {
                    var setting = new VFXSetting()
                    {
                        VFXBindType = EVFXType.InWorld, Duration = 3, Offset = new Vector3(0, 0.5f, 0), VFXPath = path
                    };

                    comp.AddVFXObject(setting);
                }
            }
        }
    }
}