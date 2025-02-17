
using System.Collections.Generic;
using Hono.Scripts.Battle.AbilitySystem;

namespace Hono.Scripts.Battle.AbilityFramework
{
    public partial class Ability
    {
        public static partial class AbilityEnv
        {
            private static void __Gen_AFuncWrap_Register()
            {
                AbilityEnv.RegisterWrap("GetBuffLayer", new __Gen_AFuncWrap_GetBuffLayer());
AbilityEnv.RegisterWrap("ChangeSkillLevel", new __Gen_AFuncWrap_ChangeSkillLevel());
AbilityEnv.RegisterWrap("DebugMessage", new __Gen_AFuncWrap_DebugMessage());
AbilityEnv.RegisterWrap("CreateHitBox", new __Gen_AFuncWrap_CreateHitBox());
AbilityEnv.RegisterWrap("CreateHitBoxes", new __Gen_AFuncWrap_CreateHitBoxes());
AbilityEnv.RegisterWrap("CreateHitBoxToTargets", new __Gen_AFuncWrap_CreateHitBoxToTargets());
AbilityEnv.RegisterWrap("CreateBullet", new __Gen_AFuncWrap_CreateBullet());
AbilityEnv.RegisterWrap("CreateBullets", new __Gen_AFuncWrap_CreateBullets());
AbilityEnv.RegisterWrap("AddVFX", new __Gen_AFuncWrap_AddVFX());
AbilityEnv.RegisterWrap("AddVFXToTargets", new __Gen_AFuncWrap_AddVFXToTargets());
AbilityEnv.RegisterWrap("RemoveVFX", new __Gen_AFuncWrap_RemoveVFX());
AbilityEnv.RegisterWrap("SelectTargets", new __Gen_AFuncWrap_SelectTargets());
AbilityEnv.RegisterWrap("CheckTalent", new __Gen_AFuncWrap_CheckTalent());
AbilityEnv.RegisterWrap("CheckActorTag", new __Gen_AFuncWrap_CheckActorTag());
AbilityEnv.RegisterWrap("AddBuff", new __Gen_AFuncWrap_AddBuff());
AbilityEnv.RegisterWrap("AddBuffToTargets", new __Gen_AFuncWrap_AddBuffToTargets());
AbilityEnv.RegisterWrap("RemoveBuff", new __Gen_AFuncWrap_RemoveBuff());
AbilityEnv.RegisterWrap("LessSkillCD", new __Gen_AFuncWrap_LessSkillCD());
AbilityEnv.RegisterWrap("GetListCount", new __Gen_AFuncWrap_GetListCount());
AbilityEnv.RegisterWrap("GetIntListItem", new __Gen_AFuncWrap_GetIntListItem());
AbilityEnv.RegisterWrap("GetListRange", new __Gen_AFuncWrap_GetListRange());
AbilityEnv.RegisterWrap("CalculateInt", new __Gen_AFuncWrap_CalculateInt());
AbilityEnv.RegisterWrap("AddFightRes", new __Gen_AFuncWrap_AddFightRes());
AbilityEnv.RegisterWrap("CalculateFloat", new __Gen_AFuncWrap_CalculateFloat());
AbilityEnv.RegisterWrap("And", new __Gen_AFuncWrap_And());
AbilityEnv.RegisterWrap("Or", new __Gen_AFuncWrap_Or());
AbilityEnv.RegisterWrap("CompareInt", new __Gen_AFuncWrap_CompareInt());
AbilityEnv.RegisterWrap("CompareFloat", new __Gen_AFuncWrap_CompareFloat());
AbilityEnv.RegisterWrap("IntSelfAdditive", new __Gen_AFuncWrap_IntSelfAdditive());
AbilityEnv.RegisterWrap("FloatSelfAdditive", new __Gen_AFuncWrap_FloatSelfAdditive());
AbilityEnv.RegisterWrap("IntSelfSubtracting", new __Gen_AFuncWrap_IntSelfSubtracting());
AbilityEnv.RegisterWrap("FloatSelfSubtracting", new __Gen_AFuncWrap_FloatSelfSubtracting());
            }
        }
    }
}
