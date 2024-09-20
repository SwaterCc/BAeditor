using System;
using Hono.Scripts.Battle.Event;
using Sirenix.OdinInspector;
using System.Collections.Generic;
using UnityEngine;

namespace Hono.Scripts.Battle.Tools
{
    public class ActorModelHandle : MonoBehaviour
    {
        [ReadOnly] public int ActorUid;

        public List<int> BuffList = new List<int>();

        public List<int> SkillList = new();
        
        public int ShowAttrId;
        [LabelText("显示指定属性")] public string ShowAttrText;

        private Actor _actor;
        
        public void OnEnable() {
	        
        }


        [Button("给自己加buff")]
        public void AddBuff(int buffConfig) {
	        if (_actor.Logic.TryGetComponent<ActorLogic.BuffComp>(out var comp)) {
		        comp.AddBuff(ActorUid,buffConfig);
	        }
        }
        
        [Button("给自己删buff")]
        public void RemoveBuff(int buffConfig) {
	        if (_actor.Logic.TryGetComponent<ActorLogic.BuffComp>(out var comp)) {
		        comp.RemoveByConfigId(buffConfig);
	        }
        }
        
        [Button]
        public void UseSkill(int skillId)
        {
            BattleEventManager.Instance.TriggerEvent(EBattleEventType.UseSkill,
                new SkillUsedEventInfo() { SkillId = skillId, ActorUid = ActorUid });
        }

		[Button]
		public void AwardSkill(int skillId) {
			if (_actor.Logic.TryGetComponent<ActorLogic.SkillComp>(out var comp)) {
				comp.LearnSkill(skillId, 1);
			}
		}

        [Button("给自己加属性")]
        public void SetAttr(int attrId,int value) {
	        _actor.SetAttr<int>(attrId, _actor.GetAttr<int>(attrId)  + value, false);
        }

        private void Update() {
	        
	        _actor ??= ActorManager.Instance.GetActor(ActorUid);
	        
	        if(_actor == null) return;
	        
	        if (Enum.IsDefined(typeof(ELogicAttr), ShowAttrId)) {
		        ShowAttrText = $"{(ELogicAttr)ShowAttrId} : " + _actor.GetAttr<int>(ShowAttrId);
	        }
	        
	        BuffList.Clear();
	        if (_actor.Logic.TryGetComponent<ActorLogic.BuffComp>(out var comp)) {
		        foreach (var pair in comp.BUffs) {
			       BuffList.Add(pair.Value.ConfigId);
		        }
	        }
		        
	        SkillList.Clear();
	        if (_actor.Logic.TryGetComponent<ActorLogic.SkillComp>(out var scomp)) {
		        foreach (var pair in scomp.Skills) {
			        SkillList.Add(pair.Key);
		        }
	        }
        }
    }
}