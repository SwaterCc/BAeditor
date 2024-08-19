using Battle.Tools.CustomAttribute;

namespace Battle.Event
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
    
    /// <summary>
    /// 注意函数有规范，第一个参数必须是EBattleEventType
    /// </summary>
    public static class EventCheckerCreator
    {
        public static EventChecker GetEmptyChecker(EBattleEventType eventType)
        {
            return new EmptyChecker(eventType);
        }
        
        public static EventChecker GetHitChecker(EBattleEventType eventType,int hitId,bool checkActor,bool checkAbility)
        {
            int actorId = checkActor ? Ability.Context.BelongActor.Uid : -1;
            int abilityId = checkAbility ? Ability.Context.CurrentAbility.Uid : -1;
            return new HitEventChecker(null,hitId,actorId,abilityId);
        }
        
        public static EventChecker GetMotionChecker(EBattleEventType eventType,int motionId)
        {
            return new MotionEventChecker(eventType, motionId, null);
        }
    }

}