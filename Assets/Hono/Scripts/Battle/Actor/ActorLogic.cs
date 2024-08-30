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
		
		/// <summary>
		/// ActorLogic配置Id
		/// </summary>
		protected int _configId;

		//逻辑层包含数据,逻辑流程
		/// <summary>
		/// 属性
		/// </summary>
		protected AttrCollection _attrs;

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
		protected Tags Tagses;

		/// <summary>
		/// 逻辑组件
		/// </summary>
		protected readonly Dictionary<Type, ALogicComponent> _components;

		public ActorLogic(Actor actor) {
			Uid = actor.Uid;
			_configId = actor.ConfigId;
			_attrs = new AttrCollection(LogicAttrCreator.Create);
			_abilityController = new AbilityController();
			_variables = new Variables(16, this);
			_stateMachine = new ActorStateMachine(this);
			_components = new Dictionary<Type, ALogicComponent>();
			Tagses = new Tags();
		}
		
		public void Init() {
			onInit();
			registerComponents();
			foreach (var component in _components) {
				component.Value.Init();
			}
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

		public T GetAttr<T>(EAttrType attrType) {
			return _attrs.GetAttr<T>(attrType.ToInt());
		}

		public object GetAttrBox(EAttrType attrType) {
			return _attrs.GetAttrBox(attrType.ToInt());
		}

		public ICommand SetAttr<T>(EAttrType attrType, T value, bool isTempData) {
			return _attrs.SetAttr(attrType.ToInt(), value, isTempData);
		}

		public ICommand SetAttrBox(EAttrType attrType, object value, bool isTempData) {
			return _attrs.SetAttrBox(attrType.ToInt(), value, isTempData);
		}

		public void AwardAbility(int configId, bool isRunNow)
		{
			_abilityController.AwardAbility(Uid, configId, isRunNow);
		}
		
		public void AddTag(int tag) {
			Tagses.Add(tag);
		}
		
		public bool HasTag(int tag) {
			return Tagses.HasTag(tag);
		}

		public bool HasAbility(int abilityConfigId) {
			return _abilityController.HasAbility(abilityConfigId);
		}

		#region LUA_Attr
		public void ShowAbility() {
			_abilityController.Show();
		}
		
		public void ShowCurTags() {
			Tagses.Show();
		}
		
		public int GetAttrLua(int attrType) {
			return _attrs.GetAttr<int>(attrType);
		}
		#endregion
	}
}