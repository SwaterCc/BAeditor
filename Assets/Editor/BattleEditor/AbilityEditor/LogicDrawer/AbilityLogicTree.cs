using System;
using System.Collections.Generic;
using Editor.AbilityEditor.TreeItem;
using Hono.Scripts.Battle;
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

        protected override string getButtonTips()
        {
            return "";
        }

        protected override void OnBtnClicked() { }
    }

    public class AbilityLogicTree : TreeView
    {
        private AbilityNodeData _cycleHeadData;
        private AbilityData _data;
        public AbilityData TreeData => _data;
        private int _drawItemCount;

        public AbilityLogicTree(TreeViewState state, AbilityData data, AbilityNodeData head) : base(state)
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
            _drawItemCount = 0;
            var cycleRoot = new CycleTreeItem(_cycleHeadData);
            cycleRoot.depth = 0;
            root.AddChild(cycleRoot);
            setupChild(_cycleHeadData, cycleRoot);
            
            EditorUtility.SetDirty(_data);
            SetupDepthsFromParentsAndChildren(root);
            return root;
        }
        
        private void setupChild(AbilityNodeData nodeData, AbilityLogicTreeItem parent)
        {
            int idx = 0;
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
                        case EAbilityNodeType.EGroup:
                            item = new GroupTreeItem(childNodeData);
                            break;
                    }

                    childNodeData.NextIdInSameLevel = 0;
                    if (idx + 1 < nodeData.ChildrenIds.Count)
                    {
                        idx++;
                        childNodeData.NextIdInSameLevel = _data.NodeDict[nodeData.ChildrenIds[idx]].NodeId;
                    }

                    item.DrawCount = ++_drawItemCount;
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
            
            if (item.depth == 0) SetExpanded(item.id, true);
            
            var rowRect = args.rowRect;
            float labelWidth = 24;
            rowRect.x = rowRect.x + 25 + 38 * item._nodeData.Depth;
            if (item._nodeData.Depth > 0)
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

        private void RemoveNode(object obj)
        {
            if (obj is AbilityLogicTreeItem select)
            {
                select.RemoveNode(_data);
                Reload();
            }
        }

        public bool CheckerOnly(EAbilityNodeType nodeType,AbilityLogicTreeItem select)
        {
            if (select._nodeData.NodeType == nodeType) return false;
            int parentId = select._nodeData.ParentId;
            while (parentId > 0)
            {
                if (_data.NodeDict[parentId].NodeType == nodeType)
                {
                    return false;
                }

                parentId = _data.NodeDict[parentId].ParentId;
            }

            return true;
        }
        
        protected override void ContextClickedItem(int id)
        {
            if (FindItem(id, rootItem) is AbilityLogicTreeItem select)
            {
                GenericMenu menu = select.GetGenericMenu();
                menu.ShowAsContext();
                
                menu.AddItem(new GUIContent("创建节点/添加Action节点"), false,
                    AddNode, (select, EAbilityNodeType.EAction));
                menu.AddItem(new GUIContent("创建节点/添加分支节点"), false,
                    AddNode, (select, EAbilityNodeType.EBranchControl));
                menu.AddItem(new GUIContent("创建节点/创建变量控制节点"), false,
                    AddNode, (select, EAbilityNodeType.EVariableControl));
                menu.AddItem(new GUIContent("创建节点/创建Event节点"), false,
                    AddNode, (select, EAbilityNodeType.EEvent));
                
                if (CheckerOnly(EAbilityNodeType.ERepeat, select))
                {
                    menu.AddItem(new GUIContent("创建节点/创建Repeat节点"), false,
                        AddNode, (select, EAbilityNodeType.ERepeat));
                }
                
                if (CheckerOnly(EAbilityNodeType.EGroup, select))
                {
                    menu.AddItem(new GUIContent("创建节点/创建Stage节点"), false,
                        AddNode, (select, EAbilityNodeType.EGroup));
                }
                
                if (CheckerOnly(EAbilityNodeType.ETimer, select))
                {
                    menu.AddItem(new GUIContent("创建节点/创建Timer节点"), false,
                        AddNode, (select, EAbilityNodeType.ETimer));
                }
                
                if (select is not CycleTreeItem)
                {
                    menu.AddItem(new GUIContent("删除节点"), false,
                        RemoveNode, select);
                }
            }
        }

        /// <summary>
        /// 设置拖拽规则
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        protected override bool CanStartDrag(CanStartDragArgs args)
        {
            return args.draggedItem is not CycleTreeItem;
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
                        return  DragAndDropVisualMode.None;
                    }
                }
                
                foreach (var treeViewItem in draggedItems)
                {
                    var parentItem = (AbilityLogicTreeItem)args.parentItem;
                    if (!parentItem.hasChildren)
                    {
                        parentItem.children = new List<TreeViewItem>();
                    }

                    var draggedItemId = treeViewItem._nodeData.NodeId;
                    var parentData = _data.NodeDict[treeViewItem._nodeData.ParentId];
                    //处理TreeVIewItem
                    if (treeViewItem.parent != parentItem)
                    {
                        treeViewItem.parent.children.Remove(treeViewItem);
                        treeViewItem.parent = parentItem;

                        parentData.ChildrenIds.Remove(draggedItemId);
                        parentData = parentItem._nodeData;

                        treeViewItem._nodeData.ParentId = parentItem._nodeData.NodeId;
                    }
                    else
                    {
                        //更新顺序
                        parentItem.children.Remove(treeViewItem);
                        parentData.ChildrenIds.Remove(draggedItemId);
                    }

                    treeViewItem.UpdateDepth(_data);

                    if (args.insertAtIndex < parentItem.children.Count) {
	                    parentItem.children.Insert(args.insertAtIndex < 0 ? 0 : args.insertAtIndex, treeViewItem);
	                    parentData.ChildrenIds.Insert(args.insertAtIndex < 0 ? 0 : args.insertAtIndex, draggedItemId);
                    }
                    else {
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