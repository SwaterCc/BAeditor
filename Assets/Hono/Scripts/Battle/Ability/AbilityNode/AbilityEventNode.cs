
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
	            _context.UpdateContext((_executor.Ability.Actor, _executor.Ability));
	            if (!string.IsNullOrEmpty(NodeData.EventNodeData.CaptureVarName)) {
		            _executor.Ability.Variables.Set(NodeData.EventNodeData.CaptureVarName, eventInfo);
	            }
                   
	            _executor.ExecuteNode(NodeData.ChildrenIds[0]);
	            _executor.Ability.Variables.Delete(NodeData.EventNodeData.CaptureVarName);
	            _context.ClearContext();
            }

            public override void DoJob() { }
        }
    }
}