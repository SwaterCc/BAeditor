

using Hono.Scripts.Battle.Tools.CustomAttribute;

namespace Hono.Scripts.Battle.Event
{
    public enum EBattleEventType
    {
        //空占位，说明没有初始化
        NoInit = 0,
       
        [EventCheckerBinder("GetHitChecker",typeof(HitInfo))]
        OnHit,
        OnBeHit,
        
        UseSkill,
        OnSkillUseSuccess,
    }
}