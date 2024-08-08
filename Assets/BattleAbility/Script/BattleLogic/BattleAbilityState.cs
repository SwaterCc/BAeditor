using System;
using System.Collections.Generic;
using Battle.Def;

namespace BattleAbility
{
    public partial class BattleAbilityBlock
    {
        /// <summary>
        /// 能力的状态控制
        /// </summary>
        public class BattleAbilityState
        {
            /// <summary>
            /// 当前绑定的能力的Id
            /// </summary>
            public BattleAbilityBlock BindAbilityBlock { get; }

            /// <summary>
            /// 默认阶段开始Id
            /// </summary>
            public int BeginStageId = 0;
            
            /// <summary>
            /// 当前运行的阶段Id，无运行阶段为-1
            /// </summary>
            public int CurStageId = -1;

            /// <summary>
            /// 下一阶段ID,没有设置则会在当前阶段播完后结束逻辑，默认为-1
            /// </summary>
            public int NextStageId = -1;
            
            /// <summary>
            /// 当前运行的阶段对象
            /// </summary>
            public BattleAbilityBlock.AbilityStage CurStage;

            /// <summary>
            /// CD
            /// </summary>
            public float AbilityCd;

            /// <summary>
            /// 能力当前处于什么状态
            /// </summary>
            public EBattleAbilityState State;

            /// <summary>
            /// 当前一共执行了几次阶段
            /// </summary>
            public int StageRunningCount;

            /// <summary>
            /// 每个阶段的计数器
            /// </summary>
            public Dictionary<int, int> StageCounter = new();
            
            //hook函数
            public Action OnStartFinish;
            public Action OnTickBefore;
            public Action OnTickFinish;
            public Action OnTickAfter;
            public Action OnEndFinish;
            
            public BattleAbilityState(BattleAbilityBlock bindAbilityBlock)
            {
                BindAbilityBlock = bindAbilityBlock;
            }

            public void AddSpecialStageCount(int stageId)
            {
                if (!StageCounter.TryAdd(stageId, 1))
                {
                    ++StageCounter[stageId];
                }
            }
            
            public int GetSpecialStageCount(int stageId)
            {
                return StageCounter.GetValueOrDefault(stageId, 0);
            }
            
        }
    }
}