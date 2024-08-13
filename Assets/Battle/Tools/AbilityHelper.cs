using System;
using System.Collections.Generic;
using System.Reflection;
using Unity.VisualScripting;
using UnityEngine;

namespace Battle.Tools
{
    public static class AbilityHelper
    {

        public class ParamParser<T>
        {
            private T _parseRes;
            private Param _param;

            public ParamParser(Param param)
            {
                _param = param;
            }

            public void Parse()
            {
                if (_param.IsBaseType)
                {
                    
                }

                if (_param.IsFunc) { }

                if (_param.IsVariable) { }

                if (_param.IsAttribute) { }
            }
        }
        
        public static void CallStack(this Stack<Param> stack)
        {
            void ParseParam()
            {
                foreach (var param in stack) { }
            }

            if (stack.Count <= 0)
            {
                Debug.LogError("Stack Is Empty!!");
                return;
            }

            var top = stack.Pop();

           
        }
    }
}