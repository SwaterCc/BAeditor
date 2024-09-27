using Cysharp.Threading.Tasks;
using System;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.AddressableAssets;
using Object = UnityEngine.Object;

namespace Hono.Scripts.Battle {
	public class BattleModeShow : ActorShow{
		public BattleModeShow(Actor actor) : base(actor) {
			
		}

		protected  override async UniTask loadModel() {
			try
			{
				Model = await Addressables.LoadAssetAsync<GameObject>("Assets/BattleData/Tools/BattleRoot.prefab").ToUniTask();
				Model = Object.Instantiate(Model, Vector3.zero, quaternion.identity);
				if (!Model.TryGetComponent<ActorModel>(out var handle)) {
					handle = Model.AddComponent<ActorModel>();
				}
				handle.ActorUid = Uid;
				handle.ActorType = Actor.ActorType;
				Model.name = $"{Actor.ActorType}:{Uid}";
			}
			catch (Exception e)
			{
				Debug.LogError($"����ģ��ʧ�ܣ�·��{ModelData.ModelPath}");
			}
		}
	}
}