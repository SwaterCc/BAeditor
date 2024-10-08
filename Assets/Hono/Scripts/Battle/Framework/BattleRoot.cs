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
	
    public class BattleRoot : MonoSingleton<BattleRoot>
    {
        public List<ActorRefreshPoint> RefreshPoints = new List<ActorRefreshPoint>();

        private static BattleRoot _instance;
        private bool _isFirstUpdate;
        private bool _allLoadFinish;
        private Actor _battleMode;
        public static Actor BattleMode => Instance._battleMode;
        public bool AutoUltimateSkill;
        public EBattleModeType BattleModeType;
        protected void Awake()
        {
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
	       
	        _battleMode = ActorManager.Instance.GetBattleMode();
	        _battleMode.Init();
            foreach (var point in RefreshPoints)
            {
                //if (point.PointType == EPointType.Player)
                {
                    point.CreateActor();
                }
            }

            _isFirstUpdate = false;
        }

        private void initBattleMode() { }

        private void initActorGroup() { }

        public void Update()
        {
            if (!AssetManager.Instance.IsLoadFinish || !ConfigManager.Instance.IsLoadFinish)
            {
                return;
            }

            if (_isFirstUpdate)
                firstUpdate();
            
            //临时做法
            //保证逻辑帧在表现帧之前执行一次
            Tick(Time.deltaTime);
            
            _battleMode.Update(Time.deltaTime);
            ActorManager.Instance.Update(Time.deltaTime);
        }

        public void Tick(float dt)
        {
	        _battleMode.Tick(Time.deltaTime);
            ActorManager.Instance.Tick(dt);
            MessageCenter.Instance.Tick(dt);
        }

        public void OnDestroy()
        {
            LuaInterface.Dispose();
        }
    }
}