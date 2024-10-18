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
        private readonly Dictionary<string, GameObject> _buildingCaches = new();
        private string[] buildingPath = {
	        "Assets/BattleRes/Model/BattleBuild/Archery_Demo2.prefab",
	        "Assets/BattleRes/Model/BattleBuild/Tower_AttackUp_Demo2.prefab",
	        "Assets/BattleRes/Model/BattleBuild/Tower_Heal_Demo2.prefab",
	        "Assets/BattleRes/Model/BattleBuild/Tower_BigBow_Demo2.prefab",
	        "Assets/BattleRes/Model/BattleBuild/Tower_MagicIce_Demo2.prefab",
	        "Assets/BattleRes/Model/BattleBuild/Tower_MagicFire_Demo2.prefab",
	        "Assets/BattleRes/Model/BattleBuild/Tower_Magic_Demo2.prefab",
	        "Assets/BattleRes/Model/BattleBuild/Tower_CrossBow_Demo2.prefab",
			"Assets/BattleRes/Model/BattleBuild/Tower_Cannon_Demo2.prefab"
		};
        public async UniTask AsyncInit()
        {
            List<UniTask> loadTasks = new List<UniTask>();

            foreach (EPreLoadGameObjectType element in Enum.GetValues(typeof(EPreLoadGameObjectType)))
            {
                loadTasks.Add(loadGameObject(element));
            }

            foreach (var path in buildingPath) {
	            loadTasks.Add(loadBuilding(path));
            }
            
            await UniTask.WhenAll(loadTasks);
        }

        public GameObject this[EPreLoadGameObjectType objectType] => _objectCaches[objectType];

        public GameObject GetBuildingCache(string path) {
	        return _buildingCaches[path];
        }
        
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
        
        private async UniTask loadBuilding(string path)
        {
	        if (string.IsNullOrEmpty(path)) 
	        {
		        Debug.LogError($"{path} 路径为空");
		        return;
	        }
	        var uObj = await Addressables.LoadAssetAsync<GameObject>(path).ToUniTask();
	        _buildingCaches.Add(path, uObj);
        }
    }
}