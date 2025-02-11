using System;
using System.Collections.Generic;
using Hono.Scripts.Battle.AbilityFramework;
using Hono.Scripts.Battle.Base;
using Hono.Scripts.Battle.Event;
using Hono.Scripts.Battle.Message;
using UnityEngine;

namespace Hono.Scripts.Battle.Core
{
    public abstract class Unit : WorldNode
    {
        /// <summary>
        /// 变量黑板
        /// </summary>
        public VariableBoard VariableBoard { get; }

        /// <summary>
        /// Tags
        /// </summary>
        public TagCollection Tags { get; }

        /// <summary>
        /// Actor属性列表
        /// </summary>
        public AttrCollection Attrs { get; }

        /// <summary>
        /// 当前位置信息
        /// </summary>
        public UnitTransform UnitTransform { get; }

        /// <summary>
        /// 状态标签
        /// </summary>
        /// <returns></returns>
        public EUnitFlag State { get; private set; }

        /// <summary>
        /// 动作系统
        /// </summary>
        private readonly ActionSystem _actionSystem;

        /// <summary>
        /// ability控制器
        /// </summary>
        private readonly AbilityDriver _abilityDriver;

        /// <summary>
        /// Actor事件容器
        /// </summary>
        private readonly UnitEventListenerCollection _evtListenerCollection;

        /// <summary>
        /// Actor消息容器
        /// </summary>
        private readonly MessageCollection _messageCollection;

        /// <summary>
        /// 逻辑组件
        /// </summary>
        private readonly Dictionary<Type, UnitComponent> _components = new();

        protected Unit()
        {
            UnitTransform = new UnitTransform();
            Attrs = new AttrCollection(this);
            VariableBoard = new VariableBoard();
            Tags = new TagCollection();

            _actionSystem = new ActionSystem(this);
            _abilityDriver = new AbilityDriver(this);
            _evtListenerCollection = new UnitEventListenerCollection(this, 10);
            _messageCollection = new MessageCollection(this, 10);
        }

        public override string ToString()
        {
            return Uid.ToString();
        }

        /// <summary>
        /// 添加组件
        /// </summary>
        /// <param name="component"></param>
        protected T addComponent<T>(T component) where T : UnitComponent
        {
            if (!_components.TryAdd(component.GetType(), component))
            {
                Debug.Log($"{GetType()} 添加组件 {component.GetType()} Failed!");
            }

            return component;
        }

        protected void Init()
        {
            Uid = World.GetUid();
            
            EventManager.Instance.AddListenerCollection(_evtListenerCollection);
            MessageManager.Instance.AddMsgCollection(_messageCollection);

            foreach (var component in _components)
            {
                component.Value.Unit = this;
                component.Value.Init();
            }
        }

        protected override void onTick(float dt)
        {
            foreach (var component in _components)
            {
                component.Value.Tick(dt);
            }

            _actionSystem.Tick(dt);
            _abilityDriver.Tick(dt);
            _evtListenerCollection.Tick(dt);
            _messageCollection.Tick(dt);
        }

        public void RemoveSelfFromParent()
        {
            World.Searcher.RemoveUnitLookup(this);
            base.RemoveSelfFromParent();
        }
        
        public void Clear()
        {
            foreach (var component in _components)
            {
                component.Value.Clear();
            }

            _components.Clear();
            _actionSystem.Clear();
            _abilityDriver.Clear();
            _evtListenerCollection.Clear();
            _messageCollection.Clear();
            
            Attrs.Clear();
            Tags.Clear();
            VariableBoard.Clear();
            
            EventManager.Instance.RemoveListenerCollection(_evtListenerCollection);
            MessageManager.Instance.RemoveMsgCollection(_messageCollection);
        }

        #region 对外接口

        /// <summary>
        /// 添加Ability
        /// </summary>
        /// <param name="abilityId"></param>
        public Ability AddAbility(int abilityId)
        {
            return _abilityDriver.AwardAbility(abilityId);
        }
        
