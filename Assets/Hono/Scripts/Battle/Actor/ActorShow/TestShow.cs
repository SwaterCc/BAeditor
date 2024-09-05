using System;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Hono.Scripts.Battle
{
    public class TestShow : ActorShow
    {
        public TestShow(int uid, ActorShowData data) : base(uid, data) { }

        protected override async UniTask loadModel()
        {
            if (_showData != null && string.IsNullOrEmpty(_showData.ModelPath))
            {
                try
                {
                    _gameObject = await Addressables.LoadAssetAsync<GameObject>(_showData.ModelPath).ToUniTask();
                }
                catch (Exception e)
                {
                    Debug.LogError($"加载模型失败，路径{_showData.ModelPath}");
                }
            }
        }
    }
}