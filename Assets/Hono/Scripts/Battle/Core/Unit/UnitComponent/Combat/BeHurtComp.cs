#region

using Hono.Scripts.Battle.Core;
using Hono.Scripts.Battle.Event;
using UnityEngine;

#endregion

namespace Hono.Scripts.Battle.Core
{
     public class BeHurtComp : UnitComponent
        {
            public override void Init() { }
            public override void onClear() { }

            public void OnBeHurt(HitDamageInfo hitDamageInfo)
            {
                var damageRow = ConfigManager.Table<DamageTable>().Get(hitDamageInfo.DamageConfigId);

                //处理血量
                progressHp(hitDamageInfo, damageRow);

                //添加受击特效
                progressBeHitVFX(damageRow.BeHitVFXPath);

               
              //  EventManager.FireEvent(Self.Uid, EBattleEventType.OnBeHit, hitDamageInfo);
                
                //事件发送者
                
            }


            private void progressHp(HitDamageInfo damageInfo, DamageTable.DamageRow damageRow)
            {
                //TODO：当前，伤害是正数，治疗是复数，后续处理掉
                //游戏中的属性数值后续统一成万分比和int，不使用float,属性完全转化为Int，Vairables里完全存储引用类型

                var curHp = Unit.GetAttr(EAttrType.AttrHp);
                var curShield = Unit.GetAttr(EAttrType.AttrShield);
                var maxShield = Unit.GetAttr(EAttrType.AttrMaxHp) * 2;

                switch ((EDamageType)(damageRow.DamageType))
                {
                    case EDamageType.Normal:
                    case EDamageType.Percent:
                    case EDamageType.Dot:
                        if (Unit.GetAttr(EAttrType.AttrInvincible) > 0)
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
                        var maxHp = Unit.GetAttr(EAttrType.AttrMaxHp);
                        curHp -= (int)(damageInfo.FinalDamageValue);
                        curHp = Mathf.Clamp(curHp, 0, maxHp);
                        break;
                }

                Unit.SetAttr(EAttrType.AttrHp, curHp, false);
                Unit.SetAttr(EAttrType.AttrShield, curShield, false);

                Debug.Log($"当前血量{Unit.GetAttr(EAttrType.AttrHp)}");
            }

            private void progressBeHitVFX(string path)
            {
                if (string.IsNullOrEmpty(path))
                {
                    return;
                }

                /*if (Self.Logic.TryGetComponent<VFXComp>(out var comp))
                {
                    var setting = new VFXSetting()
                    {
                        VFXBindType = EVFXType.InWorld, Duration = 3, Offset = new Vector3(0, 0.5f, 0), VFXPath = path
                    };

                    comp.AddVFXObject(setting);
                }*/
            }
        }
}