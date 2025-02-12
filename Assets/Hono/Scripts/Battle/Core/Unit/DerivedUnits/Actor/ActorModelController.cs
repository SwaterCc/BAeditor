using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Hono.Scripts.Battle.Core
{
    //Actor需要一个基础模型，通常是一个Character,如果该单位为部位或者组件，从设计层面上不会主动移动，则会使用Collider为其赋予碰撞体积
    public class ActorModelController
    {
        public Actor Self { get; }

        /// <summary>
        /// 模型
        /// </summary>
        public ActorModel ActorModel { get; private set; }

        /// <summary>
        /// 玩家控制
        /// </summary>
        public CharacterController CharCtrl { get; private set; }

        /// <summary>
        /// PE播放器
        /// </summary>
        public PerformanceEffectsPlayer PEPlayer { get; private set; }

        /// <summary>
        /// 模型数据配置
        /// </summary>
        public ModelTable.ModelRow ModelRow { get; private set; }

        /// <summary>
        /// 加载取消总key
        /// </summary>
        public CancellationTokenSource MainCancelToken { get; } = new();

        /// <summary>
        /// 加载完成
        /// </summary>
        public bool LoadedFinish { get; private set; }
        
        /// <summary>
        /// 加载有错误
        /// </summary>
        public bool HasLoadError { get; private set; }

        /// <summary>
        /// model加载完成
        /// </summary>
        public event Action<Actor> ModelLoadedFinish;

        public ActorModelController(Actor actor)
        {
            Self = actor;
        }

        public async void Load()
        {
            var gameObject = await UPool.Instance.Get("ActorModel", MainCancelToken);

            if (gameObject == null)
            {
                HasLoadError = true;
                return;
            }

            ActorModel = gameObject.GetComponent<ActorModel>();
            CharCtrl = gameObject.GetComponent<CharacterController>();
            PEPlayer = gameObject.GetComponent<PerformanceEffectsPlayer>();

            ActorModel.OnLoadFinish(this);
            PEPlayer.LoadPE(this);

            //初始化模型体型

            CharCtrl.height = ModelRow.Height;
            CharCtrl.radius = ModelRow.Radius;

            CharCtrl.center = Vector3.up * (ModelRow.Height < ModelRow.Radius * 2 ? ModelRow.Radius : ModelRow.Height / 2);

            LoadedFinish = true;
            
            ModelLoadedFinish?.Invoke(Self);
        }

        public void Tick(float dt)
        {
            PEPlayer.OnTick(dt);
        }

        public void Clear()
        {
            MainCancelToken.Cancel();
            PEPlayer.Clear();
            UPool.Instance.Recycle("ActorModel", ActorModel.gameObject);
        }
    }
}