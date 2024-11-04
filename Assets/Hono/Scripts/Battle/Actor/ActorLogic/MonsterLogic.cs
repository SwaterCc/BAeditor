namespace Hono.Scripts.Battle
{
    public class MonsterLogic : ActorLogic
    {
        public MonsterLogic(Actor actor) : base(actor)
        {
            MonsterConfig = ConfigManager.Table<MonsterLogicTable>().Get(actor.ConfigId);
        }

        private float _baseSpeed;
        private float _maxHpGrowUpFactor = 0.1f;
        private float _maxAtkGrowUpFactor = 0.15f;
        private float _maxDefGrowUpFactor = 0.05f;

        public MonsterLogicTable.MonsterLogicRow MonsterConfig { get; }

        protected override void setupAttrs()
        {
            SetAttr(ELogicAttr.AttrModelId, MonsterConfig.ModelId, false);
            SetAttr(ELogicAttr.AttrFaction, MonsterConfig.Faction, false);

            var attrRow = ConfigManager.Table<EntityAttrBaseTable>().Get(MonsterConfig.AttrTemplateId);
            SetAttr(ELogicAttr.AttrEntityLevel, attrRow.AttrEntityLevel, false);
            _baseSpeed = attrRow.AttrBaseSpeed;
            SetAttr(ELogicAttr.AttrBaseSpeed, attrRow.AttrBaseSpeed, false);
            SetAttr(ELogicAttr.AttrMoveSpeedPCTAdd, attrRow.AttrMoveSpeedPCTAdd, false);
            var _baseMaxHp = GetAttrWithFactor(attrRow.AttrMaxHpAdd, attrRow.AttrEntityLevel, _maxHpGrowUpFactor);
            SetAttr(ELogicAttr.AttrHp, _baseMaxHp, false);
            SetAttr(ELogicAttr.AttrMaxHpAdd, _baseMaxHp, false);
            //SetAttr(ELogicAttr.AttrMp, attrRow.AttrMaxMpAdd, false);
            SetAttr(ELogicAttr.AttrMaxMpAdd, attrRow.AttrMaxMpAdd, false);
            var _baseAttack = GetAttrWithFactor(attrRow.AttrAttackAdd, attrRow.AttrEntityLevel, _maxAtkGrowUpFactor);
            SetAttr(ELogicAttr.AttrAttackAdd, _baseAttack, false);
            SetAttr(ELogicAttr.AttrCritAdd, attrRow.AttrCritAdd, false);
            var _baseDefense = (attrRow.AttrEntityLevel * 5) + attrRow.AttrDefenseAdd;
            SetAttr(ELogicAttr.AttrDefenseAdd, _baseDefense, false);
            SetAttr(ELogicAttr.AttrHealAdd, attrRow.AttrHealAdd, false);
            SetAttr(ELogicAttr.AttrHealedAdd, attrRow.AttrHealedAdd, false);
            SetAttr(ELogicAttr.AttrCritDamageAdd, attrRow.AttrCritDamageAdd, false);
            SetAttr(ELogicAttr.AttrDmgAAdd, attrRow.AttrDmgAAdd, false);
            SetAttr(ELogicAttr.AttrDmgRedAdd, attrRow.AttrDmgRedAdd, false);
            SetAttr(ELogicAttr.AttrHealIntensityAdd, attrRow.AttrHealIntensityAdd, false);
            SetAttr(ELogicAttr.AttrIgnoreDefenseAdd, attrRow.AttrIgnoreDefenseAdd, false);
            SetAttr(ELogicAttr.AttrAttackSpeedPCTAdd, attrRow.AttrAttackSpeedPCTAdd, false);
            SetAttr(ELogicAttr.AttrElementPenPCTAdd, attrRow.AttrElementPenPCTAdd, false);
            SetAttr(ELogicAttr.AttrElementMagicRedPCTAdd, attrRow.AttrElementMagicRedPCTAdd, false);
            SetAttr(ELogicAttr.AttrElementPhysicalPenPCTAdd, attrRow.AttrElementPhysicalPenPCTAdd, false);
            SetAttr(ELogicAttr.AttrElementPhysicalRedPCTAdd, attrRow.AttrElementPhysicalRedPCTAdd, false);
            SetAttr(ELogicAttr.AttrMpRecAllAdd, attrRow.AttrMpRecAllAdd, false);
            SetAttr(ELogicAttr.AttrMpRecAllPer, attrRow.AttrMpRecAllPer, false);
        }

        protected override void onInit()
        {
            foreach (var tag in MonsterConfig.TagList)
            {
                Actor.AddTag(tag);
            }
        }

        protected override void setupComponents()
        {
            addComponent(new AttrSimpleProgress(this));
            addComponent(new BeHurtComp(this));
            addComponent(new BuffComp(this, () => MonsterConfig.OwnerBuffs));
            addComponent(new SkillComp(this, () => MonsterConfig.OwnerSkills));
            addComponent(new VFXComp(this));
            addComponent(new MotionComp(this));
            addComponent(new HateComp(this));
            addComponent(new MpComp(this));
        }

        protected override void onTick(float dt)
        {
            var finalMoveSpeed = _baseSpeed * (Actor.GetAttr<int>(ELogicAttr.AttrMoveSpeedPCT) / 10000f);
            Actor.SetAttr<float>(ELogicAttr.AttrBaseSpeed, finalMoveSpeed, false);
        }

        static int GetAttrWithFactor(int startValue, int level, float factor)
        {
            int currentValue = startValue;
            for (int i = 1; i <= level - 1; i++)
            {
                currentValue += (int)(currentValue * factor);
            }

            return currentValue;
        }

        protected override void setupStateMachine()
        {
            _stateMachine = new ActorStateMachine(this);
        }

        protected override void setupInput()
        {
            _actorInput = new AutoInput(this);
        }
    }
}