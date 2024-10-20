using Cysharp.Threading.Tasks;
using System;
using UnityEngine;
using UnityEngine.AddressableAssets;
using Object = UnityEngine.Object;

namespace Hono.Scripts.Battle
{
    public partial class ActorModelController
    {
        public class PreLoadModelSetup : ModelSetup
        {
            private EPreLoadGameObjectType _objectType;

            public void Init(EPreLoadGameObjectType objectType)
            {
                _objectType = objectType;
                _path = GameObjectPreLoadMgr.Instance.GetObjectPath(objectType);
            }

            protected override void OnLoadModel()
            {
                if (!GameObjectPool.Instance.TryGet(_path, out _gameObject))
                {
                    _gameObject = Object.Instantiate(GameObjectPreLoadMgr.Instance[_objectType]);
                }

                _loadComplete.Invoke(_gameObject);
            }

            protected override void OnUnInit()
            {
                _objectType = default;
            }
        }
    }
}