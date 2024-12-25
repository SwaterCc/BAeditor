using System.Collections.Generic;
using Editor.BattleEditor.AbilityEditor;
using UnityEditor.IMGUI.Controls;
using UnityEngine;

namespace Editor.AbilityEditor
{
    public class FunctionView : TreeView
    {
        private readonly FuncWindow _window;
        private string _groupName;

        public FunctionView(FuncWindow window) : base(new TreeViewState())
        {
            _window = window;
            showAlternatingRowBackgrounds = true;
            showBorder = true;
            Reload();
        }

        public void ChangeFunctionGroup(string groupName)
        {
            if (groupName == _groupName)
            {
                return;
            }

            _groupName = groupName;
            Reload();
        }
        
        protected override TreeViewItem BuildRoot()
        {
            var root = new TreeViewItem { id = 0, depth = -1, displayName = "Root" };

            int idx = 1;

            if (!AbilityFuncInfoCache.TryGetFuncGroup(_groupName, out var funcInfos))
            {
                return root;
            }
            
            foreach (var funcInfo in funcInfos)
            {
                if(!funcInfo.ShowInEditorView) continue;
                
                var func = new TreeViewItem() { id = ++idx, depth = 0, displayName = funcInfo.FuncName };
                root.AddChild(func);
            }

            return root;
        }

        protected override void DoubleClickedItem(int id)
        {
            var item = FindItem(id, rootItem);
            _window.OnDoubleClick(item.displayName);
        }
    }
}