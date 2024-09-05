
using Hono.Scripts.Battle.Event;
using Hono.Scripts.Battle.Tools;

namespace Hono.Scripts.Battle
{
    public partial class Ability
    {
        private class AbilityEventNode : AbilityNode
        {
            private EventChecker _checker;
            public AbilityEventNode(AbilityExecutor executor, AbilityNodeData data) : base(executor, data) { }

            public void RegisterEvent()
            {
                if (!NodeData.EventNodeData.CreateCheckerFunc.TryCallFunc(out var valueBox)) return;
                _checker = (EventChecker)valueBox;
                _checker.BindFunc(onEventFired);
                BattleEventManager.Instance.Register(_checker);
            }

            public void UnRegisterEvent()
            {
                _checker.UnRegister();
            }

            private void onEventFired(IEventInfo eventInfo)
            {
                var actor = ActorManager.Instance.GetActor(_executor.Ability.BelongActorId);
                if (actor != null)
                {
                    _context.UpdateContext((actor.Logic, _executor.Ability));
                    _executor.ExecuteNode(NodeData.ChildrenIds[0]);
                    _context.ClearContext();
                }
            }

            public override void DoJob() { }
        }
    }
}