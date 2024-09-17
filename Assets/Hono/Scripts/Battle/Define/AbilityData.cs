using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using Hono.Scripts.Battle.Event;
using Hono.Scripts.Battle.Tools;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using Unity.VisualScripting;

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

        public AbilityNodeData GetNodeData(EAbilityNodeType type)
        {
            AbilityNodeData nodeData = null;

            switch (type)
            {
                case EAbilityNodeType.EAbilityCycle:
                    nodeData = new CycleNodeData();
                    break;
                case EAbilityNodeType.EEvent:
                    nodeData = new EventNodeData();
                    break;
                case EAbilityNodeType.EBranchControl:
                    nodeData = new BranchNodeData();
                    break;
                case EAbilityNodeType.EVariableSetter:
                    nodeData = new VarSetterNodeData();
                    break;
                case EAbilityNodeType.EAttrSetter:
                    nodeData = new AttrSetterNodeData();
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

            nodeData.NodeId = GenNodeId();
            nodeData.NodeType = type;

            return nodeData;
        }


        private int GenNodeId()
        {
            var id = CommonUtility.GenerateTimeBasedHashId32();
            var maxTryCount = 500;
            var curTryCount = 0;
            while (NodeDict.ContainsKey(id))
            {
                id = CommonUtility.GenerateTimeBasedHashId32();
                if (++curTryCount > maxTryCount)
                {
                    throw new Exception("id生成错误");
                }
            }

            return id;
        }
        
        public  TNodeType DeepCopyNodeData<TNodeType>(TNodeType nodeData) where TNodeType : AbilityNodeData
        {
            using (var ms = new MemoryStream())
            {
                var formatter = new BinaryFormatter();
                formatter.Serialize(ms, nodeData);
                ms.Position = 0;
                var copy = (TNodeType)formatter.Deserialize(ms);
                return copy;
            }
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
    }

    [Serializable]
    public class ActionNodeData : AbilityNodeData
    {
        public Parameter Function = new();
    }

    [Serializable]
    public class CycleNodeData : AbilityNodeData
    {
        public EAbilityAllowEditCycle AllowEditCycleNodeData;
    }

    [Serializable]
    public class BranchNodeData : AbilityNodeData
    {
        public Parameter CompareFunc = new();
        public int BranchGroup;
    }

    [Serializable]
    public class EventNodeData : AbilityNodeData
    {
        public EBattleEventType EventType;
        public Parameter CreateChecker = new();
    }

    [Serializable]
    public class GroupNodeData : AbilityNodeData
    {
        public int GroupId;

        /// <summary>
        /// 是否为默认开启阶段
        /// </summary>
        public bool IsDefaultStart;
    }

    [Serializable]
    public class TimerNodeData : AbilityNodeData
    {
        public Parameter FirstInterval = new();
        public Parameter Interval = new();
        public Parameter MaxCount = new();
    }

    [Serializable]
    public class RepeatNodeData : AbilityNodeData
    {
        public Parameter MaxRepeatCount = new();
    }

    [Serializable]
    public class VarSetterNodeData : AbilityNodeData
    {
        public string Name;
        public string typeString = "int";
        public Parameter Value = new();
    }

    [Serializable]
    public class AttrSetterNodeData : AbilityNodeData
    {
        public ELogicAttr LogicAttr;
        public string typeString = "int";
        public Parameter Value = new();
        public bool IsTempAttr;
    }
}