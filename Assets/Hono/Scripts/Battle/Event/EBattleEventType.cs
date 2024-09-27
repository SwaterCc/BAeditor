

using Hono.Scripts.Battle.Tools.CustomAttribute;

namespace Hono.Scripts.Battle.Event
{
    public enum EBattleEventType
    {
        //空占位，说明没有初始化,千万不要改顺序！！
        NoInit = 0,
       
        [EventCheckerBinder("GetHitChecker",typeof(HitDamageInfo))]
        OnHitDamage,
        [EventCheckerBinder("GetHitOnceChecker",typeof(HitInfo))]
        OnHit,
        [EventCheckerBinder("GetBeHitChecker",typeof(HitDamageInfo))]
        OnBeHit,
        
        [EventCheckerBinder("GetMotionBeginChecker",typeof(MotionEventInfo))]
        OnMotionBegin = 10,
        [EventCheckerBinder("GetMotionCollisionChecker",typeof(MotionEventInfo))]
        OnMoveCollision,
        [EventCheckerBinder("GetMotionEndChecker",typeof(MotionEventInfo))]
        OnMotionEnd,
        
        UseSkill = 20,
        [EventCheckerBinder("GetUseSkillSuccessChecker",typeof(UsedSkillEventInfo))]
        OnSkillUseSuccess,
        [EventCheckerBinder("GetSkillEndChecker",typeof(UsedSkillEventInfo))]
        OnSkillStop,
    }
}