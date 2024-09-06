namespace Hono.Scripts.Battle
{
    public class MonsterLogic : ActorLogic
    {
        public MonsterLogic(int uid, ActorLogicData logicData) : base(uid, logicData) { }
        protected override void initAttrs()
        {
            SetAttr<int>(ELogicAttr.AttrFaction, 4, false);
        }

        protected override void onInit()
        {
            
        }

        protected override void registerChildComp()
        {
            addComponent(new BeHurtComp(this));
            addComponent(new BuffComp(this));
            addComponent(new SkillComp(this));
        }
    }
}