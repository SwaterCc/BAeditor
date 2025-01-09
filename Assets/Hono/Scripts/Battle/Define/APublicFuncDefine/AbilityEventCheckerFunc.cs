#region

using Hono.Scripts.Battle.Event;
using Hono.Scripts.Battle.Tools.CustomAttribute;

#endregion

namespace Hono.Scripts.Battle
{
    public partial class AFunctionDefine
    {
        [AbilityFunction(false)]
        public EventChecker GetHitOnceChecker(bool listenAllHitEvent,
            bool listenAllAbility, int damageConfigId = 0)
        {
            var abilityId = listenAllAbility ? -1 : AContext.Id;
            var checker = new HitEventChecker(EEventType.OnHit, Actor, abilityId,
                damageConfigId);
            checker.SetIsListenAll(listenAllHitEvent);
            return checker;
        }

        [AbilityFunction(false)]
        public  EventChecker GetBeHitChecker(bool listenAllHitEvent,
            bool listenAllAbility, int damageConfigId = 0)
        {
            var abilityId = listenAllAbility ? -1 : AContext.Id;
            var checker = new HitEventChecker(EEventType.OnBeHit, Actor, abilityId,
                damageConfigId);
            checker.SetIsListenAll(listenAllHitEvent);
            return checker;
        }

        [AbilityFunction(false)]
        public  EventChecker GetMotionBeginChecker(int motionId)
        {
            return new MotionEventChecker(EEventType.OnMotionBegin, Actor, motionId, null);
        }

        [AbilityFunction(false)]
        public  EventChecker GetMotionEndChecker(int motionId)
        {
            return new MotionEventChecker(EEventType.OnMotionEnd, Actor, motionId, null);
        }

        [AbilityFunction(false)]
        public  EventChecker GetMotionCollisionChecker(int motionId)
        {
            return new MotionEventChecker(EEventType.OnMoveCollision, Actor, motionId,
                null);
        }

        [AbilityFunction(false)]
        public  EventChecker GetUseSkillSuccessChecker(int skillId)
        {
            if (skillId == 0) skillId = AContext.Id;
            return new UseSkillChecker(EEventType.OnSkillUseSuccess, Actor, skillId,
                null);
        }

        [AbilityFunction(false)]
        public  EventChecker GetSkillEndChecker(int skillId)
        {
            if (skillId == 0) skillId = AContext.Id;
            return new UseSkillChecker(EEventType.OnSkillStop, Actor, skillId,
                null);
        }

        [AbilityFunction(false)]
        public  EventChecker GetTriggerBoxEnterChecker()
        {
            return new TriggerBoxChecker(EEventType.OnTriggerBoxEnter, Actor);
        }

        [AbilityFunction(false)]
        public  EventChecker GetTriggerBoxStayChecker()
        {
            return new TriggerBoxChecker(EEventType.OnTriggerBoxStay, Actor);
        }

        [AbilityFunction(false)]
        public  EventChecker GetTriggerBoxExitChecker()
        {
            return new TriggerBoxChecker(EEventType.OnTriggerBoxExit, Actor);
        }

        [AbilityFunction(false)]
        public  EventChecker GetRoundReadyEnterChecker()
        {
            return new RoundStateChecker(EEventType.RoundReadyEnter);
        }

        [AbilityFunction(false)]
        public  EventChecker GetRoundReadyExitChecker()
        {
            return new RoundStateChecker(EEventType.RoundReadyExit);
        }

        [AbilityFunction(false)]
        public  EventChecker GetRoundRunningEnterChecker()
        {
            return new RoundStateChecker(EEventType.RoundRunningEnter);
        }

        [AbilityFunction(false)]
        public  EventChecker GetRoundRunningExitChecker()
        {
            return new RoundStateChecker(EEventType.RoundRunningExit);
        }

        [AbilityFunction(false)]
        public  EventChecker GetRoundScoreEnterChecker()
        {
            return new RoundStateChecker(EEventType.RoundScoreEnter);
        }

        [AbilityFunction(false)]
        public  EventChecker GetRoundScoreExitChecker()
        {
            return new RoundStateChecker(EEventType.RoundScoreExit);
        }

        [AbilityFunction(false)]
        public  EventChecker GetLootChecker()
        {
            return new LootEventChecker(EEventType.LootDrop);
        }

        [AbilityFunction(false)]
        public  EventChecker GetMonsterAllDeadChecker(int generatorUid, int configId, int roundCount)
        {
            return new MonsterGenRtChecker(EEventType.OnMonsterGeneratorAllDead, generatorUid, configId,
                roundCount);
        }
    }
}