using System.Collections.Generic;

namespace BattleAbility
{
    /// <summary>
    /// 战斗能力类
    /// 通用的战斗逻辑执行模块管理战斗逻辑技能，buff，子弹的流程，基础数据来自配置，额外存有一个运行时数据类，
    /// </summary>
    public abstract class BattleAbilityBlock
    {
        public enum EBattleAbilityType
        {
            Skill,
            Buff,
            Bullet,
            TYPE_MAX_COUNT
        }
        
        /// <summary>
        /// 逻辑阶段
        /// 以技能举例，一个技能可能有多个动作，每个动作就是一个阶段
        /// </summary>
        public class AbilityStage
        {
            public int StageId;
            public BattleRuntimeDataBase StageData;
            private BattleAbilityLogicTreeNode _root;
            private BattleAbilityBlock _block;

            public AbilityStage()
            {
            }
        }
        
        public BattleRuntimeDataBase BlockRTData;
        private AbilityStage _curStage;
        private EBattleAbilityType _source;
    }

    public class CharacterBattleAbilityBlock<TBattleRTData> : BattleAbilityBlock where TBattleRTData : BattleRuntimeDataBase
    {
        public new TBattleRTData RTData => base.BlockRTData == null ? null : base.BlockRTData as TBattleRTData;
    }
}