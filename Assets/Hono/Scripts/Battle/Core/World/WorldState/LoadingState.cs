using System;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Hono.Scripts.Battle.Core
{
    public partial class World
    {
        private class LoadingState : WorldState
        {
            private AsyncOperation _asyncOperation;
            private bool _isSetupScene;
            private float _timeCounting;
            public LoadingState(World world) : base(world, EWorldState.Loading) { }

            protected async override void OnEnter()
            {
                //进入加载场景
                await SceneManager.LoadSceneAsync("BattleLoading");
                //异步加载游戏场景
                _asyncOperation = SceneManager.LoadSceneAsync(World._sceneRow.ScenePath);
                if (_asyncOperation == null)
                    throw new NullReferenceException($"加载场景{World._sceneRow.ScenePath}失败");
                _asyncOperation.allowSceneActivation = false;
                _timeCounting = Time.realtimeSinceStartup;
            }

            protected override void OnTick(float dt)
            {
                if (!_asyncOperation.isDone)
                    return;
                
                if (Mathf.Approximately(0.9f, _asyncOperation.progress) && !_asyncOperation.isDone)
                {
                    //进入场景
                    _asyncOperation.allowSceneActivation = true;
                    return;
                }

                if (!_isSetupScene)
                {
                    UIManager.Instance.SetLoadingUI(true);
                    setupScene();
                    return;
                }

                UIManager.Instance.SetLoadingUI(false);

                Debug.Log($"[setupSceneTime] {Time.realtimeSinceStartup - _timeCounting}");
                //加载完成后进入准备状态
                World._nextState = EWorldState.Ready;
            }

            private void setupScene()
            {
                _isSetupScene = true;
                //开启加载中UI
                
                //获取地图数据
                
                //创建地图单位
               
                //任务流加载
                
                //加载主UI
            }
            
            protected override void OnExit()
            {
              
                _asyncOperation = null;
            }
        }
    }
}