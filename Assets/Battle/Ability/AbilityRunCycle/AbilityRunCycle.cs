using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Battle
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

            private readonly HashSet<ITimer> _timerNodes;
            private readonly List<ITimer> _removes;
            public EAbilityState CurState => getCurState();

            protected AbilityRunCycle(Ability ability)
            {
                _ability = ability;
                _executor = ability._executor;
                _timerNodes = new HashSet<ITimer>();
                _removes = new List<ITimer>();
            }

            public void TimerStart(ITimer callBack)
            {
                _timerNodes.Add(callBack);
            }

            protected abstract EAbilityCycleType getCycleType();
            protected abstract EAbilityState getCurState();

            public void Enter()
            {
                onEnter();
                _executor.ExecuteCycleNode(getCycleType());
            }

            protected virtual void onEnter() { }

            public virtual void Tick(float dt)
            {
                foreach (var timer in _timerNodes)
                {
                    if (timer.IsFinish())
                    {
                        _removes.Add(timer);
                    }
                    else
                    {
                        timer.Add(dt);
                        if (timer.NeedCall())
                        {
                            timer.OnCallTimer();
                        }
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
                onExit();

                _timerNodes.Clear();
                _removes.Clear();
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
            public InitRunCycle(Ability ability) : base(ability) { }

            protected override EAbilityCycleType getCycleType()
            {
                return EAbilityCycleType.OnInit;
            }

            protected override EAbilityState getCurState() => EAbilityState.Init;

            protected override void onEnter()
            {
                //注册事件
                _executor.RegisterEventNode();

                //初始化特化模板数据
                //...
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
            public Ready(Ability ability) : base(ability) { }
            
            protected override EAbilityCycleType getCycleType()
            {
                return EAbilityCycleType.OnReady;
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

            protected override void onExit()
            {
                _nextState = EAbilityState.Ready;
            }

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
            public PreExecute(Ability ability) : base(ability) { }
            
            protected override EAbilityCycleType getCycleType() => EAbilityCycleType.OnPreExecute;

            protected override EAbilityState getCurState() => EAbilityState.PreExecute;

            public override EAbilityState GetNextState() => EAbilityState.Executing;
        }

        private class Executing : AbilityRunCycle
        {
            private Dictionary<int, IStageNodeProxy> _stageNodeProxies = new(8);

            public IStageNodeProxy CurProxy;

            public bool AllStageFinish;
            
            public Executing(Ability ability) : base(ability) { }
            
            protected override EAbilityCycleType getCycleType() => EAbilityCycleType.OnExecuting;

            protected override EAbilityState getCurState() => EAbilityState.Executing;

            public override EAbilityState GetNextState() => EAbilityState.EndExecute;

            public void AddStageProxy(IStageNodeProxy proxy)
            {
                _stageNodeProxies.Add(proxy.GetId(),proxy);
            }
            
            public override bool CanExit()
            {
                return base.CanExit() && AllStageFinish;
            }
        }

        private class EndExecute  : AbilityRunCycle
        {
            public EndExecute(Ability ability) : base(ability) { }
            protected override EAbilityCycleType getCycleType() => EAbilityCycleType.OnEndExecute;

            protected override EAbilityState getCurState() => EAbilityState.EndExecute;

            public override EAbilityState GetNextState() => EAbilityState.Ready;
        }
    }
}