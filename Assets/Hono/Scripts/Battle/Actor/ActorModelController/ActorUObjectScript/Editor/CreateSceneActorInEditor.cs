using UnityEditor;
using UnityEngine;

namespace Hono.Scripts.Battle.Editor {
	public static class CreateSceneActorInEditor {
		[MenuItem("GameObject/战斗工具/刷怪器",false,10)]
		private static void createMonsterGen() {
			CreatePrefab("Assets/BattleData/GameTestRes/TestModel/MonsterGenerator.prefab");
		}
		
		[MenuItem("GameObject/战斗工具/玩家出生点",false,10)]
		private static void createMonsterPlayerPoint() {
			CreatePrefab("Assets/BattleData/GameTestRes/TestModel/TeamDefaultBrithPoint.prefab");
		}
		
		[MenuItem("GameObject/战斗工具/触发器",false,10)]
		private static void createMonsterTriggerBox() {
			CreatePrefab("Assets/BattleData/GameTestRes/TestModel/TriggerBox.prefab");
		}
		
		[MenuItem("GameObject/战斗工具/可建造区域",false,10)]
		private static void createMonsterBuildArea() {
			CreatePrefab("Assets/BattleData/GameTestRes/TestModel/BuildArea.prefab");
		}
		
		[MenuItem("GameObject/战斗工具/Actor创建器",false,10)]
		private static void createActorPoint() {
			CreatePrefab("Assets/BattleData/GameTestRes/TestModel/ActorRefreshPoint.prefab");
		}
		private static void CreatePrefab(string path) {
			// 加载你的Prefab (假设它位于Assets/Prefabs/MyPrefab.prefab)
			GameObject prefab = AssetDatabase.LoadAssetAtPath<GameObject>(path);

			if (prefab != null) {
				// 实例化Prefab并将其放置到场景中
				GameObject instance = (GameObject)PrefabUtility.InstantiatePrefab(prefab);

				// 确保新实例被选中，以便用户可以立即看到它
				Selection.activeObject = instance;

				// 如果没有选择任何父对象，将新实例放在根层级
				if (Selection.activeTransform == null) {
					instance.transform.SetParent(null);
				}
				else {
					// 否则，将新实例作为当前选中对象的子对象
					instance.transform.SetParent(Selection.activeTransform);
				}

				// 重置位置以便更好地查看
				instance.transform.localPosition = Vector3.zero;
			}
			else {
				Debug.LogError("无法加载Prefab，请检查路径是否正确！");
			}
		}
	}
}