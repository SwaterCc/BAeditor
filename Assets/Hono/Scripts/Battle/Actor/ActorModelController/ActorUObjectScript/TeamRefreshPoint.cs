namespace Hono.Scripts.Battle
{
    public class TeamRefreshPoint : SceneActorModel 
    {
        public int TeamId;
        protected override void onInit()
        {
            ActorType = EActorType.TeamRefreshPoint;
        }

        public override void OnModelSetupFinish(Actor actor)
        {
            actor.Variables.Set("TeamId",TeamId);
        }
    }
}