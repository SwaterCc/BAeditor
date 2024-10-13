using Hono.Scripts.Battle.Event;

namespace Hono.Scripts.Battle
{
    public partial class BattleGround
    {
        private abstract class RoundState
        {
            protected RoundController Round { get; }
            
            private RoundData _roundData;
            protected RoundData CurrentRoundData => Round.CurrentRoundData;
            
            private float _stateDuration;
            protected float Duration => _stateDuration;

            protected RoundState(RoundController roundController)
            {
                Round = roundController;
            }

            public abstract ERoundState GetRoundState();
            
            public void Enter()
            {
                _stateDuration = 0;
                onEnter();
            }

            protected abstract void onEnter();

            public void Tick(float dt)
            {
                onTick(dt);
                _stateDuration += dt;
            }

            protected virtual void onTick(float dt) { }

            public void Exit()
            {
                onExit();
            }
            protected abstract void onExit();
        }
    }
}