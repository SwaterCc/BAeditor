using System;
using System.Collections.Generic;
using Sirenix.Utilities.Editor;
using UnityEditor;
using UnityEditor.IMGUI.Controls;
using UnityEngine;
using Object = System.Object;

namespace BattleAbility.Editor
{
    public class LogicTreeViewItem : TreeViewItem
    {
        public BattleAbilitySerializableTree.TreeNode TreeNode;
        public Action<Rect> DrawItem;

        public LogicTreeViewItem(int id, int depth, string name) : base(id, depth, name)
        {
            DrawItem = rect =>
            {
                Debug.Log("asdas");
            };
    }

        public LogicTreeViewItem(BattleAbilitySerializableTree.TreeNode treeNode) : base(treeNode.nodeId,
            treeNode.depth)
        {
            TreeNode = treeNode;

            switch (TreeNode.eNodeType)
            {
                case ENodeType.Event:
                    DrawItem = drawEventItem;
                    break;
                case ENodeType.Condition:
                    DrawItem = drawConditionItem;
                    break;
                case ENodeType.Action:
                    DrawItem = drawActionItem;
                    break;
                case ENodeType.Variable:
                    DrawItem = drawVariableItem;
                    break;
            }
        }

        private void drawEventItem(Rect lineRect)
        {
            var text = "事件按钮（后续要加事件部分参数预览）";
            treeButton(lineRect, text, Color.cyan, LogicTreeEventNodeWindow.OpenWindow);
        }

        private void drawConditionItem(Rect lineRect)
        {
            var text = "条件按钮（后续要加事件部分参数预览）";
            treeButton(lineRect, text, Color.green, LogicTreeConditionNodeWindow.OpenWindow);
        }

        private void drawActionItem(Rect lineRect)
        {
            var text = "Action节点（后续要加事件部分参数预览）";
            treeButton(lineRect, text, new Color(0.8f, 0f, 0f, 0.8f), LogicTreeActionNodeWindow.OpenWindow);
        }

        private void drawVariableItem(Rect lineRect)
        {
            var text = "创建变量（后续要加事件部分参数预览）";
            treeButton(lineRect, text, new Color(0.46f, 0.4f, 0.8f), LogicTreeVariableNodeWindow.OpenWindow);
        }

        protected void treeButton(Rect lineRect, string btnText, Color btnColor,
            Action<LogicTreeViewItem> action = null,
            float buttonWidth = 250f)
        {
            var bgColor = GUI.backgroundColor;
            GUI.backgroundColor = btnColor;
            lineRect.width = buttonWidth;
            if ( GUI.Button(lineRect, btnText))
            {
                if (Event.current.button == 0)
                {
                    action?.Invoke(this);
                }
            }

            GUI.backgroundColor = bgColor;
        }
    }


    public class LogicTreeView : TreeView
    {
        private readonly BattleAbilitySerializableTree _treeData;
        private TreeViewItem _eventRoot;
        private TreeViewItem _hideRoot;
        public LogicTreeView(TreeViewState state, BattleAbilitySerializableTree treeData) :
            base(state)
        {
            _treeData = treeData;
            showAlternatingRowBackgrounds = true;
            showBorder = true;
            Reload();
        }

        protected override TreeViewItem BuildRoot()
        {
            _hideRoot = new LogicTreeViewItem(0, -1, "root");
            _eventRoot = new LogicTreeViewItem(_treeData.allNodes[_treeData.rootKey]);
            _hideRoot.AddChild(_eventRoot);
            var eventNode = _treeData.allNodes[_treeData.rootKey];
            setupChild(eventNode);
            SetupDepthsFromParentsAndChildren(_hideRoot);
            return _hideRoot;
        }
        
        private void setupChild(BattleAbilitySerializableTree.TreeNode nodeData)
        {
            foreach (var childKey in nodeData.childKeys)
            {
                if (_treeData.allNodes.TryGetValue(childKey, out BattleAbilitySerializableTree.TreeNode childNodeData))
                {
                    var childItem = makeTreeViewItem(childNodeData);
                    _hideRoot.AddChild(childItem);
                }
            }
        }

        private TreeViewItem makeTreeViewItem(BattleAbilitySerializableTree.TreeNode nodeData)
        {
            var item = new LogicTreeViewItem(nodeData);
            if (nodeData.childKeys.Count > 0)
            {
                setupChild(nodeData);
            }

            return item;
        }

        protected override void RowGUI(RowGUIArgs args)
        {
            var item = (LogicTreeViewItem)args.item;
            base.RowGUI(args);
            item.DrawItem(args.rowRect);
        }

        private void AddLogicTreeNode(Object obj)
        {
            if (obj is (LogicTreeViewItem select, ENodeType eNodeType))
            {
                var pair = ((LogicTreeViewItem select, ENodeType eNodeType))obj;
                var node = BattleAbilitySerializableTree.GetNode(_treeData, pair.eNodeType);
                node.parentKey = pair.select.TreeNode.nodeId;
                node.depth = pair.select.TreeNode.depth + 1;
                pair.select.TreeNode.childKeys.Add(node.nodeId);
                Reload();
            }
        }

        protected override void ContextClickedItem(int id)
        {
            if (FindItem(id, _hideRoot) is LogicTreeViewItem select)
            {
                GenericMenu menu = new GenericMenu();
                menu.AddItem(new GUIContent("添加If节点"), false,
                    AddLogicTreeNode, (select, ENodeType.Condition));
                menu.ShowAsContext();
            }
        }
    }
}