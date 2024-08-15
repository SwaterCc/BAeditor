using System;
using Battle.Def;
using Battle.Tools;

namespace Battle
{
    public partial class Ability
    {
        private class AbilityEventNode : AbilityNode
        {
            private readonly EventNodeData _eventData;

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
                        checker = new TagEventChecker(_eventData.Tag, null);
                        break;
                    case EAbilityEventType.Hit:
                        checker = new HitEventChecker(_eventData.HitBoxId, null);
                        break;
                    case EAbilityEventType.MotionBegin:
                    case EAbilityEventType.MotionEnd:
                        checker = new MotionEventChecker(_eventData.EventType, _eventData.MotionId, null);
                        break;
                }

                BattleEventMgr.Instance.Register(checker);
            }


            public override void DoJob()
            {
                
            }
        }
    }
}