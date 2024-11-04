#region

using Hono.Scripts.Battle.Event;

#endregion

namespace Hono.Scripts.Battle
{
    public class ActorRefreshPointLogic : ActorLogic
    {
        private int _bindActorUid;
        private RoundStateChecker _roundStateChecker;

        public ActorRefreshPointLogic(Actor actor) : base(actor)
        {
            _roundStateChecker = new RoundStateChecker(EBattleEventType.RoundReadyEnter, onRoundReadyEnter);
        }

        protected override void onInit()
        {
            BattleEventManager.Instance.Register(_roundStateChecker);
        }

        private void onRoundReadyEnter(IEventInfo info)
        {
            if (ActorManager.Instance.GetActorRtState(_bindActorUid) != EActorRunningState.NotExist)
            {
                return;
            }

            if (!Actor.ModelController.Model.TryGetComponent<ActorRefreshPoint>(out var refreshPoint))
            {
                return;
            }

            _bindActorUid = ActorManager.Instance.CreateActor(refreshPoint.CreateActorType, refreshPoint.ConfigId,
                (actor) =>
                {
                    actor.SetAttr(ELogicAttr.AttrPosition, refreshPoint.transform.position, false);
                    actor.SetAttr(ELogicAttr.AttrOriginPos, refreshPoint.transform.position, false);
                    actor.SetAttr(ELogicAttr.AttrRot, refreshPoint.transform.rotation, false);
                    actor.OnDestroyCallBack += BattleManager.CurBattle.RtInfo.OnActorDead;
                });
        }

        protected override void onDestroy()
        {
            BattleEventManager.Instance.UnRegister(_roundStateChecker);
        }
    }
}