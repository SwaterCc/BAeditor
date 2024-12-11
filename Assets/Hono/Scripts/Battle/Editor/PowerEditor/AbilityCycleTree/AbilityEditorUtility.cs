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
        AddVariableChild,
        AddAttrChild,
        
        AddChildLimit = 100,
        
        RemoveSelf = 101,
        Copy = 102,
        Paste = 103,
        
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
                case VariableNodeData:
                    return new VariableTreeItem(tree, node);
                case AttrNodeData:
                    return new AttrTreeItem(tree, node);
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

        public static void SaveCopyItems(List<AEditorTreeNode> copySelectItems)
        {
            foreach (var item in copySelectItems)
            {
                CopyCache.Add(new AEditorTreeNode(item));
            }
        }

        public static void ClearCopyCache()
        {
            CopyCache.Clear();
        }

        public static List<AEditorTreeNode> GetCopyItems()
        {
            return CopyCache;
        }
    }
}