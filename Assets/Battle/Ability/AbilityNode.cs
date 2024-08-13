using System;
using System.Linq;
using Battle.Def;

namespace Battle
{
    public partial class Ability
    {
        private abstract class AbilityNode
        {
            public int ConfigId;

            public AbilityNodeData NodeData;

            //该节点是否以及运行过
            public bool JobFinsh;

            public bool CanDeep;
            
            protected AbilityExecutor _executor;

            protected AbilityNode(AbilityExecutor executor, AbilityNodeData data)
            {
                JobFinsh = false;
                NodeData = data;
                ConfigId = NodeData.NodeUid;
                CanDeep = true;
            }

            /// <summary>
            /// 执行节点功能
            /// </summary>
            public abstract void DoJob();

            /// <summary>
            /// 子节点全部执行完后回调
            /// </summary>
            protected virtual void childrenJobFinish() { }

            
            public virtual bool TryGetChildren(out int childIdx)
            {
                childIdx = 0;
                return NodeData.ChildrenUids.Count > 1 && CanDeep;
            }
            
            public virtual bool TryGetBrother(out int brother)
            {
                brother = NodeData.NextIdInSameLevel;
                return NodeData.NextIdInSameLevel > 0;
            }
        }
    }
}