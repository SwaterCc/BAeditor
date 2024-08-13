using System;
using Battle.Def;
using Battle.Tools;

namespace Battle
{
    public partial class Ability
    {
        private class AbilityEventNode : AbilityNode
        {
            private EventNodeData _eventData;

            public AbilityEventNode(AbilityExecutor executor, AbilityNodeData data) : base(executor, data)
            {
                _eventData = data.EventNodeData;
            }

            public void RegisterEvent()
            {
                IEventChecker checker = null;
                //注册事件
                switch (_eventData.EventType)
                {
                    case EAbilityEventType.Tag:
                        checker = new TagEventChecker(_eventData.Tag, OnEventFired);
                        break;
                    case EAbilityEventType.Hit:
                        checker = new HitEventChecker(_eventData.HitBoxId, OnEventFired);
                        break;
                    case EAbilityEventType.MotionBegin:
                    case EAbilityEventType.MotionEnd:
                        checker = new MotionEventChecker(_eventData.EventType, _eventData.MotionId, OnEventFired);
                        break;
                }

                BattleEventMgr.Instance.Register(checker);
            }

            public void OnEventFired()
            {
                //执行
                _executor.RunNode(this);
            }


            public override void DoJob()
            {
                JobFinsh = true;
            }
        }
    }
}