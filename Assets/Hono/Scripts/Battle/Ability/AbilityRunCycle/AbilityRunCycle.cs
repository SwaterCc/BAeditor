
using System.Collections.Generic;
using UnityEngine;

namespace Hono.Scripts.Battle
{
    public partial class Ability
    {
        private interface ITimer
        {
            public bool IsFinish();
            public void Add(float dt);
            public bool NeedCall();
            public void OnCallTimer();
        }

        private abstract class AbilityRunCycle
        {
            protected Ability _ability;
            protected AbilityExecutor _executor;
            protected AbilityState _state;

            protected readonly HashSet<ITimer> _timerNodes;
            protected readonly List<ITimer> _removes;
            public EAbilityState CurState => getCurState();

            protected AbilityRunCycle(AbilityState state)
            {
                _state = state;
                _ability = _state.Ability;
                _executor = _state.Ability._executor;
                _timerNodes = new HashSet<ITimer>();
                _removes = new List<ITimer>();
            }

            public virtual void TimerStart(ITimer callBack)
            {
                _timerNodes.Add(callBack);
            }

            protected abstract EAbilityCycleType getCycleType();
            protected abstract EAbilityState getCurState();

            public void Enter()
            {
                Debug.Log($"Enter {CurState},CycleType {getCycleType()}");
                onEnter();
                if (CurState != EAbilityState.Ready)
                    _executor.ExecuteCycleNode(getCycleType());
            }

            protected virtual void onEnter() { }

            public virtual void Tick(float dt)
            {
                foreach (var timer in _timerNodes)
                {
                    timer.Add(dt);
                    if (timer.NeedCall())
                    {
                        timer.OnCallTimer();
                    }

                    if (timer.IsFinish())
                    {
                        _removes.Add(timer);
                    }
                }

                onTick(dt);

                foreach (var timer in _removes)
                {
                    _timerNodes.Remove(timer);
                }

                _removes.Clear();
            }

            protected virtual void onTick(float dt) { }

            public void Exit()
            {
                Debug.Log($"Exit {CurState},CycleType {getCycleType()}");
                onExit();
                _timerNodes.Clear();
                _removes.Clear();
                if (CurState != EAbilityState.Ready)
                    _executor.ResetCycle(getCycleType());
            }

            protected virtual void onExit() { }

            public abstract EAbilityState GetNextState();

            public virtual bool CanExit()
            {
                return _timerNodes.Count == 0;
            }
        }

        private class InitRunCycle : AbilityRunCycle
        {
            public InitRunCycle(AbilityState state) : base(state) { }

            protected override EAbilityCycleType getCycleType()
            {
                return EAbilityCycleType.OnInit;
            }

            protected override EAbilityState getCurState() => EAbilityState.Init;

            protected override void onEnter()
            {
                //注册事件
                _executor.RegisterEventNode();
            }

            public override EAbilityState GetNextState()
            {
                return EAbilityState.Ready;
            }
        }

        /// <summary>
        /// 不暴露接口，纯逻辑态
        /// </summary>
        private class Ready : AbilityRunCycle
        {
            private EAbilityState _nextState = EAbilityState.Ready;
            protected override EAbilityState getCurState() => EAbilityState.Ready;
            public Ready(AbilityState state) : base(state) { }

            protected override EAbilityCycleType getCycleType()
            {
                return EAbilityCycleType.OnReady;
            }

            protected override void onEnter()
            {
                //进来前检测下
                onTick(0);
            }

            protected override void onTick(float dt)
            {
                if (_state.HasExecuteOrder)
                {
                    //运行前检测
                    if (_ability.GetCheckerRes(EAbilityCycleType.OnPreExecuteCheck))
                    {
                        _nextState = EAbilityState.PreExecute;
                    }
                    else
                    {
                        //检测失败
                        _state.ExecuteFailed();
                        Debug.Log("资源检测失败！");
                    }
                }
            }

            protected override void onExit() { }

            public override bool CanExit()
            {
                return _state.HasExecuteOrder;
            }

            public override EAbilityState GetNextState()
            {
                return _nextState;
            }
        }

        private class PreExecute : AbilityRunCycle
        {
            public PreExecute(AbilityState state) : base(state) { }

