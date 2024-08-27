using Battle.Damage;

namespace Battle
{
    public class BeHurtComp : ALogicComponent
    {
        public BeHurtComp(ActorLogic logic) : base(logic) { }
        public override ELogicComponentType GetCompType()
        {
            return ELogicComponentType.BeHurtComp;
        }
        
        public override void Init()
        {
            throw new System.NotImplementedException();
        }
        

        protected override void onTick(float dt)
        {
            
        }

        public void OnBeHurt(DamageResults damageResults)
        {
            
        }
    }
}