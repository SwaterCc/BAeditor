using System;
using Hono.Scripts.Battle.Event;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Hono.Scripts.Battle.Tools
{
    public class ActorModelHandle : MonoBehaviour
    {
        [ReadOnly] public int ActorUid;

        [OnValueChanged("ShowAttrIdChange")]
        public int ShowAttrId;
        [LabelText("显示指定属性")] public string ShowAttrText;

        [Button("给自己加buff")]
        public void AddBuff(int buffConfig) {
	        var actor = ActorManager.Instance.GetActor(ActorUid);
	        if (actor.Logic.TryGetComponent<ActorLogic.BuffComp>(out var comp)) {
		        comp.AddBuff(ActorUid,buffConfig);
	        }
        }
        
        [Button("给自己删buff")]
        public void RemoveBuff(int buffConfig) {
	        var actor = ActorManager.Instance.GetActor(ActorUid);
	        if (actor.Logic.TryGetComponent<ActorLogic.BuffComp>(out var comp)) {
		        comp.RemoveByConfigId(buffConfig);
	        }
        }
        
        [Button]
        public void UseSkill(int skillId)
        {
            BattleEventManager.Instance.TriggerEvent(EBattleEventType.UseSkill,
                new SkillUsedEventInfo() { SkillId = skillId, ActorUid = ActorUid });
        }

        [Button("给自己加属性")]
        public void SetAttr(int attrId,int value) {
	        var actor = ActorManager.Instance.GetActor(ActorUid);
	        actor.SetAttr<int>(attrId, actor.GetAttr<int>(attrId)  + value, false);
        }

        private void Update() {
	        var actor = ActorManager.Instance.GetActor(ActorUid);
	        if (Enum.IsDefined(typeof(ELogicAttr), ShowAttrId)) {
		        ShowAttrText = $"{(ELogicAttr)ShowAttrId} : " + actor.GetAttr<int>(ShowAttrId);
	        }
        }
    }
}