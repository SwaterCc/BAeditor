using System;
using System.Collections.Generic;
using Hono.Scripts.Battle.Base;
using UnityEngine;

namespace Hono.Scripts.Battle
{
    public partial class Ability
    {
        public abstract class AFunctionWrap { }

        public interface IReturnFloat
        {
            public float CallFunc(Ability caller, List<AParams> @params);
        }

        public interface IReturnInt
        {
            public int CallFunc(Ability caller, List<AParams> @params);
        }

        public interface IReturnBoolean
        {
            public bool CallFunc(Ability caller, List<AParams> @params);
        }

        public interface IReturnVector3
        {
            public Vector3 CallFunc(Ability caller, List<AParams> @params);
        }

        public interface IReturnRef
        {
            public object CallFunc(Ability caller, List<AParams> @params);
        }
        
        public interface IReturnVoid
        {
            public void CallFunc(Ability caller, List<AParams> @params);
        }
    }
}