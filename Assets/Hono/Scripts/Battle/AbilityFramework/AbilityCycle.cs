#region

using System;
using System.Collections.Generic;
using Hono.Scripts.Battle.Tools;
using UnityEngine;

#endregion

namespace Hono.Scripts.Battle.AbilityFramework
{
    public partial class Ability
    {
        public interface ITickANode
        {
            public void Tick(float dt);
        }

        private class AbilityCycle
        {
            public Ability AContext { get; }

            /// <summary>
            /// 当前状态
            /// </summary>
            public EAbilityCycle CurState { get; private set; }

            /// <summary>
            /// 周期回调
            /// </summary>
            public readonly Dictionary<EAbilityCycle, Action> CycleCallbacks = new(4);

            /// <summary>
            /// 周期头节点字典（执行入口）
            /// </summary>
            private readonly Dictionary<EAbilityCycle, ANode> _cycleHeads = new(4);

            /// <summary>
            /// 当前执行的Group
            /// </summary>
            public AGroupNode CurGroup;

            /// <summary>
            /// 下一个GroupId
            /// </summary>
            public int NextGroupId;

            /// <summary>
            /// 所有Group执行结束
            /// </summary>
            public bool IsAllGroupRunFinish { get; private set; }

            /// <summary>
            /// group周期控制
            /// </summary>
            public Dictionary<int, AGroupNode> Groups { get; } = new(8);

            /// <summary>
            /// EventNode
            /// </summary>
            private readonly List<AListenerNode> _eventNodeList = new(10);

            /// <summary>
            /// TickList，目前只有TimerNode会Tick
            /// </summary>
            private readonly List<ITickANode> _ticks = new(10);

            private readonly List<ITickANode> _tickRemoveList = new(5);

            public AbilityCycle(in Ability ability)
            {
                AContext = ability;
                CurState = EAbilityCycle.NoActive;
            }

            /// <summary>
            /// 获取Node
            /// </summary>
            /// <param name="ability"></param>
            /// <param name="data"></param>
            /// <returns></returns>
            /// <exception cref="InvalidCastException"></exception>
            public ANode GetNode(in Ability ability, in AbilityNodeData data)
            {
                ANode node = null;
                switch (data)
                {
                    case CycleNodeData:
                        node = GPool<ACycleNode>.Pool.Rent();
                        break;
                    case BranchGroupNodeData:
                        node = GPool<ABranchGroupNode>.Pool.Rent();
                        break;
                    case BranchNodeData:
                        node = GPool<ABranchNode>.Pool.Rent();
                        break;
                    case RepeatNodeData:
                        node = GPool<ARepeatNode>.Pool.Rent();
                        break;
                    case FunctionNodeData:
                        node = GPool<AFunctionNode>.Pool.Rent();
                        break;
                    case AttrModifyNodeData:
                        node = GPool<AAttrNode>.Pool.Rent();
                        break;
                    case GroupNodeData:
                        node = GPool<AGroupNode>.Pool.Rent();
                        AGroupNode groupNode = (AGroupNode)node;
                        Groups.Add(groupNode.Data.groupId, groupNode);
                        break;
                    case GroupSwitchNodeData:
                        node = GPool<AGroupSwitchNode>.Pool.Rent();
                        break;
                    case TimerNodeData:
                        node = GPool<ATimerNode>.Pool.Rent();
                        break;
                    case ListenerNodeData:
                        node = GPool<AListenerNode>.Pool.Rent();
                        _eventNodeList.Add((AListenerNode)node);
                        break;
                    case MsgSendNodeData:
                        node = GPool<AMsgSendNode>.Pool.Rent();
                        break;
                    case VariableNodeData:
                        node = GPool<AVariableNode>.Pool.Rent();
                        break;
                    default:
                        throw new InvalidCastException($"使用了不存在的Node类型 NodeType{data.GetType()}");
                }

                node.OnRent(ability, data);

                return node;
            }

            public void Init()
            {
                //构建树
                foreach (var pCycle in AContext.Data.HeadNodeDict)
                {
                    var node = GetNode(AContext, pCycle.Value);
                    node.Build(null, pCycle.Value.SerializableNodeList);
                    _cycleHeads.Add(pCycle.Key, node);
                }

                AContext.Log("Build Finish");

                //注册事件节点
                foreach (var eventNode in _eventNodeList)
                {
                    eventNode.RegisterEvent();
                }

                //执行
                doCycle(EAbilityCycle.Init);

                IsAllGroupRunFinish = false;
            }

