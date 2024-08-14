using System;
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
        }

        private void Update()
        {
            _actor.Tick(Time.deltaTime);
        }

        private void OnDestroy() { }
    }

    public interface ITick
    {
        public void Tick(float dt);
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
        
        /// <summary>
        /// Ability控制器
        /// </summary>
        public readonly AbilityController AbilityController;

        /// <summary>
        /// 状态机
        /// </summary>
        private readonly StateMachine _stateMachine;

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

        /// <summary>
        /// 变量
        /// </summary>
        private readonly VariableCollection _variables;

        private readonly AttrCollection _attrs;

        public Actor()
        {
            _actorImp = new ActorInterfaceImp(this);
            AbilityController = new AbilityController(this);
            _stateMachine = new StateMachine(this);
            _variables = new VariableCollection(8, _actorImp);
            _attrs = new AttrCollection();
        }

        public void Init()
        {
            //读取配置加载能力
        }
        
        public void Tick(float dt)
        {
            _stateMachine.Tick(dt);
            AbilityController.Tick(dt);
        }

        public VariableCollection GetVariableCollection()
        {
            return _actorImp.GetVariableCollection();
        }

        public AttrCollection GetAttrCollection()
        {
            return _attrs;
        }
    }
}