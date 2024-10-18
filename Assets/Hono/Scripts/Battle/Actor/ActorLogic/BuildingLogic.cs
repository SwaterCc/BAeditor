using UnityEngine;

namespace Hono.Scripts.Battle {
	public class BuildingLogic : ActorLogic {
		public BuildingLogic(Actor actor) : base(actor) {
			BuildingConfig = ConfigManager.Table<BuildingLogicTable>().Get(Actor.ConfigId);
		}

		public BuildingLogicTable.BuildingLogicRow BuildingConfig { get; private set; }

		protected override void setupInput() {
			_actorInput = new BuildingControlInput(this);
		}

		protected override void setupAttrs() {
			if(BuildingConfig == null) return;
			
			SetAttr(ELogicAttr.AttrModelId, BuildingConfig.Model, false);
	        SetAttr(ELogicAttr.AttrFaction, BuildingConfig.Faction, false);
	        
	        var attrRow = ConfigManager.Table<EntityAttrBaseTable>().Get(BuildingConfig.AttrTemplateId);
			
			SetAttr(ELogicAttr.AttrBaseSpeed, attrRow.AttrBaseSpeed, false);
			SetAttr(ELogicAttr.AttrMoveSpeedPCTAdd, attrRow.AttrMoveSpeedPCTAdd, false);
			SetAttr(ELogicAttr.AttrHp, attrRow.AttrMaxHpAdd, false);
	        SetAttr(ELogicAttr.AttrMaxHpAdd, attrRow.AttrMaxHpAdd,false);
	        SetAttr(ELogicAttr.AttrMp, attrRow.AttrMaxMpAdd, false);
	        SetAttr(ELogicAttr.AttrMaxMpAdd, attrRow.AttrMaxMpAdd,false);
	        SetAttr(ELogicAttr.AttrAttackAdd, attrRow.AttrAttackAdd,false);
	        SetAttr(ELogicAttr.AttrCritAdd, attrRow.AttrCritAdd,false);
	        SetAttr(ELogicAttr.AttrDefenseAdd, attrRow.AttrDefenseAdd,false);
	        SetAttr(ELogicAttr.AttrHealAdd, attrRow.AttrHealAdd,false);
	        SetAttr(ELogicAttr.AttrEntityLevel, attrRow.AttrEntityLevel,false);
	        SetAttr(ELogicAttr.AttrHealedAdd, attrRow.AttrHealedAdd,false);
	        SetAttr(ELogicAttr.AttrCritDamageAdd, attrRow.AttrCritDamageAdd,false);
	        SetAttr(ELogicAttr.AttrDmgAAdd, attrRow.AttrDmgAAdd,false);
	        SetAttr(ELogicAttr.AttrDmgRedAdd, attrRow.AttrDmgRedAdd,false);
	        SetAttr(ELogicAttr.AttrHealIntensityAdd, attrRow.AttrHealIntensityAdd,false);
	        SetAttr(ELogicAttr.AttrIgnoreDefenseAdd, attrRow.AttrIgnoreDefenseAdd,false);
	        SetAttr(ELogicAttr.AttrAttackSpeedPCTAdd, attrRow.AttrAttackSpeedPCTAdd,false);
	        SetAttr(ELogicAttr.AttrElementPenPCTAdd, attrRow.AttrElementPenPCTAdd,false);
	        SetAttr(ELogicAttr.AttrElementMagicRedPCTAdd, attrRow.AttrElementMagicRedPCTAdd,false);
	        SetAttr(ELogicAttr.AttrElementPhysicalPenPCTAdd, attrRow.AttrElementPhysicalPenPCTAdd,false);
	        SetAttr(ELogicAttr.AttrElementPhysicalRedPCTAdd, attrRow.AttrElementPhysicalRedPCTAdd,false);
		}
		
		
		protected override void setupStateMachine() {
			_stateMachine = new ActorStateMachine(this);
		}

		protected override void setupComponents() {
			addComponent(new BuffComp(this,() => BuildingConfig.OwnerBuffs));
			addComponent(new SkillComp(this, () => BuildingConfig.ownerSkills));
			addComponent(new VFXComp(this));
			addComponent(new AttrSimpleProgress(this));
			addComponent(new BeHurtComp(this));
		}
	}
}