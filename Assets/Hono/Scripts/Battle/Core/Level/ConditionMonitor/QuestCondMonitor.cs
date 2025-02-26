namespace Hono.Scripts.Battle.Core
{
    /// <summary>
    /// 任务监控器类型
    /// </summary>
    public enum EQuestStateType
    {
        Accept,
        Finish,
        Fail,
    }

    /// <summary>
    /// 任务监控器基类
    /// </summary>
    public abstract class QuestCondMonitor : LevelConditionMonitor
    {
        /// <summary>
        /// 任务系统
        /// </summary>
        private readonly QuestSystem _questSystem;
        /// <summary>
        /// 绑定的任务
        /// </summary>
        private readonly int _questId;
        /// <summary>
        /// 任务监控类型
        /// </summary>
        private readonly EQuestStateType _questMonitorType;

        protected QuestCondMonitor(QuestSystem questSystem, int questId, EQuestStateType questMonitorType)
        {
            _questSystem = questSystem;
            _questId = questId;
            _questMonitorType = questMonitorType;
        }

        protected override void OnPass()
        {
            _questSystem.OnQuestCondMonitorPass(_questId, _questMonitorType);
        }
    }
}