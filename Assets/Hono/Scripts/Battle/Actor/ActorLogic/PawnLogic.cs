namespace Hono.Scripts.Battle {
	public class PawnLogic : ActorLogic {
		
		private readonly ActorInput _autoInput;

		private readonly ActorInput _manualInput;
		
		public PawnLogic(Actor actor) : base(actor) {
			PawnLogicRow = ConfigManager.Table<PawnLogicTable>().Get(Actor.ConfigId);
			_autoInput = new AutoInput(this);
			_manualInput = new ManualControlInput(this);
		}
		
		public PawnLogicTable.PawnLogicRow PawnLogicRow { get; }
		
		protected override void setupAttrs() {
			SetAttr(ELogicAttr.AttrBaseSpeed, 10f, false);
			SetAttr(ELogicAttr.AttrFaction, PawnLogicRow.Faction, false);
			SetAttr(ELogicAttr.AttrModelId, PawnLogicRow.ModelId, false);
			
			var attrRow = ConfigManager.Table<EntityAttrBaseTable>().Get(PawnLogicRow.AttrTemplateId);
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

		protected override void onInit() { }

		protected override void setupInput() {
			ActorInput = new AutoInput(this);
		}

		protected override void setupComponents() {
			addComponent(new AttrCastLv1(this));
			addComponent(new SkillComp(this, () => PawnLogicRow.OwnerSkills));
			addComponent(new BuffComp(this,() => PawnLogicRow.OwnerBuffs));
			addComponent(new MotionComp(this));
			addComponent(new BeHurtComp(this));
			addComponent(new VFXComp(this));
		}

		protected override void onTick(float dt)
		{
			if (Actor.IsPlayerControl)
			{
				ActorInput = _manualInput;
			}
			else
			{
				ActorInput = _autoInput;
			}
		}
	}
}