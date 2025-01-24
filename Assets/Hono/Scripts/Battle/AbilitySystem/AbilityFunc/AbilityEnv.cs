using System.Collections.Generic;
using Hono.Scripts.Battle.Tools;

namespace Hono.Scripts.Battle.AbilitySystem
{
    public partial class Ability
    {
        private static class AbilityEnv
        {
            /// <summary>
            /// 运行环境是否初始化
            /// </summary>
            public static bool IsEnvInit { get; private set; } = false;

            /// <summary>
            /// 函数Wrap列表
            /// </summary>
            private static readonly Dictionary<string, IAbilityFunctionWrap> FunctionWraps = new(100);
            
            /// <summary>
            /// 初始化环境
            /// </summary>
            public static void InitEnv()
            {
                if(IsEnvInit) 
                    return;
               
                AbilityFuncWrapInit.__Gen_AFuncWrap_Register();
                IsEnvInit = true;
            }

            /// <summary>
            /// 注册函数
            /// </summary>
            /// <param name="funcName"></param>
            /// <param name="wrap"></param>
            public static void RegisterWrap(string funcName, IAbilityFunctionWrap wrap)
            {
                FunctionWraps.TryAdd(funcName, wrap);
            }

            /// <summary>
            /// 函數包
            /// </summary>
            /// <param name="funcName"></param>
            /// <returns></returns>
            public static IAbilityFunctionWrap GetWrapFunc(string funcName)
            {
                return FunctionWraps[funcName];
            }
        }
    }
}