using Hono.Scripts.Battle.Tools;
using System.Collections.Generic;

namespace Hono.Scripts.Battle {
	/// <summary>
	/// 这个Ability代表了运行时流程管理
	/// </summary>
	public partial class Ability : IVariablesBind {
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
		public int BelongActorId { get; }

		
		/// <summary>
		/// 属于Ability的变量
		/// </summary>
		private readonly Variables _variables;

		public Variables GetVariables() => _variables;

		/// <summary>
		/// 周期类
		/// </summary>
		private readonly AbilityState _state;

		/// <summary>
		/// 执行者
		/// </summary>
		private readonly AbilityExecutor _executor;
		
		//指令缓存
		private readonly HashSet<ICommand> _commands;
		
		public Ability(int uid, int belongActorId, int abilityConfigId) {
			Uid = uid;
			BelongActorId = belongActorId;
			ConfigId = abilityConfigId;
			_variables = new Variables(16, this);
			_executor = new AbilityExecutor(this);
			_state = new AbilityState(this);
			_commands = new HashSet<ICommand>();
			_executor.Setup();
		}

		public void Execute() {
			_state.TryExecute();
		}

		public void Reload() {
			//终止能力运行
			_state.Stop();

			//卸载加载好的节点
			_executor.UnInstall();

			//清理变量
			_variables.Clear();
			
			//指令撤销
			foreach (var command in _commands) {
				command.Undo();
			}
			
			//重新获取数据
			_executor.Setup();
		}

		public void OnTick(float dt) {
			_state.Tick(dt);
		}

		public void SetNextGroupId(int id) {
			if (_state.Current.CurState == EAbilityState.Executing) {
				((ExecutingCycle)_state.Current).NextGroupId = id;
			}
		}

		public void StopGroup() {
			if (_state.Current.CurState == EAbilityState.Executing) {
				((ExecutingCycle)_state.Current).CurrentGroupStop();
			}
		}

		public void AddCommand(ICommand command) {
			_commands.Add(command);
		}

		public CycleCallback GetCycleCallback(EAbilityAllowEditCycle allowEditCycle)
		{
			return _state.GetCycleCallback(allowEditCycle);
		}
		
		public void OnDestroy() {
			foreach (var command in _commands) {
				command.Undo();
			}
			_state.OnDestroy();
			_executor.OnDestroy();
		}
	}
}