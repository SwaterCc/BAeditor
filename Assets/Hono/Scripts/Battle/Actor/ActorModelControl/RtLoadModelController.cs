using System;
using Cysharp.Threading.Tasks;
using Hono.Scripts.Battle.Tools;
using UnityEngine;
using UnityEngine.AddressableAssets;
using Object = UnityEngine.Object;

namespace Hono.Scripts.Battle
{
    public class RtLoadModelController : ActorModelController
    {
        public RtLoadModelController(Actor actor) : base(actor)
        {
            
        }

        private async UniTask asyncLoadModel()
        {
            if (ModelData == null || string.IsNullOrEmpty(ModelData.ModelPath)) return;

            try
            {
                Model = await Addressables.LoadAssetAsync<GameObject>(ModelData.ModelPath).ToUniTask();
                if (!Model.TryGetComponent<ActorModel>(out var handle)) {
                    handle = Model.AddComponent<ActorModel>();
                }
                handle.ActorUid = Uid;
                handle.ActorType = Actor.ActorType;
                Model.name = $"{Actor.ActorType}:{Uid}";

                IsModelLoadFinish = true;
            }
            catch (Exception e)
            {
                Debug.LogError($"加载模型失败，路径{ModelData.ModelPath}");
            }
        }
    }
}