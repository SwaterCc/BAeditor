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
                    return BattleConstValue.BattleRootModel;
                case EPreLoadGameObjectType.BulletModel:
                    return BattleConstValue.BulletModel;
                case EPreLoadGameObjectType.HitBoxModel:
                    return BattleConstValue.HitBoxModel;
                case EPreLoadGameObjectType.TeamRefreshPoint:
                    return BattleConstValue.TeamRefreshPoint;
            }

            return null;
        }

        private async UniTask loadGameObject(EPreLoadGameObjectType element)
        {
            string path = getObjectPath(element);
            if (string.IsNullOrEmpty(path)) 
            {
                Debug.LogError($"{element} 路径为空");
                return;
            }
            var uObj = await Addressables.LoadAssetAsync<GameObject>(getObjectPath(element)).ToUniTask();
            _objectCaches.Add(element, uObj);
        }
    }
}