using System.Runtime.CompilerServices;
using Editor.AbilityEditor.TreeItem;
using Hono.Scripts.Battle;

namespace Editor.AbilityEditor
{
    
    /// <summary>
    /// 右键操作类型,注意分段，分段是有意义的
    /// </summary>
    public enum ERightClickOperationType
    {
        AddActionChild = 1,
        AddBranchGroupChild,
        AddBranchChild,
        AddListenerChild,
        AddGroupChild,
        AddTimerChild,
        AddRepeatChild,
        AddVariableChild,
        AddAttrChild,
        
        AddChildLimit = 100,
        
        RemoveSelf = 101,
        Copy = 102,
        Paste = 103,
        
        GetResult = 200,
        JoinBranchGroup = 300,
        Other,
    }
    
    public static class ATreeItemExtensions
    {
        /// <summary>
        /// 获取Node节点
        /// </summary>
        /// <param name="tree"></param>
        /// <param name="node"></param>
        /// <returns></returns>
        public static ATreeItem CreateTreeItem(AbilityCycleTree tree, ATreeEditorNode node)
        {
            ATreeItem item = null;
            switch (node.Data)
            {
                case CycleNodeData:
                    return new CycleTreeItem(tree, node);
                case ListenerNodeData:
                    return new EventTreeItem(tree, node);
                case BranchNodeData:
                    return new BranchTreeItem(tree, node);
                case VariableNodeData:
                    return new VariableTreeItem(tree, node);
                case AttrNodeData:
                    return new AttrSetterTreeItem(tree, node);
                case RepeatNodeData:
                    return new RepeatTreeItem(tree, node);
                case ActionNodeData:
                    return new ActionTreeItem(tree, node);
                case TimerNodeData:
                    return new TimerTreeItem(tree, node);
                case GroupNodeData:
                    return new GroupTreeItem(tree, node);
            }

            throw new SwitchExpressionException("不存在该类型的节点");
        }

        /*/// <summary>
        /// 检测是否有指定类型的父节点
        /// </summary>
        /// <param name="tree"></param>
        /// <param name="item"></param>
        /// <param name="checkType"></param>
        /// <returns></returns>
        public static bool CheckHasTypeParent(this AbilityCycleTree tree, ATreeItem item, EAbilityNodeType checkType)
        {
            var parent = item.EditorNode.GetParent();
            while (parent!=null)
            {
                var parentNode = tree.TreeData.NodeDict[parentId];
                if (parent.NodeType == checkType)
                {
                    return true;
                }

                parent = parentNode.ParentId;
            }

            return false;
        }*/
    }
}