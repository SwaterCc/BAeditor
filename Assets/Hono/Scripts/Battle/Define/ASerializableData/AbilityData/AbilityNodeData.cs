using System;
using System.Collections.Generic;
using Hono.Scripts.Battle.Base;
using Hono.Scripts.Battle.Event;
using UnityEngine.Serialization;

namespace Hono.Scripts.Battle
{
    [Serializable]
    public abstract class AbilityNodeData
    {
        public int NodeId;

        public int ParentId;

        public int BelongGroupId = -1;

        public List<int> ChildrenIds = new();

        public string Desc;

        public abstract AbilityNodeData Copy();
    }

    [Serializable]
    public class ActionNodeData : AbilityNodeData
    {
        public AParams Function = new();

        public override AbilityNodeData Copy()
        {
            var copy = new ActionNodeData();
            copy.Function = new AParams(copy.Function);
            return copy;
        }
    }

    [Serializable]
    public class CycleNodeData : AbilityNodeData
    {
        public EAbilityCycle cycleNodeData;

        public override AbilityNodeData Copy()
        {
            var copy = new CycleNodeData
            {
                cycleNodeData = cycleNodeData
            };
            return copy;
        }
    }

    [Serializable]
    public class BranchGroupNodeData : AbilityNodeData
    {
        public List<int> BranchNodeIds = new();

        public override AbilityNodeData Copy()
        {
            var copy = new BranchGroupNodeData();
            copy.BranchNodeIds.AddRange(BranchNodeIds);
            return copy;
        }
    }

    [Serializable]
    public class BranchNodeData : AbilityNodeData
    {
        public AParams CompareFunc = new();

        public override AbilityNodeData Copy()
        {
            var copy = new BranchNodeData();
            copy.CompareFunc = new AParams(CompareFunc);
            return copy;
        }
    }

    [Serializable]
    public class ListenerNodeData : AbilityNodeData
    {
        public bool IsEvent;
        public EBattleEventType EventType;
        public AParams GetChecker = new();
        public string MsgName;

        public override AbilityNodeData Copy()
        {
            var copy = new ListenerNodeData();
            copy.IsEvent = IsEvent;
            if (copy.IsEvent)
            {
                copy.EventType = EventType;
                copy.GetChecker = new AParams(GetChecker);
            }
            else
            {
                copy.MsgName = MsgName;
            }

            return copy;
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