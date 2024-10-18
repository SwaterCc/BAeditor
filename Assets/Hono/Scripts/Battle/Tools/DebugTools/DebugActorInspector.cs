using System;
using System.Collections.Generic;
using Hono.Scripts.Battle.Event;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Hono.Scripts.Battle.Tools.DebugTools
{
    public class DebugActorInspector : MonoBehaviour
    {
        public ActorModel ActorModel;

        public List<int> BuffList = new List<int>();

        public List<int> SkillList = new();
        
        public int ShowAttrId;
        [LabelText("显示指定属性")] public string ShowAttrText;

        private Actor _actor;


		public Vector3 AttrPos = Vector3.zero;
        
        public void Awake()
        {
            if (!TryGetComponent(out ActorModel))
            {
                Debug.LogError("未找到ActorModel脚本");
            }
        }
        
        void OnGUI()
        {
	        if(ActorModel == null) return;
	        // 将世界坐标转换为屏幕坐标
	        Vector3 screenPosition = Camera.main.WorldToScreenPoint(transform.position + new Vector3(0,2,0));

	        // 屏幕坐标的原点在左下角，而 GUI 的原点在左上角，因此需要进行转换
	        screenPosition.y = Screen.height - screenPosition.y;

	        var guiStyle = new GUIStyle();
	        guiStyle.normal.textColor = Color.blue;
	        guiStyle.fontSize = 22;
	        
	        // 绘制文本
	        GUI.Label(new Rect(screenPosition.x, screenPosition.y, 100, 20), ActorModel.ActorUid.ToString(),guiStyle);
        }

        [Button("给自己加buff")]
        public void AddBuff(int buffConfig) {
	        if (_actor.Logic.TryGetComponent<ActorLogic.BuffComp>(out var comp)) {
		        comp.AddBuff(ActorModel.ActorUid,buffConfig);
	        }
        }
        
        [Button("给自己删buff")]
        public void RemoveBuff(int buffConfig) {
	        if (_actor.Logic.TryGetComponent<ActorLogic.BuffComp>(out var comp)) {
		        comp.RemoveBuff(buffConfig);
	        }
        }

		[Button]
		public void UseSkill(int skillId) {
			if (_actor.Logic.TryGetComponent<ActorLogic.SkillComp>(out var comp)) {
				comp.LearnSkill(skillId, 1);
				_actor.TriggerEvent(EBattleEventType.UseSkill,
				new UsedSkillEventInfo() { SkillId = skillId, CasterUid = _actor.Uid });
			}
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

        
        [Button("位置回归原点")]
        public void ResetPos() {
	        _actor.SetAttr<Vector3>(ELogicAttr.AttrPosition, Vector3.zero, false);
        }

        private void Update() {
	        if(ActorModel == null) return;
	        
	        _actor ??= ActorManager.Instance.GetActor(ActorModel.ActorUid);
			

			if (_actor == null) return;

			AttrPos = _actor.Pos;

			if (Enum.IsDefined(typeof(ELogicAttr), ShowAttrId)) {
		        ShowAttrText = $"{(ELogicAttr)ShowAttrId} : " + _actor.GetAttr<int>(ShowAttrId);
	        }
	        
	        BuffList.Clear();
	        if (_actor.Logic.TryGetComponent<ActorLogic.BuffComp>(out var comp)) {
		        foreach (var pair in comp.Buffs) {
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