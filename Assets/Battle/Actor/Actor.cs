using System.Collections.Generic;
using Battle.Auto;
using UnityEngine;

namespace Battle
{
    public interface ITick
    {
        public void Tick(float dt);
    }

    public interface IBeHurt
    {
        public void BeHurt(Dictionary<string, object> damage);
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
            
            //测试代码，初始化一些属性
            var hp = new SimpleAttribute<float>();
            hp.Set(100f, true);
            _attrs.AddAttr(EAttributeType.Hp,hp);
            
            var mp = new SimpleAttribute<float>();
            mp.Set(100f, true);
            _attrs.AddAttr(EAttributeType.Mp,mp);
            
            var attack = new SimpleAttribute<float>();
            attack.Set(999f, true);
            _attrs.AddAttr(EAttributeType.Attack,attack);
            
            var pos = new SimpleAttribute<Vector3>();
            pos.Set(Vector3.zero, true);
            _attrs.AddAttr(EAttributeType.Position,pos);
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

        public virtual void OnDestroy()
        {
            
        }

        public void BeHurt(Dictionary<string, object> damage)
        {
            Debug.Log("+++++++++++++++++++++++++打中啦+++++++++++++++++++++++++");
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