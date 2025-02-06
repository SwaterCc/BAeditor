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
        /// Actor基础类型
        /// </summary>
        public EActorType ActorType { get; private set; }

        /// <summary>
        /// Actor配置数据
        /// </summary>
        public ActorTable.ActorRow ActorTableRow { get; private set; }

        /// <summary>
        /// Unity交互层
        /// </summary>
        
        public ActorModelController ModelController { get; }

        /// <summary>
        /// Actor状态标签
        /// </summary>
        /// <returns></returns>
        public EActorState State { get; private set; }

        public Actor(string jsonKey)
        {
            JsonKey = jsonKey;
            
            var info = ActorJsonAssembler.GetActorAssembleInfo(jsonKey);
            foreach (var factory in info.Factories)
            {
                addComponent(factory.CreateComponent());
            }
            
            ModelController = new ActorModelController();
        }

        #region 周期函数

        /// <summary>
        /// 初始化
        /// </summary>
        public void Init(int uid, int configId, PerformanceEffectsPlayer performanceEffectsPlayer, AttrCollection.AttrSnapshots snapshot = null)
        {
            base.Init(uid);
            
            if (!ConfigManager.Table<ActorTable>().TryGet(configId, out var row))
            {
                return;
            }

            ActorTableRow = row;
            ActorType = (EActorType)row.ActorType;
            Attrs.SetAttrsByTable(row.AttrTemplateId, true);

            if (snapshot != null)
            {
                Attrs.SetAttrsBySnapshot(snapshot, true);
            }

            ModelController.Init(performanceEffectsPlayer);
        }

        /// <summary>
        /// 设置召唤者，设置之后会变成召唤物
        /// </summary>
        public void SetSummoner()
        {
            
        }
        
        /// <summary>
        /// 逻辑帧
        /// </summary>
        /// <param name="dt"></param>
        protected override void onTick(float dt)
        {
            ModelController.Tick(dt);
        }

        protected override void onRemove()
        {
            ModelController.Clear();
            Clear();
            //回收自己
        }
        #endregion
    }
}