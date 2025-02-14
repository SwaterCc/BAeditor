using System;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;

namespace Hono.Scripts.Battle.Core
{
    public abstract class UnitComponentFactory
    {
        public ComponentCtorParams UnCtorParams { get; set; }
        public abstract UnitComponent CreateComponent();
    }

    public class UnitComponentFactory<T> : UnitComponentFactory where T : UnitComponent, new()
    {
        public override UnitComponent CreateComponent()
        {
            T component = new T();
            component.Ctor(UnCtorParams);
            return component;
        }
    }

    public interface IComponentParamParser
    {
        ComponentCtorParams Parse(JToken token);
    }

    public static class ActorConfigParser
    {
        public static ActorAssembleInfo Parse(string json)
        {
            var jsonObject = JObject.Parse(json);
            var actorData = jsonObject["actor"];

            var assembleInfo = new ActorAssembleInfo();

            // 解析组件列表
            var componentList = actorData["componentList"] as JArray;
            foreach (var componentName in componentList)
            {
                if (Enum.TryParse(componentName.ToString(), ignoreCase: true, out EUnitComponentKey compKey))
                {
                    if (ActorAssembleInfo.ComponentFactories.TryGetValue(compKey, out var factory))
                    {
                        assembleInfo.Factories.Add(factory);
                    }
                }
            }

            // 解析组件初始化参数
            var componentCtor = actorData["componentCtor"] as JObject;
            foreach (var ctorEntry in componentCtor)
            {
                if (Enum.TryParse(ctorEntry.Key, ignoreCase: true, out EUnitComponentKey compKey))
                {
                    if (ActorAssembleInfo.ParamParsers.TryGetValue(compKey, out var parser))
                    {
                        ActorAssembleInfo.ComponentFactories[compKey].UnCtorParams = parser.Parse(ctorEntry.Value);
                    }
                }
            }

            return assembleInfo;
        }
    }

    /// <summary>
    /// 解析出的结构数据
    /// </summary>
    public partial class ActorAssembleInfo
    {
        /// <summary>
        /// 组件工厂对象
        /// </summary>
        public readonly List<UnitComponentFactory> Factories = new();
    }

    /// <summary>
    /// 组装器
    /// </summary>
    public static class ActorJsonAssembler
    {
        private static readonly Dictionary<string, ActorAssembleInfo> JsonParseInfos = new();

        /// <summary>
        /// 打包时json数据会写为静态数据
        /// </summary>
        private static string[] _jsonName = { };
        /// <summary>
        /// 打包时json数据会写为静态数据
        /// </summary>
        private static string[] _jsonText = { };

        /// <summary>
        /// 初始化解析器
        /// </summary>
        public static void Init()
        {
            for (int i = 0; i < _jsonName.Length; i++)
            {
                JsonParseInfos.Add(_jsonName[i], ActorConfigParser.Parse(_jsonText[i]));
            }
        }

        public static ActorAssembleInfo GetActorAssembleInfo(string jsonKey)
        {
            return JsonParseInfos[jsonKey];
        }
    }
}