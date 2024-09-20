using Hono.Scripts.Battle.Tools;
using System.Collections.Generic;
using UnityEngine;

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
        /// 配置ID
        /// </summary>
        public int ConfigId { get; private set; }

        /// <summary>
        /// 是否无效
        /// </summary>
        public bool IsDisposable { get; private set; }

        /// <summary>
        /// 变量黑板
        /// </summary>
        public VarCollection Variables { get; }

        /// <summary>
        /// Actor基础数据
        /// </summary>
        public ActorPrototypeTable.ActorPrototypeRow PrototypeData { get; }

        /// <summary>
        /// 表现层
        /// </summary>
        private ActorShow _show;

        /// <summary>
        /// 逻辑层对外可见
        /// </summary>
        public ActorLogic Logic { get; private set; }

        /// <summary>
        /// 当前运行状态
        /// </summary>
        private readonly ActorRTState _rtState;

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
        /// 当前坐标
        /// </summary>
        public Vector3 Pos => GetAttr<Vector3>(ELogicAttr.AttrPosition);

	    /// <summary>
	    /// 子child
	    /// </summary>
        public List<Actor> Children = new List<Actor>();

        public Actor(int uid)
        {
            Uid = uid;
            _tags = new Tags();
            _rtState = new ActorRTState();
            _attrs = new AttrCollection(this, AttrCreator.Create);
            _abilityController = new AbilityController(this);
            _message = new MessageCollection(this);
            Variables = new VarCollection(this, 128);
            SetAttr(ELogicAttr.AttrUid, uid, false);
        }

        #region 生命周期

        /// <summary>
        /// Create时调用，同帧执行
        /// </summary>
        /// <param name="show"></param>
        /// <param name="logic"></param>
        public void Setup(ActorShow show, ActorLogic logic)
        {
            Logic = logic;
            _show = show;
            _rtState.Setup(show, logic, _attrs);
            Logic.Setup(_abilityController, _attrs, _tags, Variables);
            _show.Setup(_tags, Variables, _rtState);
        }

        /// <summary>
        /// 传承数据
        /// </summary>
        public void PassData() {
	        
        }
        
        /// <summary>
        /// 加入Rt列表后执行，一般是创建后的第一帧先执行了Init 之后会同帧调用 Tick Update
        /// </summary>
        public void Init()
        {
            Logic?.Init();
            _show?.Init();
            _message.Init();
        }

        /// <summary>
        /// 逻辑帧
        /// </summary>
        /// <param name="dt"></param>
        public void Tick(float dt)
        {
            _rtState.Tick(dt);
            _abilityController.Tick(dt);
        }

        /// <summary>
        /// 表现帧
        /// </summary>
        /// <param name="dt"></param>
        public void Update(float dt)
        {
            _rtState.Update(dt);
        }

        /// <summary>
        /// 删除前调用
        /// </summary>
        public void Destroy()
        {
            _show.Destroy();
            Logic.Destroy();
            _message.UnInit();
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
        
        public T GetAttr<T>(int logicAttr)
        {
	        return _attrs.GetAttr<T>(logicAttr);
        }
        
        public T GetAttr<T>(ELogicAttr logicAttr)
        {
            return _attrs.GetAttr<T>(logicAttr.ToInt());
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
            return _attrs.SetAttr(logicAttr.ToInt(), value, isTempData);
        }

        public ICommand SetAttrBox(ELogicAttr logicAttr, object value, bool isTempData)
        {
            return _attrs.SetAttrBox(logicAttr.ToInt(), value, isTempData);
        }

        public int AwardAbility(int configId, bool isRunNow)
        {
           return _abilityController.AwardAbility(configId, isRunNow);
        }

        public void ExecutingAbility(int uid)
        {
            _abilityController.ExecutingAbility(uid);
        }

        public void ExecuteAbilityByConfigId(int config)
        {
            _abilityController.ExecutingAbilityByConfig(config);
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