using System;
using System.Collections.Generic;

namespace Battle
{
    public partial class Ability
    {
        private interface IWaitCallBack
        {
            public bool IsWaiting();
            public void Add(float dt);
            public void OnCallBack();
        }
        
        private abstract class AbilityRunCycle
        {
            protected Ability _ability;
            protected AbilityExecutor _executor;
            protected AbilityState _state;
            public bool IsFinish;
            public 
            private IWaitCallBack _waitNode;
            protected AbilityRunCycle(Ability ability)
            {
                _ability = ability;
                _executor = ability._executor;
                IsFinish = false;
            }

            public void Wait(IWaitCallBack callBack)
            {
                _waitNode = callBack;
            }
            
            protected abstract EAbilityCycleType getCycleType();

            public virtual void OnEnter()
            {
                _executor.ExecuteCycleNode(getCycleType());
            }

            public virtual void OnTick(float dt)
            {
                if (_waitNode != null )
                {
                    if (_waitNode.IsWaiting())
                    {
                        _waitNode.Add(dt);
                    }
                    else
                    {
                        _waitNode.OnCallBack();
                        _waitNode = null;
                    }
                }
            }
            public virtual void OnExit() { }

            public abstract EAbilityState GetNextState();
        }

        private class InitRunCycle : AbilityRunCycle
        {
            public InitRunCycle(Ability ability) : base(ability) { }

            protected override EAbilityCycleType getCycleType()
            {
                return EAbilityCycleType.OnInit;
            }

            public override void OnEnter()
            {
                //注册事件
                _executor.RegisterEventNode();
                
                base.OnEnter();
            }

            public override void OnTick(float dt)
            {
                //初始化特化模板数据
                
            }

            public override EAbilityState GetNextState()
            {
                return EAbilityState.Ready;
            }
        }

        /// <summary>
        /// 不暴露接口，纯逻辑态
        /// </summary>
        private class Ready { }

        private class PreExecute { }

        private class Executing { }

        private class EndExecute { }
    }
}