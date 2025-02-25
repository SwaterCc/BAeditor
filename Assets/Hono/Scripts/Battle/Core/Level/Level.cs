using Cysharp.Threading.Tasks;
using System.Collections.Generic;

namespace Hono.Scripts.Battle.Core {
	//BattleManager->Data->World
	//World其实是游戏环境的初始化，进入world，同时是进入场景，然后启动相关的控制管理
	//Level是World的子级对象，世界可以没有关卡，但关卡必须在世界中存在
	//关卡处理流程加载，任务事件管理，关卡结算等周期和玩法的相关规则，任务UI显示
	//关卡基类，只实现流程
	public abstract class Level {
		/// <summary>
		/// 当前状态
		/// </summary>
		private ILevelMode _curMode;
		/// <summary>
		/// 任务控制器
		/// </summary>
		private QuestController _questController;
		///场景单位管理器
		/// 场景id影射单位id
		/// 场景事件依赖场景id，
		///
		/// 据点 -》 -》	
		/// 
		

		/// <summary>
		/// 是否加载完成
		/// </summary>
		public bool IsLoadFinish { get; private set; }

		public Level() {
			IsLoadFinish = false;
			_curMode = null;
			_questController = new QuestController();
		}

		public async void Load() {
			//加载数据
			await OnLoad();
			IsLoadFinish = true;
		}

		/// <summary>
		/// 子类实现Load方法
		/// </summary>
		protected abstract UniTask OnLoad();

		/// <summary>
		/// tick
		/// </summary>
		public void Tick() {
			if(!IsLoadFinish)
				return;
			_questController.Tick();
			_curMode?.Tick();
		}

		/// <summary>
		/// 切换状态
		/// </summary>
		/// <param name="mode"></param>
		protected void SwitchState(ILevelMode mode) {
			_curMode?.Exit();
			_curMode = mode;
			_curMode?.Start();
		}
	}
}