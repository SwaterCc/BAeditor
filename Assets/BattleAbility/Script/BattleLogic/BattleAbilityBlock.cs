using System.Collections.Generic;

namespace BattleAbility
{
    /// <summary>
    /// 战斗能力类
    /// 通用的战斗逻辑执行模块管理战斗逻辑技能，buff，子弹的流程，基础数据来自配置，额外存有一个运行时数据类，
    /// </summary>
    public partial class BattleAbilityBlock
    {
        private Dictionary<int, AbilityStage> _stages = new(7);
        private BattleAbilityState _state;
        private readonly BattleAbilityData _data;
        
        public BattleAbilityBlock(BattleAbilityState state, BattleAbilityData data)
        {
            _data = data;
        }
    }
}