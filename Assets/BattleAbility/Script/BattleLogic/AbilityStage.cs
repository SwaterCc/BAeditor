using System.Collections.Generic;

namespace BattleAbility
{
    /// <summary>
    /// 逻辑阶段
    /// 以技能举例，一个技能可能有多个动作，每个动作就是一个阶段
    /// </summary>
    public partial class BattleAbilityBlock
    {
        public class AbilityStage
        {
            private List<BattleAbilityLogicTreeNode> _rootList = new();
            private BattleAbilityBlock _block;
            public int StageId;
            public BattleRuntimeDataBase StageData;
            
            public AbilityStage(BattleAbilityBlock block)
            {
                _block = block;
                
                
            }
        }
    }
 
}