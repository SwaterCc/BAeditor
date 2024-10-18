using System;
using Hono.Scripts.Battle.Event;
using Hono.Scripts.Battle.Tools;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Profiling;

namespace Hono.Scripts.Battle
{
    /// <summary>
    /// Actor 实际的行为由Logic+Show构成，用State去管理Logic和Show Actor提供对外的接口
    /// </summary>
    public sealed partial class Actor : IVarCollectionBind
    {
        /// <summary>
        /// 运行时唯一ID
        /// </summary>
        public int Uid { get; }
        
        /// <summary>
        /// 变量黑板
        /// </summary>
        public VarCollection Variables { get; }

        /// <summary>
        /// Actor基础类型
        /// </summary>
        public EActorType ActorType { get; }
        
        /// <summary>
        /// Actor逻辑
        /// </summary>
        public ActorLogic Logic { get; private set; }

        /// <summary>
        /// Unity交互层
        /// </summary>
        private ActorModelController _modelController;
        public ActorModelController ModelController => _modelController;
        
        /// <summary>
        /// ability控制器
        /// </summary>
        private readonly AbilityController _abilityController;

        /// <summary>
        /// Actor属性 (show真的需要属性吗，暂时定为不要)
        /// </summary>
        private readonly AttrCollection _attrs;

        /// <summary>
        /// 消息容器
        /// </summary>
        private readonly MessageCollection _message;

        /// <summary>
        /// tag
        /// </summary>
        private readonly Tags _tags;

        /// <summary>
        /// 配置id
        /// </summary>
        public int ConfigId => GetAttr<int>(ELogicAttr.AttrConfigId);
        
        /// <summary>
        /// 当前坐标
        /// </summary>
        public Vector3 Pos => GetAttr<Vector3>(ELogicAttr.AttrPosition);

        /// <summary>
        /// 当前旋转
        /// </summary>
        public Quaternion Rot => GetAttr<Quaternion>(ELogicAttr.AttrRot);

        /// <summary>
        /// actor是否加载完成
        /// </summary>
        public bool ActorSetupFinish => _modelController.IsModelLoadFinish;

        /// <summary>
        /// 是否为玩家操控单位
        /// </summary>
        public bool IsPlayerControl { get; set; }

        /// <summary>
        /// 是否为过期单位
        /// </summary>
        public bool IsExpired { get; set; }

        public Action<Actor> OnModelLoadFinish { get; set; }
        public Action<Actor> OnInitCallBack { get; set; }
        public Action<Actor> OnTickCallBack { get; set; }
        public Action<Actor> OnDestroyCallBack { get; set; }

        public Actor(int uid, EActorType actorType)
        {
            Uid = uid;
            ActorType = actorType;
            OnDestroyCallBack = null;
            _tags = new Tags();
            _attrs = new AttrCollection(this, AttrCreator.Create);
            _abilityController = new AbilityController(this);
            _message = new MessageCollection(this);
            Variables = new VarCollection(this, 128);
        }

        #region 生命周期

        /// <summary>
        /// Create时调用，同帧执行
        /// </summary>
        /// <param name="modelController"></param>
        /// <param name="logic"></param>
        public void Setup(ActorModelController modelController, ActorLogic logic)
        {
            Logic = logic;
            Logic.Setup(_abilityController, _attrs, _tags, Variables);
            _modelController = modelController;
            _modelController.Setup(_tags, Variables, Logic);
        }
        
        /// <summary>
        /// 加入Rt列表后执行，一般是创建后的第一帧先执行了Init 之后会同帧调用 Tick Update
        /// </summary>
        public void Init()
        {
            Logic.Init();
            _modelController.OnEnterScene();
            _message.Init();
            OnInitCallBack?.Invoke(this);
        }

        /// <summary>
        /// 逻辑帧
        /// </summary>
        /// <param name="dt"></param>
        public void Tick(float dt)
        {
	        if (ActorType == EActorType.Pawn) {
		        Profiler.BeginSample("PawnActorTick");
	        }
	        else if(ActorType == EActorType.Monster)
	        {
		        Profiler.BeginSample("MonsterActorTick");
	        }
	        else {
		        Profiler.BeginSample("OtherActorTick");
	        }
	        if(IsExpired) return;
	        Logic.Tick(dt);
            _abilityController.Tick(dt);
            OnTickCallBack?.Invoke(this);
            Profiler.EndSample();
        }

