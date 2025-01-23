using Hono.Scripts.Battle.Event;

namespace Hono.Scripts.Battle
{
    public static class VariableBoardEx
    {
        public static void InitByHitDamageInfo(this VariableBoard board, in HitDamageInfo info)
        {
            board.Set(HitDamageInfoKeys.SourceActorId,    info.SourceActorId);
            board.Set(HitDamageInfoKeys.SourceAbilityId,  info.SourceAbilityId);
            board.Set(HitDamageInfoKeys.DamageConfigId,   info.DamageConfigId);
            board.Set(HitDamageInfoKeys.HitBoxHitCount,   info.HitBoxHitCount);
            board.Set(HitDamageInfoKeys.FinalDamageValue, info.FinalDamageValue);
            board.Set(HitDamageInfoKeys.IsCritical,       info.IsCritical);
            board.Set(HitDamageInfoKeys.IsImmunity,       info.IsImmunity);
            board.Set(HitDamageInfoKeys.IsKillTarget,     info.IsKillTarget);
        }
    }
}