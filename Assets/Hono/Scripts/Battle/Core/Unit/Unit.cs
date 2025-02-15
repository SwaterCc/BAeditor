using System;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using Hono.Scripts.Battle.AbilityFramework;
using Hono.Scripts.Battle.Base;
using Hono.Scripts.Battle.Event;
using Hono.Scripts.Battle.Message;
using Hono.Scripts.Battle.ObjectPool;
using UnityEngine;

namespace Hono.Scripts.Battle.Core
{
    /// <summary>
    /// World中运行的基本单位
    /// </summary>
    public abstract class Unit
    {
        /// <summary>
        /// 运行时唯一ID
        /// </summary>
        public int Uid { get; private set; }
        
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
        public EUnitFlag State { get; protected set; }

        /// <summary>
        /// 加载取消总key
        /// </summary>
        public CancellationTokenSource MainCancelToken { get; }

        /// <summary>
        /// ability驱动器
        /// </summary>
        private readonly AbilityDriver _abilityDriver;

        /// <summary>
        /// Unit事件容器
        /// </summary>
        private readonly EventListenerCollection _evtListenerCollection;

        /// <summary>
        /// Unit消息容器
        /// </summary>
        private readonly MessageCollection _messageCollection;

        ///Unit组件内部通信结构
        /// <summary>
        /// 逻辑组件
        /// </summary>
        private readonly Dictionary<Type, UnitComponent> _components = new(6);

        /// <summary>
        /// 加载完成后回调
        /// </summary>
        public event Action<Unit> LoadFinishCallBack;

        /// <summary>
        /// 运行第一帧前回调
        /// </summary>
        public event Action<Unit> BeforeFirstTickCallback;

        /// <summary>
        /// 帧更新前回调
        /// </summary>
        public event Action<Unit, float> BeforeTickCallBack;

        /// <summary>
        /// 帧更新后回调
        /// </summary>
        public event Action<Unit, float> AfterTickCallBack;

        /// <summary>
        /// 删除前回调
        /// </summary>
        public event Action<Unit> RecycleCallBack;

        /// <summary>
        /// 是否第一次Tick
        /// </summary>
        private bool _firstTick;

        /// <summary>
        /// 是否加载完成
        /// </summary>
        public bool IsLoadFinish { get; private set; }

        /// <summary>
        /// 是否出现加载错误
        /// </summary>
        public bool HasLoadError { get; private set; }

        /// <summary>
        /// 加载任务列表
        /// </summary>
        private readonly List<UniTask> _loadTasks = new(5);

        protected Unit()
        {
            UnitTransform = new UnitTransform();
            Attrs = new AttrCollection(this);
            
            Tags = new TagCollection();
            MainCancelToken = new CancellationTokenSource();

            _abilityDriver = new AbilityDriver(this);
            _evtListenerCollection = new EventListenerCollection(10);
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

        protected void addLoadTask(UniTask task)
        {
            _loadTasks.Add(task);
        }

        protected void Init()
        {
            Uid = World.Current.GetUid();

            EventManager.Instance.AddListenerCollection(Uid, _evtListenerCollection);
            MessageManager.Instance.AddMsgCollection(_messageCollection);

            foreach (var component in _components)
            {
                component.Value.Unit = this;
                component.Value.Init();
            }
        }

        public async void Load()
        {
            try
            {
                //有加载任务则
                if (_loadTasks.Count == 0)
                {
                    IsLoadFinish = true;
                    return;
                }

                await UniTask.WhenAll(_loadTasks);
                _loadTasks.Clear();
                IsLoadFinish = true;
                LoadFinishCallBack?.Invoke(this);
            }
            catch (Exception e)
            {
                HasLoadError = true;
                Debug.LogError(e);
            }
        }

        /// <summary>
        /// 第一次Tick之前被调用
        /// </summary>
        protected virtual void onBeforeFirstTick() { }

        public void Tick(float dt)
        {
            if (_firstTick)
            {
                foreach (var component in _components)
                {
                    component.Value.BeforeTick();
                }

                onBeforeFirstTick();
                BeforeFirstTickCallback?.Invoke(this);
                _firstTick = false;
            }

            BeforeTickCallBack?.Invoke(this, dt);
            foreach (var component in _components)
            {
                component.Value.Tick(dt);
            }

            _abilityDriver.Tick(dt);
            _evtListenerCollection.Tick(dt);
            _messageCollection.Tick(dt);
            onTick(dt);
            AfterTickCallBack?.Invoke(this, dt);
        }

        protected abstract void onTick(float dt);

        public void BaseClear()
        {
            RecycleCallBack?.Invoke(this);
            MainCancelToken.Cancel();
            
            foreach (var component in _components.Values)
            {
                component.Recycle();
            }
            _components.Clear();
            
            _abilityDriver.Clear();
            _evtListenerCollection.Clear();
            _messageCollection.Clear();

            Attrs.Clear();
            Tags.Clear();

            LoadFinishCallBack = null;
            BeforeFirstTickCallback = null;
            BeforeTickCallBack = null;
            AfterTickCallBack = null;
            RecycleCallBack = null;

            EventManager.Instance.RemoveListenerCollection(Uid, _evtListenerCollection);
            MessageManager.Instance.RemoveMsgCollection(_messageCollection);
        }

        /// <summary>
        /// 子类实现回收函数
        /// </summary>
        public abstract void Recycle();

        #region 对外接口

        /// <summary>
        /// 添加Ability
        /// </summary>
        /// <param name="abilityId"></param>
        public Ability AddAbility(int abilityId)
        {
            return _abilityDriver.AwardAbility(abilityId);
        }

        /*/// <summary>
        /// 执行Ability
        /// </summary>
        /// <param name="abilityData"></param>
        public void AddAbility(AbilityData abilityData)
        {
            return _abilityDriver.AwardAbility(abilityId);
        }*/

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
        /// 获取组件列表
        /// </summary>
        /// <returns></returns>
        public Dictionary<Type, UnitComponent> GetComponents()
        {
            return _components;
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
        public void RegisterEvtListener(UnitEventListener eventListener)
        {
            _evtListenerCollection.AddListener(eventListener);
        }

        /// <summary>
        /// 注册事件
        /// </summary>
        /// <param name="eventListener"></param>
        public void UnregisterEvtListener(UnitEventListener eventListener)
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