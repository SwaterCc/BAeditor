using System.Collections.Generic;
using UnityEngine;

namespace Battle
{
    public class ActorBehavior : MonoBehaviour
    {
        private Actor _actor;

        private void Awake()
        {
            _actor = new Actor();
            _actor.Init();
            BattleManager.Instance.Add(_actor);
        }

        private void OnDestroy() { }
    }

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
    public class Actor : ITick
    {
        /// <summary>
        /// 运行时唯一ID
        /// </summary>
        public int Uid;

        /// <summary>
        /// 配置ID
        /// </summary>
        public int ConfigId;

        private class ActorInterfaceImp : IVariableCollectionBind, IBeHurt
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

            public void BeHurt(Dictionary<string, IValueBox> damage) { }
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