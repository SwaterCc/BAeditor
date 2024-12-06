using Editor.AbilityEditor.TreeItem;
using Hono.Scripts.Battle;

namespace Editor.AbilityEditor
{
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
            switch (node.GetDataRef())
            {
                case ListenerNodeData:
                    item = new EventTreeItem(tree, node);
                    break;
                case BranchNodeData:
                    item = new BranchTreeItem(tree, node);
                    break;
                case VariableNodeData:
                    item = new VariableTreeItem(tree, node);
                    break;
                case AttrNodeData:
                    item = new AttrSetterTreeItem(tree, node);
                    break;
                case RepeatNodeData:
                    item = new RepeatTreeItem(tree, node);
                    break;
                case ActionNodeData:
                    item = new ActionTreeItem(tree, node);
                    break;
                case TimerNodeData:
                    item = new TimerTreeItem(tree, node);
                    break;
                case GroupNodeData:
                    item = new GroupTreeItem(tree, node);
                    break;
            }

            return item;
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