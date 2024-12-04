using Editor.AbilityEditor.TreeItem;
using Hono.Scripts.Battle;

namespace Editor.AbilityEditor
{
    public static class ATreeNodeExtensions
    {
        /// <summary>
        /// 获取Node节点
        /// </summary>
        /// <param name="tree"></param>
        /// <param name="nodeType"></param>
        /// <param name="nodeData"></param>
        /// <returns></returns>
        public static ATreeNode GetNode(AbilityCycleTree tree, EAbilityNodeType nodeType, AbilityNodeData nodeData)
        {
            ATreeNode node = null;
            switch (nodeType)
            {
                case EAbilityNodeType.EEvent:
                    node = new EventTreeNode<EventNodeData>(tree, nodeData);
                    break;
                case EAbilityNodeType.EBranchControl:
                    node = new BranchTreeNode<BranchNodeData>(tree, nodeData);
                    break;
                case EAbilityNodeType.EVariableSetter:
                    node = new VarSetterTreeNode<VariableNodeData>(tree, nodeData);
                    break;
                case EAbilityNodeType.EAttrSetter:
                    node = new AttrSetterTreeNode<AttrNodeData>(tree, nodeData);
                    break;
                case EAbilityNodeType.ERepeat:
                    node = new RepeatTreeNode<RepeatNodeData>(tree, nodeData);
                    break;
                case EAbilityNodeType.EAction:
                    node = new ActionTreeNode<ActionNodeData>(tree, nodeData);
                    break;
                case EAbilityNodeType.ETimer:
                    node = new TimerTreeNode<TimerNodeData>(tree, nodeData);
                    break;
                case EAbilityNodeType.EGroup:
                    node = new GroupTreeNode<GroupNodeData>(tree, nodeData);
                    break;
            }

            return node;
        }

        /// <summary>
        /// 检测是否有指定类型的父节点
        /// </summary>
        /// <param name="tree"></param>
        /// <param name="node"></param>
        /// <param name="checkType"></param>
        /// <returns></returns>
        public static bool CheckHasTypeParent(this AbilityCycleTree tree, ATreeNode node, EAbilityNodeType checkType)
        {
            int parentId = node.NodeData.ParentId;
            while (parentId > 0)
            {
                var parentNode = tree.TreeData.NodeDict[parentId];
                if (parentNode.NodeType == checkType)
                {
                    return true;
                }

                parentId = parentNode.ParentId;
            }

            return false;
        }
    }
}