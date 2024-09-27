using System;
using System.Collections.Generic;
using System.Reflection;
using Cysharp.Threading.Tasks;
using Hono.Scripts.Battle.Tools;
using Hono.Scripts.Battle.Tools.CustomAttribute;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Hono.Scripts.Battle
{
    public class FuncInfo
    {
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
		
            if (param != null && param.Length != ParamCount)
            {
                Debug.LogError("参数数量不对");
                return null;
            }

            return MethodInfo.Invoke(caller, param);
        }
    }
    
    public static class AbilityFuncPreLoader
    {
        /// <summary>
        /// 函数信息缓存
        /// </summary>
        private static readonly Dictionary<string, FuncInfo> CacheMethodInfos = new(64);
        
        public static void InitAbilityFuncCache()
        {
            CacheMethodInfos.Clear();
            Type type = typeof(AbilityFunction);

            MethodInfo[] methods = type.GetMethods(BindingFlags.Public | BindingFlags.Static);

            foreach (var method in methods)
            {
                AbilityMethod attr = null;
                foreach (var obj in method.GetCustomAttributes(typeof(AbilityMethod), false))
                {
                    if (obj is AbilityMethod cache)
                    {
                        attr = cache;
                    }
                }

                if (attr == null) continue;

                if (!CacheMethodInfos.ContainsKey(method.Name))
                {
                    var info = new FuncInfo
                    {
                        FuncName = method.Name,
                        ParamCount = method.GetParameters().Length,
                        MethodInfo = method,
                        HasParam = method.GetParameters().Length > 0,
                    };

                    CacheMethodInfos.Add(method.Name, info);
                }
            }
        }
        
        /// <summary>
        /// 获取函数缓存
        /// </summary>
        /// <param name="func"></param>
        /// <returns></returns>
        public static FuncInfo GetFuncInfo(string func)
        {
            if (CacheMethodInfos.TryGetValue(func, out var info))
            {
                return info;
            }
            
            Debug.LogError($"GetAbilityFuncCache {func} is null");
            return null;
        }
    }
}