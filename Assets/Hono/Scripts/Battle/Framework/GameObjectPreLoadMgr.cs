using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using Hono.Scripts.Battle.Tools;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Hono.Scripts.Battle
{
    public class GameObjectPreLoadMgr : Singleton<GameObjectPreLoadMgr> , IBattleFrameworkAsyncInit
    {
        private readonly Dictionary<EPreLoadGameObjectType, GameObject> _objectCaches = new();
        
        public async UniTask AsyncInit()
        {
            List<UniTask> loadTasks = new List<UniTask>();

            foreach (EPreLoadGameObjectType element in Enum.GetValues(typeof(EPreLoadGameObjectType)))
            {
                loadTasks.Add(loadGameObject(element));
            }
            
            await UniTask.WhenAll(loadTasks);
        }

        public GameObject this[EPreLoadGameObjectType objectType] => _objectCaches[objectType];
        
        private string getObjectPath(EPreLoadGameObjectType objectType)
        {
            switch (objectType)
            {
                case EPreLoadGameObjectType.BattleRootModel:
                    return "Assets/BattleData/Tools/BattleRoot.prefab";
                case EPreLoadGameObjectType.BulletModel:
                    return "Assets/BattleData/Tools/BulletModel.prefab";
            }

            return null;
        }

        private async UniTask loadGameObject(EPreLoadGameObjectType element)
        {
            var uObj = await Addressables.LoadAssetAsync<GameObject>(getObjectPath(element)).ToUniTask();
            _objectCaches.Add(element, uObj);
        }
    }
}