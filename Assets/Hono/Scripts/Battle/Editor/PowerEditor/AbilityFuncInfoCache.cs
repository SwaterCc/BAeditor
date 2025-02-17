using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using Editor.AbilityEditor;
using Hono.Scripts.Battle;
using Hono.Scripts.Battle.Base;
using Hono.Scripts.Battle.Event;
using Hono.Scripts.Battle.Tools.CustomAttribute;
using UnityEngine;
using AFunctionDefine = Hono.Scripts.Battle.AbilityFramework.AFunctionDefine;

namespace Editor.BattleEditor.AbilityEditor
{
    public static class AbilityFuncInfoCache
    {
        /// <summary>
        /// 函数信息
        /// </summary>
        public class FuncInfo
        {
            public string FuncName;
            public string FuncDesc = "";
            public string FuncReturnDesc;
            public bool ShowInEditorView;
            public Type ReturnType;
            public List<ParamInfo> ParamInfos = new();
        }

        /// <summary>
        /// 参数信息
        /// </summary>
        public class ParamInfo
        {
            public string ParamName;
            public string ParamDesc;
            public Type ParamType;
        }

        /// <summary>
        /// 事件信息
        /// </summary>
        public class AbilityEventBindInfo
        {
            public Type CheckerType;
            public Type EventInfoType;
        }

        /// <summary>
        /// 函数信息总字典
        /// </summary>
        public static readonly Dictionary<string, FuncInfo> FuncInfoDict = new(100);
        /// <summary>
        /// 函数信息分组
        /// </summary>
        public static readonly Dictionary<string, List<FuncInfo>> FuncGroupDict = new(100)
        {
            { "All", new() },
        };
        /// <summary>
        /// 事件字典
        /// </summary>
        public static readonly Dictionary<EEventType, AbilityEventBindInfo> EventBindInfoLookup = new();

        public static FuncInfo GetFuncInfo(string funcName)
        {
            return FuncInfoDict.GetValueOrDefault(funcName, null);
        }

        public static bool TryGetFuncInfo(string funcName, out FuncInfo funcInfo)
        {
            funcInfo = null;
            if (string.IsNullOrEmpty(funcName))
            {
                return false;
            }

            return FuncInfoDict.TryGetValue(funcName, out funcInfo);
        }

        public static bool TryGetFuncGroup(string groupName, out List<FuncInfo> funcInfos)
        {
            funcInfos = null;
            if (string.IsNullOrEmpty(groupName))
            {
                return false;
            }

            return FuncGroupDict.TryGetValue(groupName, out funcInfos);
        }

        public static void Init()
        {
            MethodInfo[] methods = typeof(AFunctionDefine).GetMethods(BindingFlags.Public | BindingFlags.Instance);

            //处理函数缓存
            FuncInfoDict.Clear();
            foreach (var list in FuncGroupDict.Values)
            {
                list.Clear();
            }
            foreach (var method in methods)
            {
                var abilityFunction = method.GetCustomAttribute<AbilityFunction>();
                if (abilityFunction != null)
                {
                    cacheAbilityFuncInfo(method, abilityFunction);
                }
            }
            
            EventBindInfoLookup.Clear();
            foreach (var field in typeof(EEventType).GetFields())
            {
                var checkerBinder = field.GetCustomAttribute<AbilityEventBind>();
                if (checkerBinder == null) continue;

                var enumValue = (EEventType)field.GetValue(null);
                // 获取枚举值
                var eventInfo = new AbilityEventBindInfo
                {
                    CheckerType = checkerBinder.CheckerType,
                    EventInfoType = checkerBinder.EventInfoKeyType
                };
                EventBindInfoLookup.Add(enumValue, eventInfo);
            }
        }

        private static void cacheAbilityFuncInfo(MethodInfo method, AbilityFunction attr)
        {
            var desc = method.GetCustomAttribute<AbilityFunctionDesc>();

            var info = new FuncInfo
            {
                FuncName = method.Name,
                FuncDesc = desc?.FunctionDesc,
                FuncReturnDesc = desc?.FunctionReturnDesc,
                ReturnType = method.ReturnType,
                ShowInEditorView = attr.ShowInEditorView,
            };

            if ( string.IsNullOrEmpty(info.FuncReturnDesc))
            {
                info.FuncReturnDesc = method.ReturnType == typeof(void) ? "无返回值" : info.ReturnType.ToString().Split(".")[^1];
            }

            for (var index = 0; index < method.GetParameters().Length; index++)
            {
                ParameterInfo parameter = method.GetParameters()[index];
                

                var paramInfo = new ParamInfo()
                {
                    ParamType = parameter.ParameterType,
                    ParamName = parameter.Name,
                };

                if (desc != null && index < desc.ParamsDesc.Count)
                {
                    paramInfo.ParamDesc = desc.ParamsDesc[index];
                }

                info.ParamInfos.Add(paramInfo);
            }

            FuncInfoDict.Add(method.Name, info);
            if (!FuncGroupDict.TryGetValue(attr.GroupTitle, out var groupList))
            {
                groupList = new List<FuncInfo>();
                FuncGroupDict.Add(attr.GroupTitle, groupList);
            }

            groupList.Add(info);
            FuncGroupDict["All"].Add(info);
        }
    }
}