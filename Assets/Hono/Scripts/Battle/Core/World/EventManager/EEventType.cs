#region

using Hono.Scripts.Battle.Tools.CustomAttribute;

#endregion

namespace Hono.Scripts.Battle.Event
{
    /// <summary>
    /// 事件枚举
    /// 千万不要改顺序！！
    /// </summary>
    public enum EEventType
    {
        //空占位，说明没有初始化
        NoInit = 0,
        OnAttrChanged = 1,

        [EventCheckerBinder("GetHitOnceChecker", typeof(HitDamageInfoKeys))]
        OnHit,

        OnHitMakeDamage,
        
        [EventCheckerBinder("GetBeHitChecker", typeof(HitDamageInfoKeys))]
        OnBeHit,
        
        /// <summary>
        /// Unit死亡事件
        /// </summary>
        OnDead,

        [EventCheckerBinder("GetMotionBeginChecker", typeof(MotionEventInfo))]
        OnMotionBegin = 10,

        [EventCheckerBinder("GetMotionCollisionChecker", typeof(MotionEventInfo))]
        OnMoveCollision,

        [EventCheckerBinder("GetMotionEndChecker", typeof(MotionEventInfo))]
        OnMotionEnd,

        /// <summary>
        /// 触发了锁血
        /// </summary>
        OnTriggerHpLock,
        
        UseSkill = 20,

        [EventCheckerBinder("GetUseSkillSuccessChecker", typeof(SkillEventInfo))]
        OnSkillUseSuccess,

        [EventCheckerBinder("GetSkillEndChecker", typeof(SkillEventInfo))]
        OnSkillStop,
        
        OnCallMonsterGenerator = 1000,

        [EventCheckerBinder("GetMonsterAllDeadChecker", typeof(MonsterGenRtEventInfo))]
        OnMonsterGeneratorAllDead = 1001,
        
        WorldEvent = 100000,
        WarBegin,
    }
}