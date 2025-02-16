using System;
using System.Collections.Generic;
using System.Linq;
using Hono.Scripts.Battle.Core;
using Newtonsoft.Json.Linq;
using UnityEngine;

namespace Hono.Scripts.Battle.Core
{
    public partial class ActorAssembleInfo
    {
        public static class Parser
        {
            // 预先生成组件名称到枚举的映射字典（静态初始化）
            private static readonly Dictionary<string, EUnitComponentKey> ComponentNameToKeyMap =
                new(StringComparer.OrdinalIgnoreCase);

            static Parser()
            {
                InitializeComponentMap();
            }

            // 初始化名称映射（替代硬编码 switch）
            private static void InitializeComponentMap()
            {
                foreach (var componentType in EUnitComponentKeyHelper.TypeToKeyMap.Keys)
                {
                    var componentName = componentType.Name;
                    var key = EUnitComponentKeyHelper.TypeToKeyMap[componentType];
                    ComponentNameToKeyMap[componentName] = key;
                }
            }

            /// <summary>
            /// 解析 JSON 数据并生成 ActorAssembleInfo 对象
            /// </summary>
            /// <param name="jsonContext">JSON 字符串</param>
            /// <returns>解析后的 ActorAssembleInfo 对象</returns>
            public static ActorAssembleInfo Parse(string jsonContext)
            {
                var jsonObject = JObject.Parse(jsonContext);
                var actorData = jsonObject["actor"] as JObject;
                if (actorData == null)
                {
                    Debug.LogError("JSON格式错误：缺少'actor'节点");
                    return null;
                }

                var assembleInfo = new ActorAssembleInfo();

                // 解析基础属性
                assembleInfo.ActorType = ParseEnum<EActorType>(actorData["ActorType"]?.ToString());
                assembleInfo.BaseAttrTableId = (int)(actorData["BaseAttrTableId"] ?? 0);
                assembleInfo.ModelId = (int)(actorData["ModelId"] ?? 0);
                
                // 解析Tags
                assembleInfo.Tags = actorData["Tags"]?.ToObject<List<int>>() ?? new List<int>();

                // 解析AbilityList
                assembleInfo.AbilityList = new List<ActorCtorAbilityInfo>();
                var abilityArray = actorData["AbilityList"] as JArray;
                if (abilityArray != null)
                {
                    foreach (var item in abilityArray)
                    {
                        assembleInfo.AbilityList.Add(new ActorCtorAbilityInfo
                        {
                            AbilityId = (int)(item["abilityId"] ?? 0),
                            Execute = (bool)(item["execute"] ?? false)
                        });
                    }
                }

                // 解析组件位
                var componentList = actorData["ComponentList"] as JArray;
                if (componentList != null)
                {
                    foreach (var compName in componentList)
                    {
                        var key = ParseComponentKey(compName.ToString());
                        assembleInfo.UnitComponentBits |= key;
                    }
                }

                // 解析组件构造参数
                var componentCtor = actorData["ComponentCtor"] as JObject;
                if (componentCtor != null)
                {
                    foreach (var prop in componentCtor.Properties())
                    {
                        var componentName = prop.Name;
                        var key = ParseComponentKey(componentName);
                        var paramsObj = prop.Value as JObject;

                        if (paramsObj != null)
                        {
                            // 根据组件类型动态解析参数
                            switch (key)
                            {
                                case EUnitComponentKey.CombatComp:
                                    assembleInfo.UnitCompCtorParams[key] = new CombatCompCtorParams
                                    {
                                        SkillList = paramsObj["SkillList"]?.ToObject<List<int>>() ?? new List<int>(),
                                        CombatEnergyIds = paramsObj["CombatEnergyIds"]?.ToObject<List<int>>() ??
                                                          new List<int>()
                                    };
                                    break;
                                // 添加其他组件的解析逻辑
                                default:
                                    Debug.LogWarning($"未实现{componentName}的参数解析");
                                    break;
                            }
                        }
                    }
                }

                return assembleInfo;
            }

            private static TEnum ParseEnum<TEnum>(string value) where TEnum : struct
            {
                if (Enum.TryParse<TEnum>(value, true, out var result))
                {
                    return result;
                }

                Debug.LogError($"枚举解析失败：{value} 无法转换为 {typeof(TEnum).Name}");
                return default;
            }

            /// <summary>
            /// 根据组件名称解析对应的枚举值
            /// </summary>
            /// <param name="configName">组件名称</param>
            /// <returns>对应的 EUnitComponentKey 枚举值</returns>
            private static EUnitComponentKey ParseComponentKey(string configName)
            {
                if (ComponentNameToKeyMap.TryGetValue(configName, out var key))
                {
                    return key;
                }

                throw new KeyNotFoundException(
                    $"未知组件名称: {configName}。已注册组件: {string.Join(", ", ComponentNameToKeyMap.Keys)}");
            }
        }
    }
}