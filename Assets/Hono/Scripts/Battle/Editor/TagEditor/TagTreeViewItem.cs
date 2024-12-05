using Hono.Scripts.Battle;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.IMGUI.Controls;
using UnityEngine;

namespace Editor.BattleEditor.TagEditor {
	public class TagTreeViewItem : TreeViewItem , TagTreeView.IRightMenu {
		public TagTreeItem TagTreeItem;
		public TagTreeView TreeView;

		public new TagTreeViewItem parent { get; set; }

		public TagTreeViewItem(TagTreeView treeView, TagTreeItem tagItem) : base(tagItem.tag) {
			TreeView = treeView;
			TagTreeItem = tagItem;
		}
		
		public void BuildTree() {

			var tagInfo = TreeView.GetTagInfo(id);
			
			displayName = tagInfo == null ? id.ToString() : $"{tagInfo.name} {id}";

			foreach (var item in TagTreeItem.children) {
				var treeItem = new TagTreeViewItem(TreeView, item);
				children ??= new List<TreeViewItem>();
				children.Add(treeItem);
				treeItem.BuildTree();
			}
		}

		public void ShowRightMenu() {
			GenericMenu menu = new();
			menu.AddItem(new GUIContent("添加tag"),false, () => {
				AddTagWindow.OpenWindow(TreeView, TagTreeItem);
			});
			menu.AddItem(new GUIContent("删除tag"),false, () => {
				RemoveTagWindow.OpenWindow(TreeView, this);
			});
			menu.ShowAsContext();
		}
	}
}