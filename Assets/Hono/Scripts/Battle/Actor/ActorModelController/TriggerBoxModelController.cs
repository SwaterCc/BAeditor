#region

using Hono.Scripts.Battle.Event;

#endregion

namespace Hono.Scripts.Battle
{
    public class TriggerBoxModelController : SimpleSceneModelController
    {
        private readonly TriggerBoxEventInfo _eventInfo;
        private bool _isActive;

        public TriggerBoxModelController(Actor actor, SceneActorModel sceneActorModel) : base(actor, sceneActorModel)
        {
            _eventInfo = new TriggerBoxEventInfo();
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
    }
}