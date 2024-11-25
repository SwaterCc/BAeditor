namespace Hono.Scripts.Battle
{
    public class PawnLogic : ActorLogic, IAPoolObject
    {
        public PawnLogicTable.PawnLogicRow PawnLogicRow { get; set; }

        private float _baseSpeed;
        private float _maxHpGrowUpFactor = 0.1f;
        private float _maxAtkGrowUpFactor = 0.15f;
        private float _maxDefGrowUpFactor = 0.15f;

        public PawnLogic()
        {
            resetInput(new PawnControlInput(this));
            resetStateMachine(new ActorStateMachine(this));

            addComponent(new SkillComp(this));
            addComponent(new BuffComp(this));
            addComponent(new MotionComp(this));
            addComponent(new BeHurtComp(this));
            addComponent(new VFXComp(this));
            addComponent(new HateComp(this));
            addComponent(new MpComp(this));
        }

        protected override void setupAttrs()
        {
            Self.SetAttr(EAttrType.AttrFaction, PawnLogicRow.Faction, false);
            Self.SetAttr(EAttrType.AttrModelId, PawnLogicRow.ModelId, false);

            var attrRow = ConfigManager.Table<EntityAttrBaseTable>().Get(PawnLogicRow.AttrTemplateId);
            Self.SetAttr(EAttrType.AttrEntityLevel, attrRow.AttrEntityLevel, false);
            _baseSpeed = attrRow.AttrBaseSpeed;
            Self.SetAttr(EAttrType.AttrMoveSpeedPCTAdd, attrRow.AttrMoveSpeedPCTAdd, false);
            Self.SetAttr(EAttrType.AttrHp, attrRow.AttrMaxHpAdd, false);
            Self.SetAttr(EAttrType.AttrMaxHpAdd, attrRow.AttrMaxHpAdd, false);
            Self.SetAttr(EAttrType.AttrMaxMpAdd, attrRow.AttrMaxMpAdd, false);
            Self.SetAttr(EAttrType.AttrAttackAdd, attrRow.AttrAttackAdd, false);
            Self.SetAttr(EAttrType.AttrCritAdd, attrRow.AttrCritAdd, false);
            Self.SetAttr(EAttrType.AttrDefenseAdd, attrRow.AttrDefenseAdd, false);
            Self.SetAttr(EAttrType.AttrHealAdd, attrRow.AttrHealAdd, false);
            Self.SetAttr(EAttrType.AttrHealedAdd, attrRow.AttrHealedAdd, false);
            Self.SetAttr(EAttrType.AttrCritDamageAdd, attrRow.AttrCritDamageAdd, false);
            Self.SetAttr(EAttrType.AttrDmgAAdd, attrRow.AttrDmgAAdd, false);
            Self.SetAttr(EAttrType.AttrDmgRedAdd, attrRow.AttrDmgRedAdd, false);
            Self.SetAttr(EAttrType.AttrHealIntensityAdd, attrRow.AttrHealIntensityAdd, false);
            Self.SetAttr(EAttrType.AttrIgnoreDefenseAdd, attrRow.AttrIgnoreDefenseAdd, false);
            Self.SetAttr(EAttrType.AttrAttackSpeedPCTAdd, attrRow.AttrAttackSpeedPCTAdd, false);
            Self.SetAttr(EAttrType.AttrElementPenPCTAdd, attrRow.AttrElementPenPCTAdd, false);
            Self.SetAttr(EAttrType.AttrElementMagicRedPCTAdd, attrRow.AttrElementMagicRedPCTAdd, false);
            Self.SetAttr(EAttrType.AttrElementPhysicalPenPCTAdd, attrRow.AttrElementPhysicalPenPCTAdd, false);
            Self.SetAttr(EAttrType.AttrElementPhysicalRedPCTAdd, attrRow.AttrElementPhysicalRedPCTAdd, false);
            Self.SetAttr(EAttrType.AttrMpRecAllAdd, attrRow.AttrMpRecAllAdd, false);
            Self.SetAttr(EAttrType.AttrMpRecAllPer, attrRow.AttrMpRecAllPer, false);
            Self.SetAttr(EAttrType.AttrMpRecKilledAdd, attrRow.AttrMpRecKilledAdd, false);
            Self.SetAttr(EAttrType.AttrMpRecBehitPer, attrRow.AttrMpRecBehitPer, false);
        }

        protected override void onInit()
        {
            foreach (var tag in PawnLogicRow.TagList)
            {
                Self.TagCollection.Add(tag);
            }

            var skillComp = GetComponent<SkillComp>();
            foreach (var skillInfo in PawnLogicRow.OwnerSkills)
            {
                skillComp.LearnSkill(skillInfo[0], skillInfo[1]);
            }

            var buffComp = GetComponent<BuffComp>();
            foreach (var buffId in PawnLogicRow.OwnerBuffs)
            {
                buffComp.AddBuff(buffId, 1);
            }
        }

        protected override void onTick(float dt)
        {
            var finalMoveSpeed = _baseSpeed * (Self.GetAttr(EAttrType.AttrMoveSpeedPCT) / 10000f);
            Self.SetAttr(EAttrType.AttrBaseSpeed, (int)finalMoveSpeed);
        }

        public override void RecycleLogicObject()
        {
            AObjectPool<PawnLogic>.Pool.Recycle(this);
        }
    }
}