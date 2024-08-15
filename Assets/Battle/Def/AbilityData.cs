using System;
using System.Collections.Generic;
using Battle.Auto;

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

        public int[] Tags;
        
        /// <summary>
        /// 头节点字典，用List结构是因为一个生命周期可能会有多个逻辑，且有优先级
        /// </summary>
        public Dictionary<EAbilityCycleType, List<AbilityNodeData>> HeadNodeDict = new();

        /// <summary>
        /// 事件监听字典，用List结构是因为一个生命周期可能会有多个逻辑，且有优先级
        /// </summary>
        public Dictionary<EAbilityEventType, List<AbilityNodeData>> EventNodeDict = new();
        
        /// <summary>
        /// 存储所有数据
        /// </summary>
        public Dictionary<int,AbilityNodeData> NodeDict = new();
        
    }

    public class AbilityNodeData
    {
        public int NodeId;
        
        public EAbilityNodeType NodeType;
        
        public int Parent;

        public bool IsHead;
        
        public List<int> ChildrenUids = new();

        public int NextIdInSameLevel;
        
        public int Priority;

        public Param[] ActionNodeData;

        public Param[] BranchNodeData;
        
        public EventNodeData EventNodeData;
        
        public RepeatNodeData RepeatNodeData;

        public VariableNodeData VariableNodeData; 
        
        public EAbilityCycleType CycleNodeData;

        public TimerNodeData TimerNodeData;
    }

    public class StageNodeData
    {
        public int StageId;
    }
    
    public class TimerNodeData
    {
        public float FirstInterval;
        public float Interval;
        public float MaxCount;
    }
    
    public class RepeatNodeData
    {
        public ERepeatOperationType RepeatOperationType;

        public int MaxRepeatCount;

        public float StartValue;
        public float StepValue;
        public int StepCount;
        
        public Param[] CallFuncData;
    }
    
    public class EventNodeData
    {
        public EAbilityEventType EventType;
        
        public int[] Tag;

        public int HitBoxId;

        public int MotionId;
    }
    
    public class VariableNodeData
    {
        public EVariableOperationType OperationType;
        public EVariableRange Range;
        public string Name;
        
        public Param[] VarParams;
    }
}