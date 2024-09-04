using Hono.Scripts.Battle.Tools;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Hono.Scripts.Battle {
	//逻辑层
	public abstract partial class ActorLogic : ITick, IVariablesBind {
		/// <summary>
		/// Actor的UID
		/// </summary>
		public int Uid; 
		
		//逻辑层包含数据,逻辑流程
		/// <summary>
		/// 属性
		/// </summary>
		protected AttrCollection _attrs;

		/// <summary>
		/// 逻辑数据
		/// </summary>
		protected ActorLogicData _logicData;

		public ActorLogicData LogicData => _logicData;

		/// <summary>
		/// 变量
		/// </summary>
		protected Variables _variables;

		/// <summary>
		/// 状态机
		/// </summary>
		protected ActorStateMachine _stateMachine;

		/// <summary>
		/// Ability控制器
		/// </summary>
		protected AbilityController _abilityController;

		/// <summary>
		/// tag
		/// </summary>
		protected Tags Tags;

		/// <summary>
		/// 逻辑组件
		/// </summary>
		protected readonly Dictionary<Type, ALogicComponent> _components;

		public ActorLogic(int uid,ActorLogicData logicData) {
			Uid = uid;
			_logicData = logicData;
			_attrs = new AttrCollection(LogicAttrCreator.Create);
			_abilityController = new AbilityController(this);
			_variables = new Variables(16, this);
			_stateMachine = new ActorStateMachine(this);
			_components = new Dictionary<Type, ALogicComponent>();
			Tags = new Tags();
		}
		
		public void Init() {
			InitAttr();
			onInit();
			registerComponents();
			foreach (var component in _components) {
				component.Value.Init();
			}
		}

		private void InitAttr()
		{
			
		}

		protected abstract void onInit();

		private void registerComponents() {
			registerChildComp();
		}

		protected abstract void registerChildComp();


		protected void addComponent(ALogicComponent component) {
			if (!_components.TryAdd(component.GetType(), component)) {
				Debug.Log($"{this.GetType()}  添加组件 {component.GetType()} Failed!");
			}
		}

		public T GetComponent<T>() where T : ALogicComponent {
			if (!_components.TryGetValue(typeof(T), out var component)) {
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

		public void Tick(float dt) {
			foreach (var component in _components) {
				component.Value.Tick(dt);
			}

			_stateMachine.Tick(dt);
			_abilityController.Tick(this, dt);

			onTick(dt);
		}

		public void Destroy() {
			onDestroy();
		}

		protected virtual void onDestroy() { }

		public Variables GetVariables() {
			return _variables;
		}

		public T GetAttr<T>(ELogicAttr logicAttr) {
			return _attrs.GetAttr<T>(logicAttr.ToInt());
		}

		public object GetAttrBox(ELogicAttr logicAttr) {
			return _attrs.GetAttrBox(logicAttr.ToInt());
		}

		public ICommand SetAttr<T>(ELogicAttr logicAttr, T value, bool isTempData) {
			return _attrs.SetAttr(logicAttr.ToInt(), value, isTempData);
		}

		public ICommand SetAttrBox(ELogicAttr logicAttr, object value, bool isTempData) {
			return _attrs.SetAttrBox(logicAttr.ToInt(), value, isTempData);
		}

		public void AwardAbility(int configId, bool isRunNow)
		{
			_abilityController.AwardAbility(configId, isRunNow);
		}
		
		public void AddTag(int tag) {
			Tags.Add(tag);
		}
		
		public bool HasTag(int tag) {
			return Tags.HasTag(tag);
		}

		public EActorState CurState()
		{
			return _stateMachine.Current.StateType;
		}

		public bool HasAttr(ELogicAttr attr)
		{
			return _attrs.HasAttr(attr.ToInt());
		}
		
		public bool HasAbility(int abilityConfigId) {
			return _abilityController.HasAbility(abilityConfigId);
		}

		#region LUA_Attr
		public void ShowAbility() {
			_abilityController.Show();
		}
		
		public void ShowCurTags() {
			Tags.Show();
		}
		
		public int GetAttrLua(int attrType) {
			return _attrs.GetAttr<int>(attrType);
		}
		#endregion
	}
}