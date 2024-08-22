using System;
using System.Collections.Generic;
using Battle.Event;
using Battle.Tools;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using UnityEngine.Serialization;

namespace Battle
{
    /// <summary>
    /// Ability的配置
    /// </summary>
    public class AbilityData : SerializedScriptableObject
    {
        public EAbilityType Type;

        public int ConfigId;

        public string Name = "NoName";

        public string Desc = "NoInit";

        public string IconPath = "";

        public int[] Tags;

        /// <summary>
        /// 头节点字典
        /// </summary>
        [OdinSerialize]
        public Dictionary<EAbilityCycleType, int> HeadNodeDict = new();

        /// <summary>
        /// 事件监听字典，用List结构是因为一个生命周期可能会有多个逻辑，且有优先级
        /// </summary>
        [OdinSerialize]
        public Dictionary<EBattleEventType, int> EventNodeDict = new();

        /// <summary>
        /// 存储所有数据
        /// </summary>
        [OdinSerialize]
        public Dictionary<int, AbilityNodeData> NodeDict = new();

        /// <summary>
        /// SKILL BUFF 静态字段 TODO:临时做法，后续要接Excel
        /// </summary>
        [OdinSerialize]
        public Dictionary<string, object> SpecializationData = new Dictionary<string, object>();

        [NonSerialized]
        private CommonUtility.IdGenerator _idGenerator = CommonUtility.GetIdGenerator();
        public static AbilityNodeData GetNodeData(AbilityData abilityData, EAbilityNodeType type)
        {
            var nodeData = new AbilityNodeData();
            var id = CommonUtility.GenerateTimeBasedHashId32();
            var maxTryCount = 500;
            var curTryCount = 0;
            while (abilityData.NodeDict.ContainsKey(id))
            {
                id = CommonUtility.GenerateTimeBasedHashId32();
                if (++curTryCount > maxTryCount)
                {
                    throw new Exception("id生成错误");
                }
            }

            nodeData.NodeId = id;
            nodeData.NodeType = type;

            return nodeData;
        }
    }

    [Serializable]
    public class AbilityNodeData
    {
        public int NodeId;

        public EAbilityNodeType NodeType;

        public int Parent;

        public int Depth;
        
        public List<int> ChildrenIds = new();

        public int NextIdInSameLevel;
        
        public Parameter[] ActionNodeData;
        
        public BranchNodeData BranchNodeData;
        
        public EventNodeData EventNodeData;
        
        public RepeatNodeData RepeatNodeData;
        
        public VariableNodeData VariableNodeData;
        
        public EAbilityCycleType CycleNodeData;
        
        public TimerNodeData TimerNodeData;
        
        [FormerlySerializedAs("StageNodeData")] public GroupNodeData groupNodeData;

        public void RemoveSelf(AbilityData data)
        {
            data.NodeDict.Remove(NodeId);
            if (ChildrenIds.Count > 0)
            {
                foreach (var id in ChildrenIds)
                {
                    data.NodeDict[id].RemoveSelf(data);
                }
            }
        }

        public bool IsHead()
        {
            return Parent == -1;
        }
    }

    [Serializable]
    public class BranchNodeData
    {
        public Parameter[] Left;
        public ECompareResType ResType;
        public Parameter[] Right;
    }
    
    [Serializable]
    public class EventNodeData
    {
        public EBattleEventType EventType;
        public Parameter[] CreateCheckerFunc;
    }

    [Serializable]
    public class GroupNodeData
    {
        public int GroupId;

        //逻辑不用，阶段描述
        public string Desc;

        /// <summary>
        /// 是否为默认开启阶段
        /// </summary>
        public bool IsDefaultStart;
    }

    [Serializable]
    public class TimerNodeData
    {
        public float FirstInterval;
        public float Interval;
        public float MaxCount;
    }

    [Serializable]
    public class RepeatNodeData
    {
        public ERepeatOperationType RepeatOperationType;

        public int MaxRepeatCount;

        public float StartValue;
        public float StepValue;
        public int StepCount;
    }

    [Serializable]
    public class VariableNodeData
    {
        public EVariableOperationType OperationType;
        public EVariableRange Range;
        public string Name;
        public Parameter[] VarParams;
    }
}