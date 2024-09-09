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
        public TestShow(int uid, ActorShowTable.ActorShowRow data) : base(uid, data) { }

        protected override async UniTask loadModel()
        {
            if (_showData == null || string.IsNullOrEmpty(_showData.ModelPath)) return;

            try
            {
                _gameObject = await Addressables.LoadAssetAsync<GameObject>(_showData.ModelPath).ToUniTask();
                _gameObject = Object.Instantiate(_gameObject);
                if (_gameObject.TryGetComponent<ActorModelHandle>(out var handle))
                {
                    handle.ActorUid = Uid;
                }
            }
            catch (Exception e)
            {
                Debug.LogError($"加载模型失败，路径{_showData.ModelPath}");
            }
        }
    }
}