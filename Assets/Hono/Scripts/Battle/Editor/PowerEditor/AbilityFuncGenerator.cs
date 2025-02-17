using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using Hono.Scripts.Battle.AbilityFramework;
using Hono.Scripts.Battle.Tools.CustomAttribute;
using UnityEditor;
using UnityEngine;

namespace Hono.Scripts.Battle.Editor.PowerEditor
{
    public static class AbilityFuncGenerator
    {
        private const string GeneratedFolderPath =
            "Assets/Hono/Scripts/Battle/Core/AbilityFramework/AbilityFunc/WrapGen";

        [MenuItem("Tools/生成AbilityFuncWrap")]
        public static void GenerateFiles()
        {
            // 确保生成目录存在
            if (!Directory.Exists(GeneratedFolderPath))
            {
                Directory.CreateDirectory(GeneratedFolderPath);
            }

            var methods = typeof(AFunctionDefine)
                .GetMethods(BindingFlags.Public | BindingFlags.Instance)
                .Where(m => m.GetCustomAttribute<AbilityFunction>() != null);

            var wrapperClasses = new List<string>();
            var registerCalls = new List<string>();

            foreach (var method in methods)
            {
                var methodName = method.Name;
                var parameters = method.GetParameters();
                var returnType = method.ReturnType;

                // 生成包装类
                var wrapperClass = GenerateWrapperClass(methodName, parameters, returnType);
                wrapperClasses.Add(wrapperClass);

                // 生成注册调用
                registerCalls.Add(
                    $"AbilityEnv.RegisterWrap(\"{methodName}\", new __Gen_AFuncWrap_{methodName}());");
            }

            // 组合生成的代码
            var wrappers = string.Join("\n",  wrapperClasses);
            var registers = string.Join("\n", registerCalls);

            // 保存包装类文件
            SaveToFile(Path.Combine(GeneratedFolderPath, "GeneratedWrappers.cs"), $@"
using System;
using System.Collections.Generic;
using Hono.Scripts.Battle.AbilitySystem;

namespace Hono.Scripts.Battle.AbilityFramework
{{
    public partial class Ability
    {{
        public static partial class AbilityEnv
        {{
            {wrappers}
        }}
    }}
}}
");

            // 保存注册函数文件
            var registerCode = $@"
using System.Collections.Generic;
using Hono.Scripts.Battle.AbilitySystem;

namespace Hono.Scripts.Battle.AbilityFramework
{{
    public partial class Ability
    {{
        public static partial class AbilityEnv
        {{
            private static void __Gen_AFuncWrap_Register()
            {{
                {registers}
            }}
        }}
    }}
}}
";
            SaveToFile(Path.Combine(GeneratedFolderPath, "AbilityEnvRegister.cs"), registerCode);

            Debug.Log("代码生成完成！");
        }

        private static string GenerateWrapperClass(string methodName, ParameterInfo[] parameters, Type returnType)
        {
            var paramDeclarations = new List<string>();
            var paramParsings = new List<string>();
            for (int i = 0; i < parameters.Length; i++)
            {
                var param = parameters[i];
                var paramName = $"p{i}";

                if (param.ParameterType.IsEnum)
                {
                    // 枚举类型：解析为整数并显式转换为枚举
                    paramDeclarations.Add($"{param.ParameterType.Name} {paramName}");
                    paramParsings.Add($"var {paramName} = ({param.ParameterType.Name})node.ParseInt(@params[{i}]);");
                }
                else if (param.ParameterType.IsClass)
                {
                    // 引用类型：使用常规解析方法
                    paramDeclarations.Add($"{param.ParameterType.Name} {paramName}");

                    if (param.ParameterType.IsGenericType)
                    {
                        paramParsings.Add(
                            $"var {paramName} = ({ GetGenericTypeName(param.ParameterType)})node.{GetParseMethodName(param.ParameterType)}(@params[{i}]);");
                    }
                    else
                    {
                        paramParsings.Add(
                            $"var {paramName} = ({param.ParameterType.Name})node.{GetParseMethodName(param.ParameterType)}(@params[{i}]);");
                    }
                }
                else
                {
                    // 非枚举类型：使用常规解析方法
                    paramDeclarations.Add($"{param.ParameterType.Name} {paramName}");
                    paramParsings.Add(
                        $"var {paramName} = node.{GetParseMethodName(param.ParameterType)}(@params[{i}]);");
                }
            }

            var interfaceName = GetInterfaceName(returnType);
            var callArgs = string.Join(", ", Enumerable.Range(0, parameters.Length).Select(i => $"p{i}"));
            var returnStr = returnType == typeof(void) ? "" : "return";
            return $@"
private class __Gen_AFuncWrap_{methodName} : {interfaceName}
{{
    public {GetReturnName(returnType)} CallFunc(Ability caller, ANode node, List<AParams> @params)
    {{
        {string.Join("\n", paramParsings)}
        {returnStr} caller._functionDefine.{methodName}({callArgs});
    }}
}}
";
        }

        private static string GetParseMethodName(Type parameterType)
        {
            return parameterType.Name switch
            {
                "Int32"   => "ParseInt",
                "Single"  => "ParseFloat",
                "Boolean" => "ParseBoolean",
                "Vector3" => "ParseVector3",
                _ => parameterType.IsClass
                    ? "ParseRef"
                    : throw new NotSupportedException($"Unsupported type: {parameterType.Name}")
            };
        }

        private static string GetInterfaceName(Type returnType)
        {
            return returnType.Name switch
            {
                "Int32"   => "IReturnInt",
                "Single"  => "IReturnFloat",
                "Boolean" => "IReturnBoolean",
                "Vector3" => "IReturnVector3",
                "Void"    => "IReturnVoid",
                _         => "IReturnRef"
            };
        }
        
        private static string GetReturnName(Type returnType)
        {
            return returnType.Name switch
            {
                "Int32"   => "int",
                "Single"  => "float",
                "Boolean" => "bool",
                "Vector3" => "Vector3",
                "Void"    => "void",
                _         => "object"
            };
        }

        public static string GetGenericTypeName(Type type)
        {
            if (type == null)
            {
                throw new ArgumentNullException(nameof(type));
            }

            // 如果类型是泛型
            if (type.IsGenericType)
            {
                // 获取泛型定义的名称（去掉后面的反引号和数字）
                string genericTypeName = type.Name.Split('`')[0];

                // 获取泛型参数
                Type[] genericArguments = type.GetGenericArguments();

                // 递归处理泛型参数
                string genericArgs = string.Join(", ", Array.ConvertAll(genericArguments, GetGenericTypeName));

                return $"{genericTypeName}<{genericArgs}>";
            }

            // 如果类型是数组
            if (type.IsArray)
            {
                return $"{GetGenericTypeName(type.GetElementType())}[{new string(',', type.GetArrayRank() - 1)}]";
            }

            // 返回类型的短名称
            return type.Name;
        }
        
        private static void SaveToFile(string filePath, string content)
        {
            try
            {
                File.WriteAllText(filePath, content);
                Debug.Log($"文件已保存: {filePath}");
            }
            catch (Exception e)
            {
                Debug.LogError($"保存文件失败: {e.Message}");
            }
        }
    }
}