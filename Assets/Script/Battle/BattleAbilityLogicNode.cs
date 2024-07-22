using System.Collections.Generic;

namespace Script.Battle
{
    /// <summary>
    /// 真正的节点
    /// </summary>
    public abstract class BattleAbilityLogicNode
    {
        public enum ENodeType
        {
            /// <summary>
            /// 触发时机节点
            /// </summary>
            TriggerTiming,

            /// <summary>
            /// 逻辑判定语句
            /// </summary>
            Condition,

            /// <summary>
            /// 行为
            /// </summary>
            Action,

            /// <summary>
            /// 创建临时变量
            /// </summary>
            CreateTempField,
        }
        
        /// <summary>
        /// 逻辑数据的抽象基类
        /// </summary>
        public abstract class BattleAbilityLogicNodeData
        {
            
        }

        public BattleAbilityLogicNode(ENodeType nodeType)
        {
            _nodeType = nodeType;
        }
        
        private readonly ENodeType _nodeType;
        protected BattleAbilityLogicNodeData _data;
        
        public BattleAbilityLogicNode.ENodeType GetNodeType() => _nodeType;
        public BattleAbilityLogicNode.BattleAbilityLogicNodeData GetData() => _data;
    }

    /// <summary>
    /// 节点子类创建指定模板
    /// </summary>
    /// <typeparam name="TNodeData"></typeparam>
    public abstract class BattleAbilityLogicNodeTpl<TNodeData> : BattleAbilityLogicNode
        where TNodeData : BattleAbilityLogicNode.BattleAbilityLogicNodeData
    {
        public BattleAbilityLogicNodeTpl(ENodeType nodeType) : base(nodeType)
        {
        }

        protected new TNodeData _data => base._data == null ? null : base._data as TNodeData;
    }
}