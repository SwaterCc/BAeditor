using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Hono.Scripts.Battle.Core;
using Hono.Scripts.Battle.Core.Base;
using Newtonsoft.Json.Linq;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Hono.Scripts.Battle
{
    /// <summary>
    /// 组装器
    /// </summary>
    public class ActorJsonAssemblerFactory : BattleFoundation<ActorJsonAssemblerFactory>
    {
        private readonly Dictionary<string, ActorAssembleInfo> _actorAssembleInfos = new();

        /// <summary>
        /// 运行时加载的json数据内容
        /// </summary>
        private static readonly List<string> JsonName = new();
        /// <summary>
        /// 运行时加载的json数据文件名
        /// </summary>
        private static readonly List<string> JsonText = new();

        /// <summary>
        /// 加载
        /// </summary>
        /// <exception cref="NotImplementedException"></exception>
        public override async UniTask AsyncLoad()
        {
            var jsons = await Addressables.LoadAssetsAsync<TextAsset>("aJson").ToUniTask();
            
            foreach (var jsonTextAsset in jsons)
            {
                foreach (var jProperty in JObject.Parse(jsonTextAsset.text).Properties())
                {
                    var info = ActorAssembleInfo.Parser.Parse(jProperty);
                    if (info != null) {
                        _actorAssembleInfos.Add(info.JsonName, info);
                    }
                }
            }
        }
        
        public void Assemble(string jsonKey, Actor actor)
        {
            if (!_actorAssembleInfos.TryGetValue(jsonKey, out var assembleInfo))
            {
                Debug.LogError($"找不到JsonKey {jsonKey}");
                return;
            }

            actor.Ctor(assembleInfo);
        }
    }
}