using System;
using System.Collections.Generic;
using Editor.AbilityEditor.TreeItem;
using Hono.Scripts.Battle;
using UnityEditor;
using UnityEditor.IMGUI.Controls;
using UnityEngine;

namespace Editor.AbilityEditor
{
    
    /// <summary>
    /// 换一种思维，不是将数据实例化，而是将树转数据化
    /// 即现有树，再有数据
    /// </summary>
    public class AbilityCycleTree : TreeView
    {
        private int _idCounter;
        public AbilityView View { get; }
        public AbilityData TreeData { get; }
        public EAbilityCycle Cycle { get; }
        public ATreeEditorNode Head { get; }
        
        public AbilityCycleTree(AbilityView view, EAbilityCycle cycle) : base(new TreeViewState())
        {
            View = view;
            TreeData = view.Data;
            Cycle = cycle;

            showAlternatingRowBackgrounds = true;
            showBorder = true;
            extraSpaceBeforeIconAndLabel = 30;
            rowHeight = 36;

            var headNodeId = TreeData.HeadNodeDict[Cycle];
            Head = new ATreeEditorNode(TreeData.NodeDict[headNodeId]);
            Head.Build(TreeData, new ATreeEditorNode.IdGenerator());
            Reload();
        }

        protected override TreeViewItem BuildRoot()
        {
            var root = new TreeViewItem(0, -1, "root");
          
           
           
            SetupDepthsFromParentsAndChildren(root);
            return root;
        }
        
        protected override void RowGUI(RowGUIArgs args)
        {
            base.RowGUI(args);
            if (args.item is not ATreeItem item)
            {
                return;
            }

            var rowRect = args.rowRect;
            float labelWidth = 24;
            rowRect.x = rowRect.x + 25 + 38 * item.depth;
            if (item.depth > 0)
            {
                GUIStyle customStyle = new(GUI.skin.label);
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
            if (FindItem(id, rootItem) is ATreeItem select)
            {
                select.ShowRightMenu();
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
        /// 仅能多选同一树下的子节点
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        protected override bool CanMultiSelect(TreeViewItem item)
        {
            var selections = GetSelection();
            if (selections.Count > 0)
            {
               var firstSelect = FindItem(selections[0], rootItem);
               return firstSelect.parent == item.parent;
            }

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
            var draggedRows = new List<ATreeItem>(32);
            foreach (var item in GetRows())
            {
                if (args.draggedItemIDs.Contains(item.id))
                {
                    draggedRows.Add(item as ATreeItem);
                }
            }

            DragAndDrop.SetGenericData("MyDragging", draggedRows);
            DragAndDrop.objectReferences = new UnityEngine.Object[] { };
            DragAndDrop.StartDrag("Dragging TreeViewItem");
        }

        protected override DragAndDropVisualMode HandleDragAndDrop(DragAndDropArgs args)
        {
            return DragAndDropVisualMode.Move;
        }
    }
}