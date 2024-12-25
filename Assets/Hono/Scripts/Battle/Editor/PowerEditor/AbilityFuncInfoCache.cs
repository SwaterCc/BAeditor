using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using Editor.AbilityEditor;
using Hono.Scripts.Battle;
using Hono.Scripts.Battle.Event;
using Hono.Scripts.Battle.Tools.CustomAttribute;

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
            public string FuncDesc;
            public int ParamCount;
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
        private static Dictionary<string, FuncInfo> _funcInfoDict = new(100);
        /// <summary>
        /// 函数信息分组
        /// </summary>
        private static Dictionary<string, List<FuncInfo>> _funcGroupDict = new(100)
        {
            { "All", new() },
        };


        private static readonly Dictionary<EBattleEventType, EventEditorInfo> _eventCheckerDict = new();
        public static Dictionary<EBattleEventType, EventEditorInfo> EventCheckerDict => _eventCheckerDict;

        public static FuncInfo GetFuncInfo(string funcName)
        {
            return _funcInfoDict.GetValueOrDefault(funcName, null);
        }

        public static bool TryGetFuncInfo(string funcName, out FuncInfo funcInfo)
        {
            funcInfo = null;
            if (string.IsNullOrEmpty(funcName))
            {
                return false;
            }

            return _funcInfoDict.TryGetValue(funcName, out funcInfo);
        }
        
        public static bool TryGetFuncGroup(string groupName, out List<FuncInfo> funcInfos)
        {
            funcInfos = null;
            if (string.IsNullOrEmpty(groupName))
            {
                return false;
            }

            return _funcGroupDict.TryGetValue(groupName, out funcInfos);
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
                    cacheAbilityFuncInfo(method,abilityFunction);
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
                _eventCheckerDict.Add(enumValue, eventInfo);
            }
        }

        private static void cacheAbilityFuncInfo(MethodInfo method, AbilityFunction attr)
        {
            var desc = method.GetCustomAttribute<AbilityFunctionDesc>();

            var info = new FuncInfo
            {
                FuncName = method.Name,
                FuncDesc = desc?.FunctionDesc,
                ParamCount = method.GetParameters().Length,
                ShowInEditorView = attr.ShowInEditorView,
                ReturnType = method.ReturnType
            };

            for (var index = 0; index < method.GetParameters().Length; index++)
            {
                ParameterInfo parameter = method.GetParameters()[index];
                var paramInfo = new ParamInfo()
                {
                    ParamType = parameter.ParameterType,
                    ParamName = parameter.Name,
                };

                if (desc != null && desc.ParamsDesc.Count < index)
                {
                    paramInfo.ParamDesc = desc.ParamsDesc[index];
                }

                info.ParamInfos.Add(paramInfo);
            }

            _funcInfoDict.Add(method.Name, info);
            if (!_funcGroupDict.TryGetValue(attr.GroupTitle, out var groupList))
            {
                groupList = new List<FuncInfo>();
                _funcGroupDict.Add(attr.GroupTitle, groupList);
            }
            groupList.Add(info);
            _funcGroupDict["All"].Add(info);
        }
    }
}