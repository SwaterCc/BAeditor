using System;
using System.Linq;
using Battle.Def;

namespace Battle.Ability
{
    public partial class Ability
    {
        private class AbilityNode
        {
            public AbilityNodeData NodeData;

            //该节点是否以及运行过
            public bool IsExecuted;

            private AbilityExecutor _executor;

            public AbilityNode(AbilityExecutor executor)
            {
                IsExecuted = false;
            }

            /// <summary>
            /// 获取下一个节点，返回空说明运行完了
            /// </summary>
            /// <returns></returns>
            /// <exception cref="string"></exception>
            public AbilityNode GetNextNode()
            {
                if (NodeData.ChildrenUids.Count > 1)
                {
                    //有子节点返回第一个子节点
                    if (_executor.TryGetAvailableNode(NodeData.ChildrenUids[0], out var node))
                    {
                        return node;
                    }
                }
                else if (NodeData.NextIdInSameLevel >= 0 &&
                         _executor.TryGetAvailableNode(NodeData.NextIdInSameLevel, out var node))
                {
                    //没有子节点返回自己下一个相邻节点
                    return node;
                }
                else
                {
                    return _executor.GetNode(NodeData.Parent).GetNextNode();
                }

                //走到这里要报错
                throw new Exception("节点执行顺序不对");
            }
        }
    }
}