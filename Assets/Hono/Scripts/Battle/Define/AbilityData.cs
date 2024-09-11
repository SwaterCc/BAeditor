using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using Hono.Scripts.Battle.Event;
using Hono.Scripts.Battle.Tools;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using UnityEngine;
using UnityEngine.Serialization;

namespace Hono.Scripts.Battle
{
    /// <summary>
    /// Ability的配置
    /// ID分段 1-9999 其他逻辑
    /// ID分段 10001-19999 技能
    /// ID分段 20001-29999 buff
    /// ID分段 30001-39999 子弹
    /// ID分段 40001-49999 GameMode
    /// </summary>
    public class AbilityData : SerializedScriptableObject, IAllowedIndexing
    {
        public int ID => ConfigId;
        
        public EAbilityType Type;

        public int ConfigId;

        public string Name = "NoName";

        public string Desc = "NoInit";

        public Texture IconPath;

        public List<int> Tags = new();

        public int DefaultStartGroupId = -1;

        public string PreCheckerVarName = "CHECKER";

        /// <summary>
        /// 头节点字典
        /// </summary>
        [OdinSerialize]
        public Dictionary<EAbilityAllowEditCycle, int> HeadNodeDict = new();
        
        /// <summary>
        /// 存储所有数据
        /// </summary>
        [OdinSerialize]
        public Dictionary<int, AbilityNodeData> NodeDict = new();

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

        public int BelongGroupId = -1;
        
        public List<int> ChildrenIds = new();

        public int NextIdInSameLevel;
        
        public Parameter[] ActionNodeData;
        
        public BranchNodeData BranchNodeData;
        
        public EventNodeData EventNodeData;
        
        public RepeatNodeData RepeatNodeData;
        
        public VariableNodeData VariableNodeData;
        
	    public EAbilityAllowEditCycle allowEditCycleNodeData;
        
        public TimerNodeData TimerNodeData;
        
        public GroupNodeData GroupNodeData;

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
        
        public AbilityNodeData DeepCopy()
        {
            using (var ms = new MemoryStream())
            {
                var formatter = new BinaryFormatter();
                formatter.Serialize(ms, this);
                ms.Position = 0;
                return (AbilityNodeData)formatter.Deserialize(ms);
            }
        }
    }
    
    [Serializable]
    public class BranchNodeData
    {
        public Parameter[] Left;
        public ECompareResType ResType;
        public Parameter[] Right;
        public string Desc;//编辑器描述
    }
    
    [Serializable]
    public class EventNodeData
    {
        public EBattleEventType EventType;
        public Parameter[] CreateCheckerFunc;
        public string CaptureVarName;
        public string Desc;
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
        public Parameter[] FirstInterval;
        public Parameter[] Interval;
        public Parameter[] MaxCount;
        public string Desc = "定时器";
    }

    [Serializable]
    public class RepeatNodeData
    {
        public Parameter[] MaxRepeatCount;
        public string Desc;
    }

    [Serializable]
    public class VariableNodeData
    {
        public EVariableOperationType OperationType;
        public EVariableRange Range;
        public Parameter[] ActorUid;
        public Parameter[] AbilityUid;
        public string Name;
        public Parameter[] VarParams;
        public string Desc;
    }
}