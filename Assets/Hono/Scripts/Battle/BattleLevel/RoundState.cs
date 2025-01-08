#region

using System.ComponentModel;
using Hono.Scripts.Battle.Event;

#endregion

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

                Round.GameRunningState.BattleGroundHandle.RtInfo.CurRoundState = GetRoundState();

                onEnter();

                if (GetRoundState() == ERoundState.FailedScoring)
                    Round.EventInfo.RoundScore = -1;
                if (GetRoundState() == ERoundState.SuccessScoring)
                    Round.EventInfo.RoundScore = 1;

                if (GetRoundState() != ERoundState.NoRunning)
                    EventManager.Instance.TriggerGlobalEvent(getEventType(true), Round.EventInfo);

                //Debug.Log($"[RoundState] RoundState Enter {GetRoundState()}");
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
                if (GetRoundState() != ERoundState.NoRunning)
                    EventManager.Instance.TriggerGlobalEvent(getEventType(false), Round.EventInfo);
                onExit();

                //Debug.Log($"[RoundState] RoundState Exit {GetRoundState()}");
            }

            protected abstract void onExit();


            private EBattleEventType getEventType(bool isEnter)
            {
                switch (GetRoundState())
                {
                    case ERoundState.Ready:
                        return isEnter ? EBattleEventType.RoundReadyEnter : EBattleEventType.RoundReadyExit;
                    case ERoundState.Running:
                        return isEnter ? EBattleEventType.RoundRunningEnter : EBattleEventType.RoundRunningExit;
                    case ERoundState.SuccessScoring:
                    case ERoundState.FailedScoring:
                        return isEnter ? EBattleEventType.RoundScoreEnter : EBattleEventType.RoundScoreExit;
                }

                throw new InvalidEnumArgumentException("找不到枚举");
            }
        }
    }
}