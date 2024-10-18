using Hono.Scripts.Battle.Tools;
using System.Collections.Generic;
using UnityEngine;

namespace Hono.Scripts.Battle {
	/// <summary>
	/// 这个Ability代表了运行时流程管理
	/// </summary>
	public sealed partial class Ability : IVarCollectionBind, IReloadHandle {
		/// <summary>
		/// Ability的上下文，存储当前在运行哪个Ability
		/// </summary>
		private static AbilityRuntimeContext _context;

		public static AbilityRuntimeContext Context => _context ??= new AbilityRuntimeContext();

		//基础数据
		/// <summary>
		/// 能力运行时唯一识别id
		/// </summary>
		public int Uid { get; }

		/// <summary>
		/// 基础数据配置Id
		/// </summary>
		public int ConfigId { get; }

		/// <summary>
		/// 该能力属于哪个Actor
		/// </summary>
		public Actor Actor { get; }

		/// <summary>
		/// 周期类
		/// </summary>
		private readonly AbilityState _state;

		/// <summary>
		/// 执行者
		/// </summary>
		private readonly AbilityExecutor _executor;

		/// <summary>
		/// 指令缓存
		/// </summary>
		private readonly HashSet<ICommand> _commands;
		
		/// <summary>
		/// 属于Ability的变量
		/// </summary>
		public VarCollection Variables { get; }
		
		/// <summary>
		/// tags
		/// </summary>
		public Tags Tags { get; private set; }

		/// <summary>
		/// 逻辑帧时间缩放系数
		/// </summary>
		public float TimeScaleFactory;

		public Ability(Actor actor, int abilityConfigId) {
			Uid = abilityConfigId;
			Actor = actor;
			ConfigId = abilityConfigId;
			Variables = new VarCollection(this,64);
			_executor = new AbilityExecutor(this);
			_state = new AbilityState(this);
			_commands = new HashSet<ICommand>();
			AssetManager.Instance.AddReloadHandle(this);
			Tags = new Tags();
			TimeScaleFactory = 1;
			_executor.Setup();
		}

		public void Stop() {
			if (_state.Current.CurState == EAbilityState.Executing) {
				_state.StopExecuting();
			}
		}
		
		public void Execute() {
			//Debug.Log($"[Ability] ActorUid {Actor.Uid} Execute AbilityId:{ConfigId} Execute !");
			_state.TryExecute();
		}

		public void Reload() {
			Debug.Log($"Actor {Actor.Uid} Ability {_executor.Ability.ConfigId} Reload");
			
			//终止能力运行
			_state.ForceStop();

			//卸载加载好的节点
			_executor.UnInstall();

			//清理变量
			Variables.Clear();

			//指令撤销
			foreach (var command in _commands) {
				command.Undo();
			}

			//重置error状态
			_executor.Reset();
			_state.Reset();

			//重新获取数据
			_executor.Setup();
		}

		public void OnTick(float dt) {
			_state.Tick(dt * TimeScaleFactory);
		}

		public void SetNextGroupId(int id) {
			if (_state.Current.CurState == EAbilityState.Executing) {
				//Debug.Log($"[Ability] AbilityId:{ConfigId} SetNextGroupId call NextGroupId:{id}");
				((ExecutingCycle)_state.Current).NextGroupId = id;
			}
		}

		public void StopGroup() {
			if (_state.Current.CurState == EAbilityState.Executing) {
				//Debug.Log($"[Ability] AbilityId:{ConfigId} StopGroup call");
				((ExecutingCycle)_state.Current).CurrentGroupStop();
			}
		}

		public void AddCommand(ICommand command) {
			_commands.Add(command);
		}

		public CycleCallback GetCycleCallback(EAbilityAllowEditCycle allowEditCycle) {
			return _state.GetCycleCallback(allowEditCycle);
		}

		public void OnDestroy() {
			foreach (var command in _commands) {
				command.Undo();
			}

			_state.OnDestroy();
			_executor.OnDestroy();
			AssetManager.Instance.RemoveReloadHandle(this);
		}
	}
}