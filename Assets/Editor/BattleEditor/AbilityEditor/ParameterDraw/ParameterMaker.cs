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

        public void InitFunc(string funcName)
        {
            Self = new Parameter();
            Self.IsFunc = true;
            Self.FuncName = funcName;
            if (!AbilityFuncCacheMgr.MethodCache.TryGetValue(Self.FuncName, out var methodInfo))
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
                paramNode.InitValueParam(fullName, parameterInfo.Name);
                FuncParams.Add(paramNode);
                paramNode.Parent = this;
            }
        }
        

        public void InitValueParam(string paramValueTypeStr, string paramName = "")
        {
            Self = new Parameter();
            Self.ParamType = paramValueTypeStr;
            Self.IsValueType = true;
            ParamName = paramName;
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