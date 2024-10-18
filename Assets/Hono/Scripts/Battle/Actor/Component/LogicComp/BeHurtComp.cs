using System;
using Hono.Scripts.Battle.Event;
using UnityEngine;

namespace Hono.Scripts.Battle {
	public partial class ActorLogic {
		public class BeHurtComp : ALogicComponent {
			public Action<Actor, HitDamageInfo> OnHitKillActorCallBack { get; set; }
			public BeHurtComp(ActorLogic logic) : base(logic) { }
			public override void Init() { }
			protected override void onTick(float dt) { }

			public void OnBeHurt(HitDamageInfo hitDamageInfo) {
				var damageRow = ConfigManager.Table<DamageTable>().Get(hitDamageInfo.DamageConfigId);

				//处理血量
				progressHp(hitDamageInfo, damageRow);
				
				//添加受击特效
				progressBeHitVFX(damageRow.BeHitVFXPath);

				if (hitDamageInfo.IsKillTarget) {
					OnHitKillActorCallBack?.Invoke(Actor, hitDamageInfo);
					ActorLogic._stateMachine?.SwitchState(EActorLogicStateType.Death);
				}

				BattleEventManager.Instance.TriggerActorEvent(Actor.Uid, EBattleEventType.OnBeHit, hitDamageInfo);
			}


			private void progressHp(HitDamageInfo damageInfo, DamageTable.DamageRow damageRow) {
				//TODO：当前，伤害是正数，治疗是复数，后续处理掉
				//游戏中的属性数值后续统一成万分比和int，不使用float,属性完全转化为Int，Vairables里完全存储引用类型
				//
				var curHp = Actor.GetAttr<int>(ELogicAttr.AttrHp);
				var curShield = Actor.GetAttr<int>(ELogicAttr.AttrShield);

				switch ((EDamageType)(damageRow.DamageType)) {
					case EDamageType.Normal:
					case EDamageType.Percent:
					case EDamageType.Dot:
						var lastShield = curShield - (int)damageInfo.FinalDamageValue;
						if (lastShield > 0) {
							curShield = lastShield;
						}
						else {
							curHp += lastShield;
							curShield = 0;
						}
						break;
					case EDamageType.Health:
						var maxHp = Actor.GetAttr<int>(ELogicAttr.AttrMaxHp);
						curHp -= (int)(damageInfo.FinalDamageValue);
						curHp = Mathf.Clamp(curHp, 0, maxHp);
						break;
				}
				
				Actor.SetAttr(ELogicAttr.AttrHp,curHp,false);
				Actor.SetAttr(ELogicAttr.AttrShield,curShield,false);
				
				Debug.Log($"当前血量{Actor.GetAttr<int>(ELogicAttr.AttrHp)}");
			}

			private void progressBeHitVFX(string path) {
				if (string.IsNullOrEmpty(path)) {
					return;
				}
				if (ActorLogic.TryGetComponent<VFXComp>(out var comp)) {
					var setting = new VFXSetting() {
						VFXBindType = EVFXType.InWorld, Duration = 3, Offset = new SVector3(0, 0.5f, 0), VFXPath = path
					};

					comp.AddVFXObject(setting);
				}
			}
		}
	}
}