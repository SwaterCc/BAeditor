using System;
using System.Collections.Generic;
using Hono.Scripts.Battle.AbilitySystem;
using Hono.Scripts.Battle.Base;
using Hono.Scripts.Battle.Core;
using Hono.Scripts.Battle.Event;
using Sirenix.Serialization;
using UnityEngine;
using UnityEngine.Serialization;

namespace Hono.Scripts.Battle
{
    [Serializable]
    public abstract class AbilityNodeData
    {
        public int nodeIndex;

        public int parentIndex;

        public int belongGroupId = -1;

        public List<int> childrenIndexes = new();
        
        //TODO:调试相关数据后续将其拆分为编辑器独立数据中，目前该字段无效（因为其不属于拷贝数据）
        /// <summary>
        /// 节点描述
        /// </summary>
        public string desc = "";
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
    public class CycleNodeData : AbilityNodeData
    {
        public EAbilityCycle cycleType;

        public List<AbilityNodeData> SerializableNodeList = new();

        public CycleNodeData() { }

        public CycleNodeData(EAbilityCycle cycle)
        {
            cycleType = cycle;
        }

        public override AbilityNodeData DeepCopy()
        {
            var copy = new CycleNodeData();
            copy.cycleType = cycleType;
            copy.SerializableNodeList = SerializableNodeList;
            return copy;
        }
    }

    [Serializable]
    public class FunctionNodeData : AbilityNodeData
    {
        public AParams action = new();
        public string returnType;
        public bool isCreateVariable;
        public string returnValueKey;

        public override AbilityNodeData DeepCopy()
        {
            var copy = new FunctionNodeData();
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
        public AParams value = new();
        public EVariableOperationType operationType;

        public override AbilityNodeData DeepCopy()
        {
            var copy = new VariableNodeData();
            copy.key = key;
            copy.isModify = isModify;
            copy.value = new AParams(value);
            copy.value = value;
            copy.operationType = operationType;
            return copy;
        }
    }

    [Serializable]
    public class BranchGroupNodeData : AbilityNodeData
    {
        public override AbilityNodeData DeepCopy()
        {
            return new BranchGroupNodeData();
        }
    }

    [Serializable]
    public class BranchNodeData : AbilityNodeData
    {
        public AParams condition = new();

        public override AbilityNodeData DeepCopy()
        {
            var copy = new BranchNodeData();
            copy.condition = new AParams(condition);
            return copy;
        }
    }

    [Serializable]
    public class MsgSendNodeData : AbilityNodeData
    {
        public AParams actorUid = new();
        public string msgKey;
        public List<string> msgParamKeys = new(); 
        public List<AParams> values = new();

        public override AbilityNodeData DeepCopy()
        {
            var copy = new MsgSendNodeData();
            copy.actorUid = new AParams(actorUid);
            copy.msgKey = msgKey;
            
            foreach (var param in values)
            {
                copy.values.Add(new AParams(param));
            }

            foreach (var key in msgParamKeys)
            {
                copy.msgParamKeys.Add(new string(key));
            }

            return copy;
        }
    }

    [Serializable]
    public class ListenerNodeData : AbilityNodeData
    {
        public bool isEvent;
        public EEventType eventType;
        public float eventInterval;
        public bool isGlobalEvtListener;
        [OdinSerialize]
        public IEventChecker Checker;

        public string msgName;

        public override AbilityNodeData DeepCopy()
        {
            var copy = new ListenerNodeData();
            copy.isEvent = isEvent;
            if (copy.isEvent)
            {
                copy.eventType = eventType;
                copy.eventInterval = eventInterval;
                copy.Checker = Checker;
            }
            else
            {
                copy.msgName = msgName;
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
        public AParams nextGroupId = new();

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
        public AParams firstInterval = new();
        public AParams interval = new();
        public AParams maxCount = new();

        public override AbilityNodeData DeepCopy()
        {
            var copy = new TimerNodeData();
            copy.firstInterval = new AParams(firstInterval);
            copy.interval = new AParams(interval);
            copy.maxCount = new AParams(maxCount);
            return copy;
        }
    }

    [Serializable]
    public class RepeatNodeData : AbilityNodeData
    {
        public ERepeatNodeOperationType operationType;
        public AParams repeatCount = new();
        public AParams traverseList = new();

        public override AbilityNodeData DeepCopy()
        {
            var copy = new RepeatNodeData();
            copy.operationType = operationType;
            copy.repeatCount = new AParams(repeatCount);
            copy.traverseList = new AParams(traverseList);
            return copy;
        }
    }

    [Serializable]
    public class AttrModifyNodeData : AbilityNodeData
    {
        public EAttrType attrType;
        public AParams value = new();
        public EAbilityCommandType commandType;

        public override AbilityNodeData DeepCopy()
        {
            var copy = new AttrModifyNodeData();
            copy.attrType = attrType;
            copy.value = new AParams(value);
            copy.commandType = commandType;
            return copy;
        }
    }
}