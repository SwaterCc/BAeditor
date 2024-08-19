using System;
using System.Collections.Generic;
using Battle;
using Battle.Def;
using Editor.AbilityEditor.TreeItem;
using UnityEditor;
using UnityEditor.IMGUI.Controls;
using UnityEngine;

namespace Editor.AbilityEditor
{
    public class AbilityLogicTreeDrawer : TreeView
    {
        private CycleTreeItem _cycleRoot;
        private AbilityNodeData _cycleHeadData;
        private AbilityData _data;
        public AbilityLogicTreeDrawer(TreeViewState state, AbilityData data, AbilityNodeData head) : base(state)
        {
            _cycleHeadData = head;
            _cycleHeadData.Depth = -1;
            _data = data;
        }

        
        protected override TreeViewItem BuildRoot()
        {
            _cycleRoot = new CycleTreeItem(0, -1, "root");
            setupChild(_cycleHeadData, _cycleRoot);
            
            SetupDepthsFromParentsAndChildren(_cycleRoot);
            return _cycleRoot;
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
            var item = (AbilityLogicTreeItem)args.item;
            base.RowGUI(args);
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
        
        private void AddLogicTreeNode(object obj)
        {
            if (obj is (AbilityLogicTreeItem, EAbilityNodeType))
            {
                var pair = ((AbilityLogicTreeItem select, EAbilityNodeType eNodeType))obj;
                var node = AbilityData.GetNodeData(_data, pair.eNodeType);
                node.Parent = pair.select.NodeData.NodeId;
                node.Depth = pair.select.NodeData.Depth + 1;
                pair.select.NodeData.ChildrenIds.Add(node.NodeId);
                Reload();
            }
        }
        
        protected override void ContextClickedItem(int id)
        {
            if (FindItem(id, _cycleRoot) is AbilityLogicTreeItem select)
            {
                GenericMenu menu = new GenericMenu();
                menu.AddItem(new GUIContent("添加Action节点"), true,
                    AddLogicTreeNode, (select, EAbilityNodeType.EAction));
                menu.AddItem(new GUIContent("添加分支节点"), true,
                    AddLogicTreeNode, (select, EAbilityNodeType.EBranchControl));
                menu.AddItem(new GUIContent("创建变量控制节点"), true,
                    AddLogicTreeNode, (select, EAbilityNodeType.EVariableControl));
                menu.AddItem(new GUIContent("创建Event节点"), true,
                    AddLogicTreeNode, (select, EAbilityNodeType.EEvent));
                menu.AddItem(new GUIContent("创建Repeat节点"), true,
                    AddLogicTreeNode, (select, EAbilityNodeType.ERepeat));
                menu.AddItem(new GUIContent("创建Stage节点"), true,
                    AddLogicTreeNode, (select, EAbilityNodeType.EStage));
                menu.AddItem(new GUIContent("创建Timer节点"), true,
                    AddLogicTreeNode, (select, EAbilityNodeType.ETimer));
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