using Cysharp.Threading.Tasks;
using System;
using UnityEngine;
using UnityEngine.AddressableAssets;
using Object = UnityEngine.Object;

namespace Hono.Scripts.Battle {
	public class BattleModeModelController : ActorModelController{
		public BattleModeModelController(Actor actor) : base(actor) {
			
		}

		protected  override async UniTask setupModel() {
			try
			{
				Model = await Addressables.LoadAssetAsync<GameObject>("Assets/BattleData/Tools/BattleRoot.prefab").ToUniTask();
				Model = Object.Instantiate(Model, Vector3.zero, Quaternion.identity);
				if (!Model.TryGetComponent<ActorModel>(out var handle)) {
					handle = Model.AddComponent<ActorModel>();
				}
				handle.ActorUid = Uid;
				handle.ActorType = Actor.ActorType;
				Model.name = $"{Actor.ActorType}:{Uid}";
			}
			catch (Exception e)
			{
				Debug.LogError($"加载模型失败，路径{ModelData.ModelPath}");
			}
		}
	}
}