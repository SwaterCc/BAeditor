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

        public abstract AbilityNodeData DeepCopy();
    }

    [Serializable]
    public class ActionNodeData : AbilityNodeData
    {
        public AParams Function = new();

        public override AbilityNodeData DeepCopy()
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

        public override AbilityNodeData DeepCopy()
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

        public override AbilityNodeData DeepCopy()
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

        public override AbilityNodeData DeepCopy()
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

        public override AbilityNodeData DeepCopy()
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

        public override AbilityNodeData DeepCopy()
        {
            var copy = new GroupNodeData();
            return copy;
        }
    }

    [Serializable]
    public class TimerNodeData : AbilityNodeData
    {
        public AParams FirstInterval = new();
        public AParams Interval = new();
        public AParams MaxCount = new();

        public override AbilityNodeData DeepCopy()
        {
            var copy = new TimerNodeData();
            copy.FirstInterval = new AParams(FirstInterval);
            copy.Interval = new AParams(Interval);
            copy.MaxCount = new AParams(MaxCount);
            return copy;
        }
    }

    [Serializable]
    public class RepeatNodeData : AbilityNodeData
    {
        public AParams MaxRepeatCount = new();

        public override AbilityNodeData DeepCopy()
        {
            var copy = new RepeatNodeData();
            copy.MaxRepeatCount = new AParams(MaxRepeatCount);
            return copy;
        }
    }

    [Serializable]
    public class VariableNodeData : AbilityNodeData
    {
        public string Key;
        public AParams Value = new();
        public bool IsGetReturnValue;
        public override AbilityNodeData DeepCopy()
        {
            var copy = new VariableNodeData();
            copy.Key = Key;
            copy.Value = new AParams(Value);
            return copy;
        }
    }

    [Serializable]
    public class AttrNodeData : AbilityNodeData
    {
        public EAttrType attrType;
        public AParams Value = new();
        public bool IsPersistent;

        public override AbilityNodeData DeepCopy()
        {
            var copy = new AttrNodeData();
            copy.attrType = attrType;
            copy.Value = new AParams(Value);
            copy.IsPersistent = IsPersistent;
            return copy;
        }
    }
}