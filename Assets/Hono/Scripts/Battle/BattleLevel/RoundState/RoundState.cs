using Hono.Scripts.Battle.Event;

namespace Hono.Scripts.Battle
{
    public partial class BattleLevelController
    {
        private abstract class RoundState
        {
            protected Round _round;
            protected float _stateDuration;

            protected RoundState(Round round)
            {
                _round = round;
            }

            public abstract ERoundState GetRoundState();

            public abstract ERoundState GetNextState();

            public abstract bool IsAutoEnd();

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
            protected abstract void onTick(float dt);

            public void Exit()
            {
                onExit();
            }
            protected abstract void onExit();
        }
    }
}