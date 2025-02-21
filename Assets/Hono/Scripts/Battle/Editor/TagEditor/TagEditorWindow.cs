using Hono.Scripts.Battle;
using Sirenix.Utilities;
using Sirenix.Utilities.Editor;
using System;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using SearchField = UnityEditor.IMGUI.Controls.SearchField;

namespace Editor.BattleEditor.TagEditor {
	public class TagEditorWindow : EditorWindow {

		private TagTreeView _tagTreeView ;
		private SearchField _search;
		
		[MenuItem("Power！！/Tag编辑器")]
		public static void OpenWindow() {
			var window = CreateInstance<TagEditorWindow>();
			window.titleContent = new GUIContent("Tag编辑");
			window.position = GUIHelper.GetEditorWindowRect().AlignCenter(150, 400);
			window.Init();
			window.ShowUtility();
		}

		private void Init() {
			var tagTree = AssetDatabase.LoadAssetAtPath<STagTree>(BattleEditorPath.TagTreePath);
			if (tagTree == null) {
				tagTree = CreateInstance<STagTree>();
				AssetDatabase.CreateAsset(tagTree, BattleEditorPath.TagTreePath);
			}

			_tagTreeView = new TagTreeView(tagTree);
			_search = new SearchField();
		}
		
		private void OnGUI() {
			EditorGUILayout.BeginVertical();
			_tagTreeView.searchString = _search.OnGUI(_tagTreeView.searchString);
			//绘制TagTree
			var rect = GUILayoutUtility.GetRect(100, 1000, 300, 1000);
			_tagTreeView.OnGUI(rect);
			EditorGUILayout.EndVertical();
		}
	}
}