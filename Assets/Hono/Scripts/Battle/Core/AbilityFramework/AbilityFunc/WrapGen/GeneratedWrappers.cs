using System;
using System.Collections.Generic;
using Hono.Scripts.Battle.AbilitySystem;

namespace Hono.Scripts.Battle.AbilityFramework
{
    public partial class Ability
    {
        public static partial class AbilityEnv
        {
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


            private class __Gen_AFuncWrap_ChangeSkillLevel : IReturnVoid
            {
                public void CallFunc(Ability caller, ANode node, List<AParams> @params)
                {
                    var p0 = node.ParseInt(@params[0]);
                    var p1 = node.ParseInt(@params[1]);
                    caller._functionDefine.ChangeSkillLevel(p0, p1);
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


            private class __Gen_AFuncWrap_CreateHitBox : IReturnVoid
            {
                public void CallFunc(Ability caller, ANode node, List<AParams> @params)
                {
                    var p0 = node.ParseInt(@params[0]);
                    var p1 = node.ParseInt(@params[1]);
                    var p2 = (HitParams)node.ParseRef(@params[2]);
                    var p3 = node.ParseBoolean(@params[3]);
                    caller._functionDefine.CreateHitBox(p0, p1, p2, p3);
                }
            }


            private class __Gen_AFuncWrap_CreateHitBoxes : IReturnVoid
            {
                public void CallFunc(Ability caller, ANode node, List<AParams> @params)
                {
                    var p0 = node.ParseInt(@params[0]);
                    var p1 = (List<Int32>)node.ParseRef(@params[1]);
                    var p2 = (HitParams)node.ParseRef(@params[2]);
                    var p3 = node.ParseBoolean(@params[3]);
                    caller._functionDefine.CreateHitBoxes(p0, p1, p2, p3);
                }
            }


            private class __Gen_AFuncWrap_CreateHitBoxToTargets : IReturnVoid
            {
                public void CallFunc(Ability caller, ANode node, List<AParams> @params)
                {
                    var p0 = (HitParams)node.ParseRef(@params[0]);
                    caller._functionDefine.CreateHitBoxToTargets(p0);
                }
            }


            private class __Gen_AFuncWrap_CreateBullet : IReturnVoid
            {
                public void CallFunc(Ability caller, ANode node, List<AParams> @params)
                {
                    var p0 = node.ParseInt(@params[0]);
                    var p1 = node.ParseInt(@params[1]);
                    var p2 = node.ParseBoolean(@params[2]);
                    caller._functionDefine.CreateBullet(p0, p1, p2);
                }
            }


            private class __Gen_AFuncWrap_CreateBullets : IReturnVoid
            {
                public void CallFunc(Ability caller, ANode node, List<AParams> @params)
                {
                    var p0 = (List<Int32>)node.ParseRef(@params[0]);
                    var p1 = node.ParseInt(@params[1]);
                    var p2 = node.ParseBoolean(@params[2]);
                    caller._functionDefine.CreateBullets(p0, p1, p2);
                }
            }


            private class __Gen_AFuncWrap_AddVFX : IReturnInt
            {
                public int CallFunc(Ability caller, ANode node, List<AParams> @params)
                {
                    var p0 = (VFXSetting)node.ParseRef(@params[0]);
                    var p1 = node.ParseInt(@params[1]);
                    return caller._functionDefine.AddVFX(p0, p1);
                }
            }


            private class __Gen_AFuncWrap_AddVFXToTargets : IReturnVoid
            {
                public void CallFunc(Ability caller, ANode node, List<AParams> @params)
                {
                    var p0 = (VFXSetting)node.ParseRef(@params[0]);
                    var p1 = (List<Int32>)node.ParseRef(@params[1]);
                    caller._functionDefine.AddVFXToTargets(p0, p1);
                }
            }


            private class __Gen_AFuncWrap_RemoveVFX : IReturnVoid
            {
                public void CallFunc(Ability caller, ANode node, List<AParams> @params)
                {
                    var p0 = node.ParseInt(@params[0]);
                    var p1 = node.ParseInt(@params[1]);
                    caller._functionDefine.RemoveVFX(p0, p1);
                }
            }


            private class __Gen_AFuncWrap_SelectTargets : IReturnRef
            {
                public object CallFunc(Ability caller, ANode node, List<AParams> @params)
                {
                    var p0 = node.ParseInt(@params[0]);
                    var p1 = (RangeFilterSetting)node.ParseRef(@params[1]);
                    return caller._functionDefine.SelectTargets(p0, p1);
                }
            }


            private class __Gen_AFuncWrap_CheckTalent : IReturnBoolean
            {
                public bool CallFunc(Ability caller, ANode node, List<AParams> @params)
                {
                    var p0 = (String)node.ParseRef(@params[0]);
                    return caller._functionDefine.CheckTalent(p0);
                }
            }


            private class __Gen_AFuncWrap_CheckActorTag : IReturnBoolean
            {
                public bool CallFunc(Ability caller, ANode node, List<AParams> @params)
                {
                    var p0 = node.ParseInt(@params[0]);
                    var p1 = node.ParseInt(@params[1]);
                    var p2 = node.ParseBoolean(@params[2]);
                    return caller._functionDefine.CheckActorTag(p0, p1, p2);
                }
            }


            private class __Gen_AFuncWrap_AddBuff : IReturnVoid
            {
                public void CallFunc(Ability caller, ANode node, List<AParams> @params)
                {
                    var p0 = node.ParseInt(@params[0]);
                    var p1 = node.ParseInt(@params[1]);
                    var p2 = node.ParseInt(@params[2]);
                    var p3 = node.ParseBoolean(@params[3]);
                    caller._functionDefine.AddBuff(p0, p1, p2, p3);
                }
            }


            private class __Gen_AFuncWrap_AddBuffToTargets : IReturnVoid
            {
                public void CallFunc(Ability caller, ANode node, List<AParams> @params)
                {
                    var p0 = (List<Int32>)node.ParseRef(@params[0]);
                    var p1 = node.ParseInt(@params[1]);
                    var p2 = node.ParseInt(@params[2]);
                    var p3 = node.ParseBoolean(@params[3]);
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


            private class __Gen_AFuncWrap_LessSkillCD : IReturnVoid
            {
                public void CallFunc(Ability caller, ANode node, List<AParams> @params)
                {
                    var p0 = node.ParseInt(@params[0]);
                    var p1 = node.ParseInt(@params[1]);
                    caller._functionDefine.LessSkillCD(p0, p1);
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


            private class __Gen_AFuncWrap_AddFightRes : IReturnVoid
            {
                public void CallFunc(Ability caller, ANode node, List<AParams> @params)
                {
                    var p0 = node.ParseInt(@params[0]);
                    var p1 = node.ParseBoolean(@params[1]);
                    caller._functionDefine.AddFightRes(p0, p1);
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