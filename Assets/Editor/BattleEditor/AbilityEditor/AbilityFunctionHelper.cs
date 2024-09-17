﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using Editor.AbilityEditor;
using Hono.Scripts.Battle;
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

        private static Dictionary<EParameterValueType, List<FuncInfo>> _funcInfoTypeDict;
        
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

        public static Dictionary<string, FuncInfo> GetAllFuncInfos()
        {
            return _funcInfoDict;
        }
        
        public static List<FuncInfo> GetFuncInfosByType(EParameterValueType type)
        {
            return _funcInfoTypeDict[type];
        }

        public static string GetDefaultFuncName()
        {
            return "NothingToDo";
        }
        
        public static Type GetVariableType(string typeString)
        {
            if (typeString == "int")
            {
                return typeof(int);
            }

            if (typeString == "float")
            {
                return typeof(float);
            }

            if (typeString == "bool")
            {
                return typeof(bool);
            }

            if (typeString == "string")
            {
                return typeof(string);
            }

            if (typeString == "intList")
            {
                return typeof(List<int>);
            }

            if (typeString == "floatList")
            {
                return typeof(List<float>);
            }

            var custom = Type.GetType(typeString);
            if (custom != null)
            {
                return custom;
            }

            throw new InvalidCastException("类型转换失败");
        }
        
        public static EParameterValueType GetParameterValueType(this Type type)
        {
            if (type == typeof(int))
            {
                return EParameterValueType.Int;
            }
            if (type == typeof(bool))
            {
                return EParameterValueType.Bool;
            }
            if (type == typeof(float))
            {
                return EParameterValueType.Float;
            }
            if (type == typeof(string))
            {
                return EParameterValueType.String;
            }
            /*if (type == typeof(List<int>))
            {
                return EParameterValueType.IntList;
            }*/
            if (type.IsEnum)
            {
                return EParameterValueType.Enum;
            }
            if (type.IsSerializable && type.IsClass)
            {
                return EParameterValueType.Custom;
            }

            return EParameterValueType.Any;
        }

        public static void Init()
        {
            _funcInfoDict = new Dictionary<string, FuncInfo>();   
            _funcInfoTypeDict = new();
            _funcInfoTypeDict.Add(EParameterValueType.Any, new List<FuncInfo>());

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
                if (valueType != EParameterValueType.Any)
                {
                    if (!_funcInfoTypeDict.TryGetValue(valueType, out var funcInfos))
                    {
                        funcInfos = new List<FuncInfo>();
                        _funcInfoTypeDict.Add(valueType, funcInfos);
                    }
                    funcInfos.Add(info);
                }
                _funcInfoTypeDict[EParameterValueType.Any].Add(info);
            }
        }
    }
}