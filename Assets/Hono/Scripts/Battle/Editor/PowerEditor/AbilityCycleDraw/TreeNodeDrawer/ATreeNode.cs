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
        protected AbilityNodeData _nodeData;
        public AbilityNodeData NodeData => _nodeData;

        protected AbilityCycleTree _tree;

        public EditorWindow SettingWindow;

        protected GenericMenu _menu;
        
        protected ATreeNode(AbilityCycleTree tree, AbilityNodeData nodeData) : base(nodeData.NodeId, nodeData.Depth)
        {
            _nodeData = nodeData;
            _tree = tree;
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

            if (_nodeData.NodeType != EAbilityNodeType.EAbilityCycle)
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
            if (_nodeData.ParentId > 0)
            {
                var parentItem = data.NodeDict[_nodeData.ParentId];
                _nodeData.Depth = parentItem.Depth + 1;
            }
            else
            {
                _nodeData.Depth = 0;
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
            int parentId = _nodeData.ParentId;
            while (parentId > 0)
            {
                var parentNode = _tree.TreeData.NodeDict[parentId];
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
            var node = _tree.TreeData.GetNodeData(nodeType);
            node.ParentId = _nodeData.NodeId;
            node.Depth = _nodeData.Depth + 1;
            _nodeData.ChildrenIds.Add(node.NodeId);
            _tree.TreeData.NodeDict.Add(node.NodeId, node);
            EditorUtility.SetDirty(_tree.TreeData);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
            _tree.Reload();
        }
        
        /// <summary>
        /// 添加兄弟节点
        /// </summary>
        /// <param name="oNodeType"></param>
        protected void AddNext(object oNodeType)
        {
            var nodeType = (EAbilityNodeType)oNodeType;
            var node = _tree.TreeData.GetNodeData(nodeType);
            node.ParentId = _nodeData.ParentId;
            node.Depth = _nodeData.Depth;
            _tree.TreeData.NodeDict[_nodeData.ParentId].ChildrenIds.Add(node.NodeId);
            EditorUtility.SetDirty(_tree.TreeData);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
            _tree.Reload();
        }
		
        /// <summary>
        /// 删除自己
        /// </summary>
        /// <param name="treeData"></param>
        public void Remove()
        {
            var parentNodeData = _tree.TreeData.NodeDict[_nodeData.ParentId];
            parentNodeData.ChildrenIds.Remove(_nodeData.NodeId);
            OnRemove();
            EditorUtility.SetDirty(_tree.TreeData);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
            _tree.Reload();
        }

        public void OnRemove() {
	        if (children != null) {
		        foreach (var child in children) {
			        if (child is ATreeNode aChild) {
				        var childData = aChild._nodeData;
				        _tree.TreeData.NodeDict.Remove(childData.NodeId);
				        aChild.OnRemove();
			        }
		        }
	        }
	       
	        _tree.TreeData.NodeDict.Remove(_nodeData.NodeId);
        }
    }
}