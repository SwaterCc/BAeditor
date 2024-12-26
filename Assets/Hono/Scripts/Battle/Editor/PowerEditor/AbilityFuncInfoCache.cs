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
        public class EventEditorInfo
        {
            public string CreateFuncName;
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
        public static readonly Dictionary<EBattleEventType, EventEditorInfo> EventCheckerDict = new();

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
            MethodInfo[] methods = typeof(AbilityFunctionDefine).GetMethods(BindingFlags.Public | BindingFlags.Static);

            //处理函数缓存
            foreach (var method in methods)
            {
                var abilityFunction = method.GetCustomAttribute<AbilityFunction>();
                if (abilityFunction != null)
                {
                    cacheAbilityFuncInfo(method, abilityFunction);
                }
            }

            foreach (var field in typeof(EBattleEventType).GetFields())
            {
                var checkerBinder = field.GetCustomAttribute<EventCheckerBinder>();
                if (checkerBinder == null) continue;

                var enumValue = (EBattleEventType)field.GetValue(null);
                // 获取枚举值
                var eventInfo = new EventEditorInfo();
                eventInfo.CreateFuncName = checkerBinder.CreateFunc;
                eventInfo.EventInfoType = checkerBinder.EventInfoType;
                EventCheckerDict.Add(enumValue, eventInfo);
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
                ShowInEditorView = attr.ShowInEditorView,
            };

            if (method.ReturnType == typeof(void) && string.IsNullOrEmpty(info.FuncReturnDesc))
            {
                info.FuncReturnDesc = "无返回值";
            }

            if (!ARef.TryGetRefType(method.ReturnType, out var returnRefType))
            {
                Debug.LogError($"func {info.FuncName} returnType {method.ReturnType} 是值类型但是没有对应的ARef包装");
            }

            info.ReturnType = returnRefType;

            for (var index = 0; index < method.GetParameters().Length; index++)
            {
                ParameterInfo parameter = method.GetParameters()[index];

                if (!ARef.TryGetRefType(parameter.ParameterType, out var paramType))
                {
                    Debug.LogError($"func {info.FuncName} param {parameter.Name} 是值类型但是没有对应的ARef包装");
                }

                var paramInfo = new ParamInfo()
                {
                    ParamType = paramType,
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