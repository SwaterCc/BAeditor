namespace Hono.Scripts.Battle
{
    public class PawnLogic : ActorLogic
    {
        public PawnLogic(int uid, ActorLogicData logicData) : base(uid,logicData) { }
        protected override void initAttrs()
        {
            
        }

        protected override void onInit() { }

        protected override void registerChildComp()
        {
            addComponent(new SkillComp(this));
            addComponent(new BuffComp(this));
            addComponent(new DragMoveComp(this));
            addComponent(new BeHurtComp(this));
        }

        protected override void onTick(float dt)
        {
           
        }
    }
}