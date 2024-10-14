namespace Hono.Scripts.Battle
{
    public class TeamDefaultBrithPoint : SceneActorModel
    {
        public int TeamId;
        
        protected override void onInit()
        {
            ActorType = EActorType.TeamDefaultBirthPoint;
        }
        
        public override void OnModelSetupFinish(Actor actor)
        {
            actor.Variables.Set("TeamId",TeamId);
        }
    }
}