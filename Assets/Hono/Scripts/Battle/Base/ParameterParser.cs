using System;
using System.Collections.Generic;
using UnityEngine;

namespace Hono.Scripts.Battle
{
    public static class ParamExtension
    {
        public static bool ParseParameters(this ParameterInfo parameterInfo, out object result)
        {
            result = null;
            if (parameterInfo == null || parameterInfo.Parameters == null ||
                parameterInfo.Parameters.Length == 0) return false;

            var parameters = parameterInfo.Parameters;
            
            if (parameters.Length == 1)
            {
                if (parameters[0].IsValueType)
                {
                    result = parameters[0].Value;
                    return true;
                }

                if (parameters[0].IsVariable)
                {
                    result = Ability.Context.Invoker.Variables.Get(parameters[0].VariableName);
                    return true;
                }

                if (parameters[0].IsAttr)
                {
                    // result = Ability.Context.SourceActor.GetAutoAttr(parameterInfo[0].AttrId);
                    return true;
                }
            }

            return parameters.TryCallFunc(out result);
        }

        public static bool TryCallFunc(this Parameter[] func, out object auto)
        {
            auto = null;

            if (func != null && func.Length > 0 && func[0].IsFunc)
            {
                var qFunc = new Queue<Parameter>();
                auto = qFunc.CallFunc(qFunc.Dequeue());
                return auto != null;
            }

            Debug.LogError("队首不是函数");
            return false;
        }

        /// <summary>
        /// 执行指定函数
        /// </summary>
        /// <param name="queue"></param>
        /// <param name="func"></param>
        /// <returns></returns>
        public static object CallFunc(this Queue<Parameter> queue, Parameter func)
        {
            var funcInfo = AbilityFuncPreLoader.GetFuncInfo(func.FuncName);

            if (funcInfo == null)
            {
                return null;
            }

            //TODO:有GC问题后续优化
            object[] funcParams = new object[funcInfo.ParamCount];

            for (int idx = 0; idx < funcInfo.ParamCount; idx++)
            {
                var param = queue.Dequeue();

                if (param.IsValueType)
                {
                    funcParams[idx] = param.Value;
                }

                if (param.IsFunc)
                {
                    funcParams[idx] = CallFunc(queue, param);
                }
            }

            //TODO:有消耗
            var res = funcInfo.Invoke(null, funcParams);
            return res;
        }
    }
}