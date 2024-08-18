using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;

namespace Battle.Tools
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
            public Type[] ParamTypes;
            public Type ReturnType;

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

                for (int i = 0; i < ParamCount; i++)
                {
                    _paramArr[i] = param[i];
                }

                return MethodInfo.Invoke(caller, _paramArr);
            }
        }

        /// <summary>
        /// 函数信息缓存
        /// </summary>
        private static Dictionary<string, FuncInfo> _cacheMethodInfos;

        //private static Dictionary<string, Type> _cacheTypes;

        private static bool _isInitFinsh = false;

        public static void InitCache() { }

        public static FuncInfo GetFuncInfo(string func, string genericType = "")
        {
            //TODO:字符串有消耗
            return _cacheMethodInfos[func + genericType];
        }
    }
}