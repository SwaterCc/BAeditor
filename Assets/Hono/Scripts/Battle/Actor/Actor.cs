using System;
using Hono.Scripts.Battle.Event;
using Hono.Scripts.Battle.Tools;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Profiling;

namespace Hono.Scripts.Battle
{
    /// <summary>
    /// Actor 实际的行为由Logic+ModelController组成
    /// </summary>
    public sealed partial class Actor : IVarCollectionBind, IPoolObject
    {
        /// <summary>
        /// 运行时唯一ID
        /// </summary>
        public int Uid { get; private set; }
        
        /// <summary>
        /// Actor基础类型
        /// </summary>
        public EActorType ActorType { get; private set;  }
        
        /// <summary>
        /// 变量黑板
        /// </summary>
        public VarCollection Variables { get; }
        
        /// <summary>
        /// Actor逻辑
        /// </summary>
        public ActorLogic Logic { get; private set; }

        /// <summary>
        /// Unity交互层
        /// </summary>
        public ActorModelController ModelController { get; private set; }
        
        /// <summary>
        /// ability控制器
        /// </summary>
        private readonly AbilityController _abilityController;

        /// <summary>
        /// Actor属性
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
        public bool ActorSetupFinish => ModelController.IsModelLoadFinish;

        /// <summary>
        /// 是否为玩家操控单位
        /// </summary>
        public bool IsPlayerControl { get; set; }

        /// <summary>
        /// 是否为过期单位
        /// </summary>
        public bool IsExpired { get; set; }

        /// <summary>
        /// Model加载完成后调用
        /// </summary>
        public Action<Actor> OnModelLoadFinish { get; set; }
        
        /// <summary>
        /// 进入场景后调用
        /// </summary>
        public Action<Actor> OnEnterSceneCallBack { get; set; }
        
        /// <summary>
        /// 每次Tick最后调用
        /// </summary>
        public Action<Actor> OnTickCallBack { get; set; }
        
        /// <summary>
        /// 删除时调用
        /// </summary>
        public Action<Actor> OnDestroyCallBack { get; set; }

        public Actor()
        {
            _tags = new Tags();
            _attrs = new AttrCollection(this, AttrCreator.Create);
            _abilityController = new AbilityController(this);
            _message = new MessageCollection(this);
            Variables = new VarCollection(this, 128);
        }

        public void Init(int uid, EActorType actorType)
        {
            Uid = uid;
            ActorType = actorType;
            IsExpired = false;
            IsPlayerControl = false;
        }
        
        public void OnRecycle()
        {
            //调用删除逻辑
            Destroy();
            
            //清空回调
            OnModelLoadFinish = null;
            OnEnterSceneCallBack = null;
            OnTickCallBack = null;
            OnDestroyCallBack = null;
            
            //清空容器
            _tags.Clear();
            _attrs.Clear();
            _abilityController.Clear();
            Variables.Clear();
            _message.Clear();
            
            //清空Model，Logic
            ModelController = null;
            Logic = null;
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
            ModelController = modelController;
            ModelController.Setup(_tags, Variables, Logic);
        }
        
        /// <summary>
        /// ModelController和Logic加载完成后，与Tick处与同一帧，先于Tick调用
        /// </summary>
        public void EnterScene()
        {
            Logic.EnterScene();
            ModelController.EnterScene();
            _message.Init();
            OnEnterSceneCallBack?.Invoke(this);
        }

        /// <summary>
        /// 逻辑帧
        /// </summary>
        /// <param name="dt"></param>
        public void Tick(float dt)
        {
	        if(IsExpired) return;
	        Logic.Tick(dt);
            _abilityController.Tick(dt);
            OnTickCallBack?.Invoke(this);
        }

        /// <summary>
        /// 表现帧
        /// </summary>
        /// <param name="dt"></param>
        public void Update(float dt)
        {
	        if(IsExpired) return;
	        if (ModelController == null) return;
	        if(!ModelController.IsModelLoadFinish) return;
            ModelController.Update(dt);
        }

        /// <summary>
        /// 删除前调用
        /// </summary>
        public void Destroy()
        {
            OnDestroyCallBack?.Invoke(this);
            ModelController.Destroy();
            Logic.Destroy();
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
            return  _attrs.GetAttr<T>(logicAttr.ToInt());
        }

        public object GetAttrBox(ELogicAttr logicAttr)
        {
            return _attrs.GetAttrBox(logicAttr.ToInt());
        }

        public ICommand SetAttr<T>(int logicAttr, T value, bool isTempData)
        {
	        return _attrs.SetAttr(logicAttr, value, isTempData);
        }
        
        public ICommand SetAttr<T>(ELogicAttr logicAttr, T value, bool isTempData)
        {
            return _attrs.SetAttr(logicAttr.ToInt(), value, isTempData);;
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