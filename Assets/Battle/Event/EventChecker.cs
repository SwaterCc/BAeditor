using System;
using Battle.Tools;

namespace Battle.Event
{
    public interface IEventChecker
    {
        public EBattleEventType EventType { get; }
        public bool compare(IEventInfo info);
        public void Invoke(IEventInfo info);
    }

    public abstract class EventChecker : IEventChecker
    {
        public EBattleEventType EventType { get; protected set; }

        private Action<IEventInfo> _func;

        private Type _bindEventInfo;

        private bool _isDisable;

        public EventChecker(Action<IEventInfo> func, Type bindEventInfo = null)
        {
            _func = func;
            _bindEventInfo = bindEventInfo;
            _isDisable = false;
        }

        public void BindFunc(Action<IEventInfo> func)
        {
            _func ??= func;
        }

        public void BindEventInfoType<TEventInfo>()
        {
            _bindEventInfo = typeof(TEventInfo);
        }

        public void SetDisable(bool flag)
        {
            _isDisable = flag;
        }

        public abstract bool compare(IEventInfo info);

        public virtual void Invoke(IEventInfo info)
        {
            if (_isDisable) return;
            _func?.Invoke(info);
        }

        public void UnRegister()
        {
            BattleEventManager.Instance.UnRegister(this);
        }
    }

    /// <summary>
    /// 打击点默认检测来源于该能力的打击点，全局打击点请用tag
    /// </summary>
    public class HitEventChecker : EventChecker
    {
        /// <summary>
        /// 打击点ID
        /// </summary>
        private int _hitConfigId;

        /// <summary>
        /// 属于的Actor
        /// </summary>
        private int _belongActorId;

        /// <summary>
        /// 来源的能力
        /// </summary>
        private int _sourceAbility;

        public HitEventChecker(Action<IEventInfo> func = null, int hitConfigId = -1, int belongActorId = -1,
            int sourceAbility = -1) :
            base(func)
        {
            EventType = EBattleEventType.Hit;
            _hitConfigId = hitConfigId;
            _belongActorId = belongActorId;
            _sourceAbility = sourceAbility;
        }

        public override bool compare(IEventInfo info)
        {
            var res = true;
            var hitInfo = (HitEventInfo)info;
            if (_hitConfigId != -1)
            {
                res = (res && hitInfo.HitConfigId == _hitConfigId);
            }

            if (_sourceAbility != -1)
            {
                res = (res && hitInfo.AbilityUId == _sourceAbility);
            }

            if (_belongActorId != -1)
            {
                res = (res && hitInfo.ActorId == _belongActorId);
            }

            return res;
        }
    }

    public class MotionEventChecker : EventChecker
    {
        private int _motionId;

        public MotionEventChecker(EBattleEventType eventType, int motionId, Action<IEventInfo> func) : base(func)
        {
            EventType = eventType;
            _motionId = motionId;
        }

        public override bool compare(IEventInfo info)
        {
            return true;
        }
    }
}