using Hono.Scripts.Battle.Event;
using UnityEngine;

namespace Hono.Scripts.Battle
{
    public class TriggerBoxModelController : ActorModelController, IPoolObject
    {
        private readonly TriggerBoxEventInfo _eventInfo = new();

        private readonly SceneModelSetup _sceneModelSetup = new();

        private bool _isActive;

        public void Init(Actor actor, SceneActorModel sceneActorModel)
        {
            base.Init(actor);
            _sceneModelSetup.Init(sceneActorModel);
        }

        protected override void onModelLoadComplete()
        {
            if (Model.TryGetComponent<TriggerBoxModel>(out var triggerBoxModel))
            {
                foreach (var abilityId in triggerBoxModel.AbilityIds)
                {
                    Actor.AwardAbility(abilityId, true);
                }
            }
        }

        public void SetActive(bool isActive)
        {
            _isActive = isActive;
        }

        public void OnTriggerEnter(int uid)
        {
            if (!_isActive) return;
            _eventInfo.TargetUid = uid;
            Actor.TriggerEvent(EBattleEventType.OnTriggerBoxEnter, _eventInfo);
        }

        public void OnTriggerStay(int uid)
        {
            if (!_isActive) return;
            _eventInfo.TargetUid = uid;
            Actor.TriggerEvent(EBattleEventType.OnTriggerBoxStay, _eventInfo);
        }

        public void OnTriggerExit(int uid)
        {
            if (!_isActive) return;
            _eventInfo.TargetUid = uid;
            Actor.TriggerEvent(EBattleEventType.OnTriggerBoxExit, _eventInfo);
        }

        protected override ModelSetup getModelSetup()
        {
            return _sceneModelSetup;
        }

        protected override void RecycleSelf()
        {
            AObjectPool<TriggerBoxModelController>.Pool.Recycle(this);
        }

        public void OnRecycle() { }
    }
}