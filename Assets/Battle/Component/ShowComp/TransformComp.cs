namespace Battle
{
    public class TransformComp : AShowComponent
    {
        public TransformComp(ActorShow show) : base(show) { }
        public override void Init()
        {
            
        }

        protected override void onUpdate(float dt)
        {
            
        }

        public override EShowComponentType GetCompType()
        {
            return EShowComponentType.Transform;
        }

       
    }
}