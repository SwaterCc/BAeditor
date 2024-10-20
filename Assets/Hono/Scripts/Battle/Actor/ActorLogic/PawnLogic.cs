namespace Hono.Scripts.Battle
{
    public class PawnLogic : ActorLogic, IPoolObject
    {
        private SkillComp _skillComp;
        private BuffComp _buffComp;

        private PawnControlInput _pawnControlInput;

        public PawnLogicTable.PawnLogicRow PawnLogicRow { get; private set; }

        private float _baseSpeed;

        protected override void OnInit()
        {
            PawnLogicRow = ConfigManager.Table<PawnLogicTable>().Get(Actor.ConfigId);
            //学技能
            foreach (var skillInfo in PawnLogicRow.OwnerSkills)
            {
                _skillComp.LearnSkill(skillInfo[0], skillInfo[1]);
            }

            //初始buff
            foreach (var buff in PawnLogicRow.OwnerBuffs)
            {
                _buffComp.AddBuff(Uid, buff, 1);
            }
        }

        protected override void setupAttrs()
        {
            SetAttr(ELogicAttr.AttrFaction, PawnLogicRow.Faction, false);
            SetAttr(ELogicAttr.AttrModelId, PawnLogicRow.ModelId, false);

            var attrRow = ConfigManager.Table<EntityAttrBaseTable>().Get(PawnLogicRow.AttrTemplateId);
            _baseSpeed = attrRow.AttrBaseSpeed;
            SetAttr(ELogicAttr.AttrBaseSpeed, attrRow.AttrBaseSpeed, false);
            SetAttr(ELogicAttr.AttrMoveSpeedPCTAdd, attrRow.AttrMoveSpeedPCTAdd, false);
            SetAttr(ELogicAttr.AttrHp, attrRow.AttrMaxHpAdd, false);
            SetAttr(ELogicAttr.AttrMaxHpAdd, attrRow.AttrMaxHpAdd, false);
            SetAttr(ELogicAttr.AttrMp, attrRow.AttrMaxMpAdd, false);
            SetAttr(ELogicAttr.AttrMaxMpAdd, attrRow.AttrMaxMpAdd, false);
            SetAttr(ELogicAttr.AttrAttackAdd, attrRow.AttrAttackAdd, false);
            SetAttr(ELogicAttr.AttrCritAdd, attrRow.AttrCritAdd, false);
            SetAttr(ELogicAttr.AttrDefenseAdd, attrRow.AttrDefenseAdd, false);
            SetAttr(ELogicAttr.AttrHealAdd, attrRow.AttrHealAdd, false);
            SetAttr(ELogicAttr.AttrEntityLevel, attrRow.AttrEntityLevel, false);
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
        }

        protected override void constructInput()
        {
            _pawnControlInput = new PawnControlInput(this);
            ;
            _actorInput = _pawnControlInput;
        }

        protected override void constructComponents()
        {
            _skillComp = addComponent(new SkillComp(this));
            _buffComp = addComponent(new BuffComp(this));
            addComponent(new AttrSimpleProgress(this));
            addComponent(new MotionComp(this));
            addComponent(new BeHurtComp(this));
            addComponent(new VFXComp(this));
            addComponent(new HateComp(this));
            addComponent(new MpComp(this));
        }

        protected override void onTick(float dt)
        {
            //临时处理移速的属性BUFF效果
            var finalMoveSpeed = _baseSpeed * (Actor.GetAttr<int>(ELogicAttr.AttrMoveSpeedPCT) / 10000f);
            Actor.SetAttr(ELogicAttr.AttrBaseSpeed, finalMoveSpeed, false);
        }

        protected override void RecycleSelf()
        {
            AObjectPool<PawnLogic>.Pool.Recycle(this);
        }

        protected override void constructStateMachine()
        {
            _stateMachine = new ActorStateMachine(this);
        }

        public void OnRecycle() { }
    }
}