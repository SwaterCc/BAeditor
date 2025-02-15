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
        public  string JsonKey;

        /// <summary>
        /// Actor基础类型
        /// </summary>
        public  EActorType ActorType;
        
        /// <summary>
        /// 头像Icon
        /// </summary>
        public  string RPGIcon;
        
        /// <summary>
        /// 加载UnitObjectProxy
        /// </summary>
        private readonly bool _isLoadUnityObjectProxy;
        
        /// <summary>
        /// 代理类型
        /// </summary>
        private readonly EUnityObjectProxyType _proxyType;
        
        /// <summary>
        /// 动态赋予的名字
        /// </summary>
        public string DynamicName { get; set; }
        
        public Actor(string jsonKey)
        {
            JsonKey = jsonKey;
            var info = ActorJsonAssemblerFactory.Instance.GetActorAssembleInfo(jsonKey);


            foreach (var factory in info.Factories)
            {
                addComponent(factory.CreateComponent());
            }
        }

        public new void Init()
        {
            base.Init();
        }

        #region 周期函数

        /// <summary>
        /// 逻辑帧
        /// </summary>
        /// <param name="dt"></param>
        protected override void onTick(float dt) { }

        public override void Recycle()
        {
            ActorPool.Instance.Recycle(this);
        }

        /// <summary>
        /// ActorPool回收时调用
        /// </summary>
        public void OnRecycle()
        {
            DynamicName = null;
        }

        #endregion
    }
}