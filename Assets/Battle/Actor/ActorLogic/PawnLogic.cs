namespace Battle
{
    public class PawnLogic : ActorLogic
    {
        public PawnLogic(int configId) : base(configId) { }
        protected override void onInit() { }

        protected override void registerChildComp()
        {
            addComponent(new BeHurtComp(this));
        }

        protected override void onTick(float dt)
        {
           
        }
    }
}