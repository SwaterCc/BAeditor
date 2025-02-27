
using System;
using System.Collections.Generic;
using Hono.Scripts.Battle.AbilitySystem;
using Hono.Scripts.Battle.Core;
using UnityEngine;
using Object = System.Object;

namespace Hono.Scripts.Battle.AbilityFramework
{
    public partial class Ability
    {
        public static partial class AbilityEnv
        {
            
private class __Gen_AFuncWrap_GetHitEventChecker : IReturnRef
{
    public object CallFunc(Ability caller, ANode node, List<AParams> @params)
    {
        var p0 = node.ParseInt(@params[0]);
var p1 = (EDamageSourceType)node.ParseInt(@params[1]);
var p2 = node.ParseInt(@params[2]);
var p3 = node.ParseInt(@params[3]);
        return caller._functionDefine.GetHitEventChecker(p0, p1, p2, p3);
    }
}


private class __Gen_AFuncWrap_GetSkillEventChecker : IReturnRef
{
    public object CallFunc(Ability caller, ANode node, List<AParams> @params)
    {
        var p0 = node.ParseInt(@params[0]); var p1 = node.ParseInt(@params[1]);
        return caller._functionDefine.GetSkillEventChecker(p0,p1);
    }
}


private class __Gen_AFuncWrap_GetAttrChangedEventChecker : IReturnRef
{
    public object CallFunc(Ability caller, ANode node, List<AParams> @params)
    {
        var p0 = node.ParseInt(@params[0]);
var p1 = (EAttrType)node.ParseInt(@params[1]);
        return caller._functionDefine.GetAttrChangedEventChecker(p0, p1);
    }
}


private class __Gen_AFuncWrap_DebugMessage : IReturnVoid
{
    public void CallFunc(Ability caller, ANode node, List<AParams> @params)
    {
        var p0 = (String)node.ParseRef(@params[0]);
var p1 = (String)node.ParseRef(@params[1]);
var p2 = (Object)node.ParseRef(@params[2]);
var p3 = (Object)node.ParseRef(@params[3]);
var p4 = (Object)node.ParseRef(@params[4]);
         caller._functionDefine.DebugMessage(p0, p1, p2, p3, p4);
    }
}


private class __Gen_AFuncWrap_SkillRTModify : IReturnVoid
{
    public void CallFunc(Ability caller, ANode node, List<AParams> @params)
    {
        var p0 = node.ParseInt(@params[0]);
var p1 = (ESkillModifyType)node.ParseInt(@params[1]);
var p2 = node.ParseInt(@params[2]);
var p3 = node.ParseInt(@params[3]);
         caller._functionDefine.SkillRTModify(p0, p1, p2, p3);
    }
}


private class __Gen_AFuncWrap_LessSkillCD : IReturnVoid
{
    public void CallFunc(Ability caller, ANode node, List<AParams> @params)
    {
        var p0 = node.ParseInt(@params[0]);
var p1 = node.ParseFloat(@params[1]);
         caller._functionDefine.LessSkillCD(p0, p1);
    }
}


private class __Gen_AFuncWrap_GetSkillSelectWorldPos : IReturnVector3
{
    public Vector3 CallFunc(Ability caller, ANode node, List<AParams> @params)
    {
        var p0 = node.ParseInt(@params[0]);
        return caller._functionDefine.GetSkillSelectWorldPos(p0);
    }
}


private class __Gen_AFuncWrap_GetSkillSelectTargetUid : IReturnInt
{
    public int CallFunc(Ability caller, ANode node, List<AParams> @params)
    {
        var p0 = node.ParseInt(@params[0]);
        return caller._functionDefine.GetSkillSelectTargetUid(p0);
    }
}


private class __Gen_AFuncWrap_GetSkillSelectDir : IReturnFloat
{
    public float CallFunc(Ability caller, ANode node, List<AParams> @params)
    {
        var p0 = node.ParseInt(@params[0]);
        return caller._functionDefine.GetSkillSelectDir(p0);
    }
}


private class __Gen_AFuncWrap_GetSkillSelectTargets : IReturnRef
{
    public object CallFunc(Ability caller, ANode node, List<AParams> @params)
    {
        var p0 = node.ParseInt(@params[0]);
        return caller._functionDefine.GetSkillSelectTargets(p0);
    }
}


private class __Gen_AFuncWrap_AddFightRes : IReturnVoid
{
    public void CallFunc(Ability caller, ANode node, List<AParams> @params)
    {
        var p0 = node.ParseInt(@params[0]);
var p1 = node.ParseInt(@params[1]);
         caller._functionDefine.AddFightRes(p0, p1);
    }
}


private class __Gen_AFuncWrap_GetBuffLayer : IReturnInt
{
    public int CallFunc(Ability caller, ANode node, List<AParams> @params)
    {
        var p0 = node.ParseInt(@params[0]);
var p1 = node.ParseInt(@params[1]);
var p2 = node.ParseInt(@params[2]);
        return caller._functionDefine.GetBuffLayer(p0, p1, p2);
    }
}


private class __Gen_AFuncWrap_AddBuff : IReturnVoid
{
    public void CallFunc(Ability caller, ANode node, List<AParams> @params)
    {
        var p0 = node.ParseInt(@params[0]);
var p1 = node.ParseInt(@params[1]);
var p2 = node.ParseInt(@params[2]);
var p3 = node.ParseInt(@params[3]);
         caller._functionDefine.AddBuff(p0, p1, p2, p3);
    }
}


private class __Gen_AFuncWrap_AddBuffToTargets : IReturnVoid
{
    public void CallFunc(Ability caller, ANode node, List<AParams> @params)
    {
        var p0 = node.ParseInt(@params[0]);
var p1 = (List<Int32>)node.ParseRef(@params[1]);
var p2 = node.ParseInt(@params[2]);
var p3 = node.ParseInt(@params[3]);
         caller._functionDefine.AddBuffToTargets(p0, p1, p2, p3);
    }
}


private class __Gen_AFuncWrap_RemoveBuff : IReturnVoid
{
    public void CallFunc(Ability caller, ANode node, List<AParams> @params)
    {
        var p0 = node.ParseInt(@params[0]);
var p1 = node.ParseInt(@params[1]);
var p2 = node.ParseInt(@params[2]);
         caller._functionDefine.RemoveBuff(p0, p1, p2);
    }
}


private class __Gen_AFuncWrap_SingleHit : IReturnVoid
{
    public void CallFunc(Ability caller, ANode node, List<AParams> @params)
    {
        var p0 = node.ParseInt(@params[0]);
var p1 = node.ParseInt(@params[1]);
var p2 = node.ParseInt(@params[2]);
var p3 = node.ParseBoolean(@params[3]);
         caller._functionDefine.SingleHit(p0, p1, p2, p3);
    }
}


private class __Gen_AFuncWrap_AreaHit : IReturnVoid
{
    public void CallFunc(Ability caller, ANode node, List<AParams> @params)
    {
        var p0 = node.ParseInt(@params[0]);
var p1 = node.ParseVector3(@params[1]);
var p2 = node.ParseFloat(@params[2]);
var p3 = (RangeFilterSetting)node.ParseRef(@params[3]);
var p4 = node.ParseInt(@params[4]);
var p5 = node.ParseBoolean(@params[5]);
         caller._functionDefine.AreaHit(p0, p1, p2, p3, p4, p5);
    }
}


private class __Gen_AFuncWrap_LockTargetSingleHitBox : IReturnVoid
{
    public void CallFunc(Ability caller, ANode node, List<AParams> @params)
    {
        var p0 = node.ParseInt(@params[0]);
var p1 = node.ParseInt(@params[1]);
var p2 = node.ParseInt(@params[2]);
var p3 = node.ParseFloat(@params[3]);
var p4 = node.ParseFloat(@params[4]);
var p5 = node.ParseInt(@params[5]);
var p6 = node.ParseBoolean(@params[6]);
         caller._functionDefine.LockTargetSingleHitBox(p0, p1, p2, p3, p4, p5, p6);
    }
}


private class __Gen_AFuncWrap_LockTargetAreaHitBox : IReturnVoid
{
    public void CallFunc(Ability caller, ANode node, List<AParams> @params)
    {
        var p0 = node.ParseInt(@params[0]);
var p1 = node.ParseInt(@params[1]);
var p2 = node.ParseInt(@params[2]);
var p3 = (RangeFilterSetting)node.ParseRef(@params[3]);
var p4 = node.ParseFloat(@params[4]);
var p5 = node.ParseFloat(@params[5]);
var p6 = node.ParseInt(@params[6]);
var p7 = node.ParseBoolean(@params[7]);
         caller._functionDefine.LockTargetAreaHitBox(p0, p1, p2, p3, p4, p5, p6, p7);
    }
}


private class __Gen_AFuncWrap_AreaHitBox : IReturnVoid
{
    public void CallFunc(Ability caller, ANode node, List<AParams> @params)
    {
        var p0 = node.ParseInt(@params[0]);
var p1 = node.ParseVector3(@params[1]);
var p2 = node.ParseFloat(@params[2]);
var p3 = node.ParseInt(@params[3]);
var p4 = (RangeFilterSetting)node.ParseRef(@params[4]);
var p5 = node.ParseFloat(@params[5]);
var p6 = node.ParseFloat(@params[6]);
var p7 = node.ParseInt(@params[7]);
var p8 = node.ParseBoolean(@params[8]);
         caller._functionDefine.AreaHitBox(p0, p1, p2, p3, p4, p5, p6, p7, p8);
    }
}


private class __Gen_AFuncWrap_CreateLockTargetBullet : IReturnVoid
{
    public void CallFunc(Ability caller, ANode node, List<AParams> @params)
    {
        var p0 = node.ParseInt(@params[0]);
var p1 = node.ParseInt(@params[1]);
var p2 = node.ParseInt(@params[2]);
var p3 = node.ParseVector3(@params[3]);
var p4 = node.ParseFloat(@params[4]);
var p5 = node.ParseInt(@params[5]);
var p6 = node.ParseInt(@params[6]);
         caller._functionDefine.CreateLockTargetBullet(p0, p1, p2, p3, p4, p5, p6);
    }
}


private class __Gen_AFuncWrap_CreateDirectionBullet : IReturnVoid
{
    public void CallFunc(Ability caller, ANode node, List<AParams> @params)
    {
        var p0 = node.ParseInt(@params[0]);
var p1 = node.ParseFloat(@params[1]);
var p2 = node.ParseInt(@params[2]);
var p3 = node.ParseVector3(@params[3]);
var p4 = node.ParseInt(@params[4]);
var p5 = node.ParseInt(@params[5]);
         caller._functionDefine.CreateDirectionBullet(p0, p1, p2, p3, p4, p5);
    }
}


private class __Gen_AFuncWrap_AddVFXToWorldByKey : IReturnInt
{
    public int CallFunc(Ability caller, ANode node, List<AParams> @params)
    {
        var p0 = node.ParseInt(@params[0]);
var p1 = (String)node.ParseRef(@params[1]);
var p2 = node.ParseFloat(@params[2]);
var p3 = node.ParseVector3(@params[3]);
var p4 = node.ParseVector3(@params[4]);
        return caller._functionDefine.AddVFXToWorldByKey(p0, p1, p2, p3, p4);
    }
}


private class __Gen_AFuncWrap_AddVFXToWorldByPath : IReturnInt
{
    public int CallFunc(Ability caller, ANode node, List<AParams> @params)
    {
        var p0 = (String)node.ParseRef(@params[0]);
var p1 = node.ParseFloat(@params[1]);
var p2 = node.ParseVector3(@params[2]);
var p3 = node.ParseVector3(@params[3]);
        return caller._functionDefine.AddVFXToWorldByPath(p0, p1, p2, p3);
    }
}


private class __Gen_AFuncWrap_AddVFXToUnitByKey : IReturnInt
{
    public int CallFunc(Ability caller, ANode node, List<AParams> @params)
    {
        var p0 = node.ParseInt(@params[0]);
var p1 = node.ParseInt(@params[1]);
var p2 = (String)node.ParseRef(@params[2]);
var p3 = node.ParseFloat(@params[3]);
var p4 = (String)node.ParseRef(@params[4]);
var p5 = node.ParseVector3(@params[5]);
var p6 = node.ParseVector3(@params[6]);
        return caller._functionDefine.AddVFXToUnitByKey(p0, p1, p2, p3, p4, p5, p6);
    }
}


private class __Gen_AFuncWrap_AddVFXToUnitByPath : IReturnInt
{
    public int CallFunc(Ability caller, ANode node, List<AParams> @params)
    {
        var p0 = node.ParseInt(@params[0]);
var p1 = (String)node.ParseRef(@params[1]);
var p2 = node.ParseFloat(@params[2]);
var p3 = (String)node.ParseRef(@params[3]);
var p4 = node.ParseVector3(@params[4]);
var p5 = node.ParseVector3(@params[5]);
        return caller._functionDefine.AddVFXToUnitByPath(p0, p1, p2, p3, p4, p5);
    }
}


private class __Gen_AFuncWrap_RemoveVFX : IReturnVoid
{
    public void CallFunc(Ability caller, ANode node, List<AParams> @params)
    {
        var p0 = node.ParseInt(@params[0]);
         caller._functionDefine.RemoveVFX(p0);
    }
}


private class __Gen_AFuncWrap_GetVariableBoardInt : IReturnInt
{
    public int CallFunc(Ability caller, ANode node, List<AParams> @params)
    {
        var p0 = node.ParseBoolean(@params[0]);
var p1 = (String)node.ParseRef(@params[1]);
        return caller._functionDefine.GetVariableBoardInt(p0, p1);
    }
}


private class __Gen_AFuncWrap_GetVariableBoardFloat : IReturnFloat
{
    public float CallFunc(Ability caller, ANode node, List<AParams> @params)
    {
        var p0 = node.ParseBoolean(@params[0]);
var p1 = (String)node.ParseRef(@params[1]);
        return caller._functionDefine.GetVariableBoardFloat(p0, p1);
    }
}


private class __Gen_AFuncWrap_GetVariableBoardBool : IReturnBoolean
{
    public bool CallFunc(Ability caller, ANode node, List<AParams> @params)
    {
        var p0 = node.ParseBoolean(@params[0]);
var p1 = (String)node.ParseRef(@params[1]);
        return caller._functionDefine.GetVariableBoardBool(p0, p1);
    }
}


private class __Gen_AFuncWrap_GetVariableBoardVec3 : IReturnVector3
{
    public Vector3 CallFunc(Ability caller, ANode node, List<AParams> @params)
    {
        var p0 = node.ParseBoolean(@params[0]);
var p1 = (String)node.ParseRef(@params[1]);
        return caller._functionDefine.GetVariableBoardVec3(p0, p1);
    }
}


private class __Gen_AFuncWrap_GetVariableBoardObject : IReturnRef
{
    public object CallFunc(Ability caller, ANode node, List<AParams> @params)
    {
        var p0 = node.ParseBoolean(@params[0]);
var p1 = (String)node.ParseRef(@params[1]);
        return caller._functionDefine.GetVariableBoardObject(p0, p1);
    }
}


private class __Gen_AFuncWrap_UnitHasTag : IReturnBoolean
{
    public bool CallFunc(Ability caller, ANode node, List<AParams> @params)
    {
        var p0 = node.ParseInt(@params[0]);
var p1 = node.ParseInt(@params[1]);
var p2 = node.ParseBoolean(@params[2]);
        return caller._functionDefine.UnitHasTag(p0, p1, p2);
    }
}


private class __Gen_AFuncWrap_GetUnitYAxisAngle : IReturnFloat
{
    public float CallFunc(Ability caller, ANode node, List<AParams> @params)
    {
        var p0 = node.ParseInt(@params[0]);
        return caller._functionDefine.GetUnitYAxisAngle(p0);
    }
}


private class __Gen_AFuncWrap_GetUnitPosition : IReturnVector3
{
    public Vector3 CallFunc(Ability caller, ANode node, List<AParams> @params)
    {
        var p0 = node.ParseInt(@params[0]);
        return caller._functionDefine.GetUnitPosition(p0);
    }
}


private class __Gen_AFuncWrap_SelectTargets : IReturnRef
{
    public object CallFunc(Ability caller, ANode node, List<AParams> @params)
    {
        var p0 = node.ParseInt(@params[0]);
var p1 = node.ParseVector3(@params[1]);
var p2 = node.ParseFloat(@params[2]);
var p3 = (RangeFilterSetting)node.ParseRef(@params[3]);
        return caller._functionDefine.SelectTargets(p0, p1, p2, p3);
    }
}


private class __Gen_AFuncWrap_CheckTalent : IReturnBoolean
{
    public bool CallFunc(Ability caller, ANode node, List<AParams> @params)
    {
        var p0 = node.ParseInt(@params[0]);
var p1 = (String)node.ParseRef(@params[1]);
        return caller._functionDefine.CheckTalent(p0, p1);
    }
}


private class __Gen_AFuncWrap_CheckTag : IReturnBoolean
{
    public bool CallFunc(Ability caller, ANode node, List<AParams> @params)
    {
        var p0 = node.ParseInt(@params[0]);
var p1 = node.ParseInt(@params[1]);
var p2 = node.ParseBoolean(@params[2]);
        return caller._functionDefine.CheckTag(p0, p1, p2);
    }
}


private class __Gen_AFuncWrap_GetListCount : IReturnInt
{
    public int CallFunc(Ability caller, ANode node, List<AParams> @params)
    {
        var p0 = (List<Int32>)node.ParseRef(@params[0]);
        return caller._functionDefine.GetListCount(p0);
    }
}


private class __Gen_AFuncWrap_GetIntListItem : IReturnInt
{
    public int CallFunc(Ability caller, ANode node, List<AParams> @params)
    {
        var p0 = (List<Int32>)node.ParseRef(@params[0]);
var p1 = node.ParseInt(@params[1]);
        return caller._functionDefine.GetIntListItem(p0, p1);
    }
}


private class __Gen_AFuncWrap_GetListRange : IReturnRef
{
    public object CallFunc(Ability caller, ANode node, List<AParams> @params)
    {
        var p0 = (List<Int32>)node.ParseRef(@params[0]);
var p1 = node.ParseInt(@params[1]);
        return caller._functionDefine.GetListRange(p0, p1);
    }
}


private class __Gen_AFuncWrap_CalculateInt : IReturnInt
{
    public int CallFunc(Ability caller, ANode node, List<AParams> @params)
    {
        var p0 = node.ParseInt(@params[0]);
var p1 = (ECalculateType)node.ParseInt(@params[1]);
var p2 = node.ParseInt(@params[2]);
        return caller._functionDefine.CalculateInt(p0, p1, p2);
    }
}


private class __Gen_AFuncWrap_CalculateFloat : IReturnFloat
{
    public float CallFunc(Ability caller, ANode node, List<AParams> @params)
    {
        var p0 = node.ParseFloat(@params[0]);
var p1 = (ECalculateType)node.ParseInt(@params[1]);
var p2 = node.ParseFloat(@params[2]);
        return caller._functionDefine.CalculateFloat(p0, p1, p2);
    }
}


private class __Gen_AFuncWrap_And : IReturnBoolean
{
    public bool CallFunc(Ability caller, ANode node, List<AParams> @params)
    {
        var p0 = node.ParseBoolean(@params[0]);
var p1 = node.ParseBoolean(@params[1]);
        return caller._functionDefine.And(p0, p1);
    }
}


private class __Gen_AFuncWrap_Or : IReturnBoolean
{
    public bool CallFunc(Ability caller, ANode node, List<AParams> @params)
    {
        var p0 = node.ParseBoolean(@params[0]);
var p1 = node.ParseBoolean(@params[1]);
        return caller._functionDefine.Or(p0, p1);
    }
}


private class __Gen_AFuncWrap_CompareInt : IReturnBoolean
{
    public bool CallFunc(Ability caller, ANode node, List<AParams> @params)
    {
        var p0 = node.ParseInt(@params[0]);
var p1 = (ECompareResType)node.ParseInt(@params[1]);
var p2 = node.ParseInt(@params[2]);
        return caller._functionDefine.CompareInt(p0, p1, p2);
    }
}


private class __Gen_AFuncWrap_CompareFloat : IReturnBoolean
{
    public bool CallFunc(Ability caller, ANode node, List<AParams> @params)
    {
        var p0 = node.ParseFloat(@params[0]);
var p1 = (ECompareResType)node.ParseInt(@params[1]);
var p2 = node.ParseFloat(@params[2]);
        return caller._functionDefine.CompareFloat(p0, p1, p2);
    }
}


private class __Gen_AFuncWrap_IntSelfAdditive : IReturnInt
{
    public int CallFunc(Ability caller, ANode node, List<AParams> @params)
    {
        var p0 = node.ParseInt(@params[0]);
        return caller._functionDefine.IntSelfAdditive(p0);
    }
}


private class __Gen_AFuncWrap_FloatSelfAdditive : IReturnFloat
{
    public float CallFunc(Ability caller, ANode node, List<AParams> @params)
    {
        var p0 = node.ParseFloat(@params[0]);
        return caller._functionDefine.FloatSelfAdditive(p0);
    }
}


private class __Gen_AFuncWrap_IntSelfSubtracting : IReturnInt
{
    public int CallFunc(Ability caller, ANode node, List<AParams> @params)
    {
        var p0 = node.ParseInt(@params[0]);
        return caller._functionDefine.IntSelfSubtracting(p0);
    }
}


private class __Gen_AFuncWrap_FloatSelfSubtracting : IReturnFloat
{
    public float CallFunc(Ability caller, ANode node, List<AParams> @params)
    {
        var p0 = node.ParseFloat(@params[0]);
        return caller._functionDefine.FloatSelfSubtracting(p0);
    }
}

        }
    }
}
