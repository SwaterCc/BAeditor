namespace Hono.Scripts.Battle
{
    public partial class BattleLevelRoot
    {
        public abstract class RoundState
        {
            protected Round _round;
            protected RoundState(Round round)
            {
                _round = round;
            }
            
            public void Enter()
            {
                
            }

            public void Tick(float dt)
            {
                
            }
            
            public void Exit()
            {
                
            }
        }
    }
}