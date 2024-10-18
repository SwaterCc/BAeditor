using System.Collections.Generic;
using UnityEngine;

namespace Hono.Scripts.Battle
{
    public partial class Ability
    {
        private class ExecutingCycle : AbilityRunCycle
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

            public bool ForceStop = false;

            /// <summary>
            /// 执行期最大时长
            /// </summary>
            private float _maxTime = 30f;

            private float _startTime = 0;

            public ExecutingCycle(AbilityState state) : base(state) { }

            protected override EAbilityAllowEditCycle getCycleType() => EAbilityAllowEditCycle.OnExecuting;

            protected override EAbilityState getCurState() => EAbilityState.Executing;

            public override EAbilityState GetNextState() => EAbilityState.EndExecute;

            protected override void onEnter()
            {
                _startTime = Time.realtimeSinceStartup;
                ForceStop = false;
            }

            protected override void onExit()
            {
                _startTime = 0;
                NextGroupId = _executor.AbilityData.DefaultStartGroupId;
                CurProxy = null;
                _groupTimer.Clear();
				AllStageFinish = _stageNodeProxies.Count == 0;
				ForceStop = false;
			}

            public void AddStageProxy(IGroupNodeProxy proxy)
            {
                _stageNodeProxies.Add(proxy.GetGroupId(), proxy);
                AllStageFinish = _stageNodeProxies.Count == 0;
            }

            public override void OnReset() {
	            _stageNodeProxies.Clear();
            }

            public override void TimerStart(ITimer callBack)
            {
                var node = (AbilityNode)callBack;
                if (CurProxy != null && node.BelongGroupId == CurProxy.GetGroupId())
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
	            if(CurProxy == null) return;
	            
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

                if (NextGroupId >= 0 && CurProxy == null)
                {
					Debug.Log($"ability GroupBegin actor {_ability.Actor.Uid} , abilityId {_ability.ConfigId} goNextGroup {NextGroupId}");

					if(_stageNodeProxies.TryGetValue(NextGroupId,out CurProxy)) {
						
						CurProxy.GroupBegin();
					}
                }

                //阶段定时器自己管理
                foreach (var timer in _groupTimer)
                {
                    timer.Step(dt);
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
                bool timeOut = _maxTime < Time.realtimeSinceStartup - _startTime && _ability._executor.AbilityData.Type != EAbilityType.Buff;
                if (timeOut)
                {
                    Debug.LogWarning($"Ability {_ability.Uid} Cid {_executor.AbilityData.ConfigId} TimeOut");
                }

                return (base.CanExit() && AllStageFinish) || timeOut || ForceStop;
            }
        }
    }
}