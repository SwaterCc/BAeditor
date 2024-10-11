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

            private RoundState _roundState;

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
                _roundState = null;
                Controller = controller;
                _roundStates = new()
                {
                    {ERoundState.Ready,new RoundReadyState(this)},
                    {ERoundState.Running,new RoundRunningState(this)},
                    {ERoundState.SuccessScoring,new RoundSuccessScoringState(this)},
                    {ERoundState.FailedScoring,new RoundFailedScoringState(this)},
                };
            }
                
            public void BeginNewRound(RoundData roundData)
            {
                RoundData = roundData;
                _roundState = null;
                _curStage = ERoundState.Ready;
            }
            
            public void OnTick(float dt)
            {
                if(_curStage == ERoundState.NoActive) return;

                if (_curStage == ERoundState.Ready && _roundState == null)
                {
                    _roundState = _roundStates[_curStage]; 
                    _roundState.Enter();
                }
                
                _roundState.Tick(dt);
                if (_roundState.IsAutoEnd())
                {
                    _curStage = _roundState.GetNextState();
                    _roundState.Exit();

                    if (_curStage != ERoundState.NoActive)
                    {
                        _roundState = _roundStates[_curStage];
                        _roundState.Enter();
                    }
                }
            }
        }
    }
}