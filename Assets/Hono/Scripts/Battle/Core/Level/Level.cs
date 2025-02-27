using Cysharp.Threading.Tasks;
using System.Collections.Generic;
using Unity.Collections;

namespace Hono.Scripts.Battle.Core
{
    /// <summary>
    /// 关卡模式
    /// </summary>
    public interface ILevelMode
    {
        public void Start();
        public void Tick();
        public void Exit();
    }
    
    public abstract class Level
    {
        /// <summary>
        /// 任务控制器
        /// </summary>
        protected readonly QuestSystem QuestSystem;
        /// <summary>
        /// 场景对象
        /// </summary>
        protected Dictionary<int, LevelObject> LevelObjects;
        /// <summary>
        /// 在运行中的监测器
        /// </summary>
        private readonly List<LevelConditionMonitor> _conditionMonitors;
        /// <summary>
        /// 需要tick的监控器
        /// </summary>
        private readonly List<IMonitorTickEnable> _monitorTick;
        /// <summary>
        /// 当前状态
        /// </summary>
        private ILevelMode _curMode;
        /// <summary>
        /// 上一次更新事件
        /// </summary>
        private float _beforeClearTime;

        protected Level()
        {
            _curMode = null;
            QuestSystem = new QuestSystem(this);
            LevelObjects = new Dictionary<int, LevelObject>();
            _conditionMonitors = new List<LevelConditionMonitor>(20);
        }

        /// <summary>
        /// 加载关卡相关数据
        /// </summary>
        /// <returns></returns>
        public abstract UniTask Load();

        /// <summary>
        /// tick
        /// </summary>
        public void Tick()
        {
            _curMode?.Tick();

            foreach (var tickEnable in _monitorTick)
            {
                tickEnable.Tick(World.Current.OnceTickTime);
            }
            
            if (World.Current.RealWorldTimeSinceStart - _beforeClearTime < 0.5f)
            {
                return;
            }
            
            _beforeClearTime = World.Current.RealWorldTimeSinceStart;
            for (var index = 0; index < _conditionMonitors.Count; index++)
            {
                LevelConditionMonitor monitor = _conditionMonitors[index];
                if (!monitor.IsPass) continue;
                //相当于失效了
                _conditionMonitors.RemoveSwapBack(monitor);
                --index;
            }
        }

        /// <summary>
        /// 添加检测器
        /// </summary>
        /// <param name="monitor"></param>
        public void AddMonitor(LevelConditionMonitor monitor)
        {
            _conditionMonitors.Add(monitor);
        }

        /// <summary>
        /// 切换状态
        /// </summary>
        /// <param name="mode"></param>
        protected void SwitchState(ILevelMode mode)
        {
            _curMode?.Exit();
            _curMode = mode;
            _curMode?.Start();
        }
    }
}