            /// <summary>
            /// 从指定Cycle节点开始执行
            /// </summary>
            private void doCycle(EAbilityCycle cycle)
            {
                AContext.Log($"Cycle {cycle} DoJob");
                try
                {
                    CurState = cycle;
                    if (_cycleHeads.TryGetValue(CurState, out var cycleHeadNode))
                    {
                        cycleHeadNode.DoJob();
                    }

                    CycleCallbacks[CurState]?.Invoke();
                }
                catch (Exception e)
                {
                    AContext.LogError($"{CurState} Error : " + e);
                    cycleEnd();
                }
            }

            /// <summary>
            /// 执行Ability
            /// </summary>
            public void Execute()
            {
                AContext.Log("Execute");
                //预执行
                doCycle(EAbilityCycle.PreExecute);
                //运行
                doCycle(EAbilityCycle.Executing);

                //如果有Group则设置开始GroupID
                IsAllGroupRunFinish = Groups.Count <= 0;
                if (CurGroup == null && !IsAllGroupRunFinish)
                {
                    SwitchGroup();
                }

                //没有Group且没有计时器，该ability是一帧结束的,
                //如果有Group则要等待所有Group全部执行完，如果有计时器要等待所有定时器结束
                if (_ticks.Count != 0 || !IsAllGroupRunFinish)
                {
                    return;
                }

                //结束
                cycleEnd();
            }

            /// <summary>
            /// Timer
            /// </summary>
            /// <param name="dt"></param>
            public void Tick(float dt)
            {
                if (_ticks.Count == 0) return;

                try
                {
                    foreach (ITickANode tickNode in _ticks)
                    {
                        tickNode.Tick(dt);
                    }

                    foreach (ITickANode removeTick in _tickRemoveList)
                    {
                        _ticks.RemoveSwapBack(removeTick);
                    }

                    _tickRemoveList.Clear();

                    if (_ticks.Count == 0)
                    {
                        cycleEnd();
                    }
                }
                catch (Exception e)
                {
                    Debug.LogError("Timer or Group" + e);
                    cycleEnd();
                }
            }

            /// <summary>
            /// 切换状态
            /// </summary>
            public void SwitchGroup()
            {
                CurGroup?.GroupExit();
                UnRegisterTick(CurGroup);
                CurGroup = null;
                if (Groups.TryGetValue(NextGroupId, out AGroupNode next))
                {
                    CurGroup = next;
                    RegisterTick(CurGroup);
                    CurGroup.GroupEnter();
                }
                else
                {
                    if (NextGroupId > 0)
                    {
                        AContext.LogError($"找不到指定Group Id {NextGroupId}");
                    }
                }
            }

            /// <summary>
            /// 注册Tick
            /// </summary>
            /// <param name="tickNode"></param>
            public void RegisterTick(ITickANode tickNode)
            {
                ANode node = (ANode)tickNode;
                if (node.Data.belongGroupId == 0)
                {
                    _ticks.Add(tickNode);
                }
                else
                {
                    if (CurGroup != null && CurGroup.Data.groupId == node.Data.belongGroupId)
                    {
                        CurGroup.AddTick(tickNode);
                    }
                    else
                    {
                        throw new Exception("Group中存在一个不属于该Group的TickNode，且在尝试启动");
                    }
                }
            }

            /// <summary>
            /// 注销Tick
            /// </summary>
            /// <param name="tickNode"></param>
            public void UnRegisterTick(ITickANode tickNode)
            {
                if (tickNode == null) return;

                if (_ticks.Contains(tickNode))
                {
                    _tickRemoveList.Add(tickNode);
                    return;
                }

                CurGroup.RemoveTick(tickNode);
            }

            /// <summary>
            /// 非自然停止，强制停止，需要清理上一个ability的状态
            /// 然后执行end阶段
            /// </summary>
            private void cycleEnd()
            {
                //结束Group
                CurGroup?.GroupExit();
                CurGroup = null;
                //清理定时器
                _ticks.Clear();
                //执行结束阶段
                doCycle(EAbilityCycle.EndExecute);

                AContext.ExecuteEndCallBack?.Invoke();

                //重置到Init状态
                CurState = EAbilityCycle.Init;
            }

            public void ForceStop()
            {
                if (CurState != EAbilityCycle.Executing)
                {
                    //正在执行中
                    return;
                }

                cycleEnd();
            }

            public void OnRecycle()
            {
                //注销事件节点
                foreach (var eventNode in _eventNodeList)
                {
                    eventNode.UnRegisterEvent();
                }

                _eventNodeList.Clear();

                _ticks.Clear();
                _tickRemoveList.Clear();

                Groups.Clear();
                CurGroup = null;

                CurState = EAbilityCycle.NoActive;
                foreach (var pHead in _cycleHeads)
                {
                    pHead.Value.Recycle();
                }

                //TODO:疑似子节点没回收
                _cycleHeads.Clear();
            }
        }
    }
}