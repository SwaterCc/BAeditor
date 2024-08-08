using System;
using System.Collections.Generic;

namespace Battle.Def
{
    /// <summary>
    /// Ability的配置
    /// </summary>
    public class AbilityData
    {
        public int ConfigId;

        public string Name = "NoName";

        public string Desc = "NoInit";
        
        public string IconPath = "";

        /// <summary>
        /// 头节点字典，用List结构是因为一个生命周期可能会有多个逻辑，且有优先级
        /// </summary>
        public Dictionary<EAbilityLiftFuncType, List<AbilityNodeData>> HeadNodeDict = new();

        /// <summary>
        /// 存储所有数据
        /// </summary>
        public Dictionary<int,AbilityNodeData> NodeDict = new();
        
    }

    public class AbilityNodeData
    {
        public int NodeUid;
        
        public EAbilityNodeType NodeType;
        
        public int Parent;
        
        public List<int> ChildrenUids = new();

        public int NextIdInSameLevel;
        
        public int Priority;

        public Object NodeClass;
    }
    
}