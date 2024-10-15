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

        private string _formScene;
        private BattleController _battleController;
        private BattleGround _waitEnterGround;
        private bool _popTopGround;
        private readonly Stack<BattleGround> _groundStack = new(4);
        
        
        public static BattleController BattleController => Instance._battleController;
        public static BattleGround CurrentBattleGround => Instance._groundStack.Count > 0 ? Instance._groundStack.Peek() : null;


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
            register(ConfigManager.Instance);
            register(AssetManager.Instance);
            register(BattleEventManager.Instance);
            register(MessageCenter.Instance);
            register(GameObjectPreLoadMgr.Instance);
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
            PushBattleGround(battleGroundId);
        }
        
        /// <summary>
        /// 开始战斗玩法
        /// </summary>
        public void PushBattleGround(int battleGroundId, bool useSave = false)
        {
            Debug.Log($"[BattleManager] PushBattleGround");
            var row = ConfigManager.Table<BattleSceneTable>().Get(battleGroundId);
            _waitEnterGround = new BattleGround(row.ScenePath);
            _waitEnterGround.OnCreate();
        }

        public void PopBattleGround()
        {
            _popTopGround = _groundStack.Count != 0;
            Debug.Log("[BattleManager] PopBattleGround");
        }

        private void onSwitchBattleGround()
        {
            
        }
        
        /// <summary>
        /// 退出战斗玩法返回主界面
        /// </summary>
        public void ExitBattle()
        {
            Debug.Log("[BattleManager] ExitBattle");
            
            foreach (var framework in _frameworkEnterExits)
            {
                framework.OnExitBattle();
            }

            foreach (var ground in _groundStack)
            {
                ground.OnDestroy();
            }
            _groundStack.Clear();
            
            //返回进入时的场景
            SceneManager.LoadScene(_formScene);
        }

        private void Tick(float dt)
        {
            if (_battleDataLoadState != EBattleDataLoadState.LoadFinish)
            {
                //Debug.LogError($"战斗数据未准备完成 当前状态{_battleDataLoadState}");
                return;
            }

            _battleController ??= ActorManager.Instance.GetBattleControl();
            
            foreach (var frameworkTick in _frameworkTicks)
            {
                frameworkTick.Tick(dt);
            }
            
            if (_waitEnterGround != null)
            {
                _groundStack.Push(_waitEnterGround);
                _waitEnterGround.EnterGround();
                _waitEnterGround = null;
                onSwitchBattleGround();
            }
            
            if (_groundStack.Count == 0) return;
                        
            var curGround = _groundStack.Peek();
            curGround.Tick(dt);
            
            if (_popTopGround)
            {
                var pop = _groundStack.Pop();
                pop.OnDestroy();
                if (_groundStack.Count == 0)
                {
                    ExitBattle();
                }
                else
                {
                    onSwitchBattleGround();
                }

                _popTopGround = false;
            }
        }
        
        private void Update()
        {
            Tick(Time.deltaTime);
            
        }

        #endregion
    }
}