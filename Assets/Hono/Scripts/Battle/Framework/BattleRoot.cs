using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks.Triggers;
using Hono.Scripts.Battle.Tools;
using UnityEngine;

namespace Hono.Scripts.Battle
{
	public interface ITick
	{
		public void Tick(float dt);
	}

	public static class BattleSetting {
		public const int BattleModePrototypeId = 4;
		public const int DefaultHitBoxPrototypeId = 3;
	}
	
    public class BattleRoot : MonoBehaviour
    {
        public List<ActorRefreshPoint> RefreshPoints = new List<ActorRefreshPoint>();

        private static BattleRoot _instance;
        private bool _isFirstUpdate;
        private bool _allLoadFinish;
        private Actor _battleMode;
        public static Actor BattleMode => Instance._battleMode;

        public static BattleRoot Instance
        {
            get
            {
                if (_instance == null)
                {
                    // 尝试查找现有实例
                    _instance = FindObjectOfType<BattleRoot>();

                    // 如果没有找到，则创建新的 GameObject 并添加该组件
                    if (_instance == null)
                    {
                        GameObject singletonObject = new GameObject(nameof(BattleRoot));
                        _instance = singletonObject.AddComponent<BattleRoot>();
                    }
                }

                return _instance;
            }
        }

        protected void Awake()
        {
            DontDestroyOnLoad(this.gameObject);

            // 检查是否已经存在另一个实例
            if (_instance != null && _instance != this)
            {
                Destroy(this.gameObject);
            }
            else
            {
                _instance = this;
            }

            _isFirstUpdate = true;
            //初始化Lua环境
            LuaInterface.Init();
            //反射缓存
            AbilityFuncPreLoader.InitAbilityFuncCache();
            
            init();
        }

        private void init()
        {
#if UNITY_EDITOR
            DebugMode.Instance.Init();
#endif
            ConfigManager.Instance.Init();
            AssetManager.Instance.Init();
            ActorManager.Instance.Init();
        }

        private void firstUpdate() {
	        _battleMode = ActorManager.Instance.CreateActor(BattleSetting.BattleModePrototypeId);
            foreach (var point in RefreshPoints)
            {
                //if (point.PointType == EPointType.Player)
                {
                    point.CreateActor();
                }
            }

            _isFirstUpdate = false;
        }

        public void Update()
        {
            if (!AssetManager.Instance.IsLoadFinish || !ConfigManager.Instance.IsLoadFinish)
            {
                return;
            }

            if (_isFirstUpdate)
                firstUpdate();

            _battleMode.Tick(Time.deltaTime);
            //临时做法
            //保证逻辑帧在表现帧之前执行一次
            Tick(Time.deltaTime);

            ActorManager.Instance.Update(Time.deltaTime);
        }

        public void Tick(float dt)
        {
            ActorManager.Instance.Tick(dt);
        }

        public void OnDestroy()
        {
            LuaInterface.Dispose();
        }
    }
}