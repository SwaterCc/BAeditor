namespace Hono.Scripts.Battle
{
    public class PawnLogic : ActorLogic
    {
        public PawnLogic(Actor actor, ActorLogicTable.ActorLogicRow logicData) : base(actor, logicData) { }

        protected override void setupAttrs()
        {
            SetAttr<float>(ELogicAttr.AttrBaseSpeed, 10, false);
            SetAttr<int>(ELogicAttr.AttrAttack, 500, false);
            SetAttr<int>(ELogicAttr.AttrFaction, 1,false);
        }

        protected override void onInit() { }

        protected override void registerChildComponents()
        {
            addComponent(new SkillComp(this));
            addComponent(new BuffComp(this));
            addComponent(new DragMoveComp(this));
            addComponent(new BeHurtComp(this));
        }

        protected override void onTick(float dt) { }
    }
}