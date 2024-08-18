using System;
using Battle.Def;
using BattleAbility.Editor;
using UnityEditor.IMGUI.Controls;
using UnityEngine;

namespace Editor.AbilityEditor
{
      public class AbilityLogicItem : TreeViewItem
    {
        public AbilityNodeData NodeData;
        public Action<Rect> DrawItem;

        public AbilityLogicItem(int id, int depth, string name) : base(id, depth, name)
        {
            DrawItem = rect => { Debug.Log("asdas"); };
        }

        public AbilityLogicItem(AbilityNodeData nodeData) : base(treeNode.nodeId,
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
            var text = $"事件按钮 ID {TreeNode.nodeId}（后续要加事件部分参数预览）";
            treeButton(lineRect, text, Color.cyan, LogicTreeEventNodeWindow.OpenWindow);
        }

        private void drawConditionItem(Rect lineRect)
        {
            var text = $"条件按钮 ID {TreeNode.nodeId}（后续要加事件部分参数预览）";
            treeButton(lineRect, text, Color.green, LogicTreeConditionNodeWindow.OpenWindow);
        }

        private void drawActionItem(Rect lineRect)
        {
            var text = $"Action节点 ID {TreeNode.nodeId}（后续要加事件部分参数预览）";
            treeButton(lineRect, text, new Color(1.99f, 0.2f, 0f), LogicTreeActionNodeWindow.OpenWindow);
        }

        private void drawVariableItem(Rect lineRect)
        {
            var text = $"创建变量 ID {TreeNode.nodeId}（后续要加事件部分参数预览）";
            treeButton(lineRect, text, new Color(0.46f, 0.4f, 0.8f), LogicTreeVariableNodeWindow.OpenWindow);
        }

        protected void treeButton(Rect lineRect, string btnText, Color btnColor,
            Action<LogicTreeViewItem> action = null,
            float buttonWidth = 250f)
        {
            var bgColor = GUI.backgroundColor;
            GUI.backgroundColor = btnColor;
            lineRect.width = buttonWidth;
            if (GUI.Button(lineRect, btnText))
            {
                if (Event.current.button == 0)
                {
                    action?.Invoke(this);
                }
            }

            GUI.backgroundColor = bgColor;
        }

        public void UpdateDepth(BattleAbilitySerializableTree treeData)
        {
            var parentItem = treeData.allNodes[TreeNode.parentKey];
            TreeNode.depth = parentItem.depth + 1;
            //this.depth = TreeNode.depth;
            if (hasChildren)
            {
                foreach (var treeViewItem in children)
                {
                    if (treeViewItem is LogicTreeViewItem logicTreeViewItem)
                    {
                        logicTreeViewItem.UpdateDepth(treeData);
                    }
                }
            }
        }
    }
}