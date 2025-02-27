using Hono.Scripts.Battle.Core;
using Hono.Scripts.Battle.Event;
using Hono.Scripts.Battle.Tools.CustomAttribute;

namespace Hono.Scripts.Battle.AbilityFramework
{
    public partial class AFunctionDefine 
    {
        [AbilityFunction(false)]
        public HitEventChecker GetHitEventChecker(int attackerUid, EDamageSourceType damageSourceType, int sourceAbilityId, int damageConfigId)
        {
            var checker = GPool<HitEventChecker>.Pool.Rent();
            checker.OnRent(attackerUid, damageSourceType, sourceAbilityId, damageConfigId);
            return checker;
        }

        [AbilityFunction(false)]
        public SkillEventChecker GetSkillEventChecker(int casterUid,int skillId)
        {
            var checker = GPool<SkillEventChecker>.Pool.Rent();
            checker.OnRent(casterUid,skillId);
            return checker;
        }

        [AbilityFunction(false)]
        public AttrChangedEventChecker GetAttrChangedEventChecker(int sourceUnitUid, EAttrType checkAttrType)
        {
            var checker = GPool<AttrChangedEventChecker>.Pool.Rent();
            checker.OnRent(sourceUnitUid, checkAttrType);
            return checker;
        }
    }
}