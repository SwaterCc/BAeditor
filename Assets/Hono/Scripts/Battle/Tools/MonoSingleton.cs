using UnityEngine;

namespace Hono.Scripts.Battle.Tools {
	public class MonoSingleton<T> : MonoBehaviour where T : MonoBehaviour {
		private static T _instance;
		private static readonly object _lock = new object();

		public static T Instance {
			get {
				if (_instance == null) {
					lock (_lock) {
						if (_instance == null) {
							_instance = FindObjectOfType<T>();

							if (_instance == null) {
								GameObject singletonObject = new GameObject();
								_instance = singletonObject.AddComponent<T>();
								singletonObject.name = typeof(T).ToString() + " (Singleton)";

								// 保持单例对象在场景切换时不被销毁
								DontDestroyOnLoad(singletonObject);
							}
						}
					}
				}

				return _instance;
			}
		}

		protected virtual void Awake() {
			if (_instance == null) {
				_instance = this as T;
				DontDestroyOnLoad(gameObject);
			}
			else if (_instance != this) {
				Destroy(gameObject);
			}
		}
	}
}