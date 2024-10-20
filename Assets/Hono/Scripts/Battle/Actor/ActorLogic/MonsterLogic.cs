using System;

namespace Hono.Scripts.Battle
{
    public class MonsterLogic : ActorLogic, IPoolObject
    {
        private SkillComp _skillComp;
        private BuffComp _buffComp;

        private float _baseSpeed;

        protected override void constructStateMachine()
        {
            _stateMachine = new ActorStateMachine(this);
        }

        protected override void constructInput()
        {
            _actorInput = new AutoInput(this);
        }

        protected override void constructComponents()
        {
            _buffComp = addComponent(new BuffComp(this));
            _skillComp = addComponent(new SkillComp(this));
            addComponent(new AttrSimpleProgress(this));
            addComponent(new BeHurtComp(this));
            addComponent(new VFXComp(this));
            addComponent(new MotionComp(this));
            addComponent(new HateComp(this));
        }

        public MonsterLogicTable.MonsterLogicRow MonsterConfig { get; private set; }

        protected override void setupAttrs()
        {
            SetAttr(ELogicAttr.AttrModelId, MonsterConfig.ModelId, false);
            SetAttr(ELogicAttr.AttrFaction, MonsterConfig.Faction, false);

            var attrRow = ConfigManager.Table<EntityAttrBaseTable>().Get(MonsterConfig.AttrTemplateId);
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

        protected override void OnInit()
        {
            MonsterConfig = ConfigManager.Table<MonsterLogicTable>().Get(Actor.ConfigId);
            //学技能
            foreach (var skillInfo in MonsterConfig.OwnerSkills)
            {
                _skillComp.LearnSkill(skillInfo[0], skillInfo[1]);
            }

            //初始buff
            foreach (var buff in MonsterConfig.OwnerBuffs)
            {
                _buffComp.AddBuff(Uid, buff, 1);
            }
        }

        protected override void onTick(float dt)
        {
            var finalMoveSpeed = _baseSpeed * (Actor.GetAttr<int>(ELogicAttr.AttrMoveSpeedPCT) / 10000f);
            Actor.SetAttr<float>(ELogicAttr.AttrBaseSpeed, finalMoveSpeed, false);
        }

        protected override void RecycleSelf()
        {
            AObjectPool<MonsterLogic>.Pool.Recycle(this);
        }

        public void OnRecycle() { }
    }
}