

using Hono.Scripts.Battle.Event;

namespace Hono.Scripts.Battle
{
    public class AbilityRuntimeContext
    {
	    public Actor SourceActor { get; private set; }
	    public Ability Invoker { get; private set; }

        public IEventInfo EventInfo;

        public void UpdateContext(Ability context)
        {
	        SourceActor = context.Actor;
	        Invoker = context;
        }

        public void ClearContext()
        {
	        SourceActor = null;
	        Invoker = null;
        }
    }
}