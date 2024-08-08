using Battle.Def;
using Battle.Tools;

namespace Battle.Ability
{
    /// <summary>
    /// Ability 结合了GAS的GA，GE两套东西，做一下尝试
    /// 这个Ability代表了运行时流程管理
    /// </summary>
    public class Ability
    {
        //基础数据

        /// <summary>
        /// 能力运行时唯一识别id
        /// </summary>
        public int Uid { get; }

        /// <summary>
        /// 能力是否属于激活状态
        /// </summary>
        public bool IsActive { get; }
        
        /// <summary>
        /// 编辑器数据
        /// </summary>
        public AbilityData Data { get; }
        
        /// <summary>
        /// cd计时器，视情况初始化
        /// </summary>
        private ScheduleTimer _cdTimer;

        /// <summary>
        /// 当前状态
        /// </summary>
        public EAbilityState State = EAbilityState.UnInit;
        
        //生命周期

        /// <summary>
        /// 初始化
        /// </summary>
        public void Init()
        {
            State = EAbilityState.Init;
        }
        
        /// <summary>
        /// 赋予能力时检测
        /// </summary>
        /// <returns></returns>
        public bool PreGiveCheck()
        {
            bool checkRes = true;
            return checkRes;
        }
        
        /// <summary>
        /// 检测能力使用条件
        /// </summary>
        public bool CheckCondition()
        {
            bool checkRes = true;
            return checkRes;
        }

        /// <summary>
        /// 能力启动前
        /// </summary>
        public void PreExecute()
        {
            
        }

        /// <summary>
        /// 能力执行逻辑
        /// </summary>
        public void Executing()
        {
            
        }

        /// <summary>
        /// 能力结束
        /// </summary>
        public void EndExecute()
        {
            
        }
        
       
        
        /// <summary>
        /// 注册的事件触发时
        /// </summary>
        public void OnEventFire()
        {
            
        }
        
        
    }
}