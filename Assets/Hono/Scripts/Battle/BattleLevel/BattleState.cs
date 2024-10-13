﻿using Unity.VisualScripting;

namespace Hono.Scripts.Battle
{
    public partial class BattleGround
    {
        public abstract class BattleState
        {
            public EBattleStateType StateType { get; }

            public BattleGround BattleGroundHandle { get; }

            protected BattleState(BattleGround battleGroundHandle, EBattleStateType stateType)
            {
                StateType = stateType;
                BattleGroundHandle = battleGroundHandle;
            }

            public void Enter()
            {
                onEnter();
            }

            protected abstract void onEnter();


            public void Tick(float dt)
            {
                onTick(dt);
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