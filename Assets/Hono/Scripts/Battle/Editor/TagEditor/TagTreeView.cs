using Hono.Scripts.Battle;
using UnityEditor;
using UnityEditor.IMGUI.Controls;
using UnityEngine;

namespace Editor.BattleEditor.TagEditor {
	

	
	public class TagTreeView : TreeView {
		public interface IRightMenu {
			public void ShowRightMenu();
		}
		
		public TagTreeRoot TagTreeData;
		private TagEditorInfos _tagInfos;

		public TagTreeView(TagTreeRoot tagTreeData, TagEditorInfos infos = null) : base(new TreeViewState()) {
			TagTreeData = tagTreeData;
			_tagInfos = infos;

			showAlternatingRowBackgrounds = true;
			showBorder = true;
			useScrollView = true;
			Reload();
		}

		protected override TreeViewItem BuildRoot() {
			var hideRoot = new TreeViewItem(-1, -1, "root");
			var showRoot = new TagTreeViewRootItem(this);
			hideRoot.AddChild(showRoot);

			foreach (var item in TagTreeData.root.children) {
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
				_tagInfos.Infos.Add(tag,info);
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
	}

	public class TagTreeViewRootItem : TreeViewItem, TagTreeView.IRightMenu {
		private TagTreeView _treeView;

		public TagTreeViewRootItem(TagTreeView view) : base(0, 0, "<- tag root ->") {
			_treeView = view;
		}
		public void ShowRightMenu() {
			GenericMenu menu = new();
			menu.AddItem(new GUIContent("添加tag"),false, () => {
				AddTagWindow.OpenWindow(_treeView, null);
			});
			menu.ShowAsContext();
		}
	}
}