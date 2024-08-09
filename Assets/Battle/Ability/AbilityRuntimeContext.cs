namespace Battle
{
    public class AbilityRuntimeContext
    {
        private BattleLevel _belongBattleLevel;
        
        private Actor _belongActor;

        private Ability _curAbility;


        public Actor GetActor()
        {
            return _belongActor;
        }

        public Ability GetAbility()
        {
            return _curAbility;
        }

        public bool IsNotRunning => _belongActor == null || _curAbility == null;

        public void UpdateContext((Actor,Ability) context)
        {
            _belongActor = context.Item1;
            _curAbility = context.Item2;
        }

        public void ClearContext()
        {
            _belongActor = null;
            _curAbility = null;
        }
    }
}