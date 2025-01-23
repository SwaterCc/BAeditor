using System;
using Hono.Scripts.Battle.Base;

namespace Hono.Scripts.Battle.Core
{
    /// <summary>
    /// Actor 战斗玩法中有交互的单位
    /// </summary>
    public sealed class Actor : Unit, IAPoolObject
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
        public int ConfigId { get; private set; }

        /// <summary>
        /// Actor配置数据
        /// </summary>
        public ActorTable.ActorRow ActorTableRow { get; private set; }

        /// <summary>
        /// Unity交互层
        /// </summary>
        public PerformanceEffectController PEController { get; }

        /// <summary>
        /// Actor初始化状态
        /// </summary>
        /// <returns></returns>
        public EActorState State { get; private set; }

        /// <summary>
        /// 模型加载完成后调用
        /// </summary>
        public event Action<Actor> ModelLoadFinishCallback;

        public Actor()
        {
            PEController = new PerformanceEffectController(this);
        }

        #region 周期函数

        /// <summary>
        /// 初始化
        /// </summary>
        public void Init(int uid, int configId, PerformanceEffectsPlayer performanceEffectsPlayer, AttrCollection.AttrSnapshots snapshot = null)
        {
            base.Init(uid);

            ConfigId = configId;
            if (!ConfigManager.Table<ActorTable>().TryGet(ConfigId, out var row))
            {
                State = EActorState.Error;
                return;
            }

            ActorTableRow = row;
            ActorType = (EActorType)row.ActorType;
            Attrs.SetAttrsByTable(row.AttrTemplateId, true);

            if (snapshot != null)
            {
                Attrs.SetAttrsBySnapshot(snapshot, true);
            }

            PEController.Init(performanceEffectsPlayer);
        }

        /// <summary>
        /// 逻辑帧
        /// </summary>
        /// <param name="dt"></param>
        protected override void onTick(float dt)
        {
            PEController.Tick(dt);
        }

        /// <summary>
        /// 离开场景，此时会更改layer层级保证不会再被攻击打中，以及不会再被选为目标
        /// </summary>
        public override void ExitWorld()
        {
            if (TryGetComponent<DeadComp>(out var bornComp))
            {
                bornComp.OnDead();
            }

            base.ExitWorld();
        }

        /// <summary>
        /// 删除前调用
        /// </summary>
        public void OnRecycle()
        {
            ModelLoadFinishCallback = null;
            PEController.Clear();
            Clear();
        }

        #endregion
    }
}