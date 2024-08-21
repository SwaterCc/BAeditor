using System.Collections.Generic;
using UnityEngine;

namespace Battle
{
    public interface ITick
    {
        public void Tick(float dt);
    }

    public interface IBeHurt
    {
        public void BeHurt(Dictionary<string, IValueBox> damage);
    }

    /// <summary>
    /// 游戏场景中对象的基类
    /// </summary>
    public class Actor : ITick, IBeHurt //TODO:这种靠接口的方法后续要处理掉，改成组件，目前是临时做法
    {
        /// <summary>
        /// 运行时唯一ID
        /// </summary>
        public int Uid;

        /// <summary>
        /// 配置ID
        /// </summary>
        public int ConfigId;

        public class ActorDebugHandle
        {
            private readonly Actor _actor;

            public Actor ActorHandle => _actor;
            
            public ActorDebugHandle(Actor actor)
            {
                _actor = actor;
            }
        }

        private readonly ActorDebugHandle _debugHandle;
        public ActorDebugHandle DebugHandle => _debugHandle;
        
        private class ActorInterfaceImp : IVariableCollectionBind
        {
            private readonly Actor _actor;

            public ActorInterfaceImp(Actor actor)
            {
                _actor = actor;
            }

            public VariableCollection GetVariableCollection()
            {
                return _actor._variables;
            }
        }

        private readonly ActorInterfaceImp _actorImp;

        protected bool _isDisposable;

        /// <summary>
        /// 属性
        /// </summary>
        protected AttrCollection _attrs;

        /// <summary>
        /// 变量
        /// </summary>
        protected VariableCollection _variables;

        /// <summary>
        /// 状态机
        /// </summary>
        protected StateMachine _stateMachine;

        /// <summary>
        /// Ability控制器
        /// </summary>
        protected AbilityController _abilityController;

        public Actor()
        {
            _isDisposable = false;
            _actorImp = new ActorInterfaceImp(this);
            _debugHandle = new ActorDebugHandle(this);
        }

        public virtual void Init()
        {
            _attrs = new AttrCollection();
            _abilityController = new AbilityController(this);
            _stateMachine = new StateMachine(this);
            _variables = new VariableCollection(8, _actorImp);
        }

        public virtual void Tick(float dt)
        {
            _stateMachine?.Tick(dt);
            _abilityController?.Tick(dt);
        }

        public bool IsDisposable()
        {
            return _isDisposable;
        }
        
        public void BeHurt(Dictionary<string, IValueBox> damage) { }
        
        public AttrCollection GetAttrCollection()
        {
            return _attrs;
        }

        public VariableCollection GetVariableCollection()
        {
            return _actorImp.GetVariableCollection();
        }

        public AbilityController GetAbilityController()
        {
            return _abilityController;
        }
    }
}