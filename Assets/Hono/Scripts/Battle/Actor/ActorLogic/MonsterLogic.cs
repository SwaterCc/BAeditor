namespace Hono.Scripts.Battle
{
    public class MonsterLogic : ActorLogic
    {
        public MonsterLogic(Actor actor, ActorLogicTable.ActorLogicRow logicData) : base(actor, logicData) { }
        protected override void setupAttrs()
        {
            SetAttr<int>(ELogicAttr.AttrFaction, 4, false);
        }

        protected override void onInit()
        {
            
        }

        protected override void registerChildComponents()
        {
            addComponent(new BeHurtComp(this));
            addComponent(new BuffComp(this));
            addComponent(new SkillComp(this));
        }
    }
}