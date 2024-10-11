using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;

namespace Hono.Scripts.Battle
{
    public partial class BattleLevelRoot
    {
        /// <summary>
        /// 战斗轮次 所有资源准备好以后就会进入轮次的准备阶段，准备阶段判定结束后，进入运行期，运行期可以配置静态的刷怪，触发器启动，
        /// </summary>
        public class Round
        {
            /// <summary>
            /// 当前阶段
            /// </summary>
            private ERoundState _curStage;

            /// <summary>
            /// 持续时间
            /// </summary>
            private float _duration;

            /// <summary>
            /// 波次状态
            /// </summary>
            private Dictionary<ERoundState, RoundState> _roundStates = new();
            
            public void BeginNewRound(RoundData roundData)
            {
                _duration = 0;
                _curStage = ERoundState.Ready;
                _stageFunc = Ready;
            }
            
            public void OnTick(float dt)
            {
                if(_curStage == ERoundState.NoActive) return;
                
                _stageFunc.Invoke(dt);
            }


            private void Ready(float dt)
            {
                
                
                if(r)
            }
            
            private void Running(float dt)
            {
                
            }
            
            private void Score()
            {
                
            }
        }
    }
}