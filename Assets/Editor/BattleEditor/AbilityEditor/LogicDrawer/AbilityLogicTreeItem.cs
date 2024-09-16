using System;
using System.Collections.Generic;
using Hono.Scripts.Battle;
using Sirenix.OdinInspector.Editor;
using Sirenix.Utilities;
using Sirenix.Utilities.Editor;
using UnityEditor;
using UnityEditor.IMGUI.Controls;
using UnityEngine;

namespace Editor.AbilityEditor
{
    public abstract class AbilityLogicTreeItem : TreeViewItem
    {
        public AbilityNodeData NodeData;

        public EditorWindow SettingWindow;

        public int DrawCount;

        protected AbilityLogicTreeItem(int id, int depth, string name) : base(id, depth, name) { }

        protected AbilityLogicTreeItem(AbilityNodeData nodeData) : base(nodeData.NodeId, nodeData.Depth)
        {
            NodeData = nodeData;
        }

        #region 按钮绘制

        protected abstract Color getButtonColor();

        protected abstract string getButtonText();

        protected abstract string getItemEffectInfo();

        protected virtual float getButtonWidth()
        {
            return 456f;
        }

        protected virtual GUIStyle getButtonTextStyle()
        {
            var leftAlignedButtonStyle = new GUIStyle(GUI.skin.button);
            leftAlignedButtonStyle.alignment = TextAnchor.MiddleLeft;
            return leftAlignedButtonStyle;
        }

        protected abstract void OnBtnClicked();

        public virtual void DrawItem(Rect lineRect)
        {
            var bgColor = GUI.backgroundColor;
            if (SettingWindow != null)
            {
                GUI.backgroundColor = new Color(2, 2, 2);
                ;
            }
            else
            {
                GUI.backgroundColor = getButtonColor();
            }

            lineRect.width = getButtonWidth();
            var buttonText = getButtonText();

            if (string.IsNullOrEmpty(buttonText)) buttonText = "未定义描述";
            if (buttonText.Length > 70)
            {
                buttonText = buttonText.Substring(0, 70);
                buttonText += "...";
            }

            if (NodeData.NodeType != EAbilityNodeType.EAbilityCycle)
                buttonText = $"<{DrawCount}>" + buttonText;

            if (GUI.Button(lineRect, new GUIContent(buttonText, getItemEffectInfo()), getButtonTextStyle()))
            {
                if (Event.current.button == 0)
                {
                    OnBtnClicked();
                }
            }

            GUI.backgroundColor = bgColor;
        }

        #endregion

        #region 数据处理
        public void UpdateDepth(AbilityData data)
        {
            var parentItem = data.NodeDict[NodeData.ParentId];
            NodeData.Depth = parentItem.Depth + 1;
            //this.depth = TreeNode.depth;
            if (hasChildren)
            {
                foreach (var treeViewItem in children)
                {
                    if (treeViewItem is AbilityLogicTreeItem logicTreeViewItem)
                    {
                        logicTreeViewItem.UpdateDepth(data);
                    }
                }
            }
        }

        public virtual void AddNode()
        {
            
        }
       
        
        #endregion
    }
}