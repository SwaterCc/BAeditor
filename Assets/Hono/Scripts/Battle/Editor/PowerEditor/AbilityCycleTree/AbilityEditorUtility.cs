using System.Collections.Generic;
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
        AddAttrChild,
        AddVariableChild,
        AddGroupSwitchChild,
        
        AddChildOperation = 50,

        RemoveSelf = 51,
        Copy = 52,
        Paste = 53,

        BaseOperation = 100,

        //额外拓展，动态添加，并不在基础操作中
        GetResult = 200,
        JoinBranchGroup = 300,
    }

    public static class AbilityEditorUtility
    {
        private static readonly Dictionary<int, Dictionary<EAbilityCycle, AEditorTreeHeadNode>> TreeCache = new();
        private static readonly List<AEditorTreeNode> CopyCache = new();

        /// <summary>
        /// 获取Node节点
        /// </summary>
        /// <param name="tree"></param>
        /// <param name="node"></param>
        /// <returns></returns>
        public static ATreeItem CreateTreeItem(AbilityCycleTree tree, AEditorTreeNode node)
        {
            ATreeItem item = null;
            switch (node.Data)
            {
                case ListenerNodeData:
                    return new ListenerTreeItem(tree, node);
                case BranchGroupNodeData:
                    return new BranchGroupTreeItem(tree, node);
                case BranchNodeData:
                    return new BranchTreeItem(tree, node);
                case AttrModifyNodeData:
                    return new AttrModifyTreeItem(tree, node);
                case RepeatNodeData:
                    return new RepeatTreeItem(tree, node);
                case ActionNodeData:
                    return new ActionTreeItem(tree, node);
                case TimerNodeData:
                    return new TimerTreeItem(tree, node);
                case GroupNodeData:
                    return new GroupTreeItem(tree, node);
                case GroupSwitchNodeData:
                    return new GroupSwitchTreeItem(tree, node);
                case VariableNodeData:
                    return new VariableTreeItem(tree, node);
            }

            throw new SwitchExpressionException("不存在该类型的节点");
        }

        /// <summary>
        /// 创建新的树
        /// </summary>
        /// <param name="abilityData"></param>
        /// <param name="cycle"></param>
        /// <returns></returns>
        private static AEditorTreeHeadNode CreateAEditorTree(AbilityData abilityData, EAbilityCycle cycle)
        {
            var headNode = new AEditorTreeHeadNode(abilityData, cycle);
            headNode.Build();
            if (!TreeCache.TryGetValue(abilityData.id, out var cycleDict))
            {
                cycleDict = new Dictionary<EAbilityCycle, AEditorTreeHeadNode>();
                TreeCache.Add(abilityData.id, cycleDict);
            }

            cycleDict[cycle] = headNode;
            return headNode;
        }

        /// <summary>
        /// 获取树缓存，如果不存在则会新建一个缓存
        /// </summary>
        /// <param name="abilityData"></param>
        /// <param name="cycle"></param>
        /// <returns></returns>
        public static AEditorTreeHeadNode GetTreeHead(AbilityData abilityData, EAbilityCycle cycle)
        {
            if (!TryGetExistATreeHead(abilityData, cycle, out var head))
            {
                head = CreateAEditorTree(abilityData, cycle);
            }

            return head;
        }

        /// <summary>
        /// 尝试获取一个已经存在的TreeHead
        /// </summary>
        /// <param name="abilityData"></param>
        /// <param name="cycle"></param>
        /// <param name="head"></param>
        /// <returns></returns>
        public static bool TryGetExistATreeHead(AbilityData abilityData,
            EAbilityCycle cycle,
            out AEditorTreeHeadNode head)
        {
            head = null;
            return TreeCache.TryGetValue(abilityData.id, out var cycleDict) && cycleDict.TryGetValue(cycle, out head);
        }

        /// <summary>
        /// 缓存拷贝项目
        /// </summary>
        /// <param name="copySelectItems"></param>
        public static void SaveCopyItems(List<AEditorTreeNode> copySelectItems)
        {
            foreach (var item in copySelectItems)
            {
                CopyCache.Add(new AEditorTreeNode(item));
            }
        }

        /// <summary>
        /// 清空拷贝
        /// </summary>
        public static void ClearCopyCache()
        {
            CopyCache.Clear();
        }

        /// <summary>
        /// 获取拷贝缓存
        /// </summary>
        /// <returns></returns>
        public static List<AEditorTreeNode> GetCopyItems()
        {
            return CopyCache;
        }

        /// <summary>
        /// 追溯父级类型
        /// </summary>
        /// <param name="item"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static bool HasParent<T>(this ATreeItem item) where T : ATreeItem
        {
            var parent = item.parent;
            while (parent != null)
            {
                if (parent is T)
                {
                    return true;
                }

                if (parent is CycleTreeItem)
                {
                    return typeof(T) == typeof(CycleTreeItem);
                }

                parent = parent.parent;
            }

            return false;
        }
        
        
        /// <summary>
        /// 尝试获取指定类型的第一个父级
        /// </summary>
        /// <param name="item"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static bool TryGetFirstParent<T>(this ATreeItem item,out T parentItem) where T : ATreeItem
        {
            parentItem = null;
            var parent = item.parent;
            while (parent != null)
            {
                if (parent is T)
                {
                    parentItem = (T)parent;
                    return true;
                }

                if (parent is CycleTreeItem)
                {
                    return typeof(T) == typeof(CycleTreeItem);
                }

                parent = parent.parent;
            }

            return false;
        }

        /// <summary>
        /// 判定该节点是否为传入节点的父级
        /// </summary>
        /// <param name="parent"></param>
        /// <param name="child"></param>
        /// <returns></returns>
        public static bool IsParent(this ATreeItem parent, ATreeItem child)
        {
            var checkParent = child.parent;
            while (checkParent != null)
            {
                if (checkParent == parent)
                {
                    return true;
                }

                checkParent = checkParent.parent;
            }

            return false;
        }
    }
}