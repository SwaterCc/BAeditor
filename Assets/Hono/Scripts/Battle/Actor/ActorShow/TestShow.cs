using System;
using Cysharp.Threading.Tasks;
using Hono.Scripts.Battle.Tools;
using UnityEngine;
using UnityEngine.AddressableAssets;
using Object = UnityEngine.Object;

namespace Hono.Scripts.Battle
{
    public class TestShow : ActorShow
    {
        public TestShow(Actor actor, ActorShowTable.ActorShowRow data) : base(actor, data) { }

        protected override async UniTask loadModel()
        {
            if (ShowData == null || string.IsNullOrEmpty(ShowData.ModelPath)) return;

            try
            {
                Model = await Addressables.LoadAssetAsync<GameObject>(ShowData.ModelPath).ToUniTask();
                Model = Object.Instantiate(Model);
                if (Model.TryGetComponent<ActorModelHandle>(out var handle))
                {
                    handle.ActorUid = Uid;
                }
            }
            catch (Exception e)
            {
                Debug.LogError($"加载模型失败，路径{ShowData.ModelPath}");
            }
        }
    }
}