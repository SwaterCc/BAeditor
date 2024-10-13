using Hono.Scripts.Battle.Event;
using UnityEngine;

namespace Hono.Scripts.Battle
{
    public class TriggerBoxModelController : SimpleSceneModelController
    {
        private readonly TriggerBoxEventInfo _eventInfo;
        public TriggerBoxModelController(Actor actor, SceneActorModel sceneActorModel) : base(actor,sceneActorModel)
        {
            _eventInfo = new TriggerBoxEventInfo();
        }

        protected override void onModelLoadFinish()
        {
            if (Model.TryGetComponent<TriggerBoxModel>(out var triggerBoxModel))
            {
                foreach (var abilityId in triggerBoxModel.AbilityIds)
                {
                    Actor.AwardAbility(abilityId, true);
                }
            }
        }

        public void OnTriggerEnter(int uid)
        {
            _eventInfo. TargetUid = uid;
            Actor.TriggerEvent(EBattleEventType.OnTriggerBoxEnter,_eventInfo);
        }
        
        public void OnTriggerStay(int uid)
        {
            _eventInfo. TargetUid = uid;
            Actor.TriggerEvent(EBattleEventType.OnTriggerBoxStay,_eventInfo);
        }
        
        public void OnTriggerExit(int uid)
        {
            _eventInfo. TargetUid = uid;
            Actor.TriggerEvent(EBattleEventType.OnTriggerBoxExit,_eventInfo);
        }
    }
}