        /// <summary>
        /// 表现帧
        /// </summary>
        /// <param name="dt"></param>
        public void Update(float dt)
        {
	        if(IsExpired) return;
	        if (_modelController == null) return;
	        if(!_modelController.IsModelLoadFinish) return;
	        _modelController.Update(dt);
        }

        /// <summary>
        /// 删除前调用
        /// </summary>
        public void Destroy()
        {
            OnDestroyCallBack?.Invoke(this);
            _modelController.Destroy();
            Logic.Destroy();
            _message.UnInit();
        }

        #endregion

        #region 对外接口
        
        public void TriggerEvent(EBattleEventType eventType, IEventInfo eventInfo) {
	        BattleEventManager.Instance.TriggerActorEvent(Uid, eventType, eventInfo);
        }
        
        public void AddMsgListener(MessageListener listener)
        {
            _message.AddListener(listener);
        }
        
        public void RemoveMsgListener(MessageListener listener)
        {
            _message.RemoveListener(listener);
        }
        
        public T GetAttr<T>(int logicAttr)
        {
	        return _attrs.GetAttr<T>(logicAttr);
        }
        
        public T GetAttr<T>(ELogicAttr logicAttr)
        {
	        Profiler.BeginSample("SetAttr");
	        var value = _attrs.GetAttr<T>(logicAttr.ToInt());
	        Profiler.EndSample();
            return value;
        }

        public object GetAttrBox(ELogicAttr logicAttr)
        {
            return _attrs.GetAttrBox(logicAttr.ToInt());
        }

        public ICommand SetAttr<T>(int logicAttr, T value, bool isTempData)
        {
	        Profiler.BeginSample("SetAttr");
	        var fvalue = _attrs.SetAttr(logicAttr, value, isTempData);
	        Profiler.EndSample();
	        return fvalue;
        }
        
        public ICommand SetAttr<T>(ELogicAttr logicAttr, T value, bool isTempData)
        {
	        Profiler.BeginSample("SetAttrCommand");
	        var fvalue = _attrs.SetAttr(logicAttr.ToInt(), value, isTempData);
	        Profiler.EndSample();
            return fvalue;
        }

        public ICommand SetAttrBox(ELogicAttr logicAttr, object value, bool isTempData)
        {
            return _attrs.SetAttrBox(logicAttr.ToInt(), value, isTempData);
        }
        
        public bool TryGetAbility(int uid,out Ability ability)
        {
            return _abilityController.TryGetAbility(uid, out ability);
        }
        
        public void AddTag(int tag)
        {
            _tags.Add(tag);
        }

        public bool HasTag(int tag)
        {
            return _tags.HasTag(tag);
        }

        public void RemoveTag(int tag)
        {
            _tags.Remove(tag);
        }
        
        public bool HasAttr(ELogicAttr attr)
        {
            return _attrs.HasAttr(attr.ToInt());
        }

        public bool HasAbility(int abilityConfigId)
        {
            return _abilityController.HasAbility(abilityConfigId);
        }

        public int GetBuffLayer(int buff) {
	        if (Logic.TryGetComponent<ActorLogic.BuffComp>(out var buffComp)) {
		       return buffComp.GetBuffLayer(buff);
	        }
	        return -1;
        }

        public void AwardAbility(int abilityId, bool runNow)
        {
            _abilityController.AwardAbility(abilityId, runNow);
        }
        
        public void RemoveAbility(int abilityId)
        {
            _abilityController.RemoveAbility(abilityId);
        }
        
        #endregion

        #region LUA_Attr

        public int GetAttrLua(int attrType)
        {
            return _attrs.GetAttr<int>(attrType);
        }

        public void SetAttrLua(int attrType, int value)
        {
            _attrs.SetAttr<int>(attrType, value, false);
        }

        public void SetAttrLuaByType(ELogicAttr attrType, int value)
        {
            _attrs.SetAttr<int>(attrType.ToInt(), value, false);
        }

        public int GetAttrLuaByType(ELogicAttr attrType)
        {
            return _attrs.GetAttr<int>(attrType.ToInt());
        }

        #endregion
    }
}