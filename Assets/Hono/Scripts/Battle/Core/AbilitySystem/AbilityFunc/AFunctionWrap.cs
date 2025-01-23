using System.Collections.Generic;
using UnityEngine;

namespace Hono.Scripts.Battle.AbilitySystem
{
    public partial class Ability
    {
        private interface IAbilityFunctionWrap { }

        private interface IReturnFloat : IAbilityFunctionWrap
        {
            public float CallFunc(Ability caller, ANode node, List<AParams> @params);
        }

        private interface IReturnInt : IAbilityFunctionWrap
        {
            public int CallFunc(Ability caller, ANode node, List<AParams> @params);
        }

        private interface IReturnBoolean : IAbilityFunctionWrap
        {
            public bool CallFunc(Ability caller, ANode node, List<AParams> @params);
        }

        private interface IReturnVector3 : IAbilityFunctionWrap
        {
            public Vector3 CallFunc(Ability caller, ANode node, List<AParams> @params);
        }

        private interface IReturnRef : IAbilityFunctionWrap
        {
            public object CallFunc(Ability caller, ANode node, List<AParams> @params);
        }

        private interface IReturnVoid : IAbilityFunctionWrap
        {
            public void CallFunc(Ability caller, ANode node, List<AParams> @params);
        }
    }
}