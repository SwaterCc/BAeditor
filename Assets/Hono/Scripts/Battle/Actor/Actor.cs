using System;
using System.Collections.Generic;
using Hono.Scripts.Battle.Base;
using Hono.Scripts.Battle.Event;
using Hono.Scripts.Battle.Message;
using UnityEngine;

namespace Hono.Scripts.Battle
{
    /// <summary>
    /// Actor 战斗玩法中最基本的单位
    /// </summary>
    public sealed partial class Actor : IAPoolObject
    {
        /// <summary>
        /// 运行时唯一ID
        /// </summary>
        public int Uid { get; private set; }

        /// <summary>
        /// Actor基础类型
        /// </summary>
        public EActorType ActorType { get; private set; }
        
        /// <summary>
        /// Actor逻辑
        /// </summary>
        public ActorLogic Logic { get; private set; }
        
        /// <summary>
        /// Actor配置数据
        /// </summary>
        public int ConfigId { get; private set; }
        
        /// <summary>
        /// Actor配置数据
        /// </summary>
        public ActorTable.ActorRow ActorTableRow { get; private set; }
        
        /// <summary>
        /// Unity交互层
        /// </summary>
        public ModelController ModelController { get; }

        /// <summary>
        /// 变量黑板
        /// </summary>
        public VariableBoard VariableBoard { get; }

        /// <summary>
        /// Tag
        /// </summary>
        public TagCollection TagCollection { get; }

        /// <summary>
        /// Actor属性列表
        /// </summary>
        public AttrCollection Attrs { get; }
        
        /// <summary>
        /// ability控制器
        /// </summary>
        public AbilityCollection Abilities { get; }

        /// <summary>
        /// 动作系统
        /// </summary>
        public ActionSystem ActionSystem { get; }

        /// <summary>
        /// Actor初始化状态
        /// </summary>
        /// <returns></returns>
        public EActorInitState InitState;

        /// <summary>
        /// 当前坐标
        /// </summary>
        public Vector3 Pos;

        /// <summary>
        /// 目标坐标
        /// </summary>
        public Vector3 TargetPos;

        /// <summary>
        /// 当前旋转
        /// </summary>
        public Quaternion Rot;

        /// <summary>
        /// Actor事件容器
        /// </summary>
        private readonly ActorEventListenerCollection _evtListenerCollection;

        /// <summary>
        /// Actor消息容器
        /// </summary>
        private readonly MessageCollection _messageCollection;

        #region 回调周期

        /// <summary>
        /// 模型加载完成后调用
        /// </summary>
        public Action<Actor> ModelLoadFinishCallback { get; set; }

        /// <summary>
        /// 进入场景后回调
        /// </summary>
        public Action<Actor> EnterSceneCallback { get; set; }

        /// <summary>
        /// 帧更新前回调
        /// </summary>
        public Action<Actor, float> BeforeTickCallBack { get; set; }

        /// <summary>
        /// 帧更新后回调
        /// </summary>
        public Action<Actor, float> AfterTickCallBack { get; set; }

        /// <summary>
        /// 离开场景后回调
        /// </summary>
        public Action<Actor> ExitSceneCallBack { get; set; }

        #endregion

        public Actor()
        {
            Attrs = new AttrCollection(this);
            Abilities = new AbilityCollection(this);
            ActionSystem = new ActionSystem(this);
            VariableBoard = new VariableBoard();
            TagCollection = new TagCollection();
            ModelController = new ModelController(this);

            _evtListenerCollection = new ActorEventListenerCollection(this, 10);
            _messageCollection = new MessageCollection(this, 10);
        }

        #region 周期函数

        /// <summary>
        /// 初始化
        /// </summary>
        public void Init(int uid, int configId, ActorModel actorModel, List<AttrSnapshot> snapshots = null)
        {
            InitState = EActorInitState.Initializing;
            Uid = uid;
            ConfigId = configId;
            if (!ConfigManager.Table<ActorTable>().TryGet(ConfigId, out var row))
            {
                InitState = EActorInitState.InitConfigFailed;
                return;
            }
            ActorTableRow = row;
            ActorType = (EActorType)row.ActorType;
            Attrs.Init(snapshots);
            ModelController.Init(actorModel);
            Logic = ActorLogicPrototype.RentLogic(this);
            
            EventManager.Instance.AddListenerCollection(_evtListenerCollection);
            MessageManager.Instance.AddMsgCollection(_messageCollection);
        }

        /// <summary>
        /// 进入场景时调用
        /// </summary>
        public void EnterScene()
        {
            
            EnterSceneCallback?.Invoke(this);
        }

        /// <summary>
        /// 逻辑帧
        /// </summary>
        /// <param name="dt"></param>
        public void Tick(float dt)
        {
            BeforeTickCallBack?.Invoke(this, dt);
            Logic.Tick(dt);
            ModelController.Tick(dt);
            Abilities.Tick(dt);
            _evtListenerCollection.Tick(dt);
            _messageCollection.Tick(dt);
            AfterTickCallBack?.Invoke(this, dt);
        }

        /// <summary>
        /// 离开场景，此时会更改layer层级保证不会再被攻击打中，以及不会再被选为目标
        /// </summary>
        public void ExitScene()
        {
            ExitSceneCallBack?.Invoke(this);
            ModelController.ExitScene();
            EventManager.Instance.RemoveListenerCollection(_evtListenerCollection);
            MessageManager.Instance.RemoveMsgCollection(_messageCollection);
        }

        /// <summary>
        /// 删除前调用
        /// </summary>
        public void OnRecycle()
        {
            ModelLoadFinishCallback = null;
            EnterSceneCallback = null;
            BeforeTickCallBack = null;
            AfterTickCallBack = null;
            ExitSceneCallBack = null;

            TagCollection.Clear();
            Abilities.Clear();
            VariableBoard.Clear();
            ModelController.Clear();
            ActionSystem.Clear();
            Logic.Recycle();
            Logic = null;

            _evtListenerCollection.Clear();
            _messageCollection.Clear();
        }

        #endregion

        #region 对外接口

        public int GetAttr(EAttrType attrType)
        {
            var value = Attrs.GetAttr(attrType);
            return value;
        }

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
            EventManager.Instance.FireEvent(eventType, Uid, board);
        }

        /// <summary>
        /// 触发全局事件
        /// </summary>
        /// <param name="eventType"></param>
        /// <param name="board"></param>
        public void FireGlobalEvent(EEventType eventType, VariableBoard board = null)
        {
            EventManager.Instance.FireEvent(eventType, -1, board);
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