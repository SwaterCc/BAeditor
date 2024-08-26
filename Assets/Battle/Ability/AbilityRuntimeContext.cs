using Battle.Event;

namespace Battle
{
    public class AbilityRuntimeContext
    {
        private BattleLevel _belongBattleLevel;
        public BattleLevel BelongBattleLevel => _belongBattleLevel;

        private Actor _belongActor;
        public Actor BelongActor => _belongActor;
        
        private ActorLogic _belongLogic;
        public ActorLogic BelongLogic => _belongLogic;

        private Ability _curAbility;
        public Ability CurrentAbility => _curAbility;

        public IEventInfo EventInfo;

        public bool IsNotRunning => _belongLogic == null || _curAbility == null;

        public void UpdateContext((ActorLogic,Ability) context)
        {
            _belongLogic = context.Item1;
            _curAbility = context.Item2;
        }

        public void ClearContext()
        {
            _belongBattleLevel = null;
            _belongLogic = null;
            _curAbility = null;
        }
    }
}