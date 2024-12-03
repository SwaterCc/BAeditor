using System;
using System.Collections.Generic;
using Editor.AbilityEditor.TreeItem;
using Hono.Scripts.Battle;
using UnityEditor;
using UnityEditor.IMGUI.Controls;
using UnityEngine;

namespace Editor.AbilityEditor
{
    public class AbilityCycleTree : TreeView
    {
        private AbilityData _treeData;
        private AbilityNodeData _cycleHeadData;
       
        public AbilityData TreeData => _treeData;

        private int _drawItemCount;

        public AbilityCycleTree(AbilityData treeData, AbilityNodeData head) : base(new TreeViewState())
        {
            _cycleHeadData = head;
            _cycleHeadData.Depth = -1;
            _treeData = treeData;
            showAlternatingRowBackgrounds = true;
            showBorder = true;
            extraSpaceBeforeIconAndLabel = 30;
            rowHeight = 36;
            Reload();
        }

        protected override TreeViewItem BuildRoot()
        {
            var root = new TreeViewItem(0, -1, "root");

            var cycleRoot = new CycleTreeNode(this, _cycleHeadData);
            cycleRoot.depth = 0;
            root.AddChild(cycleRoot);
            setupChild(_cycleHeadData, cycleRoot);

            EditorUtility.SetDirty(_treeData);
            SetupDepthsFromParentsAndChildren(root);
            cycleRoot.UpdateDepth(_treeData);
            return root;
        }

        private void setupChild(AbilityNodeData nodeData, ATreeNode parent)
        {
            foreach (var childId in nodeData.ChildrenIds)
            {
                if (_treeData.NodeDict.TryGetValue(childId, out var childNodeData))
                {
                    ATreeNode node = null;
                    switch (childNodeData.NodeType)
                    {
                        case EAbilityNodeType.EAbilityCycle:
                            throw new Exception("出现了生命周期节点，有bug！！");
                        case EAbilityNodeType.EEvent:
                            node = new EventTreeNode(this, childNodeData);
                            break;
                        case EAbilityNodeType.EBranchControl:
                            node = new BranchTreeNode(this, childNodeData);
                            break;
                        case EAbilityNodeType.EVariableSetter:
                            node = new VarSetterTreeNode(this, childNodeData);
                            break;
                        case EAbilityNodeType.EAttrSetter:
                            node = new AttrSetterTreeNode(this, childNodeData);
                            break;
                        case EAbilityNodeType.ERepeat:
                            node = new RepeatTreeNode(this, childNodeData);
                            break;
                        case EAbilityNodeType.EAction:
                            node = new ActionTreeNode(this, childNodeData);
                            break;
                        case EAbilityNodeType.ETimer:
                            node = new TimerTreeNode(this, childNodeData);
                            break;
                        case EAbilityNodeType.EGroup:
                            node = new GroupTreeNode(this, childNodeData);
                            break;
                    }
                    
                    parent.AddChild(node);
                    setupChild(childNodeData, node);
                }
            }
        }

        protected override void RowGUI(RowGUIArgs args)
        {
            base.RowGUI(args);
            if (args.item is not ATreeNode item)
            {
                return;
            }

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
        
        protected override void ContextClickedItem(int id)
        {
            if (FindItem(id, rootItem) is ATreeNode select)
            {
                select.ShowMenu();
            }
        }

        /// <summary>
        /// 设置拖拽规则
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        protected override bool CanStartDrag(CanStartDragArgs args)
        {
            return args.draggedItem is not CycleTreeNode;
        }

        /// <summary>
        /// 实现拖拽操作
        /// </summary>
        /// <param name="args"></param>
        protected override void SetupDragAndDrop(SetupDragAndDropArgs args)
        {
            if (hasSearch) return;

            DragAndDrop.PrepareStartDrag();
            var draggedRows = new List<ATreeNode>(16);
            foreach (var item in GetRows())
            {
                if (args.draggedItemIDs.Contains(item.id))
                {
                    draggedRows.Add(item as ATreeNode);
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
                var draggedItems = (List<ATreeNode>)genericData;

                bool CheckLoop(TreeViewItem item)
                {
                    if (item.children == null)
                    {
                        return false;
                    }

                    foreach (var child in item.children)
                    {
                        if (child == args.parentItem)
                        {
                            return true;
                        }

                        return CheckLoop(child);
                    }

                    return false;
                }

                foreach (var draggedItem in draggedItems)
                {
                    //子节点拖到自己身上不移动
                    if (draggedItem == args.parentItem)
                    {
                        return DragAndDropVisualMode.None;
                    }

                    if (CheckLoop(draggedItem))
                    {
                        return DragAndDropVisualMode.None;
                    }
                }

                foreach (var treeViewItem in draggedItems)
                {
                    var parentItem = (ATreeNode)args.parentItem;
                    if (!parentItem.hasChildren)
                    {
                        parentItem.children = new List<TreeViewItem>();
                    }

                    var draggedItemId = treeViewItem.NodeData.NodeId;
                    var parentData = _treeData.NodeDict[treeViewItem.NodeData.ParentId];
                    //处理TreeVIewItem
                    if (treeViewItem.parent != parentItem)
                    {
                        treeViewItem.parent.children.Remove(treeViewItem);
                        treeViewItem.parent = parentItem;

                        parentData.ChildrenIds.Remove(draggedItemId);
                        parentData = parentItem.NodeData;

                        treeViewItem.NodeData.ParentId = parentItem.NodeData.NodeId;
                    }
                    else
                    {
                        //更新顺序
                        parentItem.children.Remove(treeViewItem);
                        parentData.ChildrenIds.Remove(draggedItemId);
                    }

                    treeViewItem.UpdateDepth(_treeData);

                    if (args.insertAtIndex < parentItem.children.Count)
                    {
                        parentItem.children.Insert(args.insertAtIndex < 0 ? 0 : args.insertAtIndex, treeViewItem);
                        parentData.ChildrenIds.Insert(args.insertAtIndex < 0 ? 0 : args.insertAtIndex, draggedItemId);
                    }
                    else
                    {
                        parentItem.children.Add(treeViewItem);
                        parentData.ChildrenIds.Add(draggedItemId);
                    }
                }

                Reload();
                return DragAndDropVisualMode.Move;
            }

            return DragAndDropVisualMode.Move;
        }
    }
}