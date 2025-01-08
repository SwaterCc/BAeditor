#region

using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using Hono.Scripts.Battle.Base;
using UnityEngine;
using UnityEngine.AddressableAssets;
using Object = UnityEngine.Object;

#endregion

namespace Hono.Scripts.Battle
{
    //非引擎逻辑与引擎逻辑的接口，负责管理GameObject的创建与删除
    public class ModelController
    {
        public Actor Self { get; }
        public int Uid => Self.Uid;
        public TagCollection TagCollection => Self.TagCollection;
        public VariableBoard Variables => Self.VariableBoard;
        public ActorLogic Logic => Self.Logic;

        /// <summary>
        /// 加载取消总key
        /// </summary>
        public CancellationTokenSource MainCancelToken { get; } = new();

        /// <summary>
        /// unity中对应的对象
        /// </summary>
        public ActorModel Model { get; set; }

        /// <summary>
        /// 半径
        /// </summary>
        public float Radius { get; set; }

        /// <summary>
        /// Actor是否需要加载模型
        /// </summary>
        public bool ActorNeedLoadModel = true;

        /// <summary>
        /// 模型是否为预加载模型
        /// 重写以改变加载流程
        /// </summary>
        public bool GameObjectIsPreLoaded = false;

        /// <summary>
        /// 模型路径
        /// </summary>
        public string ModelPath = BattleConstValue.DefaultModel;

        public ModelController(Actor actor)
        {
            Self = actor;
        }

        public async void Setup()
        {
            //启动加载流程
            if (Model == null && ActorNeedLoadModel)
            {
                //先尝试从池中获取
                if (!UObjectPool.Instance.TryGet(ModelPath, out var gameObject))
                {
                    //池中没有则走创建流程
                    if (GameObjectIsPreLoaded)
                    {
                        var instance = GameObjectPreLoadMgr.Instance[ModelPath];
                        gameObject = Object.Instantiate(instance);
                    }
                    else
                    {
                        try
                        {
                            gameObject = await Addressables.LoadAssetAsync<GameObject>(ModelPath)
                                .ToUniTask(cancellationToken: MainCancelToken.Token);
                        }
                        catch (OperationCanceledException e) { }
                        catch (Exception e)
                        {
                            Debug.LogError(e);
                        }
                    }
                }

                if (Model == null)
                {
                    Debug.LogError("Model缺少ActorModel脚本！");
                    ActorManager.Instance.RemoveActor(Uid);
                    return;
                }

                Model = gameObject.GetComponent<ActorModel>();
            }

            Model?.Setup(this);
            Self.ModelLoadFinishCallback?.Invoke(Self);
        }

        /// <summary>
        /// ActorModel实例化到场景中
        /// </summary>
        public void EnterScene()
        {
            Model?.OnEnterScene();
        }

        /// <summary>
        /// 离开场景
        /// </summary>
        public void ExitScene()
        {
            Model?.OnExitScene();
            MainCancelToken.Cancel();
        }

        public void Tick(float dt)
        {
            if (Model == null) return;

            Model.transform.localPosition = Self.Pos;
            Model.transform.localRotation = Self.Rot;
            Model.OnTick(dt);
        }

        public void Clear()
        {
            if (Model != null)
            {
                //用池回收
                Model.Recycle();
            }
        }
    }
}