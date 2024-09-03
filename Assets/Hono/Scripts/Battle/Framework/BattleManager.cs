using Hono.Scripts.Battle.Tools;
using UnityEngine;

namespace Hono.Scripts.Battle {
	public class BattleManager : MonoBehaviour {
		private static BattleManager _instance;

		public static BattleManager Instance {
			get {
				if (_instance == null) {
					// 尝试查找现有实例
					_instance = FindObjectOfType<BattleManager>();

					// 如果没有找到，则创建新的 GameObject 并添加该组件
					if (_instance == null) {
						GameObject singletonObject = new GameObject(nameof(BattleManager));
						_instance = singletonObject.AddComponent<BattleManager>();
					}
				}

				return _instance;
			}
		}

		/// <summary>
		/// 获取Id生成器
		/// </summary>
		private static CommonUtility.IdGenerator _idGenerator = CommonUtility.GetIdGenerator();

		
		protected void Awake() {
			DontDestroyOnLoad(this.gameObject);

			// 检查是否已经存在另一个实例
			if (_instance != null && _instance != this) {
				Destroy(this.gameObject);
			}
			else {
				_instance = this;
			}
			
			init();
		}

		private void init() {
			AssetManager.Instance.Init();
			AbilityDataMgr.Instance.Init();
			ActorManager.Instance.Init();
		}

		public void Update() {
			//临时做法
			//保证逻辑帧在表现帧之前执行一次
			Tick(Time.deltaTime);

			ActorManager.Instance.Update(Time.deltaTime);
		}

		public void Tick(float dt) {
			ActorManager.Instance.Tick(dt);
		}
	}
}