using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using Hono.Scripts.Battle.Core.Base;
using UnityEngine;

namespace Hono.Scripts.Battle.Core
{
    //Actor需要一个基础模型，通常是一个Character,如果该单位为部位或者组件，从设计层面上不会主动移动，则会使用Collider为其赋予碰撞体积
    [JsonUnitComponent]
    public partial class ModelControllerComp : UnitComponent, UnitComponent.IAsyncLoadTask, IMovable
    {
        [JsonUnitComponentParam]
        public string UnitModelType;
        /// <summary>
        /// 模型
        /// </summary>
        public UnitModel UnitModel { get; private set; }

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
        /// model加载完成
        /// </summary>
        public event Action<Unit> ModelLoadedFinish;

        public override void Init() { }

        public async UniTask LoadTask(CancellationTokenSource tokenSource)
        {
            var gameObject = await UPool.Instance.Get("ActorModel", tokenSource);

            if (gameObject == null)
            {
                throw new Exception("[ModelControllerComp] 模型加载失败");
            }

            UnitModel = gameObject.GetComponent<UnitModel>();
            CharCtrl = gameObject.GetComponent<CharacterController>();
            PEPlayer = gameObject.GetComponent<PerformanceEffectsPlayer>();

            UnitModel.OnLoadFinish(this);
            PEPlayer.LoadPE(this);

            //初始化模型体型

            CharCtrl.height = ModelRow.Height;
            CharCtrl.radius = ModelRow.Radius;

            CharCtrl.center =
                Vector3.up * (ModelRow.Height < ModelRow.Radius * 2 ? ModelRow.Radius : ModelRow.Height / 2);

            ModelLoadedFinish?.Invoke(Unit);
        }

        protected override void onTick(float dt)
        {
            PEPlayer.OnTick(dt);
        }

        protected override void onClear()
        {
            PEPlayer.Clear();
            UPool.Instance.Recycle("ActorModel", UnitModel.gameObject);
        }

        public void Move(Vector3 velocity)
        {
            CharCtrl.SimpleMove(velocity);
        }
    }
}