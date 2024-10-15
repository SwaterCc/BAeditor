using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Hono.Scripts.Battle
{
    public partial class BattleGround
    {
        /// <summary>
        /// 战斗轮次 所有资源准备好以后就会进入轮次的准备阶段，准备阶段判定结束后，进入运行期，运行期可以配置静态的刷怪，触发器启动，
        /// </summary>
        private class RoundController
        {
            /// <summary>
            /// 当前状态
            /// </summary>
            private RoundState _currentRoundState;

            /// <summary>
            /// 当前状态
            /// </summary>
            private ERoundState _currentState;

            public ERoundState CurrentState => _currentState;

            /// <summary>
            /// 下一状态
            /// </summary>
            private ERoundState _nextState;

            /// <summary>
            /// 所有回合数据
            /// </summary>
            private List<RoundData> _allRoundData;

            public RoundData CurrentRoundData => _allRoundData[_roundIndex];

            /// <summary>
            /// 波次状态
            /// </summary>
            private readonly Dictionary<ERoundState, RoundState> _roundStates;

            /// <summary>
            /// 战场
            /// </summary>
            public GameRunningState GameRunningState { get; }

            /// <summary>
            /// 当前回合Index
            /// </summary>
            private int _roundIndex;

            /// <summary>
            /// 当前是否为最后一轮
            /// </summary>
            public bool IsFinalRound => _roundIndex >= _allRoundData.Count - 1;

            /// <summary>
            /// 是否初始化下个阶段的数据
            /// </summary>
            private bool _isInitNextRound;

            /// <summary>
            /// 失败后可否重开
            /// </summary>
            public bool CanRepeat { get; private set; }

            public RoundController(GameRunningState gameRunningState)
            {
                GameRunningState = gameRunningState;
                _roundStates = new()
                {
                    { ERoundState.NoRunning, new RoundNoRunningState(this) },
                    { ERoundState.Ready, new RoundReadyState(this) },
                    { ERoundState.Running, new RoundRunningState(this) },
                    { ERoundState.SuccessScoring, new RoundSuccessScoringState(this) },
                    { ERoundState.FailedScoring, new RoundFailedScoringState(this) },
                };

                _currentState = ERoundState.NoRunning;
                _nextState = ERoundState.NoRunning;
                _currentRoundState = _roundStates[_currentState];
                _currentRoundState.Enter();
            }

            /// <summary>
            /// 初始化回合控制器
            /// </summary>
            /// <param name="roundDatas"></param>
            /// <param name="canRepeat"></param>
            /// <returns></returns>
            public bool InitRoundController(in List<RoundData> roundDatas, bool canRepeat)
            {
                if (roundDatas.Count == 0)
                {
                    Debug.LogError("回合数据是空的");
                    return false;
                }

                _allRoundData = roundDatas;
                _roundIndex = 0;
                CanRepeat = canRepeat;

                return true;
            }
            
            public void SwitchState(ERoundState nextState)
            {
                Debug.Log($"[RoundState] switchState Next {nextState}");
                _nextState = nextState;
            }

            public void RoundCountAdd()
            {
                _roundIndex++;
                Debug.Log($"[RoundState] RoundCountAdd NextRound {_roundIndex}");
            }

            public void Tick(float dt)
            {
                _currentRoundState.Tick(dt);
                
                if (_currentState != _nextState)
                {
                    _currentRoundState.Exit();
                    _currentRoundState = _roundStates[_nextState];
                    _currentState = _nextState;
                    _currentRoundState.Enter();
                }
            }
        }
    }
}