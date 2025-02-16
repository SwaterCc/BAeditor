#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Reflection;
using Hono.Scripts.Battle.Core;
using Hono.Scripts.Battle.Core.Base;
using Newtonsoft.Json.Linq;

namespace Hono.Scripts.Battle.Editor
{
    public static class JsonSchemaGenerator
    {
        [MenuItem("Tools/JsonSchemaGenerator")]
        public static void GenerateJsonSchema()
        {
            try
            {
                var schema = new JObject();

                // 生成组件列表结构
                var components = CollectComponents();
                schema["components"] = GenerateComponentsSchema(components);

                // 生成组件参数结构
                var componentCtors = CollectComponentCtors();
                schema["componentCtors"] = GenerateComponentCtorsSchema(componentCtors);

                // 生成ActorAssembleInfo字段和属性结构
                var actorFields = CollectActorFields();
                schema["actorFields"] = GenerateActorFieldsSchema(actorFields);

                // 保存文件
                string path = EditorUtility.SaveFilePanel(
                    "Save JSON Schema",
                    Application.dataPath,
                    "ActorSchema.json",
                    "json");

                if (!string.IsNullOrEmpty(path))
                {
                    System.IO.File.WriteAllText(path, schema.ToString());
                    AssetDatabase.Refresh();
                }
            }
            catch (Exception ex)
            {
                Debug.LogError($"生成JSON Schema失败: {ex}");
            }
        }

        #region 数据收集
        private static List<ComponentInfo> CollectComponents()
        {
            var components = new List<ComponentInfo>();

            foreach (var type in GetAllTypes<UnitComponent>())
            {
                var attr = type.GetCustomAttribute<JsonUnitComponent>();
                if (attr != null)
                {
                    components.Add(new ComponentInfo
                    {
                        Name = type.Name,
                        Description = attr.Desc,
                        CtorParamsType = GetCtorParamsType(type)
                    });
                }
            }
            return components;
        }

        private static List<ComponentCtorInfo> CollectComponentCtors()
        {
            var ctors = new List<ComponentCtorInfo>();

            foreach (var type in GetAllTypes<UnitCompCtorParams>())
            {
                var attr = type.GetCustomAttribute<JsonUnitCompCtorParams>();
                if (attr != null)
                {
                    var fields = new List<FieldInfo>();
                    foreach (var field in type.GetFields())
                    {
                        var fieldAttr = field.GetCustomAttribute<JsonUnitCompCtorParam>();
                        if (fieldAttr != null)
                        {
                            fields.Add(new FieldInfo
                            {
                                Name = field.Name,
                                Type = field.FieldType.Name,
                                Description = fieldAttr.ParamDesc
                            });
                        }
                    }
                    ctors.Add(new ComponentCtorInfo
                    {
                        ComponentType = attr.ComponentType.Name,
                        Fields = fields
                    });
                }
            }
            return ctors;
        }

        private static List<ActorFieldInfo> CollectActorFields()
        {
            var fields = new List<ActorFieldInfo>();

            var actorAssembleInfoType = typeof(ActorAssembleInfo);

            // 收集所有public字段
            foreach (var field in actorAssembleInfoType.GetFields(BindingFlags.Public | BindingFlags.Instance))
            {
                var attr = field.GetCustomAttribute<JsonActorParam>();
                fields.Add(new ActorFieldInfo
                {
                    Name = field.Name,
                    Type = field.FieldType.Name,
                    Description = attr?.Desc ?? string.Empty // 如果有特性，添加描述；否则为空
                });
            }

            // 收集所有public属性
            foreach (var property in actorAssembleInfoType.GetProperties(BindingFlags.Public | BindingFlags.Instance))
            {
                var attr = property.GetCustomAttribute<JsonActorParam>();
                fields.Add(new ActorFieldInfo
                {
                    Name = property.Name,
                    Type = property.PropertyType.Name,
                    Description = attr?.Desc ?? string.Empty // 如果有特性，添加描述；否则为空
                });
            }

            return fields;
        }
        #endregion

        #region Schema生成
        private static JObject GenerateComponentsSchema(List<ComponentInfo> components)
        {
            var schema = new JObject();
            schema["type"] = "array";
            schema["description"] = "可用的组件列表";

            var items = new JObject();
            items["type"] = "string";
            items["enum"] = new JArray(components.Select(c => c.Name).ToArray());
            items["enumDescriptions"] = new JArray(components.Select(c => c.Description).ToArray());

            schema["items"] = items;
            return schema;
        }

        private static JObject GenerateComponentCtorsSchema(List<ComponentCtorInfo> ctors)
        {
            var schema = new JObject();
            schema["type"] = "object";
            schema["properties"] = new JObject();

            foreach (var ctor in ctors)
            {
                var propSchema = new JObject();
                propSchema["type"] = "object";
                propSchema["description"] = $"{ctor.ComponentType}构造参数";

                var properties = new JObject();
                foreach (var field in ctor.Fields)
                {
                    properties[field.Name] = new JObject
                    {
                        ["type"] = GetJsonType(field.Type),
                        ["description"] = field.Description
                    };
                }

                propSchema["properties"] = properties;
                schema["properties"][ctor.ComponentType] = propSchema;
            }

            return schema;
        }

        private static JObject GenerateActorFieldsSchema(List<ActorFieldInfo> fields)
        {
            var schema = new JObject();
            schema["type"] = "object";
            schema["description"] = "ActorAssembleInfo的字段和属性";

            var properties = new JObject();
            foreach (var field in fields)
            {
                var fieldSchema = new JObject
                {
                    ["type"] = GetJsonType(field.Type)
                };

                // 如果有描述信息，添加到Schema中
                if (!string.IsNullOrEmpty(field.Description))
                {
                    fieldSchema["description"] = field.Description;
                }

                properties[field.Name] = fieldSchema;
            }

            schema["properties"] = properties;
            return schema;
        }
        #endregion

        #region 辅助方法
        private static Type GetCtorParamsType(Type componentType)
        {
            var ctorMethod = componentType.GetMethod("Ctor");
            return ctorMethod?.GetParameters()
                .FirstOrDefault(p => p.ParameterType.IsSubclassOf(typeof(UnitCompCtorParams)))
                ?.ParameterType;
        }

        private static string GetJsonType(string typeName)
        {
            switch (typeName.ToLower())
            {
                case "int32":
                case "int":
                    return "integer";
                case "single":
                case "float":
                    return "number";
                case "boolean":
                    return "boolean";
                case "string":
                    return "string";
                case "list`1":
                    return "array";
                default:
                    return "object";
            }
        }

        private static IEnumerable<Type> GetAllTypes<T>()
        {
            return AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(a => a.GetTypes())
                .Where(t => typeof(T).IsAssignableFrom(t) && !t.IsAbstract);
        }
        #endregion

        #region 数据结构
        private class ComponentInfo
        {
            public string Name;
            public string Description;
            public Type CtorParamsType;
        }

        private class ComponentCtorInfo
        {
            public string ComponentType;
            public List<FieldInfo> Fields;
        }

        private class FieldInfo
        {
            public string Name;
            public string Type;
            public string Description;
        }

        private class ActorFieldInfo
        {
            public string Name;
            public string Type;
            public string Description;
        }
        #endregion
    }
}
#endif