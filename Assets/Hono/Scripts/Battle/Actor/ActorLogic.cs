using Hono.Scripts.Battle.Tools;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Profiling;

namespace Hono.Scripts.Battle
{
    /// <summary>
    /// Actor的逻辑，包含逻辑层自身的逻辑和关联组件，状态机，最终决定出当前Actor逻辑层的属性,逻辑层必然存在
    /// ,逻辑层从设计理念上来讲是不知晓表现层的存在的，所以不要用逻辑层调用表现层
    /// </summary>
    public abstract partial class ActorLogic
    {
        /// <summary>
        /// Actor的UID
        /// </summary>
        public int Uid { get; private set; }

        /// <summary>
        /// Actor
        /// </summary>
        public Actor Actor { get; private set; }

        /// <summary>
        /// 输入来源
        /// </summary>
        protected ActorInput _actorInput;

        /// <summary>
        /// 状态机
        /// </summary>
        protected ActorStateMachine _stateMachine;

        /// <summary>
        /// 逻辑组件
        /// </summary>
        private readonly Dictionary<Type, ALogicComponent> _components = new();

        /// <summary>
        /// Actor的Ability控制器
        /// </summary>
        protected Actor.AbilityController AbilityController { get; private set; }

        /// <summary>
        /// Actor的属性
        /// </summary>
        protected AttrCollection Attrs { get; private set; }

        /// <summary>
        /// Actor的Tags
        /// </summary>
        protected Tags Tags { get; private set; }

        /// <summary>
        /// Actor的黑板数据
        /// </summary>
        protected VarCollection Variables { get; private set; }

        protected ActorLogic()
        {
            Constructor();
        }

        private void Constructor()
        {
            constructInput();
            constructComponents();
            constructStateMachine();
        }

        protected virtual void constructComponents() { }

        protected virtual void constructInput()
        {
            _actorInput = new NoInput(this);
        }

        protected virtual void constructStateMachine() { }

        public void Init(Actor actor)
        {
            Actor = actor;
            Uid = actor.Uid;
            
            OnInit();
        }

        protected virtual void OnInit() { }

        public void Setup(Actor.AbilityController controller, AttrCollection attrs, Tags tags,
            VarCollection varCollection)
        {
            Attrs = attrs;
            AbilityController = controller;
            Tags = tags;
            Variables = varCollection;

            setupAttrs();
        }

        protected virtual void setupAttrs() { }


        public void EnterScene()
        {
            foreach (var component in _components)
            {
                component.Value.Init();
            }

            _actorInput?.Init();
            _stateMachine?.Init();

            onEnterScene();
        }

        protected virtual void onEnterScene() { }

        protected T addComponent<T>(T component) where T : ALogicComponent
        {
            if (!_components.TryAdd(component.GetType(), component))
            {
                Debug.Log($"{this.GetType()}  添加组件 {component.GetType()} Failed!");
            }

            return component;
        }

        public T GetComponent<T>() where T : ALogicComponent
        {
            if (!_components.TryGetValue(typeof(T), out var component))
            {
                Debug.Log($"{this.GetType()} 获取组件 {typeof(T)} 失败!");
            }

            return (T)component;
        }

        public bool TryGetComponent<T>(out T comp) where T : ALogicComponent
        {
            comp = null;
            if (_components.TryGetValue(typeof(T), out var component))
            {
                comp = (T)component;
                return true;
            }

            return false;
        }

        protected virtual void onTick(float dt) { }

        public void Tick(float dt)
        {
            _actorInput.Tick(dt);
            foreach (var component in _components)
            {
                component.Value.Tick(dt);
            }

            _stateMachine?.Tick(dt);
            onTick(dt);
        }

        public void Destroy()
        {
            onDestroy();

            foreach (var component in _components)
            {
                component.Value.UnInit();
            }
            
            Attrs = null;
            AbilityController = null;
            Tags = null;
            Variables = null;

            RecycleSelf();
        }

        protected virtual void onDestroy() { }

        protected abstract void RecycleSelf();

        public EActorLogicStateType CurState()
        {
            return _stateMachine.CurStateType;
        }


        public T GetAttr<T>(ELogicAttr logicAttr)
        {
            return Actor.GetAttr<T>(logicAttr);
        }

        public object GetAttrBox(ELogicAttr logicAttr)
        {
            return Actor.GetAttrBox(logicAttr);
        }

        public ICommand SetAttr<T>(ELogicAttr logicAttr, T value, bool isTempData)
        {
            return Actor.SetAttr(logicAttr, value, isTempData);
        }

        public ICommand SetAttrBox(ELogicAttr logicAttr, object value, bool isTempData)
        {
            return Actor.SetAttrBox(logicAttr, value, isTempData);
        }
    }
}