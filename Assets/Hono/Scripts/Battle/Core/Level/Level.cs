using Cysharp.Threading.Tasks;
using System.Collections.Generic;

namespace Hono.Scripts.Battle.Core {
    /// <summary>
    /// 关卡模式
    /// </summary>
    public interface ILevelMode {
        public void Start();
        public void Tick();
        public void Exit();
    }

    /// <summary>
    /// 关卡对象
    /// </summary>
    public interface ILevelObject
    {
        public int LevelId { get; set; }
    }
    
	public abstract class Level {
		/// <summary>
		/// 任务控制器
		/// </summary>
		public QuestController QuestController { get; }

        /// <summary>
        /// 场景对象
        /// </summary>
        private Dictionary<int, ILevelObject> _levelObjects;
        /// <summary>
        /// 当前状态
        /// </summary>
        private ILevelMode _curMode;

        protected Level() {
			_curMode = null;
			QuestController = new QuestController(this);
            _levelObjects = new Dictionary<int, ILevelObject>();
        }

        /// <summary>
        /// 加载关卡相关数据
        /// </summary>
        /// <returns></returns>
        public abstract UniTask Load();

		/// <summary>
		/// tick
		/// </summary>
		public void Tick() {
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