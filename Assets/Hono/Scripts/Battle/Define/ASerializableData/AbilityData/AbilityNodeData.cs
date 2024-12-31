using System;
using System.Collections.Generic;
using Hono.Scripts.Battle.Base;
using Hono.Scripts.Battle.Event;
using UnityEngine;
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

        public string Desc = "";

        //TODO:调试相关数据后续将其拆分为编辑器独立数据中
        /// <summary>
        /// 跳过执行
        /// </summary>
        public bool skipExecute;
        /// <summary>
        /// 打印执行日志
        /// </summary>
        public bool showLog;

        public abstract AbilityNodeData DeepCopy();
    }

    [Serializable]
    public class ActionNodeData : AbilityNodeData
    {
        public AParams action = new();
        public string returnType;
        public bool isCreateVariable;
        public string returnValueKey;

        public override AbilityNodeData DeepCopy()
        {
            var copy = new ActionNodeData();
            copy.action = new AParams(action);
            copy.isCreateVariable = isCreateVariable;
            copy.returnValueKey = returnValueKey;
            return copy;
        }
    }

    [Serializable]
    public class VariableNodeData : AbilityNodeData
    {
        public string key = "";
        public bool isModify;
        public string valueType = typeof(int).ToString();
        public string value = "0";
        public EVariableOperationType operationType;

        public override AbilityNodeData DeepCopy()
        {
            var copy = new VariableNodeData();
            copy.key = key;
            copy.isModify = isModify;
            copy.valueType = valueType;
            copy.value = value;
            return copy;
        }
    }


    [Serializable]
    public class CycleNodeData : AbilityNodeData
    {
        public EAbilityCycle cycleNodeData;

        public CycleNodeData() { }

        public CycleNodeData(EAbilityCycle cycle)
        {
            cycleNodeData = cycle;
        }

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
            copy.autoNext = autoNext;
            return copy;
        }
    }

    [Serializable]
    public class GroupSwitchNodeData : AbilityNodeData
    {
        public bool switchGroupNow;
        public AParams nextGroupId;

        public override AbilityNodeData DeepCopy()
        {
            var copy = new GroupSwitchNodeData();
            copy.switchGroupNow = switchGroupNow;
            copy.nextGroupId = new AParams(nextGroupId);
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
        public ERepeatNodeOperationType operationType;
        public int repeatCount;
        public AParams traverseList;
        public bool isCatchForeachCount;
        public string foreachVarName;
        public bool isCatchForeachObject;
        public string listItemVarName;

        public override AbilityNodeData DeepCopy()
        {
            var copy = new RepeatNodeData();
            copy.operationType = operationType;
            copy.repeatCount = repeatCount;
            copy.traverseList = new AParams(traverseList);
            copy.isCatchForeachCount = isCatchForeachCount;
            copy.foreachVarName = foreachVarName;
            copy.isCatchForeachObject = isCatchForeachObject;
            copy.listItemVarName = listItemVarName;
            return copy;
        }
    }

    [Serializable]
    public class AttrModifyNodeData : AbilityNodeData
    {
        public EAttrType attrType;
        public AParams value = new();
        public EAttrModifyEffectType modifyEffectType;

        public override AbilityNodeData DeepCopy()
        {
            var copy = new AttrModifyNodeData();
            copy.attrType = attrType;
            copy.value = new AParams(value);
            copy.modifyEffectType = modifyEffectType;
            return copy;
        }
    }
}