using Hono.Scripts.Battle;
using Sirenix.OdinInspector;
using Sirenix.Utilities;
using Sirenix.Utilities.Editor;
using System;
using UnityEditor;

namespace Editor.BattleEditor.TagEditor {
	public class RemoveTagWindow : EditorWindow {
		private TagTreeView _tagTree;
		private TagTreeViewItem _removeItem;
		
		public static void OpenWindow(TagTreeView tagTreeView, TagTreeViewItem removeItem) {
			var window = GetWindow<RemoveTagWindow>("移除Tag：");
			window._tagTree = tagTreeView;
			window._removeItem = removeItem;
			window.position = GUIHelper.GetEditorWindowRect().AlignCenter(300, 100);
			window.ShowModal();
		}

		private void OnGUI() {
			EditorGUILayout.BeginVertical();
			EditorGUILayout.LabelField($"是否要删除Tag{_removeItem.displayName}及其子tag");

			EditorGUILayout.Space(1);
			if (SirenixEditorGUI.Button("确定", ButtonSizes.Medium)) {
				var parent = _tagTree.TagTreeData.root.FindTag(_removeItem.TagTreeItem.parent.tag);
				parent.children.Remove(_removeItem.TagTreeItem);
				_tagTree.RemoveTagInfo(_removeItem.TagTreeItem.tag);
				foreach (var tagTreeItem in _removeItem.TagTreeItem.children) {
					_tagTree.RemoveTagInfo(tagTreeItem.tag);
				}
				EditorUtility.SetDirty(_tagTree.TagTreeData);
				AssetDatabase.SaveAssetIfDirty(_tagTree.TagTreeData);
				Close();
				_tagTree.Reload();
			}

			if (SirenixEditorGUI.Button("取消", ButtonSizes.Medium)) {
				Close();
			}
			
			EditorGUILayout.EndVertical();
		}
	}
}