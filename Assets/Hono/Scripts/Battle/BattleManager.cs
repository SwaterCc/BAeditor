#region

using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Hono.Scripts.Battle.AbilityFramework;
using Hono.Scripts.Battle.Base;
using Hono.Scripts.Battle.BattleFoundation;
using Hono.Scripts.Battle.Core;
using Hono.Scripts.Battle.Core.AbilityFramework;
using Hono.Scripts.Battle.Core.Base;
using Hono.Scripts.Battle.Define;
using Hono.Scripts.Battle.Event;
using Hono.Scripts.Battle.Tools;
using UnityEngine;
using UnityEngine.SceneManagement;
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
    
    #endregion

    public class BattleManager : MonoSingleton<BattleManager>
    {
        private EBattleDataLoadState _battleDataLoadState;
        private readonly List<IBattleFoundation> _foundations = new(32);

        private TagTree _tree = new();
        public static TagTree TagTree => Instance._tree;
        
        public Action<bool> ExitBattleCallBack { get; set; }
        public event Action OnLateUpdate;
        private WorldInstance _currentWorldInstance;
        private string _formScene;
        protected void Start()
        {
            InitEnv();
        }

        #region 框架初始化

        private void register(IBattleFoundation framework)
        {
            if (!_foundations.Contains(framework))
            {
                _foundations.Add(framework);
            }
        }

        private void registerAllFrameworks()
        {
            register(ConfigDataBase.Instance);
            register(AssetManager.Instance);
            register(LuaBridge.Instance);
            register(ActorJsonAssemblerFactory.Instance);
            register(SkinTemplateDateBase.Instance);
        }

        /// <summary>
        /// 装载战斗框架重要资源
        /// </summary>
        public async void InitEnv()
        {
            //注册所有的框架
            registerAllFrameworks();
            
            try
            {
                //加载资源
                await asyncFrameworkLoad();
            }
            catch (Exception e)
            {
                _battleDataLoadState = EBattleDataLoadState.LoadFailed;
                Debug.LogError("数据加载失败！" + e);
                return;
            }

            initOtherFoundations();
            
            _battleDataLoadState = EBattleDataLoadState.LoadFinish;
        }
        
        private async UniTask asyncFrameworkLoad()
        {
            var beginTime = Time.realtimeSinceStartup;
            _battleDataLoadState = EBattleDataLoadState.Loading;
            List<UniTask> tasks = new List<UniTask>();

            foreach (var foundation in _foundations)
            {
                tasks.Add(foundation.AsyncLoad());
            }

            await UniTask.WhenAll(tasks);
            
            Debug.Log($"战斗数据加载完成！耗时 {Time.realtimeSinceStartup - beginTime}");
        }

        private void initOtherFoundations()
        {
            //Ability模板加载
            Ability.AbilityEnv.InitEnv();
            //属性链接初始化
            AttrHelper.Instance.Init();
            //初始化TagTree
            _tree.BuildTree();
        }

        #endregion

        #region 战斗玩法流程

        /// <summary>
        /// 进入战争玩法
        /// </summary>
        /// <param name="fromScene"></param>
        /// <param name="battleGroundId"></param>
        public void EnterWorld(string fromScene, int battleGroundId)
        {
            _formScene = fromScene;
            _currentWorldInstance = new WorldInstance(battleGroundId);
            _currentWorldInstance.Enter();
        }
        
        private void Update()
        {
            if (_battleDataLoadState != EBattleDataLoadState.LoadFinish)
            {
                Debug.Log($"战斗数据未准备完成 当前状态 {_battleDataLoadState}");
                return;
            }

            World.Current?.Tick(Time.deltaTime);
        }

        private void LateUpdate()
        {
            OnLateUpdate?.Invoke();
        }

        /// <summary>
        ///     退出战斗玩法返回主界面
        /// </summary>
        public void ExitWorld()
        {
            Debug.Log("[BattleManager] ExitBattle");
            _currentWorldInstance.Exit();
            _currentWorldInstance = null;
            //回到之前的场景中
            SceneManager.LoadScene(_formScene);
        }
        #endregion
    }
}