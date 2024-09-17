using System.Collections.Generic;
using Editor.BattleEditor.AbilityEditor;
using UnityEditor.IMGUI.Controls;
using UnityEngine;

namespace Editor.AbilityEditor
{
    public class FunctionView : TreeView
    {
        public string CurSelect { get; private set; }

        private readonly FuncWindow _window;
        private readonly List<AbilityFunctionHelper.FuncInfo> _funcInfos;

        public FunctionView(TreeViewState state, FuncWindow window, string funcName,
            List<AbilityFunctionHelper.FuncInfo> infos) : base(state)
        {
            _funcInfos = infos;
            _window = window;
            CurSelect = funcName;
            showAlternatingRowBackgrounds = true;
            showBorder = true;
            Reload();
            
            if(string.IsNullOrEmpty(CurSelect))
                return;
            foreach (var item in rootItem.children)
            {
                if (item.displayName == CurSelect)
                {
                    SelectionClick(item, false);
                }
            }
        }
        
        protected override TreeViewItem BuildRoot()
        {
            var root = new TreeViewItem { id = 0, depth = -1, displayName = "Root" };

            int idx = 1;

            foreach (var funcInfo in _funcInfos)
            {
                if(!funcInfo.ShowInEditorView) continue;
                
                var func = new TreeViewItem() { id = ++idx, depth = 0, displayName = funcInfo.FuncName };
                root.AddChild(func);
            }

            return root;
        }

        protected override void SingleClickedItem(int id)
        {
            CurSelect = FindItem(id, rootItem).displayName;
        }

        protected override void DoubleClickedItem(int id)
        {
            var item = FindItem(id, rootItem);
            _window.OnDoubleClick(item.displayName);
        }
    }
}