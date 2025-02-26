using System;

namespace Hono.Scripts.Battle.Core
{
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
        
        protected abstract void OnPass();
    }
}