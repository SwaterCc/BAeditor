using System.Collections.Generic;
using Hono.Scripts.Battle;

namespace Editor.AbilityEditor
{
    public static class AbilityCycleTreeUtility
    {
        private static Dictionary<int, Dictionary<EAbilityCycle, ATreeEditorNode>> _treeCache = new();
        private static List<ATreeEditorNode> _copyCache = new();
        private static ATreeEditorNode CreateAEditorTree(AbilityData abilityData, EAbilityCycle cycle)
        {
            var headNodeId = abilityData.HeadNodeDict[cycle];
            var headNode = new ATreeEditorNode(abilityData.NodeDict[headNodeId]);
            headNode.Build(abilityData, new ATreeEditorNode.IdGenerator());
            if (!_treeCache.TryGetValue(abilityData.id, out var cycleDict))
            {
                cycleDict = new Dictionary<EAbilityCycle, ATreeEditorNode>();
                _treeCache.Add(abilityData.id, cycleDict);
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
        public static ATreeEditorNode GetTreeHead(AbilityData abilityData, EAbilityCycle cycle)
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
            out ATreeEditorNode head)
        {
            head = null;
            return _treeCache.TryGetValue(abilityData.id, out var cycleDict) && cycleDict.TryGetValue(cycle, out head);
        }

        public static void SaveCopyItems(List<ATreeEditorNode> copySelectItems)
        {
            foreach (var item in copySelectItems)
            {
                _copyCache.Add(new ATreeEditorNode(item));
            }
        }

        public static void ClearCopyCache()
        {
            _copyCache.Clear();
        }

        public static List<ATreeEditorNode>  GetCopyItems()
        {
            return _copyCache;
        }
    }
}