using Hono.Scripts.Battle.Tools;
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
        /// tag
        /// </summary>
        private readonly Tags _tags;

        /// <summary>
        /// 当前坐标
        /// </summary>
        public Vector3 Pos => GetAttr<Vector3>(ELogicAttr.AttrPosition);

        public Actor(int uid)
        {
            Uid = uid;
            _tags = new Tags();
            _rtState = new ActorRTState();
            _attrs = new AttrCollection(this, AttrCreator.Create);
            _abilityController = new AbilityController(this);
            Variables = new VarCollection(this, 128);
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
        /// 加入Rt列表后执行，一般是创建后的第一帧先执行了Init 之后会同帧调用 Tick Update
        /// </summary>
        public void Init()
        {
            Logic?.Init();
            _show?.Init();
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
        }

        #endregion

        #region 对外接口

        public T GetAttr<T>(ELogicAttr attrId)
        {
            return _attrs.GetAttr<T>(attrId.ToInt());
        }
        
        public ICommand SetAttr<T>(ELogicAttr attrId, T value, bool isTempData = false)
        {
            return _attrs.SetAttr(attrId.ToInt(), value, isTempData);
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
        
        public bool HasAbility(int abilityConfigId)
        {
            return _abilityController.HasAbility(abilityConfigId);
        }

        public bool TryGetAbility(int uid,out Ability ability)
        {
            return _abilityController.TryGetAbility(uid, out ability);
        }
        
        public void AddTag(int tag)
        {
            _tags.Add(tag);
        }

        public void RemoveTag(int tag)
        {
           
        }
        
        public bool HasTag(int tag)
        {
            return _tags.HasTag(tag);
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