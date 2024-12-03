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
    public abstract class ATreeNode : TreeViewItem
    {
        public AbilityCycleTree Tree { get; }
        public AbilityNodeData NodeData { get; }
        
        public EditorWindow SettingWindow;

        private readonly GenericMenu _menu;
        
        protected ATreeNode(AbilityCycleTree tree, AbilityNodeData nodeData) : base(nodeData.NodeId, nodeData.Depth)
        {
            Tree = tree;
            NodeData = nodeData;

            _menu = new GenericMenu();
        }

        public void ShowMenu()
        {
            buildMenu();
            
            
            _menu.ShowAsContext();
        }

        protected abstract void buildMenu();
        
        #region 按钮绘制
        protected abstract Color getButtonColor();
        protected abstract string getButtonText();
        protected abstract string getButtonTips();
        

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

        protected abstract void OnBtnClicked(Rect btnRect);

        public virtual void DrawItem(Rect lineRect)
        {
            var bgColor = GUI.backgroundColor;
            if (SettingWindow != null)
            {
                GUI.backgroundColor = new Color(2, 2, 2);
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
                buttonText = $"<{NodeData.NodeId}>" + buttonText;

            if (GUI.Button(lineRect, new GUIContent(buttonText, getButtonTips()), getButtonTextStyle()))
            {
                var btnRect =  EditorGUIUtility.GetMainWindowPosition();
                
                if (Event.current.button == 0)
                {
                    OnBtnClicked(btnRect);
                }
            }
            GUI.backgroundColor = bgColor;
        }

        #endregion

      
        public void UpdateDepth(AbilityData data)
        {
            if (NodeData.ParentId > 0)
            {
                var parentItem = data.NodeDict[NodeData.ParentId];
                NodeData.Depth = parentItem.Depth + 1;
            }
            else
            {
                NodeData.Depth = 0;
            }
            //this.depth = TreeNode.depth;
            if (hasChildren)
            {
                foreach (var treeViewItem in children)
                {
                    if (treeViewItem is ATreeNode logicTreeViewItem)
                    {
                        logicTreeViewItem.UpdateDepth(data);
                    }
                }
            }
        }

        /// <summary>
        /// 检测是否有指定类型的父节点
        /// </summary>
        /// <param name="checkType"></param>
        /// <returns></returns>
        protected bool checkHasParent(EAbilityNodeType checkType)
        {
            int parentId = NodeData.ParentId;
            while (parentId > 0)
            {
                var parentNode = Tree.TreeData.NodeDict[parentId];
                if (parentNode.NodeType == checkType)
                {
                    return true;
                }

                parentId = parentNode.ParentId;
            }

            return false;
        }
        
        /// <summary>
        /// 添加子节点
        /// </summary>
        /// <param name="oNodeType"></param>
        protected void AddChild(object oNodeType)
        {
            var nodeType = (EAbilityNodeType)oNodeType;
            var node = Tree.TreeData.GetNodeData(nodeType);
            node.ParentId = NodeData.NodeId;
            node.Depth = NodeData.Depth + 1;
            NodeData.ChildrenIds.Add(node.NodeId);
            Tree.TreeData.NodeDict.Add(node.NodeId, node);
            EditorUtility.SetDirty(Tree.TreeData);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
            Tree.Reload();
        }
        
        /// <summary>
        /// 添加兄弟节点
        /// </summary>
        /// <param name="oNodeType"></param>
        protected void AddNext(object oNodeType)
        {
            var nodeType = (EAbilityNodeType)oNodeType;
            var node = Tree.TreeData.GetNodeData(nodeType);
            node.ParentId = NodeData.ParentId;
            node.Depth = NodeData.Depth;
            Tree.TreeData.NodeDict[NodeData.ParentId].ChildrenIds.Add(node.NodeId);
            EditorUtility.SetDirty(Tree.TreeData);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
            Tree.Reload();
        }
		
        /// <summary>
        /// 删除自己
        /// </summary>
        /// <param name="treeData"></param>
        public void Remove()
        {
            var parentNodeData = Tree.TreeData.NodeDict[NodeData.ParentId];
            parentNodeData.ChildrenIds.Remove(NodeData.NodeId);
            OnRemove();
            EditorUtility.SetDirty(Tree.TreeData);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
            Tree.Reload();
        }

        public void OnRemove() {
	        if (children != null) {
		        foreach (var child in children) {
			        if (child is ATreeNode aChild) {
				        var childData = aChild.NodeData;
				        Tree.TreeData.NodeDict.Remove(childData.NodeId);
				        aChild.OnRemove();
			        }
		        }
	        }
	       
	        Tree.TreeData.NodeDict.Remove(NodeData.NodeId);
        }
    }
}