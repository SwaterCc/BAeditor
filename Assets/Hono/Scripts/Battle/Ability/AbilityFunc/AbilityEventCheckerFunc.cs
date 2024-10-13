using Hono.Scripts.Battle.Event;
using Hono.Scripts.Battle.Tools.CustomAttribute;

namespace Hono.Scripts.Battle
{
    public partial class AbilityFunction
    {
        [AbilityMethod(false)]
        public static EventChecker GetHitChecker(EBattleEventType battleEventType, bool listenAllHitEvent,
            bool listenAllAbility, int damageConfigId = 0)
        {
            var abilityId = listenAllAbility ? -1 : Ability.Context.Invoker.ConfigId;
            var checker = new HitEventChecker(EBattleEventType.OnHitDamage, Ability.Context.SourceActor, abilityId,
                damageConfigId);
            checker.SetIsListenAll(listenAllHitEvent);
            return checker;
        }

        [AbilityMethod(false)]
        public static EventChecker GetHitOnceChecker(bool listenAllHitEvent,
            bool listenAllAbility, int damageConfigId = 0)
        {
            var abilityId = listenAllAbility ? -1 : Ability.Context.Invoker.ConfigId;
            var checker = new HitEventChecker(EBattleEventType.OnHit, Ability.Context.SourceActor, abilityId,
                damageConfigId);
            checker.SetIsListenAll(listenAllHitEvent);
            return checker;
        }

        [AbilityMethod(false)]
        public static EventChecker GetBeHitChecker(bool listenAllHitEvent,
            bool listenAllAbility, int damageConfigId = 0)
        {
            var abilityId = listenAllAbility ? -1 : Ability.Context.Invoker.ConfigId;
            var checker = new HitEventChecker(EBattleEventType.OnBeHit, Ability.Context.SourceActor, abilityId,
                damageConfigId);
            checker.SetIsListenAll(listenAllHitEvent);
            return checker;
        }

        [AbilityMethod(false)]
        public static EventChecker GetMotionBeginChecker(int motionId)
        {
            return new MotionEventChecker(EBattleEventType.OnMotionBegin, Ability.Context.SourceActor, motionId, null);
        }

        [AbilityMethod(false)]
        public static EventChecker GetMotionEndChecker(int motionId)
        {
            return new MotionEventChecker(EBattleEventType.OnMotionEnd, Ability.Context.SourceActor, motionId, null);
        }

        [AbilityMethod(false)]
        public static EventChecker GetMotionCollisionChecker(int motionId)
        {
            return new MotionEventChecker(EBattleEventType.OnMoveCollision, Ability.Context.SourceActor, motionId,
                null);
        }

        [AbilityMethod(false)]
        public static EventChecker GetUseSkillSuccessChecker(int skillId)
        {
            if (skillId == 0) skillId = Ability.Context.Invoker.ConfigId;
            return new UseSkillChecker(EBattleEventType.OnSkillUseSuccess, Ability.Context.SourceActor, skillId,
                null);
        }

        [AbilityMethod(false)]
        public static EventChecker GetSkillEndChecker(int skillId)
        {
            if (skillId == 0) skillId = Ability.Context.Invoker.ConfigId;
            return new UseSkillChecker(EBattleEventType.OnSkillStop, Ability.Context.SourceActor, skillId,
                null);
        }

        [AbilityMethod(false)]
        public static EventChecker GetTriggerBoxEnterChecker()
        {
            return new TriggerBoxChecker(EBattleEventType.OnTriggerBoxEnter, Ability.Context.SourceActor);
        }

        [AbilityMethod(false)]
        public static EventChecker GetTriggerBoxStayChecker()
        {
            return new TriggerBoxChecker(EBattleEventType.OnTriggerBoxStay, Ability.Context.SourceActor);
        }

        [AbilityMethod(false)]
        public static EventChecker GetTriggerBoxExitChecker()
        {
            return new TriggerBoxChecker(EBattleEventType.OnTriggerBoxExit, Ability.Context.SourceActor);
        }
    }
}