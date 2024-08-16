using System;
using System.Collections.Generic;
using Battle.Auto;
using Battle.Event;
using Sirenix.OdinInspector;

namespace Battle.Def
{
    /// <summary>
    /// Ability的配置
    /// </summary>
    public class AbilityData : SerializedScriptableObject
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
        public Dictionary<EBattleEventType, List<AbilityNodeData>> EventNodeDict = new();

        /// <summary>
        /// 存储所有数据
        /// </summary>
        public Dictionary<int, AbilityNodeData> NodeDict = new();
        
        /// <summary>
        /// SKILL BUFF 静态字段 TODO:临时做法，后续要接Excel
        /// </summary>
        public Dictionary<string, IValueBox> SpecializationData = new Dictionary<string, IValueBox>();
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

        public Param[] EventNodeData;

        public RepeatNodeData RepeatNodeData;

        public VariableNodeData VariableNodeData;

        public EAbilityCycleType CycleNodeData;

        public TimerNodeData TimerNodeData;

        public StageNodeData StageNodeData;
    }

    public class StageNodeData
    {
        public int StageId;

        //逻辑不用，阶段描述
        public string Desc;

        /// <summary>
        /// 是否为默认开启阶段
        /// </summary>
        public bool IsDefaultStart;
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


    public class VariableNodeData
    {
        public EVariableOperationType OperationType;
        public EVariableRange Range;
        public string Name;

        public Param[] VarParams;
    }
}