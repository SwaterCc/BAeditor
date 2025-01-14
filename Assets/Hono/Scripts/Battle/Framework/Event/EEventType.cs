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

        [EventCheckerBinder("GetHitOnceChecker", typeof(HitDamageInfoKeys))]
        OnHit,

        [EventCheckerBinder("GetBeHitChecker", typeof(HitDamageInfoKeys))]
        OnBeHit,

        [EventCheckerBinder("GetMotionBeginChecker", typeof(MotionEventInfo))]
        OnMotionBegin = 10,

        [EventCheckerBinder("GetMotionCollisionChecker", typeof(MotionEventInfo))]
        OnMoveCollision,

        [EventCheckerBinder("GetMotionEndChecker", typeof(MotionEventInfo))]
        OnMotionEnd,

        UseSkill = 20,

        [EventCheckerBinder("GetUseSkillSuccessChecker", typeof(UsedSkillEventInfo))]
        OnSkillUseSuccess,

        [EventCheckerBinder("GetSkillEndChecker", typeof(UsedSkillEventInfo))]
        OnSkillStop,
        
        OnCallMonsterGenerator = 1000,

        [EventCheckerBinder("GetMonsterAllDeadChecker", typeof(MonsterGenRtEventInfo))]
        OnMonsterGeneratorAllDead = 1001,
    }
}