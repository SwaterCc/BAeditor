using System;
using System.Collections.Generic;
using Editor.BattleEditor.AbilityEditor;
using Sirenix.Utilities.Editor;
using UnityEditor;
using UnityEditor.IMGUI.Controls;
using UnityEngine;

namespace Editor.AbilityEditor
{
    public partial class FuncWindow
    {
        private readonly MultiColumnHeaderState.Column[] _columns =
        {
            new()
            {
                headerContent = new GUIContent("函数名"),
                headerTextAlignment = TextAlignment.Center,
                sortedAscending = true,
                sortingArrowAlignment = TextAlignment.Center,
                width = 200,
                minWidth = 60,
                autoResize = true
            },
            new()
            {
                headerContent = new GUIContent("函数描述"),
                headerTextAlignment = TextAlignment.Center,
                sortedAscending = true,
                sortingArrowAlignment = TextAlignment.Center,
                width = 320,
                minWidth = 120,
                autoResize = true
            },
            new()
            {
                headerContent = new GUIContent("返回值描述"),
                headerTextAlignment = TextAlignment.Center,
                sortedAscending = true,
                sortingArrowAlignment = TextAlignment.Center,
                width = 120,
                minWidth = 120,
                autoResize = true
            },
        };

        private class FunctionTreeItem : TreeViewItem
        {
            public AbilityFuncInfoCache.FuncInfo FuncInfo { get; }

            public FunctionTreeItem(AbilityFuncInfoCache.FuncInfo funcInfo, int idx) : base(idx)
            {
                FuncInfo = funcInfo;
            }
        }


        private class FunctionView : TreeView
        {
            private readonly FuncWindow _window;
            private string _groupName;

            public FunctionView(FuncWindow window, string groupName) : base(
                new TreeViewState(), new MultiColumnHeader(new MultiColumnHeaderState(window._columns)))
            {
                _window = window;
                showAlternatingRowBackgrounds = true;
                showBorder = true;
                _groupName = groupName;
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
                    var func = new TreeViewItem() { id = -1, depth = 0, displayName = "(None)" };
                    root.AddChild(func);
                    return root;
                }

                foreach (var funcInfo in funcInfos)
                {
                    if (!funcInfo.ShowInEditorView) continue;
                    if (!_window._filters.Filter(funcInfo.ReturnType))
                    {
                        continue;
                    }

                    var func = new FunctionTreeItem(funcInfo, ++idx);
                    root.AddChild(func);
                }

                if (root.children != null && root.children.Count != 0) return root;
                {
                    var func = new TreeViewItem() { id = -1, depth = 0, displayName = "(None)" };
                    root.AddChild(func);
                    return root;
                }
            }

            protected override void RowGUI(RowGUIArgs args)
            {
                if (args.item is not FunctionTreeItem functionTreeItem)
                {
                    base.RowGUI(args);
                    return;
                }

                GUI.Label(args.GetCellRect(0),"    " + functionTreeItem.FuncInfo.FuncName,
                          new GUIStyle(EditorStyles.label) { alignment = TextAnchor.MiddleLeft,fontStyle = FontStyle.Bold});
                GUI.Label(args.GetCellRect(1), "    " + functionTreeItem.FuncInfo.FuncDesc,
                          new GUIStyle(EditorStyles.label) { alignment = TextAnchor.MiddleLeft });
                GUI.Label(args.GetCellRect(2), "    " + functionTreeItem.FuncInfo.FuncReturnDesc,
                          new GUIStyle(EditorStyles.label) { alignment = TextAnchor.MiddleLeft });
            }

            protected override void DoubleClickedItem(int id)
            {
                if (FindItem(id, rootItem) is FunctionTreeItem functionTreeItem)
                {
                    _window.ChangeSelectFunction(functionTreeItem.FuncInfo.FuncName);
                }
            }
        }
    }
}