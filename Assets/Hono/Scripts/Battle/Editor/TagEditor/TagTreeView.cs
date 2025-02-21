using Hono.Scripts.Battle;
using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.IMGUI.Controls;
using UnityEngine;

namespace Editor.BattleEditor.TagEditor {
	public class TagTreeView : TreeView {
		public interface IRightMenu {
			public void ShowRightMenu();
		}

		public STagTree STagTreeData;
		private TagEditorInfos _tagInfos;

		public TagTreeView(STagTree sTagTreeData, TagEditorInfos infos = null) : base(new TreeViewState()) {
			STagTreeData = sTagTreeData;
			_tagInfos = infos;

			showAlternatingRowBackgrounds = true;
			showBorder = true;
			useScrollView = true;
			Reload();

			// 设置行高
			customFoldoutYOffset = 2;
			extraSpaceBeforeIconAndLabel = 5;
		}

		protected override TreeViewItem BuildRoot() {
			var hideRoot = new TreeViewItem(-1, -1, "root");
			var showRoot = new TagTreeViewRootItem(this);
			hideRoot.AddChild(showRoot);

			foreach (var item in STagTreeData.root.children) {
				var tagViewItem = new TagTreeViewItem(this, item);
				showRoot.AddChild(tagViewItem);
				tagViewItem.BuildTree();
			}

			SetupDepthsFromParentsAndChildren(hideRoot);

			return hideRoot;
		}

		private TagEditorInfos GetTagInfos() {
			if (_tagInfos == null) {
				_tagInfos = AssetDatabase.LoadAssetAtPath<TagEditorInfos>(BattleEditorPath.TagEditorInfoPath);
			}

			if (_tagInfos == null) {
				_tagInfos = ScriptableObject.CreateInstance<TagEditorInfos>();
				AssetDatabase.CreateAsset(_tagInfos, BattleEditorPath.TagEditorInfoPath);
			}

			return _tagInfos;
		}

		public TagInfo GetTagInfo(int tag) {
			if (GetTagInfos().Infos.TryGetValue(tag, out var info)) {
				return info;
			}

			return null;
		}

		public void SetTagInfo(int tag, string name) {
			if (!GetTagInfos().Infos.TryGetValue(tag, out var info)) {
				info = new TagInfo();
				_tagInfos.Infos.Add(tag, info);
			}

			info.name = name;
			EditorUtility.SetDirty(_tagInfos);
			AssetDatabase.SaveAssetIfDirty(_tagInfos);
		}

		public void RemoveTagInfo(int tag) {
			if (!GetTagInfos().Infos.Remove(tag)) {
				EditorUtility.SetDirty(_tagInfos);
				AssetDatabase.SaveAssetIfDirty(_tagInfos);
			}
		}

		protected override bool CanMultiSelect(TreeViewItem item) {
			return item is not TagTreeViewRootItem;
		}

		protected override void ContextClickedItem(int id) {
			var item = FindItem(id, rootItem);
			if (item is IRightMenu rightMenu) {
				rightMenu.ShowRightMenu();
			}
		}
		
		protected override void SetupDragAndDrop(SetupDragAndDropArgs args) {
			DragAndDrop.PrepareStartDrag();
			DragAndDrop.SetGenericData("TAG_DRAG", args.draggedItemIDs);
			DragAndDrop.StartDrag("Tag Drag");
		}

		protected override DragAndDropVisualMode HandleDragAndDrop(DragAndDropArgs args) {
		
			return DragAndDropVisualMode.Rejected;
		}
		
		// 添加双击修改功能
		protected override void DoubleClickedItem(int id) {
			var item = FindItem(id, rootItem) as TagTreeViewItem;
			if (item != null) {
				ShowEditNameWindow(item);
			}
		}

		private void ShowEditNameWindow(TagTreeViewItem item) {
		
		}


	}

	public class TagTreeViewRootItem : TreeViewItem, TagTreeView.IRightMenu {
		private TagTreeView _treeView;

		public TagTreeViewRootItem(TagTreeView view) : base(0, 0, "<- tag root ->") {
			_treeView = view;
		}

		public void ShowRightMenu() {
			GenericMenu menu = new();
			menu.AddItem(new GUIContent("添加tag"), false, () => {
				AddTagWindow.OpenWindow(_treeView, null);
			});
			menu.ShowAsContext();
		}
	}
}