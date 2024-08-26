using System.Collections.Generic;
using Battle.Auto;
using Battle.Event;
using Unity.VisualScripting;
using UnityEngine;

namespace Battle
{
    public interface ITick
    {
        public void Tick(float dt);
    }

    /// <summary>
    /// 游戏场景中对象的基类
    /// </summary>
    public class Actor
    {
        /// <summary>
        /// 运行时唯一ID
        /// </summary>
        public int Uid;

        /// <summary>
        /// 配置ID
        /// </summary>
        protected int _configId;

        /// <summary>
        /// actor类型
        /// </summary>
        public EActorType ActorType;

        /// <summary>
        /// 是否无效
        /// </summary>
        protected bool _isDisposable;

        public bool IsDisposable => _isDisposable;

        /// <summary>
        /// 表现层
        /// </summary>
        private ActorShow _show;

        /// <summary>
        /// 逻辑层
        /// </summary>
        private ActorLogic _logic;

        /// <summary>
        /// 当前运行状态
        /// </summary>
        private ActorRTState _rtState;

        /// <summary>
        /// 运行状态
        /// </summary>
        public ActorRTState RTState => _rtState;

        public Actor(ActorShow show, ActorLogic logic)
        {
            _isDisposable = false;
            _show = show;
            _logic = logic;
            _rtState = new ActorRTState(show, logic);
        }

        //生命周期
        public void Init()
        {
            _show.Init();
            _logic.Init();
        }

        public void SetLogicAttr() { }

        public void GetLogicAttr() { }
        
        public void GetShowAttr() { }

        public void AwardAbility(int configId, bool isRunNow)
        {
            _logic?.AbilityController.AwardAbility(configId, isRunNow);
        }

        public void EnterScene()
        {
            _rtState.OnEnterScene();
        }

        public void Update(float dt)
        {
            _rtState.Update(dt);
        }

        public void Tick(float dt)
        {
            _rtState.Tick(dt);
        }

        public void ExitScene()
        {
            _rtState.OnExitScene();
        }

        public virtual void Destroy() { }
    }
}