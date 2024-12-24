using System;
using Hono.Scripts.Battle.Base;
using Hono.Scripts.Battle.Message;
using UnityEngine;

namespace Hono.Scripts.Battle
{
    /// <summary>
    /// Actor 战斗玩法中最基本的单位
    /// </summary>
    public sealed class Actor : IAPoolObject
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
        /// Unity交互层
        /// </summary>
        public ModelController ModelController { get; }

        /// <summary>
        /// 变量黑板
        /// </summary>
        public VariableBoard Variables { get; }

        /// <summary>
        /// Tag
        /// </summary>
        public TagCollection TagCollection { get; }
        
        /// <summary>
        /// Actor属性列表
        /// </summary>
        public AttrCollection Attrs { get; }

        /// <summary>
        /// Actor逻辑
        /// </summary>
        public ActorLogic Logic { get; private set; }

        /// <summary>
        /// ability控制器
        /// </summary>
        public AbilityController Abilities { get; }

        /// <summary>
        /// 玩家位置信息
        /// </summary>
        private readonly ActorLocation _location;
        
        /// <summary>
        /// 消息容器
        /// </summary>
        private readonly MessageCollection _message;

        /// <summary>
        /// 配置id
        /// </summary>
        public int ConfigId => GetAttr(EAttrType.AttrConfigId);

        /// <summary>
        /// 当前坐标
        /// </summary>
        public Vector3 Pos { get; set; }

        /// <summary>
        /// 目标坐标
        /// </summary>
        public Vector3 TargetPos { get; set; }

        /// <summary>
        /// 当前旋转
        /// </summary>
        public Quaternion Rot { get; set; }

        /// <summary>
        /// 是否为玩家操控单位
        /// </summary>
        public bool IsPlayerControl { get; set; }

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
            _message = new MessageCollection(this);
            Attrs = new AttrCollection(this);
            TagCollection = new TagCollection();
            Abilities = new AbilityController(this);
            Variables = new VariableBoard(128);
            ModelController = new ModelController(this);
        }

        #region 周期函数

        /// <summary>
        /// 初始化
        /// </summary>
        /// <param name="uid"></param>
        /// <param name="actorType"></param>
        public void Init(int uid, EActorType actorType)
        {
            Uid = uid;
            SetAttr(EAttrType.AttrUid, uid, false);
            ActorType = actorType;
            _message.Init();
        }

        /// <summary>
        /// 组合
        /// </summary>
        /// <param name="logic"></param>
        public void Setup(in ActorLogic logic)
        {
            Logic = logic;
            Logic.OnSetup(this);
            ModelController.Setup();
        }

        /// <summary>
        /// 进入场景时调用
        /// </summary>
        public void EnterScene()
        {
            Logic.EnterScene();
            ModelController.EnterScene();
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
            AfterTickCallBack?.Invoke(this, dt);
        }

        /// <summary>
        /// 离开场景，此时会更改layer层级保证不会再被攻击打中，以及不会再被选为目标
        /// </summary>
        public void ExitScene()
        {
            ModelController.ExitScene();
            ExitSceneCallBack?.Invoke(this);
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

            _message.Clear();
            TagCollection.Clear();
            Abilities.Clear();
            Variables.Clear();
            ModelController.Clear();
            Logic.RecycleLogicObject();
            Logic = null;
        }

        #endregion

        #region 对外接口

        public void AddMsgListener(MessageListener listener)
        {
            _message.AddListener(listener);
        }

        public void RemoveMsgListener(MessageListener listener)
        {
            _message.RemoveListener(listener);
        }

        public int GetAttr(EAttrType attrType)
        {
            var value = Attrs.GetAttr(attrType);
            return value;
        }

        public Attr GetAttrNoParse(EAttrType attrType)
        {
            var value = Attrs.GetAttr(attrType);
            return value;
        }

        public void SetAttr(EAttrType attrType, int value, bool isCommand = false)
        {
            Attrs.SetAttr(attrType, value, isCommand);
        }

        #endregion
    }
}