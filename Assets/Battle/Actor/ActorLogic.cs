using System;
using System.Collections.Generic;
using UnityEngine;

namespace Battle
{
    //逻辑层
    public abstract class ActorLogic : ITick, IVariablesBind
    {
        protected int _configId;
        
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
        protected readonly Dictionary<Type, ALogicComponent> _components;
        
        public ActorLogic(int configId)
        {
            _configId = configId;
            _abilityController = new AbilityController();
            _variables = new Variables(16, this);
            _stateMachine = new StateMachine(this);
            _components = new Dictionary<Type, ALogicComponent>();
        }

        public void Init()
        {
            onInit();
            registerComponents();
            foreach (var component in _components)
            {
                component.Value.Init();
            }
        }

        protected abstract void onInit();

        private void registerComponents()
        {
            registerChildComp();
        }

        protected abstract void registerChildComp();


        protected void addComponent(ALogicComponent component)
        {
            if (!_components.TryAdd(component.GetType(), component))
            {
                Debug.Log($"{this.GetType()}  添加组件 {component.GetCompType()} Failed!");
            }
        }

        public T GetComponent<T>() where T : ALogicComponent
        {
            if (!_components.TryGetValue(typeof(T), out var component))
            {
                Debug.Log($"{this.GetType()} 获取组件 {typeof(T)} 失败!");
            }

            return (T)component;
        }

        protected virtual void onTick(float dt) { }

        public void Tick(float dt)
        {
            foreach (var component in _components)
            {
                component.Value.Tick(dt);
            }
            
            _stateMachine.Tick(dt);
            _abilityController.Tick(this, dt);
            
            onTick(dt);
        }

        public Variables GetVariables()
        {
            return _variables;
        }
        
        public Attribute GetAttr(EAttributeType attributeType)
        {
            return _attrs.GetAttr(attributeType);
        }
        
        public SimpleAttribute<T> GetSimpleAttr<T>(EAttributeType attributeType)
        {
            return _attrs.GetAttr(attributeType) as SimpleAttribute<T>;
        }
        
        public CompositeAttribute GetCompositeAttr(EAttributeType attributeType)
        {
            return _attrs.GetAttr(attributeType) as CompositeAttribute;
        }
    }
}