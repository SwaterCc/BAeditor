using Hono.Scripts.Battle.Tools;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Hono.Scripts.Battle {
	/// <summary>
	/// Actor的逻辑，包含逻辑层自身的逻辑和关联组件，状态机，最终决定出当前Actor逻辑层的属性,逻辑层必然存在
	/// ,逻辑层从设计理念上来讲是不知晓表现层的存在的，所以不要用逻辑层调用表现层
	/// </summary>
	public abstract partial class ActorLogic : ITick {
		/// <summary>
		/// Actor的UID
		/// </summary>
		public int Uid;

		/// <summary>
		/// 模型Id，没有则为-1
		/// </summary>
		public int ModelId;

		/// <summary>
		/// Actor
		/// </summary>
		public Actor Actor { get; }

		/// <summary>
		/// 状态机
		/// </summary>
		protected ActorStateMachine _stateMachine;

		/// <summary>
		/// 逻辑组件
		/// </summary>
		private readonly Dictionary<Type, ALogicComponent> _components;

		/// <summary>
		/// Actor的Ability控制器
		/// </summary>
		private Actor.AbilityController _abilityController;
		protected Actor.AbilityController AbilityController => _abilityController;

		/// <summary>
		/// Actor的属性
		/// </summary>
		private AttrCollection _attrs;
		protected AttrCollection Attrs => _attrs;

		/// <summary>
		/// Actor的Tags
		/// </summary>
		private Tags _tags;
		protected Tags Tags => _tags;

		/// <summary>
		/// Actor的黑板数据
		/// </summary>
		private VarCollection _variables;
		protected VarCollection Variables => _variables;

		/// <summary>
		/// 输入来源
		/// </summary>
		protected ActorInput ActorInput;

		public ActorLogic(Actor actor) {
			Actor = actor;
			Uid = actor.Uid;
			ActorInput = new NoInput(this);
			_stateMachine = new ActorStateMachine(this);
			_components = new Dictionary<Type, ALogicComponent>();
		}

		public void Setup(Actor.AbilityController controller, AttrCollection attrs, Tags tags,
			VarCollection varCollection) {

			_attrs = attrs;
			_abilityController = controller;
			_tags = tags;
			_variables = varCollection;
			
			setupAttrs();
			setupInput();
			setupComponents();
			setupStateMachine();
		}

		public void Init() {
			onInit();
			
			foreach (var component in _components) {
				component.Value.Init();
			}

			_stateMachine?.Init();
		}

		protected virtual void setupAttrs() { }
		protected virtual void setupInput() { }
		protected virtual void setupStateMachine() { }
		protected virtual void onInit() { }
		protected virtual void setupComponents() { }

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

		public bool TryGetComponent<T>(out T comp) where T : ALogicComponent {
			comp = null;
			if (_components.TryGetValue(typeof(T), out var component)) {
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

			onTick(dt);
		}

		public void Destroy() {
			onDestroy();

			foreach (var component in _components) {
				component.Value.UnInit();
			}
		}

		protected virtual void onDestroy() { }

		public EActorState CurState() {
			return _stateMachine.Current.StateType;
		}


		public T GetAttr<T>(ELogicAttr logicAttr) {
			return Actor.GetAttr<T>(logicAttr);
		}

		public object GetAttrBox(ELogicAttr logicAttr) {
			return Actor.GetAttrBox(logicAttr);
		}

		public ICommand SetAttr<T>(ELogicAttr logicAttr, T value, bool isTempData) {
			return Actor.SetAttr(logicAttr, value, isTempData);
		}

		public ICommand SetAttrBox(ELogicAttr logicAttr, object value, bool isTempData) {
			return Actor.SetAttrBox(logicAttr, value, isTempData);
		}
	}
}