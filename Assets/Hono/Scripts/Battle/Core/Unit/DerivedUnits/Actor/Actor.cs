using System;
using Hono.Scripts.Battle.Base;

namespace Hono.Scripts.Battle.Core
{
    /// <summary>
    /// Actor 战斗玩法中有交互的单位
    /// </summary>
    public sealed class Actor : Unit , Unit.ILoadableUnit
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
        /// 是否加载完成
        /// </summary>
        public bool IsLoadFinish => ModelController.LoadedFinish;

        public bool HasLoadError => ModelController.HasLoadError;

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
            
            if (TryGetComponent(out CombatComp combatComp))
            {
                //学习技能
                foreach (var pSkill in ActorTableRow.OwnerSkills)
                {
                    combatComp.LearnSkill(pSkill[0]);
                }
                
                //添加buff
                foreach (var buffInfo in ActorTableRow.OwnerBuffs)
                {
                    combatComp.AddBuff(buffInfo[0], Uid, buffInfo[1]);
                }
            }

            foreach (var abilityInfo in ActorTableRow.ownerOtherAbility)
            {
                var ability = AddAbility(abilityInfo[0]);
                if (abilityInfo[1] > 0)
                {
                    ability.Execute(false);
                }
            }
        }
        
        public void Load()
        {
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

        public override void Recycle()
        {
            ActorPool.Instance.Recycle(this);
        }

        /// <summary>
        /// ActorPool回收时调用
        /// </summary>
        public void OnRecycle()
        {
            ModelController.Clear();
        }
        
        #endregion
    }
}