            protected override EAbilityCycleType getCycleType() => EAbilityCycleType.OnPreExecute;

            protected override EAbilityState getCurState() => EAbilityState.PreExecute;

            public override EAbilityState GetNextState() => EAbilityState.Executing;
        }

        private class Executing : AbilityRunCycle
        {
            private Dictionary<int, IGroupNodeProxy> _stageNodeProxies = new(8);

            public IGroupNodeProxy CurProxy;

            public int NextGroupId = -1;

            /// <summary>
            /// 存储当前组对象的timer
            /// </summary>
            private HashSet<ITimer> _groupTimer = new();

            private List<ITimer> _groupRemoveList = new List<ITimer>();

            public bool AllStageFinish = true;

            private bool _clearTimer = false;

            /// <summary>
            /// 执行期最大时长
            /// </summary>
            private float _maxTime = 10f;

            private float _startTime = 0;

            public Executing(AbilityState state) : base(state) { }

            protected override EAbilityCycleType getCycleType() => EAbilityCycleType.OnExecuting;

            protected override EAbilityState getCurState() => EAbilityState.Executing;

            public override EAbilityState GetNextState() => EAbilityState.EndExecute;

            protected override void onEnter()
            {
                _startTime = Time.realtimeSinceStartup;
            }

            protected override void onExit()
            {
                _startTime = 0;
                NextGroupId = _ability._abilityData.DefaultStartGroupId;
                CurProxy = null;
                _groupTimer.Clear();
            }

            public void AddStageProxy(IGroupNodeProxy proxy)
            {
                _stageNodeProxies.Add(proxy.GetGroupId(), proxy);
                AllStageFinish = false;
            }

            public override void TimerStart(ITimer callBack)
            {
                var node = (AbilityNode)callBack;
                if (node.NodeData.BelongGroupId == CurProxy.GetGroupId())
                {
                    _groupTimer.Add(callBack);
                }
                else
                {
                    base.TimerStart(callBack);
                }
            }

            public void CurrentGroupStop()
            {
                //清理当前阶段计时器
                _clearTimer = true;
                
                CurProxy.GroupEnd();

                if (NextGroupId != -1 && _stageNodeProxies.ContainsKey(NextGroupId))
                {
                    CurProxy = null;
                    _startTime = Time.realtimeSinceStartup;
                }
                else
                {
                    if (NextGroupId != -1)
                    {
                        Debug.LogError($"尝试切换到不存在的GroupID :{NextGroupId}");
                    }
                    CurProxy = null;
                    AllStageFinish = true;
                }
            }

            protected override void onTick(float dt)
            {
                if (_clearTimer)
                {
                    _groupTimer.Clear();
                    _clearTimer = false;
                }
                
                if (NextGroupId != -1 && CurProxy == null)
                {
                    CurProxy = _stageNodeProxies[NextGroupId];
                    CurProxy.GroupBegin();
                }
                
                //阶段定时器自己管理
                foreach (var timer in _groupTimer)
                {
                    timer.Add(dt);
                    if (timer.NeedCall())
                    {
                        timer.OnCallTimer();
                    }

                    if (timer.IsFinish())
                    {
                        _groupRemoveList.Add(timer);
                    }
                }

                foreach (var timer in _groupRemoveList)
                {
                    _groupTimer.Remove(timer);
                }

                _groupRemoveList.Clear();
            }

            public override bool CanExit()
            {
                bool timeOut = _maxTime < Time.realtimeSinceStartup - _startTime;
                if (timeOut)
                {
                    Debug.LogWarning($"Ability {_ability.Uid} Cid {_ability._abilityData.ConfigId} TimeOut");
                }

                return (base.CanExit() && AllStageFinish) || timeOut;
            }
        }

        private class EndExecute : AbilityRunCycle
        {
            public EndExecute(AbilityState state) : base(state) { }
            protected override EAbilityCycleType getCycleType() => EAbilityCycleType.OnEndExecute;

            protected override EAbilityState getCurState() => EAbilityState.EndExecute;

            public override EAbilityState GetNextState() => EAbilityState.Ready;

            protected override void onExit()
            {
                _ability.GetVariables().Clear();
            }
        }
    }
}