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
        public AbilityView View { get; }
        public AbilityData TreeData { get; }
        public CycleTreeNode Head { get; }

        public AbilityCycleTree(AbilityView view, EAbilityCycle cycle) : base(new TreeViewState())
        {
            View = view;
            TreeData = view.Data;
            var headNodeId = TreeData.HeadNodeDict[cycle];
            Head = new CycleTreeNode(this, TreeData.NodeDict[headNodeId]);

            showAlternatingRowBackgrounds = true;
            showBorder = true;
            extraSpaceBeforeIconAndLabel = 30;
            rowHeight = 36;
            Reload();
        }
      
        protected override TreeViewItem BuildRoot()
        {
            var root = new TreeViewItem(0, -1, "root");
            root.AddChild(Head);
            SetupDepthsFromParentsAndChildren(root);
            return root;
        }
        
        public void SaveTree()
        {
            Head.Serialize();
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
            return DragAndDropVisualMode.Move;
        }
    }
}