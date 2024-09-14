using System;

namespace Hono.Scripts.Battle
{
    public class MonsterLogic : ActorLogic
    {
        public MonsterLogic(Actor actor, ActorLogicTable.ActorLogicRow logicData) : base(actor, logicData) { }
        protected override void setupAttrs() {

	        var attrRow = ConfigManager.Instance.GetTable<EntityAttrBaseTable>().Get(LogicData.AttrTemplateId);
	        SetAttr(ELogicAttr.AttrMaxHpAdd, attrRow.AttrMaxHpAdd,false);
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
            SetAttr(ELogicAttr.AttrFaction, 4, false);
        }

        protected override void onInit()
        {
            
        }

        protected override void registerChildComponents()
        {
            addComponent(new BeHurtComp(this));
            addComponent(new BuffComp(this));
            addComponent(new SkillComp(this));
            addComponent(new AttrCastLv1(this));
        }
    }
}