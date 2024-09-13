using System;
using System.Collections.Generic;
using System.Reflection;
using Hono.Scripts.Battle;

namespace Editor.AbilityEditor
{
    public class AbilityFuncCache
    {
        public struct FuncInfo
        {
            public Type ReturnType;
            public string FuncName;
            public int ParamCount;
            public List<string> ParamNames;
        }

        private static Dictionary<Type, List<FuncInfo>> _flagMethodCache = new();

        private static bool _flagMethodCacheInit = false;

        public static Dictionary<Type, List<FuncInfo>> FlagMethodCache
        {
            get
            {
                if (!_flagMethodCacheInit)
                {
                    initFuncCache();
                }

                return _flagMethodCache;
            }
        }

        private static Dictionary<string, MethodInfo> _methodCache =
            new Dictionary<string, MethodInfo>();

        public static Dictionary<string, MethodInfo> MethodCache
        {
            get
            {
                if (_methodCache.Count == 0)
                {
                    initFuncCache();
                }

                return _methodCache;
            }
        }

        private static void initFuncCache()
        {
            // 获取 MyStaticClass 类型信息
            Type type = typeof(AbilityFunction);

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

                if (!_methodCache.TryGetValue(method.Name, out var methodInfo))
                {
                    methodInfo = method;
                    _methodCache.Add(method.Name, method);
                }

                var info = new FuncInfo
                {
                    ReturnType = methodInfo.ReturnType,
                    FuncName = method.Name,
                    ParamCount = method.GetParameters().Length
                };
                
                foreach (var parameter in method.GetParameters())
                {
                    if (info.ParamNames == null)
                    {
                        info.ParamNames = new List<string>();
                    }

                    info.ParamNames.Add(parameter.Name);
                }

                if(_flagMethodCache.TryGetValue(info.ReturnType,out var infoList))
            }

            _flagMethodCacheInit = true;
        }

    }
}