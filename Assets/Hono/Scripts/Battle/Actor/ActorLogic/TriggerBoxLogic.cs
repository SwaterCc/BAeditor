namespace Hono.Scripts.Battle
{
    public class TriggerBoxLogic : ActorLogic
    {
        public TriggerBoxLogic(Actor actor) : base(actor) { }

        protected override void onInit()
        {
            if (Actor.ModelController.Model.TryGetComponent<TriggerBoxModel>(out var triggerBoxModel))
            {
                foreach (var abilityId in triggerBoxModel.AbilityIds)
                {
                    Actor.AwardAbility(abilityId, true);
                }
            }
        }
    }
}