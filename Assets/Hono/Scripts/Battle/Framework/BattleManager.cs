using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Hono.Scripts.Battle.Event;
using Hono.Scripts.Battle.Scene;
using Hono.Scripts.Battle.Tools;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.SceneManagement;

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

    public interface IBattleFramework
    {
    }

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
        private BattleLevelRoot _levelRoot;
        private string _fromScene;
        
        #region 框架初始化

        private void register(IBattleFramework framework)
        {
            if (_frameworks.Contains(framework))
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
            register(ConfigManager.Instance);
            register(AssetManager.Instance);
            register(BattleEventManager.Instance);
            register(MessageCenter.Instance);
        }

        /// <summary>
        /// 装载战斗框架
        /// </summary>
        public void SetupBattleFramework()
        {
            //初始化Lua环境
            LuaInterface.Init();
            //反射缓存
            AbilityFuncPreLoader.InitAbilityFuncCache();

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
            var beginTime = Time.time;
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
            Debug.LogError($"战斗数据加载完成！耗时 {Time.time - beginTime}");
        }

        #endregion

        #region 战斗玩法流程

        private async UniTask switchScene(string sceneName)
        {
            //先进入LoadingScene
            SceneManager.LoadScene(0);
            List<UniTask> tasks = new List<UniTask>();
            //固定等待0.2秒
            tasks.Add(UniTask.WaitForSeconds(0.5f));
            tasks.Add(SceneManager.LoadSceneAsync(sceneName).ToUniTask());
            await UniTask.WhenAll(tasks);
        }

        /// <summary>
        /// 给关卡创建流程
        /// </summary>
        private async void setupBattleLevelRoot(bool useSave)
        {
            try
            {
                //加载关卡数据
                var battleLevelData = await Addressables.LoadAssetAsync<BattleLevelData>("path").ToUniTask();
                
                //创建关卡root
                _levelRoot = new BattleLevelRoot();
                _levelRoot.Setup(battleLevelData, useSave);
            }
            catch (Exception e)
            {
                Debug.LogError($"缺少关卡数据（BattleLevelData）对象 {e}");
                //可以考虑切换回原场景
            }
        }
        
        /// <summary>
        /// 开始战斗玩法
        /// </summary>
        public async void EnterBattle(string fromScene, string battleLevelName, bool useSave = false)
        {
            _fromScene = fromScene;

            foreach (var framework in _frameworkEnterExits)
            {
                framework.OnEnterBattle();
            }
            
            if (_battleDataLoadState != EBattleDataLoadState.LoadFinish)
            {
                Debug.LogError($"战斗数据未准备完成 当前状态{_battleDataLoadState}");
                return;
            }
            
            //切换场景到关卡场景
            await switchScene(battleLevelName);
            
            //切换完成，装载关卡流程
            setupBattleLevelRoot(useSave);
        }
        
        /// <summary>
        /// 退出战斗玩法返回主界面
        /// </summary>
        public void ExitBattle()
        {
            _levelRoot?.ExitBattleLevel();
            
            foreach (var framework in _frameworkEnterExits)
            {
                framework.OnExitBattle();
            }
            
            switchScene(_fromScene).Forget();
        }

        private void Tick()
        {
            foreach (var frameworkTick in _frameworkTicks)
            {
                frameworkTick.Tick(Time.deltaTime);
            }
            
            if(_levelRoot == null || _levelRoot.AllLoadedFinish == false) 
                return;
            
            _levelRoot.OnTick(Time.deltaTime);
        }
        
        private void Update()
        {
            if(_levelRoot == null || _levelRoot.AllLoadedFinish == false) 
                return;

            Tick();
            
            _levelRoot.OnUpdate(Time.deltaTime);
        }

        #endregion
    }
}