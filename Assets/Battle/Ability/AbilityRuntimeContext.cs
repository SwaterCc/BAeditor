using Battle.Event;

namespace Battle
{
    public class AbilityRuntimeContext
    {
        private BattleLevel _belongBattleLevel;
        public BattleLevel BelongBattleLevel => _belongBattleLevel;
        
        private ActorLogic _belongActor;
        public ActorLogic BelongActor => _belongActor;

        private Ability _curAbility;
        public Ability CurrentAbility => _curAbility;

        public IEventInfo EventInfo;

        public bool IsNotRunning => _belongActor == null || _curAbility == null;

        public void UpdateContext((ActorLogic,Ability) context)
        {
            _belongActor = context.Item1;
            _curAbility = context.Item2;
        }

        public void ClearContext()
        {
            _belongBattleLevel = null;
            _belongActor = null;
            _curAbility = null;
        }
    }
}