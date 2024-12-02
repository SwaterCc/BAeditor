#region

using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using Hono.Scripts.Battle.Base;
using Hono.Scripts.Battle.Event;
using Hono.Scripts.Battle.Tools;
using Sirenix.OdinInspector;
using Sirenix.Serialization;

#endregion

namespace Hono.Scripts.Battle
{
    public class AbilityData : ASerializableData
    {
        public EAbilityType Type;

        public int DefaultStartGroupId = -1;

        /// <summary>
        /// 头节点字典
        /// </summary>
        [OdinSerialize] public Dictionary<EAbilityCycle, int> HeadNodeDict = new();

        /// <summary>
        /// 存储所有数据
        /// </summary>
        [Searchable] [OdinSerialize] public Dictionary<int, AbilityNodeData> NodeDict = new();

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
                    nodeData = new VariableNodeData();
                    break;
                case EAbilityNodeType.EAttrSetter:
                    nodeData = new AttrNodeData();
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

        public TNodeType DeepCopyNodeData<TNodeType>(TNodeType nodeData) where TNodeType : AbilityNodeData
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

        public bool DebugSkip = false;

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

        public virtual void CopyTo(AbilityNodeData copy)
        {
            NodeType = copy.NodeType;
            Desc = copy.Desc;
            Depth = copy.Depth;
        }

        public bool IsHead()
        {
            return ParentId == -1;
        }
    }

    [Serializable]
    public class ActionNodeData : AbilityNodeData
    {
        public AParams Function = new();

        public override void CopyTo(AbilityNodeData copy)
        {
            base.CopyTo(copy);
            Function = new AParams(((ActionNodeData)copy).Function);
        }
    }

    [Serializable]
    public class CycleNodeData : AbilityNodeData
    {
        public EAbilityCycle cycleNodeData;
    }

    [Serializable]
    public class BranchNodeData : AbilityNodeData
    {
        public AParams CompareFunc = new();
        public int BranchGroupId;

        public override void CopyTo(AbilityNodeData copy)
        {
            base.CopyTo(copy);
            var branch = (BranchNodeData)copy;
            CompareFunc = new AParams(branch.CompareFunc);
            BranchGroupId = branch.BranchGroupId + 100;
        }
    }

    [Serializable]
    public class EventNodeData : AbilityNodeData
    {
        public bool IsEvent;
        public EBattleEventType EventType;
        public AParams CreateChecker = new();
        public string MsgName;

        public override void CopyTo(AbilityNodeData copy)
        {
            base.CopyTo(copy);
            var eventNode = (EventNodeData)copy;
            IsEvent = eventNode.IsEvent;
            EventType = eventNode.EventType;
            CreateChecker = new AParams(eventNode.CreateChecker);
            MsgName = eventNode.MsgName;
        }
    }

    [Serializable]
    public class GroupNodeData : AbilityNodeData
    {
        public int groupId;
        public bool autoNext;
        public int defaultNextGroupId;

        public override void CopyTo(AbilityNodeData copy)
        {
            base.CopyTo(copy);
            var groupNode = (GroupNodeData)copy;
            groupId = groupNode.groupId + 100;
        }
    }

    [Serializable]
    public class TimerNodeData : AbilityNodeData
    {
        public AParams FirstInterval = new();
        public AParams Interval = new();
        public AParams MaxCount = new();

        public override void CopyTo(AbilityNodeData copy)
        {
            base.CopyTo(copy);
            var timerNode = (TimerNodeData)copy;
            FirstInterval = new AParams(timerNode.FirstInterval);
            Interval = new AParams(timerNode.Interval);
            MaxCount = new AParams(timerNode.MaxCount);
        }
    }

    [Serializable]
    public class RepeatNodeData : AbilityNodeData
    {
        public AParams MaxRepeatCount = new();

        public override void CopyTo(AbilityNodeData copy)
        {
            base.CopyTo(copy);
            var repeatNode = (RepeatNodeData)copy;
            MaxRepeatCount = new AParams(repeatNode.MaxRepeatCount);
        }
    }

    [Serializable]
    public class VariableNodeData : AbilityNodeData
    {
        public string Name;
        public string typeString = "int";
        public AParams Value = new();

        public override void CopyTo(AbilityNodeData copy)
        {
            base.CopyTo(copy);
            var varSetter = (VariableNodeData)copy;
            Value = new AParams(varSetter.Value);
            Name = varSetter.Name;
            typeString = varSetter.typeString;
        }
    }

    [Serializable]
    public class AttrNodeData : AbilityNodeData
    {
        public EAttrType attrType;
        public AParams Value = new();
        public bool IsTempAttr;

        public override void CopyTo(AbilityNodeData copy)
        {
            base.CopyTo(copy);
            var attrSetter = (AttrNodeData)copy;
            Value = new AParams(attrSetter.Value);
            attrType = attrSetter.attrType;
            IsTempAttr = attrSetter.IsTempAttr;
        }
    }
}