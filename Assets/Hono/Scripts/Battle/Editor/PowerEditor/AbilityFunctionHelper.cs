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
    public static class AbilityFunctionHelper
    {
        public class FuncInfo
        {
            public string FuncName;
            public int ParamCount;
            public bool ShowInEditorView;
            public Type ReturnType;
            public List<ParamInfo> ParamInfos = new List<ParamInfo>();
        }

        public class ParamInfo
        {
            public string ParamName;
            public Type ParamType;
        }
        
        private static Dictionary<string, FuncInfo> _funcInfoDict;

        private static Dictionary<EParamValueType, List<FuncInfo>> _funcInfoTypeDict;

        public class EventEditorInfo
        {
            public string CreateFuncName;
            public Type EventInfoType;
        }
        
        private static readonly Dictionary<EBattleEventType, EventEditorInfo> _eventCheckerDict = new();
        public static Dictionary<EBattleEventType, EventEditorInfo> EventCheckerDict => _eventCheckerDict;

        private static readonly List<EBattleEventType> _allowEvent = new();
        public static  List<EBattleEventType>  AllowEvent => _allowEvent;
        
        public static FuncInfo GetFuncInfo(string funcName)
        {
            return _funcInfoDict[funcName];
        }
        
        public static bool TryGetFuncInfo(string funcName,out FuncInfo funcInfo)
        {
            funcInfo = null;
            if (string.IsNullOrEmpty(funcName))
            {
                return false;
            }
            
            return _funcInfoDict.TryGetValue(funcName, out funcInfo);
        }
        
        public static List<FuncInfo> GetFuncInfosByType(EParamValueType type)
        {
            return _funcInfoTypeDict[type];
        }
        
        public static EParamValueType GetParameterValueType(this Type type)
        {
            if (type == typeof(int))
            {
                return EParamValueType.Int;
            }
            if (type == typeof(bool))
            {
                return EParamValueType.Bool;
            }
            if (type == typeof(float))
            {
                return EParamValueType.Float;
            }
            if (type == typeof(string))
            {
                return EParamValueType.String;
            }
            /*if (type == typeof(List<int>))
            {
                return EParameterValueType.IntList;
            }*/
            if (type == typeof(object)) {
	            return EParamValueType.Object;
            }
            if (type.IsEnum)
            {
                return EParamValueType.Enum;
            }
            if (type.IsSerializable && type.IsClass)
            {
                return EParamValueType.Custom;
            }

            return EParamValueType.Any;
        }

        public static void Init()
        {
            InitAbilityFuncCache();
            InitEvent();
        }

        private static void InitAbilityFuncCache()
        {
            _funcInfoDict = new Dictionary<string, FuncInfo>();   
            _funcInfoTypeDict = new();
            _funcInfoTypeDict.Add(EParamValueType.Any, new List<FuncInfo>());

            Type type = typeof(AbilityFunctionDefine);

            MethodInfo[] methods = type.GetMethods(BindingFlags.Public | BindingFlags.Static);

            foreach (var method in methods)
            {
                AbilityFunction attr = null;
                foreach (var obj in method.GetCustomAttributes(typeof(AbilityFunction), false))
                {
                    if (obj is AbilityFunction cache)
                    {
                        attr = cache;
                    }
                }

                if (attr == null) continue;

                var info = new FuncInfo
                {
                    FuncName = method.Name,
                    ParamCount = method.GetParameters().Length,
                    ShowInEditorView = attr.ShowInEditorView,
                    ReturnType = method.ReturnType
                };
                foreach (var parameter in method.GetParameters())
                {
                    var paramInfo = new ParamInfo()
                    {
                        ParamType = parameter.ParameterType,
                        ParamName = parameter.Name
                    };
                    info.ParamInfos.Add(paramInfo);
                }

                _funcInfoDict.Add(method.Name, info);

                var valueType = method.ReturnType.GetParameterValueType();
                if (valueType != EParamValueType.Any)
                {
                    if (!_funcInfoTypeDict.TryGetValue(valueType, out var funcInfos))
                    {
                        funcInfos = new List<FuncInfo>();
                        _funcInfoTypeDict.Add(valueType, funcInfos);
                    }
                    funcInfos.Add(info);
                }
                _funcInfoTypeDict[EParamValueType.Any].Add(info);
            }
        }

        private static void InitEvent()
        {
            if (_eventCheckerDict.Count != 0) return;

            foreach (var field in typeof(EBattleEventType).GetFields())
            {
                
                EventCheckerBinder checkerBinder = null;
                foreach (var attribute in  field.GetCustomAttributes(typeof(EventCheckerBinder), false))
                {
                    if (attribute is EventCheckerBinder binder )
                    {
                        checkerBinder = binder;
                        break;
                    }
                }
                
                if(checkerBinder == null) continue;
                
                var enumValue = (EBattleEventType)field.GetValue(null);
                _allowEvent.Add(enumValue);
                // 获取枚举值
                var eventInfo = new EventEditorInfo();
                eventInfo.CreateFuncName = checkerBinder.CreateFunc;
                eventInfo.EventInfoType = checkerBinder.EventInfoType;
                _eventCheckerDict.Add(enumValue, eventInfo);
            }
        }
    }
}