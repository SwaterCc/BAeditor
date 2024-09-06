using System;

namespace Hono.Scripts.Battle.Event
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

        /// <summary>
        /// 检测通过后调用函数
        /// </summary>
        private Action<IEventInfo> _func;

        /// <summary>
        /// 绑定的数据类型
        /// </summary>
        private Type _bindEventInfo;

        private bool _isDisable;

        protected EventChecker(Action<IEventInfo> func = null, Type bindEventInfo = null)
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


    public class EmptyChecker : EventChecker
    {
        public EmptyChecker(EBattleEventType eventType, Action<IEventInfo> func = null, Type bindEventInfo = null) :
            base(func, bindEventInfo)
        {
            EventType = eventType;
        }

        public override bool compare(IEventInfo info)
        {
            return true;
        }
    }

    public class UseSkillChecker : EventChecker
    {
        private readonly int _uid;

        public UseSkillChecker(int uid) : base(null,
            typeof(SkillUsedEventInfo))
        {
            EventType = EBattleEventType.UseSkill;
            _uid = uid;
            BindFunc(useSkill);
        }

        private void useSkill(IEventInfo eventInfo)
        {
            var actor = ActorManager.Instance.GetActor(_uid);
            if (actor != null && actor.Logic.TryGetComponent<ActorLogic.SkillComp>(out var skillComp))
            {
                skillComp.UseSkill(((SkillUsedEventInfo)eventInfo).SkillId);
            }
        }

        public override bool compare(IEventInfo info)
        {
            var skillUsedEventInfo = (SkillUsedEventInfo)info;
            if (_uid == skillUsedEventInfo.ActorUid)
            {
                return true;
            }

            return false;
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