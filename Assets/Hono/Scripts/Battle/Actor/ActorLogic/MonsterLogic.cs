using System;

namespace Hono.Scripts.Battle
{
    public class MonsterLogic : ActorLogic
    {
	    public MonsterLogic(Actor actor) : base(actor) {
		    MonsterConfig = ConfigManager.Table<MonsterLogicTable>().Get(actor.ConfigId);
	    }

        public MonsterLogicTable.MonsterLogicRow MonsterConfig { get; }

        protected override void setupAttrs() {
	        SetAttr(ELogicAttr.AttrModelId, MonsterConfig.ModelId, false);
	        SetAttr(ELogicAttr.AttrFaction, MonsterConfig.Faction, false);
	        
	        var attrRow = ConfigManager.Table<EntityAttrBaseTable>().Get(MonsterConfig.AttrTemplateId);
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

        protected override void onInit()
        {
            
        }

        protected override void setupComponents()
        {
	        addComponent(new AttrCastLv1(this));
            addComponent(new BeHurtComp(this));
            addComponent(new BuffComp(this,() => MonsterConfig.OwnerBuffs));
            addComponent(new SkillComp(this, () => MonsterConfig.OwnerSkills));
            addComponent(new VFXComp(this));
        }
    }
}