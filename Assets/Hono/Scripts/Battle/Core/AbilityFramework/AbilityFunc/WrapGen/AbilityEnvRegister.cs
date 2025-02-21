
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
                AbilityEnv.RegisterWrap("GetHitEventChecker", new __Gen_AFuncWrap_GetHitEventChecker());
AbilityEnv.RegisterWrap("GetSkillEventChecker", new __Gen_AFuncWrap_GetSkillEventChecker());
AbilityEnv.RegisterWrap("GetAttrChangedEventChecker", new __Gen_AFuncWrap_GetAttrChangedEventChecker());
AbilityEnv.RegisterWrap("DebugMessage", new __Gen_AFuncWrap_DebugMessage());
AbilityEnv.RegisterWrap("SkillRTModify", new __Gen_AFuncWrap_SkillRTModify());
AbilityEnv.RegisterWrap("LessSkillCD", new __Gen_AFuncWrap_LessSkillCD());
AbilityEnv.RegisterWrap("GetSkillSelectWorldPos", new __Gen_AFuncWrap_GetSkillSelectWorldPos());
AbilityEnv.RegisterWrap("GetSkillSelectTargetUid", new __Gen_AFuncWrap_GetSkillSelectTargetUid());
AbilityEnv.RegisterWrap("GetSkillSelectDir", new __Gen_AFuncWrap_GetSkillSelectDir());
AbilityEnv.RegisterWrap("GetSkillSelectTargets", new __Gen_AFuncWrap_GetSkillSelectTargets());
AbilityEnv.RegisterWrap("AddFightRes", new __Gen_AFuncWrap_AddFightRes());
AbilityEnv.RegisterWrap("GetBuffLayer", new __Gen_AFuncWrap_GetBuffLayer());
AbilityEnv.RegisterWrap("AddBuff", new __Gen_AFuncWrap_AddBuff());
AbilityEnv.RegisterWrap("AddBuffToTargets", new __Gen_AFuncWrap_AddBuffToTargets());
AbilityEnv.RegisterWrap("RemoveBuff", new __Gen_AFuncWrap_RemoveBuff());
AbilityEnv.RegisterWrap("SingleHit", new __Gen_AFuncWrap_SingleHit());
AbilityEnv.RegisterWrap("AreaHit", new __Gen_AFuncWrap_AreaHit());
AbilityEnv.RegisterWrap("LockTargetSingleHitBox", new __Gen_AFuncWrap_LockTargetSingleHitBox());
AbilityEnv.RegisterWrap("LockTargetAreaHitBox", new __Gen_AFuncWrap_LockTargetAreaHitBox());
AbilityEnv.RegisterWrap("AreaHitBox", new __Gen_AFuncWrap_AreaHitBox());
AbilityEnv.RegisterWrap("CreateLockTargetBullet", new __Gen_AFuncWrap_CreateLockTargetBullet());
AbilityEnv.RegisterWrap("CreateDirectionBullet", new __Gen_AFuncWrap_CreateDirectionBullet());
AbilityEnv.RegisterWrap("AddVFXToWorldByKey", new __Gen_AFuncWrap_AddVFXToWorldByKey());
AbilityEnv.RegisterWrap("AddVFXToWorldByPath", new __Gen_AFuncWrap_AddVFXToWorldByPath());
AbilityEnv.RegisterWrap("AddVFXToUnitByKey", new __Gen_AFuncWrap_AddVFXToUnitByKey());
AbilityEnv.RegisterWrap("AddVFXToUnitByPath", new __Gen_AFuncWrap_AddVFXToUnitByPath());
AbilityEnv.RegisterWrap("RemoveVFX", new __Gen_AFuncWrap_RemoveVFX());
AbilityEnv.RegisterWrap("GetVariableBoardInt", new __Gen_AFuncWrap_GetVariableBoardInt());
AbilityEnv.RegisterWrap("GetVariableBoardFloat", new __Gen_AFuncWrap_GetVariableBoardFloat());
AbilityEnv.RegisterWrap("GetVariableBoardBool", new __Gen_AFuncWrap_GetVariableBoardBool());
AbilityEnv.RegisterWrap("GetVariableBoardVec3", new __Gen_AFuncWrap_GetVariableBoardVec3());
AbilityEnv.RegisterWrap("GetVariableBoardObject", new __Gen_AFuncWrap_GetVariableBoardObject());
AbilityEnv.RegisterWrap("UnitHasTag", new __Gen_AFuncWrap_UnitHasTag());
AbilityEnv.RegisterWrap("GetUnitYAxisAngle", new __Gen_AFuncWrap_GetUnitYAxisAngle());
AbilityEnv.RegisterWrap("GetUnitPosition", new __Gen_AFuncWrap_GetUnitPosition());
AbilityEnv.RegisterWrap("SelectTargets", new __Gen_AFuncWrap_SelectTargets());
AbilityEnv.RegisterWrap("CheckTalent", new __Gen_AFuncWrap_CheckTalent());
AbilityEnv.RegisterWrap("CheckTag", new __Gen_AFuncWrap_CheckTag());
AbilityEnv.RegisterWrap("GetListCount", new __Gen_AFuncWrap_GetListCount());
AbilityEnv.RegisterWrap("GetIntListItem", new __Gen_AFuncWrap_GetIntListItem());
AbilityEnv.RegisterWrap("GetListRange", new __Gen_AFuncWrap_GetListRange());
AbilityEnv.RegisterWrap("CalculateInt", new __Gen_AFuncWrap_CalculateInt());
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
