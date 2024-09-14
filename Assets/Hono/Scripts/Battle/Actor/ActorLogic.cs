using Hono.Scripts.Battle.Tools;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Hono.Scripts.Battle {
	/// <summary>
	/// Actor的逻辑，包含逻辑层自身的逻辑和关联组件，状态机，最终决定出当前Actor逻辑层的属性
	/// ActorLogic可被Actor外界影响
	/// </summary>
	public abstract partial class ActorLogic : ITick {
		/// <summary>
		/// Actor的UID
		/// </summary>
		public int Uid;

		/// <summary>
		/// Actor
		/// </summary>
		public Actor Actor { get; }

		/// <summary>
		/// 逻辑数据
		/// </summary>
		public ActorLogicTable.ActorLogicRow LogicData { get; private set; }

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
		protected Actor.AbilityController _abilityController;

		/// <summary>
		/// Actor的属性
		/// </summary>
		protected AttrCollection _attrs;

		/// <summary>
		/// Actor的Tags
		/// </summary>
		protected Tags _tags;

		/// <summary>
		/// Actor的黑板数据
		/// </summary>
		protected VarCollection _variables;

		/// <summary>
		/// 输入来源
		/// </summary>
		protected IInputHandle _inputHandle;

		public ActorLogic(Actor actor, ActorLogicTable.ActorLogicRow logicData) {
			Actor = actor;
			Uid = actor.Uid;
			LogicData = logicData;
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
		}

		public void Init() {
			onInit();
			onInputInit();
			registerComponents();
			registerChildComponents();
			foreach (var component in _components) {
				component.Value.Init();
			}

			_stateMachine.Init();
		}

		protected abstract void setupAttrs();

		protected abstract void onInit();

		protected virtual void onInputInit() {
			switch ((EActorLogicType)LogicData.LogicType) {
				case EActorLogicType.Pawn:
					_inputHandle = new PawnLeaderInput();
					break;
				case EActorLogicType.Monster:
				case EActorLogicType.Building:
				case EActorLogicType.HitBox:
				default:
					_inputHandle = new NoInput();
					break;
			}
		}

		private void registerComponents() { }

		protected abstract void registerChildComponents();

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