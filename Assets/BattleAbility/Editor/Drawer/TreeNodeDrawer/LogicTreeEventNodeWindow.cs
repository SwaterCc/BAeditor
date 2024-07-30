using Sirenix.OdinInspector.Editor;
using Sirenix.Utilities;
using Sirenix.Utilities.Editor;
using UnityEditor.Graphs;
using UnityEngine;

namespace BattleAbility.Editor
{
    public class LogicTreeEventNodeWindow : OdinEditorWindow
    {
        private BattleAbilitySerializableTree.TreeNode _treeNode;
        
        public static void OpenWindow(LogicTreeViewItem treeViewItem)
        {
            var window = GetWindow<LogicTreeEventNodeWindow>("Event节点配置");
            window.position = GUIHelper.GetEditorWindowRect().AlignCenter(400, 600);
            
        }
        
        protected override void OnEnable()
        {
            base.OnEnable();
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
        }
    }
}