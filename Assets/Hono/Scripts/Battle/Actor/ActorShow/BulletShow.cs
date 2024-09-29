using Cysharp.Threading.Tasks;
using System;
using UnityEngine;
using UnityEngine.AddressableAssets;
using Object = UnityEngine.Object;

namespace Hono.Scripts.Battle {
	public class BulletShow :ActorShow {
		public BulletShow(Actor actor) : base(actor) {
			
		}

		protected override async UniTask loadModel() {
			try
			{
				Model = await Addressables.LoadAssetAsync<GameObject>("Assets/BattleData/Tools/BulletModel.prefab").ToUniTask();
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
				Debug.LogError($"����ģ��ʧ�ܣ�·��{ModelData.ModelPath}");
			}
		}
	}
}