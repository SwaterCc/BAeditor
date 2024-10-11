using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;

namespace Hono.Scripts.Battle
{
    public partial class BattleLevelController
    {
        /// <summary>
        /// 战斗轮次 所有资源准备好以后就会进入轮次的准备阶段，准备阶段判定结束后，进入运行期，运行期可以配置静态的刷怪，触发器启动，
        /// </summary>
        private class Round
        {
            /// <summary>
            /// 当前阶段
            /// </summary>
            private ERoundState _curStage;

            public RoundData RoundData { get; private set; }

            /// <summary>
            /// 波次状态
            /// </summary>
            private readonly Dictionary<ERoundState, RoundState> _roundStates;

            /// <summary>
            /// 关卡控制器
            /// </summary>
            public BattleLevelController Controller { get; }

            public Round(BattleLevelController controller)
            {
                Controller = controller;
                _roundStates = new()
                {
                    {ERoundState.Ready,new RoundReadyState(this)},
                    {ERoundState.Running,new RoundReadyState(this)},
                    {ERoundState.Scoring,new RoundReadyState(this)},
                };
            }
                
            public void BeginNewRound(RoundData roundData)
            {
                RoundData = roundData;
                _curStage = ERoundState.Ready;
                _roundStates[_curStage].Enter();
            }
            
            public void OnTick(float dt)
            {
                if(_curStage == ERoundState.NoActive) return;
                var state = _roundStates[_curStage];
                state.Tick(dt);
                if (state.IsAutoEnd())
                {
                    _curStage = state.GetNextState();
                    state.Exit();

                    if (_curStage != ERoundState.NoActive)
                    {
                        _roundStates[_curStage].Enter();
                    }
                }
            }
        }
    }
}