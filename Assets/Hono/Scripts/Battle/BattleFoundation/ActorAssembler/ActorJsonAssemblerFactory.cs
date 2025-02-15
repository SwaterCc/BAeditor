using System;
using System.Collections.Generic;
using Hono.Scripts.Battle.Core;
using Hono.Scripts.Battle.Core.Base;
using Newtonsoft.Json.Linq;

namespace Hono.Scripts.Battle
{
    /// <summary>
    /// 组装器
    /// </summary>
    public class ActorJsonAssemblerFactory : Singleton<ActorJsonAssemblerFactory>
    {
        private readonly Dictionary<string, ActorCtorConfig> _jsonParseInfos = new();

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
        public void Init()
        {
            for (int i = 0; i < _jsonName.Length; i++)
            {
                _jsonParseInfos.Add(_jsonName[i], ActorConfigParser.Parse(_jsonText[i]));
            }
        }
        
        public ActorCtorConfig GetActorAssembleInfo(string jsonKey)
        {
            return _jsonParseInfos.GetValueOrDefault(jsonKey, null);
        }
    }
    
    public static class ActorConfigParser
    {
        public static ActorCtorConfig Parse(string json)
        {
            var jsonObject = JObject.Parse(json);
            var actorData = jsonObject["actor"];

            var assembleInfo = new ActorCtorConfig();

            // 解析组件列表
            var componentList = actorData["componentList"] as JArray;
            foreach (var componentName in componentList)
            {
                if (Enum.TryParse(componentName.ToString(), ignoreCase: true, out EUnitComponentKey compKey))
                {
                    if (ActorCtorConfig.ComponentFactories.TryGetValue(compKey, out var factory))
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
                    if (ActorCtorConfig.ParamParsers.TryGetValue(compKey, out var parser))
                    {
                        ActorCtorConfig.ComponentFactories[compKey].UnCtorParams = parser.Parse(ctorEntry.Value);
                    }
                }
            }

            return assembleInfo;
        }
    }
}