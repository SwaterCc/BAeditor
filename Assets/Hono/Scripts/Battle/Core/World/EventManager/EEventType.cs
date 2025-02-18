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
        [AbilityEventBind(typeof(AttrChangedEventInfo), "GetAttrChangedEventChecker")]
        OnAttrChanged,
        [AbilityEventBind(typeof(HitInfoKey), "GetHitEventChecker")]
        OnHit,
        [AbilityEventBind(typeof(MakeDamageInfoKeys), "GetHitEventChecker")]
        OnMakeDamage,
        [AbilityEventBind(typeof(MakeDamageInfoKeys), "GetHitEventChecker")]
        OnBeHit,

        /// <summary>
        /// Unit死亡事件
        /// </summary>
        OnDead,

        OnMotionBegin,

        OnMoveCollision,

        OnMotionEnd,

        /// <summary>
        /// 触发了锁血
        /// </summary>
        OnTriggerHpLock,

        OnAddVFXByKey,
        OnAddVFXByPath,

        UseSkill = 20,

        [AbilityEventBind(typeof(SkillEventInfo), "GetSkillEventChecker")]
        OnSkillUsed,

        [AbilityEventBind(typeof(SkillEventInfo), "GetSkillEventChecker")]
        OnSkillStop,

        OnCallMonsterGenerator = 1000,

        OnMonsterGeneratorAllDead = 1001,

        WorldEvent = 100000,
        WarBegin,
    }
}