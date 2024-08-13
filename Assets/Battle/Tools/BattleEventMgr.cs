using System;
using System.Collections.Generic;


namespace Battle.Tools
{
    public class BattleEventHandle
    {
        private readonly Action _onFire;

        public BattleEventHandle(Action onFire)
        {
            _onFire = onFire;
        }

        public void TryFire()
        {
            _onFire?.Invoke();
        }
    }

    public interface IEventChecker
    {
        public EAbilityEventType EventType { get; }
        public bool compare(int i);

        public void Invoke();
    }

    public abstract class EventChecker : IEventChecker
    {
        public EAbilityEventType EventType { get; }

        private Action _func;

        public EventChecker(Action func)
        {
            _func = func;
        }

        public abstract bool compare(int i);

        public virtual void Invoke()
        {
            _func?.Invoke();
        }
    }

    public class TagEventChecker : EventChecker
    {
        public EAbilityEventType EventType => EAbilityEventType.Tag;
        private readonly Tag _tag;

        public TagEventChecker(int[] tag, Action func) : base(func)
        {
            _tag = new Tag(tag);
        }

        public override bool compare(int tag)
        {
            return _tag.HasTag(tag);
        }
    }

    /// <summary>
    /// 打击点默认检测来源于该能力的打击点，全局打击点请用tag
    /// </summary>
    public class HitEventChecker : EventChecker
    {
        public EAbilityEventType EventType => EAbilityEventType.Hit;

        private int _hitConfigId;
        private Action _func;

        public HitEventChecker(int hitConfigId, Action func) : base(func)
        {
            _hitConfigId = hitConfigId;
            _func = func;
        }

        public override bool compare(int i)
        {
            return true;
        }
    }

    public class MotionEventChecker : EventChecker
    {
        private EAbilityEventType _eventType;
        public EAbilityEventType EventType => _eventType;

        private int _motionId;

        public MotionEventChecker(EAbilityEventType eventType, int motionId, Action func) : base(func)
        {
            _eventType = eventType;
            _motionId = motionId;
        }

        public override bool compare(int i)
        {
            return true;
        }
    }

    /// <summary>
    /// 管理战斗逻辑中事件节点的注册和监听
    /// </summary>
    public class BattleEventMgr
    {
        private static BattleEventMgr _instance;

        public static BattleEventMgr Instance
        {
            get
            {
                //TODO:目前简单写，后续要判定线程加锁 @shirui
                return _instance ??= new BattleEventMgr();
            }
        }

        /// <summary>
        /// 事件注册列表
        /// </summary>
        private Dictionary<EAbilityEventType, List<BattleEventHandle>> _eventDict = new();

        /// <summary>
        /// 
        /// </summary>
        public void Register(EAbilityEventType type, BattleEventHandle handle)
        {
            if (!_eventDict.TryGetValue(type, out var handles))
            {
                handles = new List<BattleEventHandle>();
                _eventDict.Add(type, handles);
            }

            if (!handles.Contains(handle))
            {
                handles.Add(handle);
            }
            else
            {
                //发个warning
            }
        }

        public void Register(IEventChecker checker) { }

        /// <summary>
        /// 事件触发
        /// </summary>
        public void Fired() { }

        /// <summary>
        /// 事件触发
        /// </summary>
        public void Fired<TKey>(TKey key) { }
    }
}