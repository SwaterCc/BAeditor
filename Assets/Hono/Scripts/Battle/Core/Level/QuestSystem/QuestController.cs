using Hono.Scripts.Battle.Core.Base;

namespace Hono.Scripts.Battle.Core {
	/// <summary>
	/// 任务控制器
	/// </summary>
	public class QuestController
    {
        /// <summary>
        /// 当前关卡
        /// </summary>
        private Level _level;

        public QuestController(Level level)
        {
            _level = level;
        }
	}
}