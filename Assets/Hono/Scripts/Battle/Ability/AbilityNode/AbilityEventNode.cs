using Hono.Scripts.Battle.Event;
using Hono.Scripts.Battle.Tools;

namespace Hono.Scripts.Battle
{
    public partial class Ability
    {
        private class AbilityEventNode : AbilityNode
        {
            private EventChecker _checker;

            private new EventNodeData _data;

            public AbilityEventNode(AbilityExecutor executor, AbilityNodeData data) : base(executor, data) { }

            public void RegisterEvent()
            {
                if (!_data.CreateCheckerFunc.ParseParameters(out var auto))
                    return;
                _checker = auto.GetRef<EventChecker>();
                _checker.BindFunc(onEventFired);
                BattleEventManager.Instance.Register(_checker);
            }

            public void UnRegisterEvent()
            {
                _checker.UnRegister();
            }

            private void onEventFired(IEventInfo eventInfo)
            {
                _context.UpdateContext((_executor.Ability.Actor, _executor.Ability));
                eventInfo.PushInVarCollection(_executor.Ability.Variables);
                DoChildrenJob();
                eventInfo.ClearInVarCollection(_executor.Ability.Variables);
                _context.ClearContext();
            }

            public override void DoJob() { }
        }
    }
}