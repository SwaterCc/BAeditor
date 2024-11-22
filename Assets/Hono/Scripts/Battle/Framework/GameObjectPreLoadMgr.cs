#region

using Cysharp.Threading.Tasks;
using Hono.Scripts.Battle.Tools;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;

#endregion

namespace Hono.Scripts.Battle {
	public class GameObjectPreLoadMgr : Singleton<GameObjectPreLoadMgr>, IBattleFrameworkAsyncInit {
		private Dictionary<string, GameObject> _objectCaches;
		
		private readonly string[] _path = {
			"Assets/BattleRes/Model/BattleBuild/Archery_Demo2.prefab",
			"Assets/BattleRes/Model/BattleBuild/Tower_AttackUp_Demo2.prefab",
			"Assets/BattleRes/Model/BattleBuild/Tower_Heal_Demo2.prefab",
			"Assets/BattleRes/Model/BattleBuild/Tower_BigBow_Demo2.prefab",
			"Assets/BattleRes/Model/BattleBuild/Tower_MagicIce_Demo2.prefab",
			"Assets/BattleRes/Model/BattleBuild/Tower_MagicFire_Demo2.prefab",
			"Assets/BattleRes/Model/BattleBuild/Tower_Magic_Demo2.prefab",
			"Assets/BattleRes/Model/BattleBuild/Tower_CrossBow_Demo2.prefab",
			"Assets/BattleRes/Model/BattleBuild/Tower_Cannon_Demo2.prefab",
			"Assets/BattleRes/Characters/Character_MagicGirl_Build.prefab",
			BattleConstValue.BattleRootModel,
			BattleConstValue.BulletModel,
			BattleConstValue.HitBoxModel,
			BattleConstValue.TeamRefreshPoint,
			BattleConstValue.LootModel,
		};
		
		public async UniTask AsyncInit() {

			_objectCaches = new Dictionary<string, GameObject>(_path.Length);
			List<UniTask> loadTasks = new(_path.Length);
			
			foreach (var path in _path) {
				loadTasks.Add(loadGameObject(path));
			}

			await UniTask.WhenAll(loadTasks);
			
			Debug.Log("GameObjectPreLoadMgr Init Finish！");
		}
		
		public GameObject this[string path] => _objectCaches[path];
		public GameObject GetBuildingCache(string path) {
			return _objectCaches[path];
		}

		private async UniTask loadGameObject(string path) {
			if (string.IsNullOrEmpty(path)) {
				Debug.LogError($"{path} 路径为空");
				return;
			}

			var uObj = await Addressables.LoadAssetAsync<GameObject>(path).ToUniTask();
			_objectCaches.Add(path, uObj);
		}
	}
}