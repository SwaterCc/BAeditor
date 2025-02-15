using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using Hono.Scripts.Battle.Core;
using Hono.Scripts.Battle.Core.Base;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Hono.Scripts.Battle.Tools
{
    public static class JsonSchemaGenerator
    {
#if UNITY_EDITOR
        /// <summary>
        /// 生成 JSON 格式化文件及配套代码文件
        /// </summary>
        [MenuItem("Tools/JsonSchemaGenerator")]
#endif
        public static void GenerateAll()
        {
            string outputDir = "Assets/Hono/Scripts/Battle/Define/Auto";
            var assembly = Assembly.GetExecutingAssembly();
            var componentTypes = assembly.GetTypes()
                .Where(t => t.IsSubclassOf(typeof(UnitComponent)) && t.GetCustomAttribute<JsonUnitComponent>() != null);

            // 生成 JSON 格式化文件
            GenerateJsonSchema(outputDir, componentTypes);

            // 生成 EUnitComponentKey 枚举
            GenerateEUnitComponentKeyEnum(outputDir, componentTypes);

            // 生成 IComponentParamParser 实现类
            GenerateComponentParamParsers(outputDir, componentTypes);

            // 生成 ComponentFactories 和 ParamParsers 静态字段
            GenerateComponentFactoriesAndParamParsers(outputDir, componentTypes);

            // 生成 ComponentCtorParams 类和 Ctor 方法
            GenerateComponentCtorParamsAndCtorMethods(outputDir, componentTypes);
        }

        /// <summary>
        /// 生成 JSON 格式化文件
        /// </summary>
        private static void GenerateJsonSchema(string outputDir, IEnumerable<Type> componentTypes)
        {
            var componentList = new List<string>();
            var componentCtor = new Dictionary<string, Dictionary<string, string>>();

            foreach (var type in componentTypes)
            {
                var componentName = type.Name;
                componentList.Add(componentName);

                var fields = type.GetFields(BindingFlags.Public | BindingFlags.Instance)
                    .Where(f => f.GetCustomAttribute<JsonUnitComponentParam>() != null)
                    .ToDictionary(
                        f => f.Name,
                        f => GetTypeName(f.FieldType)
                    );

                if (fields.Count > 0)
                {
                    componentCtor[componentName] = fields;
                }
            }

            var jsonSchema = new
            {
                componentList,
                componentCtor
            };

            var jsonFilePath = Path.Combine(outputDir, "JsonSchema.json");
            File.WriteAllText(jsonFilePath,
                              Newtonsoft.Json.JsonConvert.SerializeObject(
                                  jsonSchema, Newtonsoft.Json.Formatting.Indented));
            Debug.Log($"JSON 格式化文件已生成: {jsonFilePath}");
        }

        /// <summary>
        /// 生成 EUnitComponentKey 枚举
        /// </summary>
        private static void GenerateEUnitComponentKeyEnum(string outputDir, IEnumerable<Type> componentTypes)
        {
            var enumContent = $@"
using System;

namespace Hono.Scripts.Battle.Core
{{
    [Flags]
    public enum EUnitComponentKey
    {{
        None = 0,
{string.Join(",\n", componentTypes.Select((t, i) => $"        {t.Name} = 1 << {i}"))}
    }}
}}
";

            var enumFilePath = Path.Combine(outputDir, "EUnitComponentKey.cs");
            File.WriteAllText(enumFilePath, enumContent);
            Debug.Log($"EUnitComponentKey 枚举已生成: {enumFilePath}");
        }

        private static void GenerateComponentParamParsers(string outputDir, IEnumerable<Type> componentTypes)
        {
            foreach (var type in componentTypes)
            {
                var componentName = type.Name;
                var fields = type.GetFields(BindingFlags.Public | BindingFlags.Instance)
                    .Where(f => f.GetCustomAttribute<JsonUnitComponentParam>() != null);

                var parserContent = $@"
using System;
using Newtonsoft.Json.Linq;

namespace Hono.Scripts.Battle.Core
{{
    public class {componentName}ParamParser : IComponentParamParser
    {{
        public ComponentCtorParams Parse(JToken token)
        {{
            return new {componentName}CtorParams
            {{
{string.Join(",\n", fields.Select(f => $"                {f.Name} = token[\"{f.Name}\"]{(IsValueType(f.FieldType) ? $".Value<{GetTypeName(f.FieldType)}>()" : $"?.ToObject<{GetTypeName(f.FieldType)}>()")}"))}
            }};
        }}
    }}
}}
";

                var parserFilePath = Path.Combine(outputDir, $"{componentName}ParamParser.cs");
                File.WriteAllText(parserFilePath, parserContent);
                Debug.Log($"{componentName}ParamParser 已生成: {parserFilePath}");
            }
        }

        /// <summary>
        /// 生成 ComponentFactories 和 ParamParsers 静态字段
        /// </summary>
        private static void GenerateComponentFactoriesAndParamParsers(string outputDir,
            IEnumerable<Type> componentTypes)
        {
            var factoriesContent = new List<string>();
            var parsersContent = new List<string>();

            foreach (var type in componentTypes)
            {
                var componentName = type.Name;
                factoriesContent.Add(
                    $"            {{ EUnitComponentKey.{componentName}, new UnitComponentFactory<{type.Name}>() }},");
                parsersContent.Add(
                    $"            {{ EUnitComponentKey.{componentName}, new {componentName}ParamParser() }},");
            }

            var codeContent = $@"
using System.Collections.Generic;

namespace Hono.Scripts.Battle.Core
{{
    public partial class ActorAssembleInfo
    {{
        public static readonly Dictionary<EUnitComponentKey, UnitComponentFactory> ComponentFactories = new()
        {{
{string.Join("\n", factoriesContent)}
        }};

        public static readonly Dictionary<EUnitComponentKey, IComponentParamParser> ParamParsers = new()
        {{
{string.Join("\n", parsersContent)}
        }};
    }}
}}
";

            var codeFilePath = Path.Combine(outputDir, "ActorAssembleInfo.Generated.cs");
            File.WriteAllText(codeFilePath, codeContent);
            Debug.Log($"ComponentFactories 和 ParamParsers 已生成: {codeFilePath}");
        }

        /// <summary>
        /// 生成 ComponentCtorParams 类和 Ctor 方法
        /// </summary>
        private static void GenerateComponentCtorParamsAndCtorMethods(string outputDir,
            IEnumerable<Type> componentTypes)
        {
            foreach (var type in componentTypes)
            {
                var componentName = type.Name;
                var fields = type.GetFields(BindingFlags.Public | BindingFlags.Instance)
                    .Where(f => f.GetCustomAttribute<JsonUnitComponentParam>() != null);

                // 生成 ComponentCtorParams 类
                var ctorParamsContent = $@"
using System;

namespace Hono.Scripts.Battle.Core
{{
    public class {componentName}CtorParams : ComponentCtorParams
    {{
{string.Join("\n", fields.Select(f => $"        public {GetTypeName(f.FieldType)} {f.Name} {{ get; set; }}"))}
    }}
}}
";

                var ctorParamsFilePath = Path.Combine(outputDir, $"{componentName}CtorParams.cs");
                File.WriteAllText(ctorParamsFilePath, ctorParamsContent);
                Debug.Log($"{componentName}CtorParams 已生成: {ctorParamsFilePath}");

                // 生成 Ctor 方法
                var ctorMethodContent = $@"
using System;

namespace Hono.Scripts.Battle.Core
{{
    public partial class {componentName}
    {{
        public override void Ctor(ComponentCtorParams param)
        {{
            if (param is {componentName}CtorParams ctorParams)
            {{
{string.Join("\n", fields.Select(f => $"                this.{f.Name} = ctorParams.{f.Name};"))}
            }}
        }}
    }}
}}
";

                var ctorMethodFilePath = Path.Combine(outputDir, $"{componentName}.Ctor.Generated.cs");
                File.WriteAllText(ctorMethodFilePath, ctorMethodContent);
                Debug.Log($"{componentName} 的 Ctor 方法已生成: {ctorMethodFilePath}");
            }
        }

        /// <summary>
        /// 判断类型是否为值类型
        /// </summary>
        private static bool IsValueType(Type type)
        {
            return type.IsValueType || type == typeof(string);
        }

        /// <summary>
        /// 获取类型的名称
        /// </summary>
        private static string GetTypeName(Type type)
        {
            if (type == typeof(int)) return "int";
            if (type == typeof(float)) return "float";
            if (type == typeof(bool)) return "bool";
            if (type == typeof(string)) return "string";
            return type.Name;
        }
    }
}