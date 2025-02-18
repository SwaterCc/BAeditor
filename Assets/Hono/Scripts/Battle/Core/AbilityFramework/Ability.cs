#region

using System;
using Hono.Scripts.Battle.Core;
using UnityEngine;

#endregion

namespace Hono.Scripts.Battle.AbilityFramework
{
    /// <summary>
    /// 这个Ability代表了运行时流程管理
    /// </summary>
    public sealed partial class Ability : IGPoolObject
    {
        /// <summary>
        /// 基础数据配置Id
        /// </summary>
        public int Id { get; private set; }

        /// <summary>
        /// 该能力属于哪个Actor
        /// </summary>
        public Unit Unit { get; private set; }

        /// <summary>
        /// 所属驱动器
        /// </summary>
        public AbilityDriver Driver { get; private set; }

        /// <summary>
        /// Ability数据
        /// </summary>
        public AbilityData Data { get; private set; }

        /// <summary>
        /// 属于Ability的变量
        /// </summary>
        public VariableBoard VariableBoard { get; }

        /// <summary>
        /// Ability函数定义
        /// </summary>
        private readonly AFunctionDefine _functionDefine;

        /// <summary>
        /// 周期管控
        /// </summary>
        private readonly AbilityCycle _abilityCycle;

        /// <summary>
        /// 周期结束时指令缓存
        /// </summary>
        private readonly CmdCollection<AttrCommand> _cycleCmdCollection;

        /// <summary>
        /// 周期结束时指令缓存
        /// </summary>
        private readonly CmdCollection<AttrCommand> _abilityCmdCollection;

        /// <summary>
        /// Ability执行结束时触发的事件
        /// </summary>
        public event Action ExecuteEndCallBack;

        /// <summary>
        /// 逻辑帧时间缩放系数
        /// </summary>
        public float TimeScaleFactory { get; set; }

        public Ability()
        {
            if (!AbilityEnv.IsEnvInit)
            {
                Debug.LogWarning("AbilityEnv not init When Game Begin");
                return;
            }

            _abilityCycle = new AbilityCycle(this);
            _functionDefine = new AFunctionDefine(this);
            _cycleCmdCollection = new CmdCollection<AttrCommand>();
            _abilityCmdCollection = new CmdCollection<AttrCommand>();

            VariableBoard = new VariableBoard();
        }

        /// <summary>
        /// 初始化
        /// </summary>
        /// <param name="unit"></param>
        /// <param name="driver"></param>
        /// <param name="configId"></param>
        /// <returns></returns>
        public bool Init(Unit unit, AbilityDriver driver, in int configId)
        {
            Unit = unit;
            Driver = driver;

            Data = AssetManager.Instance.GetData<AbilityData>(configId);
            if (Data == null)
            {
                Debug.LogError($"加载Ability {configId} 失败");
                return false;
            }

            Id = Data.id;
            TimeScaleFactory = 1;
            _abilityCycle.Init();
            return true;
        }

        /// <summary>
        /// 初始化
        /// </summary>
        /// <param name="unit"></param>
        /// <param name="driver"></param>
        /// <param name="abilityData"></param>
        /// <returns></returns>
        public bool Init(Unit unit, AbilityDriver driver, AbilityData abilityData)
        {
            Unit = unit;
            Driver = driver;
            Data = abilityData;
            if (abilityData == null)
            {
                Debug.LogError($"加载Ability 传入了空的abilityData 失败");
                return false;
            }

            Id = Data.id;
            TimeScaleFactory = 1;
            _abilityCycle.Init();
            return true;
        }


        /// <summary>
        /// 执行Ability的逻辑
        /// </summary>
        /// <param name="isReplay"></param>
        public void Execute(bool isReplay)
        {
            if (_abilityCycle.CurState == EAbilityCycle.Executing)
            {
                //正在执行中
                if (isReplay)
                    _abilityCycle.ForceStop();
            }

            _abilityCycle.Execute();
        }

        /// <summary>
        /// 停止，当帧执行
        /// </summary>
        public void Stop()
        {
            _abilityCycle.ForceStop();
        }

        /// <summary>
        /// 重加载，Debug模式下运行
        /// </summary>
        public void Reload()
        {
            //终止能力运行
            _abilityCycle.ForceStop();

            //指令撤销
            _cycleCmdCollection.Clear();
            _abilityCmdCollection.Clear();

            //清理变量
            VariableBoard.Clear();

            //重新加载数据
            Data = AssetManager.Instance.GetData<AbilityData>(Id);
            if (Data == null)
            {
                Debug.LogError($"Reload Ability {Id} 失败");
                return;
            }

            //重新加载
            _abilityCycle.Init();
        }

        public void OnTick(float dt)
        {
            float timeFactory = Mathf.Clamp(TimeScaleFactory, 0, 5);
            _abilityCycle.Tick(dt * timeFactory);
        }

        public bool TryGetLocalVariable<T>(string key, out T value)
        {
            var curNode = _abilityCycle.CurCallFuncNode;
            value = default;
            if (curNode == null)
            {
                return false;
            }

            var node = curNode;
            while (node is not ACycleNode)
            {
                if (node is ILocalVariableBoardHandle variableBoardHandle)
                {
                    if (variableBoardHandle.LocalVariableBoard.TryGet(key, out value))
                    {
                        return true;
                    }
                }
                node = node.Parent;
            }

            return false;
        }
        
        public bool TryGetLocalVariableRef(string key, out object value)
        {
            var curNode = _abilityCycle.CurCallFuncNode;
            value = default;
            if (curNode == null)
            {
                return false;
            }

            var node = curNode;
            while (node is not ACycleNode)
            {
                if (node is ILocalVariableBoardHandle variableBoardHandle)
                {
                    if (variableBoardHandle.LocalVariableBoard.TryGetRef(key, out value))
                    {
                        return true;
                    }
                }
                node = node.Parent;
            }

            return false;
        }

        public void OnRecycle()
        {
            //周期停止
            _abilityCycle.ForceStop();
            _abilityCycle.OnRecycle();

            //指令撤销
            _cycleCmdCollection.Clear();
            _abilityCmdCollection.Clear();

            //数据重置
            Id = 0;
            Unit = null;
            Driver = null;
            Data = null;
            //重置时间系数
            TimeScaleFactory = 1;
            VariableBoard.Clear();
        }
    }

    public static class AbilityDebug
    {
        private static string AbilityInfo(this Ability ability)
        {
            return $"[Ability] Id:{ability.Id} ActorUid:{ability.Unit.Uid} ";
        }

        public static void Log(this Ability ability, in string pattern, params object[] args)
        {
#if _ABILITY_DEBUG_
			Debug.Log(string.Format(AbilityInfo(ability) + pattern, args));
#endif
            // Debug.Log(string.Format(AbilityInfo(ability) + pattern, args));
        }

        public static void LogError(this Ability ability, in string pattern, params object[] args)
        {
#if _ABILITY_DEBUG_
			Debug.LogError(string.Format(AbilityInfo(ability) + pattern, args));
#endif
            // Debug.LogError(string.Format(AbilityInfo(ability) + pattern, args));
        }
    }
}