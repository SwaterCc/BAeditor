namespace Hono.Scripts.Battle
{
    public class BuildingLogic : ActorLogic, IAPoolObject
    {
        public BuildingLogicTable.BuildingLogicRow BuildingConfig { get; set; }

        public BuildingLogic()
        {
            resetInput(new BuildingControlInput(this));
           

            addComponent(new BuffComp(this));
            addComponent(new SkillComp(this));
            addComponent(new VFXComp(this));
            addComponent(new BeHurtComp(this));
            addComponent(new MpComp(this));
        }

        protected override void setupAttrs()
        {
            if (BuildingConfig == null) return;

            SetAttr(EAttrType.AttrModelId, BuildingConfig.Model, false);
            SetAttr(EAttrType.AttrFaction, BuildingConfig.Faction, false);

            var attrRow = ConfigManager.Table<EntityAttrBaseTable>().Get(BuildingConfig.AttrTemplateId);

            SetAttr(EAttrType.AttrBaseSpeed, (int)attrRow.AttrBaseSpeed, false);
            SetAttr(EAttrType.AttrMoveSpeedPCTAdd, attrRow.AttrMoveSpeedPCTAdd, false);
            SetAttr(EAttrType.AttrHp, attrRow.AttrMaxHpAdd, false);
            SetAttr(EAttrType.AttrMaxHpAdd, attrRow.AttrMaxHpAdd, false);
            //SetAttr(ELogicAttr.AttrMp, attrRow.AttrMaxMpAdd, false);
            SetAttr(EAttrType.AttrMaxMpAdd, attrRow.AttrMaxMpAdd, false);
            SetAttr(EAttrType.AttrAttackAdd, attrRow.AttrAttackAdd, false);
            SetAttr(EAttrType.AttrCritAdd, attrRow.AttrCritAdd, false);
            SetAttr(EAttrType.AttrDefenseAdd, attrRow.AttrDefenseAdd, false);
            SetAttr(EAttrType.AttrHealAdd, attrRow.AttrHealAdd, false);
            SetAttr(EAttrType.AttrEntityLevel, attrRow.AttrEntityLevel, false);
            SetAttr(EAttrType.AttrHealedAdd, attrRow.AttrHealedAdd, false);
            SetAttr(EAttrType.AttrCritDamageAdd, attrRow.AttrCritDamageAdd, false);
            SetAttr(EAttrType.AttrDmgAAdd, attrRow.AttrDmgAAdd, false);
            SetAttr(EAttrType.AttrDmgRedAdd, attrRow.AttrDmgRedAdd, false);
            SetAttr(EAttrType.AttrHealIntensityAdd, attrRow.AttrHealIntensityAdd, false);
            SetAttr(EAttrType.AttrIgnoreDefenseAdd, attrRow.AttrIgnoreDefenseAdd, false);
            SetAttr(EAttrType.AttrAttackSpeedPCTAdd, attrRow.AttrAttackSpeedPCTAdd, false);
            SetAttr(EAttrType.AttrElementPenPCTAdd, attrRow.AttrElementPenPCTAdd, false);
            SetAttr(EAttrType.AttrElementMagicRedPCTAdd, attrRow.AttrElementMagicRedPCTAdd, false);
            SetAttr(EAttrType.AttrElementPhysicalPenPCTAdd, attrRow.AttrElementPhysicalPenPCTAdd, false);
            SetAttr(EAttrType.AttrElementPhysicalRedPCTAdd, attrRow.AttrElementPhysicalRedPCTAdd, false);
        }

        protected override void onInit()
        {
            foreach (var tag in BuildingConfig.TagList)
            {
                Self.TagCollection.Add(tag);
            }

            var skillComp = GetComponent<SkillComp>();
            foreach (var skillInfo in BuildingConfig.ownerSkills)
            {
                skillComp.LearnSkill(skillInfo[0], skillInfo[1]);
            }

            var buffComp = GetComponent<BuffComp>();
            foreach (var buffId in BuildingConfig.OwnerBuffs)
            {
                buffComp.AddBuff(buffId, 1);
            }
        }

        public override void RecycleLogicObject()
        {
            APool<BuildingLogic>.Pool.Recycle(this);
        }
    }
}