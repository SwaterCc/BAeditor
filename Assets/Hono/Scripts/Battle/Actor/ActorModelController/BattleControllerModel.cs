namespace Hono.Scripts.Battle
{
    public class BattleControllerModel : ActorModelController
    {
        public BattleControllerModel(Actor actor) : base(actor) { }

        protected override ModelSetup getModelSetup()
        {
            return new PreLoadModelSetup(EPreLoadGameObjectType.BattleRootModel);
        }
    }
}