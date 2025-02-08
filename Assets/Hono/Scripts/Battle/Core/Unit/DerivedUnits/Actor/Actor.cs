using System;
using Hono.Scripts.Battle.Base;

namespace Hono.Scripts.Battle.Core
{
    /// <summary>
    /// Actor 战斗玩法中有交互的单位
    /// </summary>
    public sealed class Actor : Unit
    {
        /// <summary>
        /// Json类型
        /// </summary>
        public string JsonKey { get; }

        /// <summary>
        /// 允许玩家操控
        /// </summary>
        public bool AllowPlayerControl { get; }

        /// <summary>
        /// Actor基础类型
        /// </summary>
        public EActorType ActorType { get; private set; }

        /// <summary>
        /// Actor配置数据
        /// </summary>
        public ActorTable.ActorRow ActorTableRow { get; private set; }

        /// <summary>
        /// Unity模型管理
        /// </summary>
        public ActorModelController ModelController { get; }

        public Actor(string jsonKey)
        {
            JsonKey = jsonKey;

            var info = ActorJsonAssembler.GetActorAssembleInfo(jsonKey);

            AllowPlayerControl = info.AllowControl;
            foreach (var factory in info.Factories)
            {
                addComponent(factory.CreateComponent());
            }

            ModelController = new ActorModelController(this);
        }

        public void Init(ActorTable.ActorRow actorRow)
        {
            base.Init();
            ActorTableRow = actorRow;
            ActorType = (EActorType)ActorTableRow.ActorType;
            ModelController.Load();
        }

        #region 周期函数

        /// <summary>
        /// 逻辑帧
        /// </summary>
        /// <param name="dt"></param>
        protected override void onTick(float dt)
        {
            ModelController.Tick(dt);
        }

        /// <summary>
        /// ActorPool回收时调用
        /// </summary>
        public void OnRecycle()
        {
            ModelController.Clear();
            Clear();
        }

        protected override void OnRemove()
        {
            ActorPool.Instance.Recycle(this);
        }

        #endregion
    }
}