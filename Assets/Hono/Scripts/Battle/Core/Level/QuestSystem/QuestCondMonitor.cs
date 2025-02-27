using System;

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
    /// 任务监控器
    /// </summary>
    public sealed class QuestCondMonitor : LevelConditionMonitor
    {
        /// <summary>
        /// 原生监控器
        /// </summary>
        private LevelConditionMonitor _monitor;
        /// <summary>
        /// 绑定的任务
        /// </summary>
        private readonly int _questId;
        /// <summary>
        /// 任务监控类型
        /// </summary>
        private readonly EQuestStateType _questMonitorType;
        /// <summary>
        /// 任务通过的回调
        /// </summary>
        private Action<int,EQuestStateType> _onQuestCondPassCallBack;
        
        public QuestCondMonitor(int questId, EQuestStateType questMonitorType)
        {
            _questId = questId;
            _questMonitorType = questMonitorType;
        }

        public void SetPassAction(Action<int,EQuestStateType> action)
        {
            _onQuestCondPassCallBack = action;
        }

        public void SetMonitor(LevelConditionMonitor monitor)
        {
            _monitor = monitor;
        }

        public override void OnMonitorExecute()
        {
            _monitor.OnMonitorExecute();
        }

        public override void OnPass()
        {
            _monitor.OnPass();
            _onQuestCondPassCallBack.Invoke(_questId, _questMonitorType);
        }
    }
}