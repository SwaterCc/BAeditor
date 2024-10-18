namespace Hono.Scripts.Battle {
	public class PawnLogic : ActorLogic
	{

		private PawnControlInput _pawnControlInput;
		
		public PawnLogic(Actor actor) : base(actor) {
			PawnLogicRow = ConfigManager.Table<PawnLogicTable>().Get(Actor.ConfigId);
		}
		
		public PawnLogicTable.PawnLogicRow PawnLogicRow { get; }
		
		private float _baseSpeed;

		protected override void setupAttrs() {

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
		
		protected override void setupInput()
		{
			_pawnControlInput = new PawnControlInput(this);;
			_actorInput = _pawnControlInput;
		}

		protected override void setupComponents() {
			addComponent(new AttrSimpleProgress(this));
			addComponent(new SkillComp(this, () => PawnLogicRow.OwnerSkills));
			addComponent(new BuffComp(this,() => PawnLogicRow.OwnerBuffs));
			addComponent(new MotionComp(this));
			addComponent(new BeHurtComp(this));
			addComponent(new VFXComp(this));
			addComponent(new HateComp(this));
			addComponent(new MpComp(this));
		}
		
		protected override void onTick(float dt) {
			//临时处理移速的属性BUFF效果
			var finalMoveSpeed = _baseSpeed * (Actor.GetAttr<int>(ELogicAttr.AttrMoveSpeedPCT) / 10000f);
			Actor.SetAttr(ELogicAttr.AttrBaseSpeed, finalMoveSpeed, false);
		}
		
		protected override void setupStateMachine()
		{
			_stateMachine = new ActorStateMachine(this);
		}
	}
}