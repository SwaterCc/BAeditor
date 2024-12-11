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
        public Dictionary<EAbilityCycle, AbilityNodeData> HeadNodeDict = new()
        {
            { EAbilityCycle.Init, new CycleNodeData(EAbilityCycle.Init) },
            { EAbilityCycle.PreExecute, new CycleNodeData(EAbilityCycle.PreExecute) },
            { EAbilityCycle.Executing, new CycleNodeData(EAbilityCycle.Executing) },
            { EAbilityCycle.EndExecute, new CycleNodeData(EAbilityCycle.EndExecute) }
        };

        /// <summary>
        /// 存储非头节点的其他节点
        /// </summary>
        [Searchable]
        [OdinSerialize]
        public Dictionary<int, AbilityNodeData> NodeDict = new();
    }
}