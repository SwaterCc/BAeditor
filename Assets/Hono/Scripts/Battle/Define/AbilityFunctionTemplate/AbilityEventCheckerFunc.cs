#region

using Hono.Scripts.Battle.Event;
using Hono.Scripts.Battle.Tools.CustomAttribute;

#endregion

namespace Hono.Scripts.Battle
{
    public partial class AbilityFunctionDefine
    {
        [AbilityFunction(false)]
        public static EventChecker GetHitOnceChecker(bool listenAllHitEvent,
            bool listenAllAbility, int damageConfigId = 0)
        {
            var abilityId = listenAllAbility ? -1 : ARunningTime.AContext.Id;
            var checker = new HitEventChecker(EBattleEventType.OnHit, ARunningTime.Actor, abilityId,
                damageConfigId);
            checker.SetIsListenAll(listenAllHitEvent);
            return checker;
        }

        [AbilityFunction(false)]
        public static EventChecker GetBeHitChecker(bool listenAllHitEvent,
            bool listenAllAbility, int damageConfigId = 0)
        {
            var abilityId = listenAllAbility ? -1 : ARunningTime.AContext.Id;
            var checker = new HitEventChecker(EBattleEventType.OnBeHit, ARunningTime.Actor, abilityId,
                damageConfigId);
            checker.SetIsListenAll(listenAllHitEvent);
            return checker;
        }

        [AbilityFunction(false)]
        public static EventChecker GetMotionBeginChecker(int motionId)
        {
            return new MotionEventChecker(EBattleEventType.OnMotionBegin, ARunningTime.Actor, motionId, null);
        }

        [AbilityFunction(false)]
        public static EventChecker GetMotionEndChecker(int motionId)
        {
            return new MotionEventChecker(EBattleEventType.OnMotionEnd, ARunningTime.Actor, motionId, null);
        }

        [AbilityFunction(false)]
        public static EventChecker GetMotionCollisionChecker(int motionId)
        {
            return new MotionEventChecker(EBattleEventType.OnMoveCollision, ARunningTime.Actor, motionId,
                null);
        }

        [AbilityFunction(false)]
        public static EventChecker GetUseSkillSuccessChecker(int skillId)
        {
            if (skillId == 0) skillId = ARunningTime.AContext.Id;
            return new UseSkillChecker(EBattleEventType.OnSkillUseSuccess, ARunningTime.Actor, skillId,
                null);
        }

        [AbilityFunction(false)]
        public static EventChecker GetSkillEndChecker(int skillId)
        {
            if (skillId == 0) skillId = ARunningTime.AContext.Id;
            return new UseSkillChecker(EBattleEventType.OnSkillStop, ARunningTime.Actor, skillId,
                null);
        }

        [AbilityFunction(false)]
        public static EventChecker GetTriggerBoxEnterChecker()
        {
            return new TriggerBoxChecker(EBattleEventType.OnTriggerBoxEnter, ARunningTime.Actor);
        }

        [AbilityFunction(false)]
        public static EventChecker GetTriggerBoxStayChecker()
        {
            return new TriggerBoxChecker(EBattleEventType.OnTriggerBoxStay, ARunningTime.Actor);
        }

        [AbilityFunction(false)]
        public static EventChecker GetTriggerBoxExitChecker()
        {
            return new TriggerBoxChecker(EBattleEventType.OnTriggerBoxExit, ARunningTime.Actor);
        }

        [AbilityFunction(false)]
        public static EventChecker GetRoundReadyEnterChecker()
        {
            return new RoundStateChecker(EBattleEventType.RoundReadyEnter);
        }

        [AbilityFunction(false)]
        public static EventChecker GetRoundReadyExitChecker()
        {
            return new RoundStateChecker(EBattleEventType.RoundReadyExit);
        }

        [AbilityFunction(false)]
        public static EventChecker GetRoundRunningEnterChecker()
        {
            return new RoundStateChecker(EBattleEventType.RoundRunningEnter);
        }

        [AbilityFunction(false)]
        public static EventChecker GetRoundRunningExitChecker()
        {
            return new RoundStateChecker(EBattleEventType.RoundRunningExit);
        }

        [AbilityFunction(false)]
        public static EventChecker GetRoundScoreEnterChecker()
        {
            return new RoundStateChecker(EBattleEventType.RoundScoreEnter);
        }

        [AbilityFunction(false)]
        public static EventChecker GetRoundScoreExitChecker()
        {
            return new RoundStateChecker(EBattleEventType.RoundScoreExit);
        }

        [AbilityFunction(false)]
        public static EventChecker GetLootChecker()
        {
            return new LootEventChecker(EBattleEventType.LootDrop);
        }

        [AbilityFunction(false)]
        public static EventChecker GetMonsterAllDeadChecker(int generatorUid, int configId, int roundCount)
        {
            return new MonsterGenRtChecker(EBattleEventType.OnMonsterGeneratorAllDead, generatorUid, configId,
                roundCount);
        }
    }
}