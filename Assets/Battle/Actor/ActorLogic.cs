using System.Collections.Generic;
using UnityEngine;

namespace Battle
{
    //逻辑层
    public abstract class ActorLogic : ITick, IVariablesBind
    {
        /// <summary>
        /// ActorState
        /// </summary>
        protected ActorRTState _rtState;

        //逻辑层包含数据,逻辑流程
        /// <summary>
        /// 属性
        /// </summary>
        protected AttrCollection _attrs;

        /// <summary>
        /// 变量
        /// </summary>
        protected Variables _variables;

        /// <summary>
        /// 状态机
        /// </summary>
        protected StateMachine _stateMachine;

        /// <summary>
        /// Ability控制器
        /// </summary>
        protected AbilityController _abilityController;

        public AbilityController AbilityController => _abilityController;

        /// <summary>
        /// 逻辑组件
        /// </summary>
        protected readonly Dictionary<ELogicComponentType, ALogicComponent> _components;

        public ActorLogic(ActorRTState rtState)
        {
            _rtState = rtState;
            _abilityController = new AbilityController();
            _variables = new Variables(16, this);
            _stateMachine = new StateMachine(this);
            _components = new Dictionary<ELogicComponentType, ALogicComponent>();
        }

        public void Init()
        {
            registerComponents();
            foreach (var component in _components)
            {
                component.Value.Init();
            }
        }

        private void registerComponents()
        {
            registerChildComp();
        }

        protected virtual void registerChildComp() { }


        protected void addComponent(ALogicComponent component)
        {
            if (!_components.TryAdd(component.GetCompType(), component))
            {
                Debug.Log($"{this.GetType()}  添加组件 {component.GetCompType()} Failed!");
            }
        }

        public T GetComponent<T>(ELogicComponentType componentType) where T : ALogicComponent
        {
            if (!_components.TryGetValue(componentType, out var component))
            {
                Debug.Log($"{this.GetType()} 获取组件 {componentType} 失败!");
            }

            return (T)component;
        }

        public void Tick(float dt)
        {
            foreach (var component in _components)
            {
                component.Value.Tick(dt);
            }

            _stateMachine.Tick(dt);
            _abilityController.Tick(this, dt);
        }

        public Variables GetVariables()
        {
            return _variables;
        }
    }
}