using UnityEngine;

namespace Hono.Scripts.Battle
{
    public partial class Ability
    {
        /// <summary>
        /// 不暴露接口，纯逻辑态
        /// </summary>
        private class ReadyCycle : AbilityRunCycle
        {
            private EAbilityState _nextState = EAbilityState.Ready;
            protected override EAbilityState getCurState() => EAbilityState.Ready;

            private readonly PreExecuteChecker _checker;

            public ReadyCycle(AbilityState state) : base(state)
            {
                _checker = new PreExecuteChecker(_ability);
            }

            protected override EAbilityAllowEditCycle getCycleType()
            {
                return EAbilityAllowEditCycle.OnReady;
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
                    if (_checker.GetCheckRes())
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
    }
 
}