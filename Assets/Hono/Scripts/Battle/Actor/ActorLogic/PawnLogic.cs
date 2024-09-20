namespace Hono.Scripts.Battle
{
    public class PawnLogic : ActorLogic
    {
        public PawnLogic(Actor actor, ActorLogicTable.ActorLogicRow logicData) : base(actor, logicData) { }

        protected override void setupAttrs()
        {
            SetAttr<float>(ELogicAttr.AttrBaseSpeed, 10, false);
            SetAttr<int>(ELogicAttr.AttrFaction, 1,false);
          
            
            var attrRow = ConfigManager.Instance.GetTable<EntityAttrBaseTable>().Get(LogicData.AttrTemplateId);
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

        protected override void onInit() { }

        protected override void registerChildComponents()
        {
            addComponent(new SkillComp(this));
            addComponent(new BuffComp(this));
            addComponent(new MotionComp(this));
            addComponent(new BeHurtComp(this));
            addComponent(new AttrCastLv1(this));
        }

        protected override void onTick(float dt) { }
    }
}