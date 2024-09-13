using System;
using System.Collections.Generic;
using Hono.Scripts.Battle;
using Sirenix.OdinInspector.Editor;
using Sirenix.Utilities;
using Sirenix.Utilities.Editor;
using UnityEditor;
using UnityEditor.IMGUI.Controls;
using UnityEngine;

namespace Editor.AbilityEditor {
	public abstract class AbilityLogicTreeItem : TreeViewItem
	{
		protected AbilityLogicTree _tree;
		
		public EditorWindow SettingWindow;
		protected AbilityNodeData _nodeData;
		public int DrawCount;

		protected AbilityLogicTreeItem(int id, int depth, string name) : base(id, depth, name) { }

		protected AbilityLogicTreeItem(AbilityLogicTree tree ,AbilityNodeData nodeData) : base(nodeData.NodeId, nodeData.Depth) {
			_nodeData = nodeData;
			_tree = tree;
		}

		protected void AddNode(object oNodeType)
		{
			var nodeType = (EAbilityNodeType)oNodeType;
			var node = AbilityData.GetNodeData(_tree.TreeData, nodeType);
			node.ParentId = _nodeData.NodeId;
			node.Depth = _nodeData.Depth + 1;
			_nodeData.ChildrenIds.Add(node.NodeId);
			EditorUtility.SetDirty(_tree.TreeData);
			AssetDatabase.SaveAssets();
			AssetDatabase.Refresh();
			_tree.Reload();
		}
		
		public void RemoveNode(AbilityData treeData)
		{
			var parentNodeData = treeData.NodeDict[_nodeData.ParentId];
			parentNodeData.ChildrenIds.Remove(_nodeData.NodeId);
			treeData.NodeDict.Remove(_nodeData.NodeId);
			EditorUtility.SetDirty(treeData);
			AssetDatabase.SaveAssets();
			AssetDatabase.Refresh();
		}
		
		protected abstract Color getButtonColor();

		protected abstract string getButtonText();

		protected abstract string getButtonTips();

		protected virtual float getButtonWidth() {
			return 456f;
		}

		public abstract GenericMenu GetGenericMenu();
		
		protected virtual GUIStyle getButtonTextStyle() {
			var leftAlignedButtonStyle = new GUIStyle(GUI.skin.button);
			leftAlignedButtonStyle.alignment = TextAnchor.MiddleLeft;
			return leftAlignedButtonStyle;
		}

		protected abstract void OnBtnClicked();

		public virtual void DrawItem(Rect lineRect) {
			var bgColor = GUI.backgroundColor;
			if (SettingWindow != null) {
				GUI.backgroundColor = new Color(2, 2, 2);
				;
			}
			else {
				GUI.backgroundColor = getButtonColor();
			}

			lineRect.width = getButtonWidth();
			var buttonText = getButtonText();

			if (string.IsNullOrEmpty(buttonText)) buttonText = "未定义描述";
			if (buttonText.Length > 70) {
				buttonText = buttonText.Substring(0, 70);
				buttonText += "...";
			}

			if (_nodeData.NodeType != EAbilityNodeType.EAbilityCycle)
				buttonText = $"<{DrawCount}>" + buttonText;

			if (GUI.Button(lineRect, new GUIContent(buttonText, getButtonTips()), getButtonTextStyle())) {
				if (Event.current.button == 0) {
					OnBtnClicked();
				}
			}

			GUI.backgroundColor = bgColor;
		}

		public void UpdateDepth(AbilityData data) {
			var parentItem = data.NodeDict[_nodeData.ParentId];
			_nodeData.Depth = parentItem.Depth + 1;
			if (hasChildren) {
				foreach (var treeViewItem in children) {
					if (treeViewItem is AbilityLogicTreeItem logicTreeViewItem) {
						logicTreeViewItem.UpdateDepth(data);
					}
				}
			}
		}
	}
}