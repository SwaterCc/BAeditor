using System;
using System.Collections.Generic;
using Battle;
using Editor.AbilityEditor.TreeItem;
using UnityEditor;
using UnityEditor.IMGUI.Controls;
using UnityEngine;

namespace Editor.AbilityEditor
{
    public class EmptyItem : AbilityLogicTreeItem
    {
        public EmptyItem(int id, int depth, string name) : base(id, depth, name) { }

        public override void DrawItem(Rect lineRect) { }

        protected override Color getButtonColor()
        {
            return Color.black;
        }

        protected override string getButtonText()
        {
            return null;
        }

        protected override void OnBtnClicked() { }
    }

    public class AbilityLogicTreeDrawer : TreeView
    {
        private AbilityNodeData _cycleHeadData;
        private AbilityData _data;

        public AbilityLogicTreeDrawer(TreeViewState state, AbilityData data, AbilityNodeData head) : base(state)
        {
            _cycleHeadData = head;
            _cycleHeadData.Depth = -1;
            _data = data;
            showAlternatingRowBackgrounds = true;
            showBorder = true;
            extraSpaceBeforeIconAndLabel = 30;
            rowHeight = 36;
            Reload();
        }

        protected override TreeViewItem BuildRoot()
        {
            var root = new EmptyItem(0, -1, "root");

            var cycleRoot = new CycleTreeItem(_cycleHeadData);
            cycleRoot.depth = 0;
            root.AddChild(cycleRoot);
            setupChild(_cycleHeadData, cycleRoot);

            SetupDepthsFromParentsAndChildren(root);
            return root;
        }

        private void setupChild(AbilityNodeData nodeData, AbilityLogicTreeItem parent)
        {
            foreach (var childId in nodeData.ChildrenIds)
            {
                if (_data.NodeDict.TryGetValue(childId, out var childNodeData))
                {
                    AbilityLogicTreeItem item = null;
                    switch (childNodeData.NodeType)
                    {
                        case EAbilityNodeType.EAbilityCycle:
                            throw new Exception("出现了生命周期节点，有bug！！");
                        case EAbilityNodeType.EEvent:
                            item = new EventTreeItem(childNodeData);
                            break;
                        case EAbilityNodeType.EBranchControl:
                            item = new BranchTreeItem(childNodeData);
                            break;
                        case EAbilityNodeType.EVariableControl:
                            item = new VariableTreeItem(childNodeData);
                            break;
                        case EAbilityNodeType.ERepeat:
                            item = new RepeatTreeItem(childNodeData);
                            break;
                        case EAbilityNodeType.EAction:
                            item = new ActionTreeItem(childNodeData);
                            break;
                        case EAbilityNodeType.ETimer:
                            item = new TimerTreeItem(childNodeData);
                            break;
                        case EAbilityNodeType.EStage:
                            item = new StageTreeItem(childNodeData);
                            break;
                    }

                    parent.AddChild(item);
                    setupChild(childNodeData, item);
                }
            }
        }

