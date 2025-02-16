using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Hono.Scripts.Battle.Core
{
    public partial class WorldInstance
    {
        private class LoadingState : WorldState
        {
            private enum LoadingStage
            {
                None,
                SceneLoading,
                ReadyToLoadMainUI,
                MainUILoading,
                ReadyToLoadSceneObject,
                SceneObjectLoading,
                LoadFinish,
            }
            private LoadingStage _stage = LoadingStage.None;
            /// <summary>
            /// 是否正在加载主ui
            /// </summary>
            private bool _isDoLoadMainUINow;
           
            
            private AsyncOperation _asyncOperation;
           
            private float _timeCounting;
            public LoadingState(WorldInstance worldInstance) : base(worldInstance, EWorldState.Loading) { }

            protected async override void OnEnter()
            {
                //进入加载场景
                await SceneManager.LoadSceneAsync("BattleLoading");
                //异步加载游戏场景
                _asyncOperation = SceneManager.LoadSceneAsync(WorldInstance._sceneRow.ScenePath);
                if (_asyncOperation == null)
                    throw new NullReferenceException($"加载场景{WorldInstance._sceneRow.ScenePath}失败");
                _asyncOperation.allowSceneActivation = false;
                _timeCounting = Time.realtimeSinceStartup;
                _stage = LoadingStage.SceneLoading;
            }

            protected override void OnTick(float dt)
            {
                if (_stage == LoadingStage.None)
                {
                    return;
                }
                
                Debug.Log("加载场景中..");
                if (_stage == LoadingStage.SceneLoading)
                {
                    if (Mathf.Approximately(0.9f, _asyncOperation.progress) && !_asyncOperation.isDone)
                    {
                        //进入场景
                        _asyncOperation.allowSceneActivation = true;
                        _stage = LoadingStage.ReadyToLoadMainUI;
                    }
                    return;
                }

                if ( _stage is LoadingStage.ReadyToLoadMainUI or LoadingStage.MainUILoading )
                {
                    if (_stage == LoadingStage.MainUILoading) 
                        return;
                    _stage = LoadingStage.MainUILoading;
                    loadMainCanvas();
                    return;
                }
                
                if ( _stage is LoadingStage.ReadyToLoadSceneObject or LoadingStage.SceneObjectLoading )
                {
                    if (_stage == LoadingStage.SceneObjectLoading) 
                        return;
                    _stage = LoadingStage.SceneObjectLoading;
                    //开启加载中UI
                    setupScene();
                    return;
                }

                if (_stage == LoadingStage.LoadFinish)
                {
                    Debug.Log($"[setupSceneTime] {Time.realtimeSinceStartup - _timeCounting}");
                    //加载完成后进入准备状态
                    WorldInstance._nextState = EWorldState.Ready;
                    _stage = LoadingStage.None;
                }
            }

            private async void loadMainCanvas()
            {
                if(_isDoLoadMainUINow)
                //加载主UI
                Debug.Log("加载主UI..");
                try
                {
                    await UIManager.Instance.LoadMainCanvas();
                }
                catch (Exception e)
                {
                    Debug.LogError("加载主UI失败，跳过加载");
                    Debug.LogError(e);
                }
               
                _stage = LoadingStage.ReadyToLoadSceneObject;
            }

            private async void setupScene()
            {
                //获取地图数据
                Debug.Log("加载地图数据..");
                //创建地图单位
                Debug.Log("创建地图单位..");
                //任务流加载
                Debug.Log("任务流加载..");
                //worldRoot放置
                Debug.Log("WorldRoot放置..");
                WorldInstance.WorldRoot = new WorldRoot();
                try
                {
                    await UnityAdapter.Instance.CreateUnityObjectProxy(WorldInstance.WorldRoot);
                    WorldInstance.addUnitToWorld(WorldInstance.WorldRoot); 
                }
                catch (Exception e)
                {
                    Debug.LogError("加载世界代理对象失败！");
                    Debug.LogError(e);
                    WorldInstance.addUnitToWorld(WorldInstance.WorldRoot); 
                }
                
                _stage = LoadingStage.LoadFinish;
            }

            protected override void OnExit()
            {
                Debug.Log("加载流程结束..");
                UIManager.Instance.SetLoadingUI(false);
                _stage = LoadingStage.None;
                _asyncOperation = null;
            }
        }
    }
}