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
        public ActorModel Model { get; private set; }

        /// <summary>
        /// 模型配置
        /// </summary>
        public ModelTable.ModelRow ModelRow { get; private set; }

        public ModelController(Actor actor)
        {
            Self = actor;
        }

        public async void Init(ActorModel model)
        {
            if (!ConfigManager.Table<ModelTable>().TryGet(Self.ActorTableRow.ModelId, out var modelRow))
            {
                modelRow = ConfigManager.Table<ModelTable>().Get(1);
            }

            ModelRow = modelRow;
            
            if (model != null)
            {
                Model = model;
                return;
            }

            var gameObject = await UPool.Instance.Get(ModelRow.ModelPath, MainCancelToken);

            if (gameObject == null)
            {
                Self.InitState = EActorInitState.ModelLoadFailed;
                return;
            }
            
            Model = gameObject.GetComponent<ActorModel>();

            Model?.OnInit(this);
            Self.ModelLoadFinishCallback?.Invoke(Self);
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