        protected override void RowGUI(RowGUIArgs args)
        {
            base.RowGUI(args);
            var item = (AbilityLogicTreeItem)args.item;
            if (item is EmptyItem)
            {
                return;
            }

            SetExpanded(item.id, true);
            var rowRect = args.rowRect;
            float labelWidth = 24;
            rowRect.x = rowRect.x + 25 + 38 * item.NodeData.Depth;
            if (item.NodeData.Depth > 0)
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

        private void AddNode(object obj)
        {
            if (obj is (AbilityLogicTreeItem, EAbilityNodeType))
            {
                var pair = ((AbilityLogicTreeItem select, EAbilityNodeType eNodeType))obj;
                var node = AbilityData.GetNodeData(_data, pair.eNodeType);
                node.Parent = pair.select.NodeData.NodeId;
                node.Depth = pair.select.NodeData.Depth + 1;
                pair.select.NodeData.ChildrenIds.Add(node.NodeId);
                _data.NodeDict.Add(node.NodeId, node);
                switch (pair.eNodeType)
                {
                    case EAbilityNodeType.EEvent:
                        node.EventNodeData = new EventNodeData();
                        break;
                    case EAbilityNodeType.EBranchControl:
                        node.BranchNodeData = new BranchNodeData();
                        break;
                    case EAbilityNodeType.EVariableControl:
                        node.VariableNodeData = new VariableNodeData();
                        break;
                    case EAbilityNodeType.ERepeat:
                        node.RepeatNodeData = new RepeatNodeData();
                        break;
                    case EAbilityNodeType.ETimer:
                        node.TimerNodeData = new TimerNodeData();
                        break;
                    case EAbilityNodeType.EStage:
                        node.StageNodeData = new StageNodeData();
                        break;
                }
                EditorUtility.SetDirty(_data);
                AssetDatabase.SaveAssets();
                AssetDatabase.Refresh();
                Reload();
            }
        }

        private void RemoveNode(object obj)
        {
            if (obj is AbilityLogicTreeItem select)
            {
                var parent = _data.NodeDict[select.NodeData.Parent];
                parent.ChildrenIds.Remove(select.NodeData.NodeId);
                select.NodeData.RemoveSelf(_data);
                EditorUtility.SetDirty(_data);
                AssetDatabase.SaveAssets();
                AssetDatabase.Refresh();
                Reload();
            }
        }

        protected override void ContextClickedItem(int id)
        {
            if (FindItem(id, rootItem) is AbilityLogicTreeItem select)
            {
                GenericMenu menu = new GenericMenu();
                menu.AddItem(new GUIContent("创建节点/添加Action节点"), false,
                    AddNode, (select, EAbilityNodeType.EAction));
                menu.AddItem(new GUIContent("创建节点/添加分支节点"), false,
                    AddNode, (select, EAbilityNodeType.EBranchControl));
                menu.AddItem(new GUIContent("创建节点/创建变量控制节点"), false,
                    AddNode, (select, EAbilityNodeType.EVariableControl));
                menu.AddItem(new GUIContent("创建节点/创建Event节点"), false,
                    AddNode, (select, EAbilityNodeType.EEvent));
                menu.AddItem(new GUIContent("创建节点/创建Repeat节点"), false,
                    AddNode, (select, EAbilityNodeType.ERepeat));
                menu.AddItem(new GUIContent("创建节点/创建Stage节点"), false,
                    AddNode, (select, EAbilityNodeType.EStage));
                menu.AddItem(new GUIContent("创建节点/创建Timer节点"), false,
                    AddNode, (select, EAbilityNodeType.ETimer));
                if (select is not CycleTreeItem)
                {
                    menu.AddItem(new GUIContent("删除节点"), false,
                        RemoveNode, select);
                }

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
            return true;
        }

        /// <summary>
        /// 实现拖拽操作
        /// </summary>
        /// <param name="args"></param>
        protected override void SetupDragAndDrop(SetupDragAndDropArgs args)
        {
            if (hasSearch) return;

            DragAndDrop.PrepareStartDrag();
            var draggedRows = new List<AbilityLogicTreeItem>(16);
            foreach (var item in GetRows())
            {
                if (args.draggedItemIDs.Contains(item.id))
                {
                    draggedRows.Add(item as AbilityLogicTreeItem);
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
                var draggedItems = (List<AbilityLogicTreeItem>)genericData;
                foreach (var treeViewItem in draggedItems)
                {
                    var parentItem = (AbilityLogicTreeItem)args.parentItem;
                    if (!parentItem.hasChildren)
                    {
                        parentItem.children = new List<TreeViewItem>();
                    }

                    var draggedItemId = treeViewItem.NodeData.NodeId;
                    var parentData = _data.NodeDict[treeViewItem.NodeData.Parent];
                    //处理TreeVIewItem
                    if (treeViewItem.parent != parentItem)
                    {
                        treeViewItem.parent.children.Remove(treeViewItem);
                        treeViewItem.parent = parentItem;

                        parentData.ChildrenIds.Remove(draggedItemId);
                        parentData = parentItem.NodeData;

                        treeViewItem.NodeData.Parent = parentItem.NodeData.NodeId;
                    }
                    else
                    {
                        //更新顺序
                        parentItem.children.Remove(treeViewItem);
                        parentData.ChildrenIds.Remove(draggedItemId);
                    }

                    treeViewItem.UpdateDepth(_data);

                    parentItem.children.Insert(args.insertAtIndex < 0 ? 0 : args.insertAtIndex, treeViewItem);
                    parentData.ChildrenIds.Insert(args.insertAtIndex < 0 ? 0 : args.insertAtIndex, draggedItemId);
                }

                Reload();
                return DragAndDropVisualMode.Move;
            }

            return DragAndDropVisualMode.Move;
        }
    }
}