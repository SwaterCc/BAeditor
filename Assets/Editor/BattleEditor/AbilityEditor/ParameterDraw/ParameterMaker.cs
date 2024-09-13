using System;
using System.Collections.Generic;
using Hono.Scripts.Battle;
using Sirenix.Utilities;
using UnityEditor;
using UnityEngine;

namespace Editor.AbilityEditor
{
    public class ParameterMaker
    {
        public Parameter Self;

        public string ParamName;

        public ParameterMaker Parent;

        public List<ParameterMaker> FuncParams = new();

        public Action<Parameter[]> OnSave;

        public void Init(Parameter[] source,bool isFunc ,string name,EParameterInfoType valueType)
        {
            if (source == null || source.Length == 0)
            {  
                FuncParams.Clear();
                if (isFunc)
                {
                    CreateFuncParam("NoThingToDo");
                }
                else
                {
                    CreateValueParam();
                }
            }
            else
            {
                int start = 0;
                FuncParams.Clear();
                Parse(source, ref start);
            }
        }

        public override string ToString()
        {
            if (Self.IsValueType)
            {
                var valueStrs = Self.Value.ToString().Split(".");
                var valueStr = valueStrs[^1];
                return $"参数:{ParamName}:{valueStr}";
            }

            if (Self.IsFunc)
            {
                string func = $"调用函数:{Self.FuncName}(";
                for (var index = 0; index < FuncParams.Count; index++)
                {
                    var parameter = FuncParams[index];
                    if (index != FuncParams.Count - 1)
                        func = func + parameter + ",";
                    else
                        func += parameter;
                }

                func += ")";
                return func;
            }

            return "????";
        }

        public void CreateFuncParam(string funcName)
        {
            var paramName = Self.ParamName;
            Self = new Parameter();
            Self.IsFunc = true;
            Self.FuncName = funcName;
            Self.ParamName = paramName;
            if (!FuncWindow.MethodCache.TryGetValue(Self.FuncName, out var methodInfo))
            {
                Debug.LogError($"{Self.FuncName} 该函数未定义！");
                return;
            }

            Self.ParamCount = methodInfo.GetParameters().Length;
            FuncParams.Clear();
            foreach (var parameterInfo in methodInfo.GetParameters())
            {
                var paramNode = new ParameterMaker();
                var fullName = AbilityEditorHelper.GetTypeAllName(parameterInfo.ParameterType);
                paramNode.CreateValueParam(fullName, parameterInfo.Name);
                FuncParams.Add(paramNode);
                paramNode.Parent = this;
            }
        }

        public void CreateValueParam(string paramValueTypeStr, string paramName = "")
        {
            Self = new Parameter();
            Self.ParamType = paramValueTypeStr;
            Self.IsValueType = true;
            Self.ParamName = paramName;
            var valueType = Type.GetType(paramValueTypeStr);
            if (valueType == null)
            {
                Debug.LogError("获取type失败");
                return;
            }

            Self.Value = (AutoValue)valueType.InstantiateDefault(true);
        }

        public void Parse(Parameter[] source, ref int start)
        {
            if (source == null || source.Length == 0)
            {
                Debug.LogError("源数据是空的");
                return;
            }

            Self = source[start];

            if (Self.IsFunc)
            {
                for (int i = 0; i < Self.ParamCount; ++i)
                {
                    var paramNode = new ParameterMaker();
                    ++start;
                    paramNode.Parse(source, ref start);
                    FuncParams.Add(paramNode);
                    paramNode.Parent = this;
                }
            }
        }

        public void ChangeToValueType(object valueTypeStr)
        {
            ChangeToValueType((string)valueTypeStr);
        }

        public void ChangeToValueType(string valueTypeStr)
        {
            if (Self.IsValueType && valueTypeStr == Self.ParamType) return;
            CreateValueParam(valueTypeStr, Self.ParamName);
            FuncParams.Clear();
        }

        public void ChangeToFunc(string funcName)
        {
            if (Self.IsFunc && Self.FuncName != funcName) return;
            CreateFuncParam(funcName);
        }

        public void ChangeToDefaultFunc(EFuncCacheFlag flag)
        {
            if (FuncWindow.FlagMethodCache[flag].Count > 0)
            {
                var info = FuncWindow.FlagMethodCache[flag][0];
                if (Self.IsFunc && Self.FuncName != info.FuncName) return;
                CreateFuncParam(info.FuncName);
            }
            else
            {
                Debug.LogError("该类型函数没有反射");
            }
        }

        public Parameter[] ToArray()
        {
            var list = new List<Parameter>();

            list.Add(Self);
            foreach (var parameterNode in FuncParams)
            {
                list.AddRange(parameterNode.ToArray());
            }

            return list.ToArray();
        }

        public void Save()
        {
            OnSave?.Invoke(ToArray());
        }
    }
}