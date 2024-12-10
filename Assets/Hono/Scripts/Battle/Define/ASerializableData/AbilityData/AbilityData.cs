#region

using System.Collections.Generic;
using Sirenix.OdinInspector;
using Sirenix.Serialization;

#endregion

namespace Hono.Scripts.Battle
{
    public class AbilityData : ASerializableData
    {
        /// <summary>
        /// 默认起始Group
        /// </summary>
        public int defaultStartGroupId = -1;

        /// <summary>
        /// 头节点字典
        /// </summary>
        [OdinSerialize]
        public Dictionary<EAbilityCycle, AbilityNodeData> HeadNodeDict = new();

        /// <summary>
        /// 存储非头节点的其他节点
        /// </summary>
        [Searchable]
        [OdinSerialize]
        public Dictionary<int, AbilityNodeData> NodeDict = new();

        public AbilityData()
        {
            HeadNodeDict[EAbilityCycle.Init] = new CycleNodeData(EAbilityCycle.Init);
            HeadNodeDict[EAbilityCycle.PreExecute] = new CycleNodeData(EAbilityCycle.PreExecute);
            HeadNodeDict[EAbilityCycle.Executing] = new CycleNodeData(EAbilityCycle.Executing);
            HeadNodeDict[EAbilityCycle.EndExecute] = new CycleNodeData(EAbilityCycle.EndExecute);
        }
    }
}