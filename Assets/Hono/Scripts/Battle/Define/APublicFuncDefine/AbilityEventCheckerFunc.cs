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
            var checker = new HitEventChecker(EBattleEventType.OnHit, Actor, abilityId,
                damageConfigId);
            checker.SetIsListenAll(listenAllHitEvent);
            return checker;
        }

        [AbilityFunction(false)]
        public  EventChecker GetBeHitChecker(bool listenAllHitEvent,
            bool listenAllAbility, int damageConfigId = 0)
        {
            var abilityId = listenAllAbility ? -1 : AContext.Id;
            var checker = new HitEventChecker(EBattleEventType.OnBeHit, Actor, abilityId,
                damageConfigId);
            checker.SetIsListenAll(listenAllHitEvent);
            return checker;
        }

        [AbilityFunction(false)]
        public  EventChecker GetMotionBeginChecker(int motionId)
        {
            return new MotionEventChecker(EBattleEventType.OnMotionBegin, Actor, motionId, null);
        }

        [AbilityFunction(false)]
        public  EventChecker GetMotionEndChecker(int motionId)
        {
            return new MotionEventChecker(EBattleEventType.OnMotionEnd, Actor, motionId, null);
        }

        [AbilityFunction(false)]
        public  EventChecker GetMotionCollisionChecker(int motionId)
        {
            return new MotionEventChecker(EBattleEventType.OnMoveCollision, Actor, motionId,
                null);
        }

        [AbilityFunction(false)]
        public  EventChecker GetUseSkillSuccessChecker(int skillId)
        {
            if (skillId == 0) skillId = AContext.Id;
            return new UseSkillChecker(EBattleEventType.OnSkillUseSuccess, Actor, skillId,
                null);
        }

        [AbilityFunction(false)]
        public  EventChecker GetSkillEndChecker(int skillId)
        {
            if (skillId == 0) skillId = AContext.Id;
            return new UseSkillChecker(EBattleEventType.OnSkillStop, Actor, skillId,
                null);
        }

        [AbilityFunction(false)]
        public  EventChecker GetTriggerBoxEnterChecker()
        {
            return new TriggerBoxChecker(EBattleEventType.OnTriggerBoxEnter, Actor);
        }

        [AbilityFunction(false)]
        public  EventChecker GetTriggerBoxStayChecker()
        {
            return new TriggerBoxChecker(EBattleEventType.OnTriggerBoxStay, Actor);
        }

        [AbilityFunction(false)]
        public  EventChecker GetTriggerBoxExitChecker()
        {
            return new TriggerBoxChecker(EBattleEventType.OnTriggerBoxExit, Actor);
        }

        [AbilityFunction(false)]
        public  EventChecker GetRoundReadyEnterChecker()
        {
            return new RoundStateChecker(EBattleEventType.RoundReadyEnter);
        }

        [AbilityFunction(false)]
        public  EventChecker GetRoundReadyExitChecker()
        {
            return new RoundStateChecker(EBattleEventType.RoundReadyExit);
        }

        [AbilityFunction(false)]
        public  EventChecker GetRoundRunningEnterChecker()
        {
            return new RoundStateChecker(EBattleEventType.RoundRunningEnter);
        }

        [AbilityFunction(false)]
        public  EventChecker GetRoundRunningExitChecker()
        {
            return new RoundStateChecker(EBattleEventType.RoundRunningExit);
        }

        [AbilityFunction(false)]
        public  EventChecker GetRoundScoreEnterChecker()
        {
            return new RoundStateChecker(EBattleEventType.RoundScoreEnter);
        }

        [AbilityFunction(false)]
        public  EventChecker GetRoundScoreExitChecker()
        {
            return new RoundStateChecker(EBattleEventType.RoundScoreExit);
        }

        [AbilityFunction(false)]
        public  EventChecker GetLootChecker()
        {
            return new LootEventChecker(EBattleEventType.LootDrop);
        }

        [AbilityFunction(false)]
        public  EventChecker GetMonsterAllDeadChecker(int generatorUid, int configId, int roundCount)
        {
            return new MonsterGenRtChecker(EBattleEventType.OnMonsterGeneratorAllDead, generatorUid, configId,
                roundCount);
        }
    }
}