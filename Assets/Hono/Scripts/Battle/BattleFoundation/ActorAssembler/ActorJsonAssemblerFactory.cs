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
        private static readonly List<string> _jsonName = new();
        /// <summary>
        /// 运行时加载的json数据文件名
        /// </summary>
        private static readonly List<string> _jsonText = new();

        /// <summary>
        /// 加载
        /// </summary>
        /// <exception cref="NotImplementedException"></exception>
        public override async UniTask AsyncLoad()
        {
            var jsons = await Addressables.LoadAssetsAsync<TextAsset>("aJson").ToUniTask();
            foreach (var textAsset in jsons)
            {
                _jsonName.Add(textAsset.name);
                _jsonText.Add(textAsset.text);
            }
        }
        
        /// <summary>
        /// 初始化解析器
        /// </summary>
        public void Init()
        {
            //创建解析信息
            for (int i = 0; i < _jsonName.Count; i++)
            {
                _actorAssembleInfos.Add(_jsonName[i], ActorAssembleInfo.Parser.Parse(_jsonText[i]));
            }
        }

        public bool Assemble(string jsonKey, Actor actor)
        {
            if (!_actorAssembleInfos.TryGetValue(jsonKey, out var assembleInfo))
            {
                return false;
            }

            actor.Ctor(assembleInfo);
            return true;
        }

    
    }
}