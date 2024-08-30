using Hono.Scripts.Battle.Tools.CustomAttribute;
using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

namespace Hono.Scripts.Battle.Tools
{
    public static class AbilityPreLoad
    {
        /// <summary>
        /// 初始化的时候寻找指定标签存储标签内的数值
        /// </summary>
        public class FuncInfo
        {
            private object[] _paramArr = new object[3];

            public string FuncName;
            public int ParamCount = 3;
            public MethodInfo MethodInfo;
            public bool HasParam;

            public object Invoke(object caller)
            {
                return MethodInfo.Invoke(caller, null);
            }

            public object Invoke(object caller, object[] param)
            {
                if (param.Length != ParamCount)
                {
                    Debug.LogError("参数数量不对");
                    return null;
                }

                return MethodInfo.Invoke(caller, param);
            }
        }

        /// <summary>
        /// 函数信息缓存
        /// </summary>
        private static readonly Dictionary<string, List<FuncInfo>>
            _cacheMethodInfos = new Dictionary<string, List<FuncInfo>>(64);
        

        public static void InitCache()
        {
            _cacheMethodInfos.Clear();
            Type type = typeof(AbilityCacheFuncDefine);

            MethodInfo[] methods = type.GetMethods(BindingFlags.Public | BindingFlags.Static);

            foreach (var method in methods)
            {
                AbilityFuncCache attr = null;
                foreach (var obj in method.GetCustomAttributes(typeof(AbilityFuncCache), false))
                {
                    if (obj is AbilityFuncCache cache)
                    {
                        attr = cache;
                    }
                }

                if (attr == null) continue;

                if (!_cacheMethodInfos.TryGetValue(method.Name, out var methodInfos))
                {
                    methodInfos = new List<FuncInfo>();
                    _cacheMethodInfos.Add(method.Name, methodInfos);
                }

                var info = new FuncInfo
                {
                    FuncName = method.Name,
                    ParamCount = method.GetParameters().Length,
                    MethodInfo = method,
                    HasParam = method.GetParameters().Length > 0,
                };

                methodInfos.Add(info);
            }
        }

        public static FuncInfo GetFuncInfo(string func)
        {
            //TODO:字符串有消耗
            return _cacheMethodInfos[func][0];
        }
    }
}