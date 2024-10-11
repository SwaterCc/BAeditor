
using Hono.Scripts.Battle.Event;
using UnityEngine;

namespace Hono.Scripts.Battle
{
    public partial class ActorLogic
    {
        public class BeHurtComp : ALogicComponent
        {
            public BeHurtComp(ActorLogic logic) : base(logic) { }

            public override void Init() { }


            protected override void onTick(float dt) { }

            public void OnBeHurt(HitDamageInfo hitDamageInfo)
            {
                Actor.SetAttr(ELogicAttr.AttrHp, (int)(Actor.GetAttr<int>(ELogicAttr.AttrHp) - hitDamageInfo.FinalDamageValue),
	                false);
                Debug.Log($"当前血量{Actor.GetAttr<int>(ELogicAttr.AttrHp)}");
                BattleEventManager.Instance.TriggerActorEvent(Actor.Uid, EBattleEventType.OnBeHit, hitDamageInfo);
                //添加受击特效

                var damageRow = ConfigManager.Table<DamageTable>().Get(hitDamageInfo.DamageConfigId);
                if (ActorLogic.TryGetComponent<VFXComp>(out var comp)) {

	                var setting = new VFXSetting() {
		                VFXBindType = EVFXType.InWorld,
		                Duration = 3,
		                Offset = new SVector3(0, 0.5f, 0),
		                VFXPath = damageRow.BeHitVFXPath
	                };

	                comp.AddVFXObject(setting);
                }
            }
        }
    }
}