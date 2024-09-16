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

        public string IconPath;

        public List<int> Tags = new();

        public int DefaultStartGroupId = -1;

        public string PreCheckerVarName = "CHECKER";

        /// <summary>
        /// 头节点字典
        /// </summary>
        [OdinSerialize] public Dictionary<EAbilityAllowEditCycle, int> HeadNodeDict = new();

        /// <summary>
        /// 存储所有数据
        /// </summary>
        [OdinSerialize] public Dictionary<int, AbilityNodeData> NodeDict = new();

        [NonSerialized] private CommonUtility.IdGenerator _idGenerator = CommonUtility.GetIdGenerator();

        public static AbilityNodeData GetNodeData(AbilityData abilityData, EAbilityNodeType type)
        {
            AbilityNodeData nodeData = null;

            switch (type)
            {
                case EAbilityNodeType.EAbilityCycle:
                    nodeData = new ActionNodeData();
                    break;
                case EAbilityNodeType.EEvent:
                    nodeData = new EventNodeData();
                    break;
                case EAbilityNodeType.EBranchControl:
                    nodeData = new BranchNodeData();
                    break;
                case EAbilityNodeType.EVariableControl:
                    nodeData = new VariableNodeData();
                    break;
                case EAbilityNodeType.ERepeat:
                    nodeData = new RepeatNodeData();
                    break;
                case EAbilityNodeType.EAction:
                    nodeData = new ActionNodeData();
                    break;
                case EAbilityNodeType.ETimer:
                    nodeData = new TimerNodeData();
                    break;
                case EAbilityNodeType.EGroup:
                    nodeData = new GroupNodeData();
                    break;
            }

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
    public abstract class AbilityNodeData
    {
        public int NodeId;

        public EAbilityNodeType NodeType;

        public int ParentId;

        public int Depth;

        public int BelongGroupId = -1;

        public List<int> ChildrenIds = new();
        
        public string Desc;
        
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
            return ParentId == -1;
        }

        public AbilityNodeData DeepCopy()
        {
            using (var ms = new MemoryStream())
            {
                var formatter = new BinaryFormatter();
                formatter.Serialize(ms, this);
                ms.Position = 0;
                var copy = (AbilityNodeData)formatter.Deserialize(ms);

                // 重置 NodeId，排除其内容
                copy.NodeId = 0; // 或者任何适当的默认值

                return copy;
            }
        }
        
        public virtual void CopyTo(AbilityNodeData data)
        {
            NodeType = data.NodeType;
            
        }
    }

    [Serializable]
    public class ActionNodeData : AbilityNodeData
    {
        public Parameter[] Function;
    }

    [Serializable]
    public class CycleNodeData : AbilityNodeData
    {
        public EAbilityAllowEditCycle AllowEditCycleNodeData;
    }

    [Serializable]
    public class BranchNodeData : AbilityNodeData
    {
        public Parameter[] CompareFunc;
        public int BranchGroup;
    }

    [Serializable]
    public class EventNodeData : AbilityNodeData
    {
        public EBattleEventType EventType;
        public Parameter[] CreateChecker;
    }

    [Serializable]
    public class GroupNodeData : AbilityNodeData
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
    public class TimerNodeData : AbilityNodeData
    {
        public Parameter[] FirstInterval;
        public Parameter[] Interval;
        public Parameter[] MaxCount;
    }

    [Serializable]
    public class RepeatNodeData : AbilityNodeData
    {
        public Parameter[] MaxRepeatCount;
       
    }

    [Serializable]
    public class VariableNodeData : AbilityNodeData
    {
        public string Name;
        public Parameter[] Value;
        public string Desc;
    }
}