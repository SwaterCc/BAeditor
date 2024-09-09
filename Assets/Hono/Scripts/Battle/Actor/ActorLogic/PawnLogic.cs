namespace Hono.Scripts.Battle
{
    public class PawnLogic : ActorLogic
    {
        public PawnLogic(int uid, ActorLogicTable.ActorLogicRow logicData) : base(uid, logicData) { }

        protected override void initAttrs()
        {
            SetAttr<float>(ELogicAttr.AttrBaseSpeed, 10, false);
            SetAttr<int>(ELogicAttr.AttrAttack, 500, false);
            SetAttr<int>(ELogicAttr.AttrFaction, 1,false);
        }

        protected override void onInit() { }

        protected override void registerChildComp()
        {
            addComponent(new SkillComp(this));
            addComponent(new BuffComp(this));
            addComponent(new DragMoveComp(this));
            addComponent(new BeHurtComp(this));
        }

        protected override void onTick(float dt) { }
    }
}