        /// <summary>
        /// 执行Ability
        /// </summary>
        /// <param name="abilityId"></param>
        public void ExecuteAbility(int abilityId)
        {
            _abilityDriver.ExecuteAbility(abilityId);
        }
        
        /// <summary>
        /// 停止Ability
        /// </summary>
        /// <param name="abilityId"></param>
        public void StopAbility(int abilityId)
        {
            _abilityDriver.StopAbility(abilityId);
        }

        /// <summary>
        /// 删除Ability
        /// </summary>
        /// <param name="abilityId"></param>
        public void RemoveAbility(int abilityId)
        {
            _abilityDriver.RemoveAbility(abilityId);
        }

        /// <summary>
        /// 获取组件
        /// </summary>
        /// <typeparam name="T">可以通过基类类型获取具体的子类</typeparam>
        /// <returns>会返回空</returns>
        public T GetComponent<T>() where T : UnitComponent
        {
            if (_components.TryGetValue(typeof(T), out var component))
                return (T)component;

            foreach (var comp in _components.Values)
            {
                if (comp is T unitComponent)
                {
                    return unitComponent;
                }
            }

            Debug.Log($"{this.GetType()} 获取组件 {typeof(T)} 失败!");

            return null;
        }

        /// <summary>
        /// 尝试获取组件
        /// </summary>
        /// <param name="comp"></param>
        /// <typeparam name="T">可以通过基类类型获取具体的子类</typeparam>
        /// <returns></returns>
        public bool TryGetComponent<T>(out T comp) where T : UnitComponent
        {
            comp = null;
            if (_components.TryGetValue(typeof(T), out var result))
            {
                comp = (T)result;
                return true;
            }

            foreach (var unitComp in _components.Values)
            {
                if (unitComp is T tComp)
                {
                    comp = tComp;
                    return true;
                }
            }

            Debug.Log($"{GetType()} 获取组件 {typeof(T)} 失败!");
            return false;
        }

        /// <summary>
        /// 获取属性
        /// </summary>
        /// <param name="attrType"></param>
        /// <returns></returns>
        public int GetAttr(EAttrType attrType)
        {
            var value = Attrs.GetAttr(attrType);
            return value;
        }

        /// <summary>
        /// 设置属性
        /// </summary>
        /// <param name="attrType"></param>
        /// <param name="value"></param>
        /// <param name="forceDirty"></param>
        public void SetAttr(EAttrType attrType, int value, bool forceDirty = false)
        {
            Attrs.SetAttr(attrType, value, forceDirty);
        }

        /// <summary>
        /// 注册事件
        /// </summary>
        /// <param name="eventListener"></param>
        public void RegisterEvtListener(ActorEventListener eventListener)
        {
            _evtListenerCollection.AddListener(eventListener);
        }

        /// <summary>
        /// 注册事件
        /// </summary>
        /// <param name="eventListener"></param>
        public void UnregisterEvtListener(ActorEventListener eventListener)
        {
            _evtListenerCollection.RemoveListener(eventListener);
        }

        /// <summary>
        /// 触发仅限定于本Actor内部的事件监听
        /// </summary>
        /// <param name="eventType"></param>
        /// <param name="board"></param>
        public void FireEvent(EEventType eventType, VariableBoard board = null)
        {
            _evtListenerCollection.FireEvent(eventType, board);
        }

        /// <summary>
        /// 触发全局事件
        /// </summary>
        /// <param name="eventType"></param>
        /// <param name="board"></param>
        public void FireWorldEvent(EEventType eventType, VariableBoard board = null)
        {
            EventManager.Instance.FireWorldEvent(eventType, Uid, board);
        }

        /// <summary>
        /// 注册消息监听
        /// </summary>
        /// <param name="messageListener"></param>
        public void RegisterMsgListener(MessageListener messageListener)
        {
            _messageCollection.AddListener(messageListener);
        }

        /// <summary>
        /// 注册消息监听
        /// </summary>
        /// <param name="messageListener"></param>
        public void UnregisterMsgListener(MessageListener messageListener)
        {
            _messageCollection.RemoveListener(messageListener);
        }

        #endregion
    }
}