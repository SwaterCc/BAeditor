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
            public override async void SetupModel(ActorModelController modelController, Action loadComplete = null)
            {
                await asyncLoadModel(modelController);
                loadComplete?.Invoke();
            }

            private async UniTask asyncLoadModel(ActorModelController modelController)
            {
                var modelId = modelController.Actor.GetAttr<int>(ELogicAttr.AttrModelId);
                var modelData = ConfigManager.Table<ModelTable>().Get(modelId);
                if (modelData == null || string.IsNullOrEmpty(modelData.ModelPath)) return;

                try
                {
                    modelController._model = await Addressables.LoadAssetAsync<GameObject>(modelData.ModelPath).ToUniTask();
                    modelController.IsModelLoadFinish = true;
                }
                catch (Exception e)
                {
                    Debug.LogError($"加载模型失败，路径{modelData.ModelPath}");
                }
            }

           
        }
    }
}