using System;

namespace Hono.Scripts.Battle.Core
{
    /// <summary>
    /// 运行监控器Tick
    /// </summary>
    public interface IMonitorTickEnable
    {
        public void Tick(float dt);
    }

    /// <summary>
    /// 关卡监控器
    /// </summary>
    public abstract class LevelConditionMonitor
    {
        /// <summary>
        /// 检测器开始执行
        /// </summary>
        public abstract void OnMonitorExecute();

        private bool _isPass;

        /// <summary>
        /// 是否通过检测
        /// </summary>
        public bool IsPass => _isPass;

        /// <summary>
        /// 设置为通过
        /// </summary>
        public void SetPass()
        {
            OnPass();
            _isPass = true;
        }

        public abstract void OnPass();

        /// <summary>
        /// 转化为任务用的条件检测器
        /// </summary>
        /// <param name="questId"></param>
        /// <param name="stateType"></param>
        /// <param name="action"></param>
        /// <returns></returns>
        public QuestCondMonitor AsQuestCondMonitor(int questId, EQuestStateType stateType, Action<int, EQuestStateType> action)
        {
            var questCondMonitor = new QuestCondMonitor(questId, stateType);
            questCondMonitor.SetMonitor(this);
            questCondMonitor.SetPassAction(action);
            return questCondMonitor;
        }
    }
}