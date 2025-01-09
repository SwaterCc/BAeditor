#region

using Hono.Scripts.Battle.Tools.CustomAttribute;

#endregion

namespace Hono.Scripts.Battle.Event
{
    public enum EEventType
    {
        //空占位，说明没有初始化,千万不要改顺序！！
        NoInit = 0,

        [EventCheckerBinder("GetHitOnceChecker", typeof(HitDamageInfo))]
        OnHit,

        [EventCheckerBinder("GetBeHitChecker", typeof(HitDamageInfo))]
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

        OnActorEnterScene = 30,
        OnActorDead,

        [EventCheckerBinder("GetTriggerBoxEnterChecker", typeof(TriggerBoxEventInfo))]
        OnTriggerBoxEnter = 50,

        [EventCheckerBinder("GetTriggerBoxStayChecker", typeof(TriggerBoxEventInfo))]
        OnTriggerBoxStay = 51,

        [EventCheckerBinder("GetTriggerBoxExitChecker", typeof(TriggerBoxEventInfo))]
        OnTriggerBoxExit = 52,

        SkillCDBegin = 70,
        SkillCDEnd,

        OnCallMonsterGenerator = 1000,

        [EventCheckerBinder("GetMonsterAllDeadChecker", typeof(MonsterGenRtEventInfo))]
        OnMonsterGeneratorAllDead = 1001,

        //游戏流程事件
        [EventCheckerBinder("GetRoundReadyEnterChecker", typeof(RoundStateEventInfo))]
        RoundReadyEnter = 500001,

        [EventCheckerBinder("GetRoundReadyExitChecker", typeof(RoundStateEventInfo))]
        RoundReadyExit,

        [EventCheckerBinder("GetRoundRunningEnterChecker", typeof(RoundStateEventInfo))]
        RoundRunningEnter = 500011,

        [EventCheckerBinder("GetRoundRunningExitChecker", typeof(RoundStateEventInfo))]
        RoundRunningExit,

        [EventCheckerBinder("GetRoundScoreEnterChecker", typeof(RoundStateEventInfo))]
        RoundScoreEnter = 500021,

        [EventCheckerBinder("GetRoundScoreExitChecker", typeof(RoundStateEventInfo))]
        RoundScoreExit,

        [EventCheckerBinder("GetLootChecker", typeof(LootEventInfo))]
        LootDrop = 500031
    }
}