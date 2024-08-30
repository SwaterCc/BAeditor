

using Hono.Scripts.Battle.Tools.CustomAttribute;

namespace Hono.Scripts.Battle.Event
{
    public enum EBattleEventType
    {
        //空占位，说明没有初始化
        NoInit = 0,
        
        //周期回调
        [EventCheckerBinder("GetEmptyChecker")]
        ActorStateEnter,
        ActorStateExit,
        //事件
        [EventCheckerBinder("GetHitChecker")]
        Hit,
        [EventCheckerBinder("GetMotionChecker")]
        MotionBegin,
        [EventCheckerBinder("GetMotionChecker")]
        MotionEnd,
    }
}