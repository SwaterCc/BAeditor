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
        
        /// <summary>
        /// 用于执行能力节点序列
        /// </summary>
        private class AbilityExecutor
        {
            private readonly Ability _ability;
            private AbilityNode _curNode;
            /// <summary>
            /// 节点的配置ID-节点对象
            /// </summary>
            private Dictionary<int, AbilityNode> _nodes;

            public AbilityExecutor(Ability ability)
            {
                _ability = ability;
            }

            public void Setup()
            {
                var nodeDict = _ability._abilityData.NodeDict;
                if (nodeDict is { Count: > 0 })
                {
                    _nodes = new();
                    //数据转化为实际逻辑节点
                    foreach (var pair in nodeDict)
                    {
                        
                    }
                }
            }

            public void UnInstall()
            {
            }

            public void Tick()
            {
                if (_curNode == null)
                {
                    return;
                }
                
                
                
            }
            
            public AbilityNode GetNode(int id)
            {
                return _nodes[id];
            }
            
            public void RunNode(AbilityNode node)
            {
               
            }
            
            /// <summary>
            /// 获取下一个节点，返回空说明运行完了
            /// </summary>
            /// <returns></returns>
            /// <exception cref="string"></exception>
            public AbilityNode GetNextNode(AbilityNode node)
            {
                if (node.TryGetChildren(out var next)) 
                {
                    //有子节点返回第一个子节点
                    return _nodes[next];
                }

                if (node.TryGetBrother(out next))
                {
                    //没有子节点返回自己下一个相邻节点
                    return _nodes[next];
                }

                return GetNextNode(_nodes[node.NodeData.Parent]);

                //走到这里要报错
                throw new Exception("节点执行顺序不对");
            }
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
        /// 当前状态
        /// </summary>
        public EAbilityState State = EAbilityState.UnInit;

        /// <summary>
        /// 执行者
        /// </summary>
        private readonly AbilityExecutor _executor;
        
        /// <summary>
        /// 属于Ability的变量
        /// </summary>
        private readonly VariableCollection _variables;
        public VariableCollection GetVariableCollection() => _variables;
        
        public Ability(int abilityConfigId)
        {
            _abilityConfigId = abilityConfigId;
            _abilityData = AbilityDataCacheMgr.Instance.GetAbilityData(_abilityConfigId);
            _variables = new VariableCollection(16, this);
            _executor = new AbilityExecutor(this);
            _executor.Setup();
        }
        
        public void Reload()
        {
            //终止能力运行
            State = EAbilityState.AbilityReady;

            //卸载加载好的节点
            _executor.UnInstall();

            //重新获取数据
            _abilityData = AbilityDataCacheMgr.Instance.GetAbilityData(_abilityConfigId);
            _executor.Setup();
        }
        
        //生命周期

        /// <summary>
        /// 初始化
        /// </summary>
        public void Init()
        {
            State = EAbilityState.Init;
            //初始化变量
            //创建节点对象
           
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
            _executor.Tick();
        }

        /// <summary>
        /// 能力结束
        /// </summary>
        public void EndExecute()
        {
            _variables.Clear();
        }


        /// <summary>
        /// 注册的事件触发时
        /// </summary>
        public void OnEventFire()
        {
            
        }

      
    }
}