using System;
using System.Collections.Generic;
using Hono.Scripts.Battle.Base;
using Hono.Scripts.Battle.Event;

namespace Hono.Scripts.Battle
{
    [Serializable]
    public abstract class AbilityNodeData
    {
        public int NodeId;

        public EAbilityNodeType NodeType;

        public int ParentId;

        public int BelongGroupId = -1;

        public List<int> ChildrenIds = new();

        public string Desc;
        
        public virtual void CopyTo(AbilityNodeData copy)
        {
            NodeType = copy.NodeType;
            Desc = copy.Desc;
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