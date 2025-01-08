#region

using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Hono.Scripts.Battle.Define;
using Hono.Scripts.Battle.Event;
using Hono.Scripts.Battle.Tools;
using UnityEngine;
using UnityEngine.SceneManagement;
#if UNITY_EDITOR
#else
using UnityEngine.AddressableAssets;
#endif

#endregion

namespace Hono.Scripts.Battle
{
    #region BattleManager接口

    public enum EBattleDataLoadState
    {
        NoLoaded,
        Loading,
        LoadFinish,
        LoadFailed,
    }

    public interface IBattleFramework { }

    public interface IBattleFrameworkEnterExit : IBattleFramework
    {
        public void OnEnterBattle();
        public void OnExitBattle();
    }

    public interface IBattleFrameworkInit : IBattleFramework
    {
        public void Init();
    }

    public interface IBattleFrameworkAsyncInit : IBattleFramework
    {
        public UniTask AsyncInit();
    }

    public interface IBattleFrameworkTick : IBattleFramework
    {
        public void Tick(float dt);
    }

    #endregion

    public class BattleManager : MonoSingleton<BattleManager>
    {
        private EBattleDataLoadState _battleDataLoadState;

        private readonly List<IBattleFramework> _frameworks = new(32);
        private readonly List<IBattleFrameworkInit> _frameworkInits = new(16);
        private readonly List<IBattleFrameworkEnterExit> _frameworkEnterExits = new(16);
        private readonly List<IBattleFrameworkAsyncInit> _frameworkAsyncLoads = new(16);
        private readonly List<IBattleFrameworkTick> _frameworkTicks = new(16);
        
        private Paths _paths;
        private string _formScene;
        private BattleGround _curGround;

        public static BattleController BattleController => CurBattle.BattleController;
        public static BattleGround CurBattle => Instance._curGround;
        public static Paths Paths => Instance._paths;
        
        public Action<bool> ExitBattleCallBack { get; set; }

        protected void Start()
        {
            SetupBattleFramework();
        }

        #region 框架初始化

        private void register(IBattleFramework framework)
        {
            if (!_frameworks.Contains(framework))
            {
                _frameworks.Add(framework);
            }
            else
            {
                return;
            }

            if (framework is IBattleFrameworkInit frameworkInit)
            {
                _frameworkInits.Add(frameworkInit);
            }

            if (framework is IBattleFrameworkAsyncInit frameworkLoad)
            {
                _frameworkAsyncLoads.Add(frameworkLoad);
            }

            if (framework is IBattleFrameworkEnterExit frameworkEnterExit)
            {
                _frameworkEnterExits.Add(frameworkEnterExit);
            }

            if (framework is IBattleFrameworkTick frameworkTick)
            {
                _frameworkTicks.Add(frameworkTick);
            }
        }

        private void registerAllFrameworks()
        {
            //register(LuaInterface.Instance);
            register(ConfigManager.Instance);
            //register(AssetManager.Instance);
            register(EventManager.Instance);
            register(MessageCenter.Instance);
            //register(GameObjectPreLoadMgr.Instance);
            register(ActorManager.Instance);
        }

        /// <summary>
        ///     装载战斗框架
        /// </summary>
        public async void SetupBattleFramework()
        {
            //反射缓存
            AbilityFuncPreLoader.InitAbilityFuncCache();

#if UNITY_EDITOR
#else
			 //加载路径资源文件
            try
            {
	            _paths = await Addressables.LoadAssetAsync<Paths>(BattleConstValue.PathFile);
            }
            catch (Exception e)
            {
	            Debug.LogError("path文件加载失败！战斗环境初始化失败！");
	            throw;
            }
#endif
            //注册所有的框架
            registerAllFrameworks();

            //初始化框架
            initFramework();

            //加载资源
            asyncFrameworkLoad();
        }

        private void initFramework()
        {
            foreach (var framework in _frameworkInits)
            {
                framework.Init();
            }
        }

        private async void asyncFrameworkLoad()
        {
            var beginTime = Time.realtimeSinceStartup;
            _battleDataLoadState = EBattleDataLoadState.Loading;
            List<UniTask> tasks = new List<UniTask>();

            foreach (var framework in _frameworkAsyncLoads)
            {
                tasks.Add(framework.AsyncInit());
            }

            try
            {
                await UniTask.WhenAll(tasks);
            }
            catch (Exception e)
            {
                _battleDataLoadState = EBattleDataLoadState.LoadFailed;
                Debug.LogError("数据加载失败！" + e);
                return;
            }

            _battleDataLoadState = EBattleDataLoadState.LoadFinish;
            Debug.Log($"战斗数据加载完成！耗时 {Time.realtimeSinceStartup - beginTime}");
        }

        #endregion

        #region 战斗玩法流程

        public void EnterBattle(string fromScene, int battleGroundId)
        {
            _formScene = fromScene;
            _curGround = new BattleGround(battleGroundId);
            _curGround.ExitGround();
        }

        /// <summary>
        ///     退出战斗玩法返回主界面
        /// </summary>
        public void ExitBattle()
        {
            Debug.Log("[BattleManager] ExitBattle");

            foreach (var framework in _frameworkEnterExits)
            {
                framework.OnExitBattle();
            }

            _curGround.ExitGround();

            /*if (LoadingPanel.Exists)
            {
                LoadingPanel.Instance.Show(() =>
                {
                    //返回进入时的场景
                    SceneManager.LoadScene(_formScene);
                    ExitBattleCallBack?.Invoke(result);
                });
            }*/
        }

        private void Tick(float dt)
        {
            if (_battleDataLoadState != EBattleDataLoadState.LoadFinish)
            {
                Debug.Log($"战斗数据未准备完成 当前状态 {_battleDataLoadState}");
                return;
            }

            foreach (var frameworkTick in _frameworkTicks)
            {
                frameworkTick.Tick(dt);
            }
            
            _curGround?.Tick(dt);
        }

        private void Update()
        {
            Tick(Time.deltaTime);
        }

        #endregion
    }
}