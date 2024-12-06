#region

using System.Collections.Generic;

#endregion

namespace Hono.Scripts.Battle
{
    public partial class Ability
    {
        private abstract class ANode
        {
            /// <summary>
            /// node配置数据
            /// </summary>
            public AbilityNodeData Data { get; private set; }

            /// <summary>
            /// Ability
            /// </summary>
            public Ability AContext { get; private set; }

            /// <summary>
            /// 周期控制
            /// </summary>
            public AbilityCycle ACycles { get; private set; }

            /// <summary>
            /// 父节点
            /// </summary>
            protected ANode Parent { get; private set; }

            /// <summary>
            /// 子节点
            /// </summary>
            protected readonly List<ANode> Children = new(15);

            /// <summary>
            /// 分支组 bran
            /// </summary>
            private readonly Dictionary<int, bool> _branchGroupState = new(10);

            public void OnRent(Ability ability, AbilityNodeData data)
            {
                AContext = ability;
                ACycles = ability._abilityCycle;
                Data = data;
            }

            /// <summary>
            /// 构建树，递归补全子节点
            /// </summary>
            /// <param name="parent"></param>
            public void Build(in ANode parent)
            {
                //设置父节点
                Parent = parent;
                //填充子节点
                foreach (var nodeId in Data.ChildrenIds)
                {
                    var node = ACycles.GetNode(AContext, AContext.Data.NodeDict[nodeId]);
                    Children.Add(node);
                    node.Build(this);

                    if (node is ABranchNode branchNode)
                    {
                        _branchGroupState.TryAdd(branchNode.Data.BranchGroupId, false);
                    }
                }
            }

            /// <summary>
            /// 执行节点功能
            /// </summary>
            public abstract void DoJob();

            /// <summary>
            /// 执行子节点功能
            /// </summary>
            protected void DoChildrenJob()
            {
                foreach (var node in Children)
                {
                    if (node is ABranchNode branchNode)
                    {
                        var ifGroupId = branchNode.Data.BranchGroupId;
                        if (_branchGroupState.TryGetValue(ifGroupId, out var hasSuccess) && hasSuccess)
                        {
                            continue;
                        }
                    }

                    //Debug.Log($"[Ability] AbilityId:{_executor.AbilityData.ConfigId} nodeId {NodeId} NodeType {NodeType} DoChildrenJob Next Node is {node.NodeType} : {node.NodeId}");
                    node.DoJob();
                }

                OnChildrenJobFinish();
            }

            /// <summary>
            /// 该节点的子节点全部执行完了
            /// </summary>
            protected virtual void OnChildrenJobFinish() { }


            /// <summary>
            /// 返回子节点中branchGroup的状态
            /// </summary>
            /// <returns></returns>
            public bool GetBranchGroupState(int branchGroupId)
            {
                return _branchGroupState.GetValueOrDefault(branchGroupId, false);
            }

            /// <summary>
            /// 设置组通过
            /// </summary>
            /// <param name="branchGroupId"></param>
            public void SetBranchGroupPass(int branchGroupId)
            {
                _branchGroupState[branchGroupId] = true;
            }

            /// <summary>
            /// 重置该节点，注意，同时该节点会被重置为未执行过
            /// </summary>
            protected void Reset()
            {
                onReset();
                _branchGroupState.Clear();
                resetChildren();
            }

            protected void resetChildren()
            {
                foreach (var child in Children)
                {
                    child.Reset();
                }
            }

            protected virtual void onReset() { }

            public abstract void Recycle();

            public void OnRecycle()
            {
                Data = null;
                AContext = null;
                ACycles = null;
                Parent = null;
                foreach (var child in Children)
                {
                    child.Recycle();
                }

                Children.Clear();
                _branchGroupState.Clear();
            }
        }

        private abstract class ANode<TNodeData> : ANode where TNodeData : AbilityNodeData
        {
            public new readonly TNodeData Data;

            protected ANode()
            {
                Data = (TNodeData)base.Data;
            }
        }
    }
}