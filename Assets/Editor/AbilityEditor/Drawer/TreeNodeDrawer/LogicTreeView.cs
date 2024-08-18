using System;
using System.Collections.Generic;
using Battle.Def;
using UnityEditor;
using UnityEditor.IMGUI.Controls;
using UnityEngine;
using Object = System.Object;

namespace BattleAbility.Editor
{
    
    public class LogicTreeView : TreeView
    {
        private readonly BattleAbilitySerializableTree _treeData;
        private LogicTreeViewItem _eventRoot;
        private LogicTreeViewItem _hideRoot;

        public LogicTreeView(TreeViewState state, BattleAbilitySerializableTree treeData) :
            base(state)
        {
            _treeData = treeData;
            showAlternatingRowBackgrounds = true;
            showBorder = true;
            extraSpaceBeforeIconAndLabel = 30;
            rowHeight = 36;

            Reload();
        }

        protected override TreeViewItem BuildRoot()
        {
            _hideRoot = new TreeViewItem(0, -1, "root");
            _eventRoot = new LogicTreeViewItem(_treeData.allNodes[_treeData.rootKey]);
            _hideRoot.AddChild(_eventRoot);
            var eventNode = _treeData.allNodes[_treeData.rootKey];
            setupChild(eventNode, _eventRoot);
            SetupDepthsFromParentsAndChildren(_hideRoot);

            return _hideRoot;
        }

        private void setupChild(BattleAbilitySerializableTree.TreeNode nodeData, LogicTreeViewItem parent)
        {
            foreach (var childKey in nodeData.childKeys)
            {
                if (_treeData.allNodes.TryGetValue(childKey, out BattleAbilitySerializableTree.TreeNode childNodeData))
                {
                    var childItem = new LogicTreeViewItem(childNodeData);
                    parent.AddChild(childItem);
                    if (nodeData.childKeys.Count > 0)
                    {
                        setupChild(childNodeData, childItem);
                    }
                }
            }
        }

        protected override void RowGUI(RowGUIArgs args)
        {
            var item = (LogicTreeViewItem)args.item;
            base.RowGUI(args);
            var rowRect = args.rowRect;
            float labelWidth = 24;
            rowRect.x = rowRect.x + 25 + 38 * item.TreeNode.depth;
            if (item.TreeNode.depth > 0)
            {
                GUIStyle customStyle = new GUIStyle(GUI.skin.label);
                // 设置字体大小
                customStyle.fontSize = 44;
                // 设置字体颜色
                customStyle.normal.textColor = new Color(0.9f, 0.8f, 0.6f);
                rowRect.y -= 5;
                EditorGUI.LabelField(rowRect, "∟", customStyle);
                rowRect.y += 5;
                rowRect.x += labelWidth;
            }

            item.DrawItem(rowRect);
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
                menu.AddItem(new GUIContent("添加Action节点"), false,
                    AddLogicTreeNode, (select, ENodeType.Action));
                menu.AddItem(new GUIContent("添加If节点"), false,
                    AddLogicTreeNode, (select, ENodeType.Condition));
                menu.AddItem(new GUIContent("创建变量"), false,
                    AddLogicTreeNode, (select, ENodeType.Variable));
                menu.ShowAsContext();
            }
        }

        /// <summary>
        /// 设置拖拽规则
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        protected override bool CanStartDrag(CanStartDragArgs args)
        {
            var draggedItem = args.draggedItem as LogicTreeViewItem;
            return draggedItem.TreeNode.eNodeType != ENodeType.Event;
        }

        /// <summary>
        /// 实现拖拽操作
        /// </summary>
        /// <param name="args"></param>
        protected override void SetupDragAndDrop(SetupDragAndDropArgs args)
        {
            if (hasSearch) return;

            DragAndDrop.PrepareStartDrag();
            var draggedRows = new List<LogicTreeViewItem>(16);
            foreach (var item in GetRows())
            {
                if (args.draggedItemIDs.Contains(item.id))
                {
                    draggedRows.Add(item as LogicTreeViewItem);
                }
            }

            DragAndDrop.SetGenericData("MyDragging", draggedRows);
            DragAndDrop.objectReferences = new UnityEngine.Object[] { };
            DragAndDrop.StartDrag("Dragging TreeViewItem");
        }

        protected override DragAndDropVisualMode HandleDragAndDrop(DragAndDropArgs args)
        {
            var objRefs = DragAndDrop.objectReferences;
            var genericData = DragAndDrop.GetGenericData("MyDragging");

            if (args.parentItem == null || genericData == null)
            {
                return DragAndDropVisualMode.None;
            }

            if (args.performDrop)
            {
                var draggedItems = (List<LogicTreeViewItem>)genericData;
                foreach (var treeViewItem in draggedItems)
                {
                    var parentItem = (LogicTreeViewItem)args.parentItem;
                    if (!parentItem.hasChildren)
                    {
                        parentItem.children = new List<TreeViewItem>();
                    }

                    var draggedItemId = treeViewItem.TreeNode.nodeId;
                    var parentData = _treeData.allNodes[treeViewItem.TreeNode.parentKey];
                    //处理TreeVIewItem
                    if (treeViewItem.parent != parentItem)
                    {
                        treeViewItem.parent.children.Remove(treeViewItem);
                        treeViewItem.parent = parentItem;

                        parentData.childKeys.Remove(draggedItemId);
                        parentData = parentItem.TreeNode;

                        treeViewItem.TreeNode.parentKey = parentItem.TreeNode.nodeId;
                    }
                    else
                    {
                        //更新顺序
                        parentItem.children.Remove(treeViewItem);
                        parentData.childKeys.Remove(draggedItemId);
                    }

                    treeViewItem.UpdateDepth(_treeData);

                    parentItem.children.Insert(args.insertAtIndex < 0 ? 0 : args.insertAtIndex, treeViewItem);
                    parentData.childKeys.Insert(args.insertAtIndex < 0 ? 0 : args.insertAtIndex, draggedItemId);
                }

                Reload();
                return DragAndDropVisualMode.Move;
            }

            return DragAndDropVisualMode.Move;
        }
    }
}