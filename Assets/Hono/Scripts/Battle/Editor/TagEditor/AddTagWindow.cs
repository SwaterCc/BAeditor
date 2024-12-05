using Hono.Scripts.Battle;
using Sirenix.OdinInspector;
using Sirenix.Utilities;
using Sirenix.Utilities.Editor;
using System;
using UnityEditor;

namespace Editor.BattleEditor.TagEditor {
	public class AddTagWindow : EditorWindow {
		private TagTreeView _tagTree;

		private int _tag;
		private string _tagName;
		private bool _hasError;
		private TagTreeItem _parent;

		public static void OpenWindow(TagTreeView tagTreeView, TagTreeItem parent) {
			var window = GetWindow<AddTagWindow>("添加Tag：");
			window._tagTree = tagTreeView;
			window._hasError = false;
			window._parent = parent;
			window.position = GUIHelper.GetEditorWindowRect().AlignCenter(300, 100);
			window.ShowModal();
		}

		private void OnGUI() {
			EditorGUILayout.BeginVertical();
			_tag = SirenixEditorFields.IntField("tag value :", _tag);
			_tagName = SirenixEditorFields.TextField("tag name :", _tagName);

			if (_hasError) {
				SirenixEditorGUI.MessageBox("Tag重复！请重新输入。", MessageType.Error);
			}

			EditorGUILayout.Space(1);
			if (SirenixEditorGUI.Button("创建", ButtonSizes.Medium)) {
				_hasError = !_tagTree.TagTreeData.root.TagIsUnique(_tag);
				if (_hasError) {
					return;
				}

				var item = new TagTreeItem() { tag = _tag };

				if (_parent == null) {
					_tagTree.TagTreeData.root.children.Add(item);
				}
				else {
					item.parent = _parent.tag;
					_parent.children.Add(item);
				}

				if (!string.IsNullOrEmpty(_tagName)) {
					_tagTree.SetTagInfo(_tag, _tagName);
				}
				EditorUtility.SetDirty(_tagTree.TagTreeData);
				AssetDatabase.SaveAssetIfDirty(_tagTree.TagTreeData);
				Close();
				_tagTree.Reload();
			}
			
			EditorGUILayout.EndVertical();
		}
	}
}