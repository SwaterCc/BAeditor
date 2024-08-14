using System;
using System.Collections.Generic;
using Battle.Def;
using Battle.Tools;

namespace Battle
{
    /// <summary>
    /// Ability 结合了GAS的GA，GE两套东西，做一下尝试
    /// 这个Ability代表了运行时流程管理
    /// </summary>
    public partial class Ability : IVariableCollectionBind
    {
        /// <summary>
        /// 能力的上下文，存储当前在运行哪个能力
        /// </summary>
        private static AbilityRuntimeContext _context;

        public static AbilityRuntimeContext Context => _context;

        public static void InitContext()
        {
            _context = new AbilityRuntimeContext();
        }

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
        private AbilityData _abilityData;

        private int _abilityConfigId;

        /// <summary>
        /// cd计时器，视情况初始化
        /// </summary>
        private ScheduleTimer _cdTimer;

        /// <summary>
        /// 执行者
        /// </summary>
        private readonly AbilityExecutor _executor;

        /// <summary>
        /// 属于Ability的变量
        /// </summary>
        private readonly VariableCollection _variables;

        public VariableCollection GetVariableCollection() => _variables;

        /// <summary>
        /// 检测类
        /// </summary>
        private readonly Dictionary<EAbilityCycleType, AbilityChecker> _checkers;

        /// <summary>
        /// 周期类
        /// </summary>
        private readonly AbilityState _state;

        public Ability(int abilityConfigId)
        {
            _abilityConfigId = abilityConfigId;
            _abilityData = AbilityDataCacheMgr.Instance.GetAbilityData(_abilityConfigId);
            _variables = new VariableCollection(16, this);
            _executor = new AbilityExecutor(this);

            _checkers = new Dictionary<EAbilityCycleType, AbilityChecker>()
            {
                { EAbilityCycleType.OnPreAwardCheck, new PreAwardChecker(this) },
                { EAbilityCycleType.OnPreExecuteCheck, new PreExecuteCheck(this) },
            };

            _state = new AbilityState(this);
            
            _executor.Setup();
        }

        public void Reload()
        {
            //终止能力运行
            _state.Stop();

            //卸载加载好的节点
            _executor.UnInstall();

            //重新获取数据
            _abilityData = AbilityDataCacheMgr.Instance.GetAbilityData(_abilityConfigId);
            _executor.Setup();
        }

        public void OnTick(float dt)
        {
            _state.Tick(dt);
        }
    }
}