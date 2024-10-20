using System;
using Cysharp.Threading.Tasks;
using Hono.Scripts.Battle.Tools;
using UnityEngine;
using UnityEngine.AddressableAssets;
using Object = UnityEngine.Object;

namespace Hono.Scripts.Battle
{
    public partial class ActorModelController
    {
        public class AsyncLoadModelSetup : ModelSetup
        {
            private async UniTask asyncLoadModel(string path)
            {
                try
                {
                    var gameObject = await Addressables.LoadAssetAsync<GameObject>(path).ToUniTask();
                    _gameObject = Object.Instantiate(gameObject);
                }
                catch (Exception e)
                {
                    Debug.LogError($"加载模型失败，路径{path}");
                }
            }

            protected override async void OnLoadModel()
            {
                var modelId = _modelController.Actor.GetAttr<int>(ELogicAttr.AttrModelId);
                var modelData = ConfigManager.Table<ModelTable>().Get(modelId);
                if (modelData == null || string.IsNullOrEmpty(modelData.ModelPath)) return;

                _path = modelData.ModelPath;

                if (!GameObjectPool.Instance.TryGet(modelData.ModelPath, out _gameObject))
                {
                    await asyncLoadModel(modelData.ModelPath);
                }

                _loadComplete.Invoke(_gameObject);
            }

            protected override void OnUnInit() { }
        }
    }
}