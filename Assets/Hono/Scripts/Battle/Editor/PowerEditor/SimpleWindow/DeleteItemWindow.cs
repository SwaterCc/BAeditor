using System;
using Sirenix.OdinInspector;
using Sirenix.OdinInspector.Editor;
using Sirenix.Utilities;
using Sirenix.Utilities.Editor;
using UnityEditor;

namespace Hono.Scripts.Battle.Editor.PowerEditor.SimpleWindow
{
    public class DeleteItemWindow : EditorWindow
    {
        public PowerEditorMenuItemBase MenuItem { get; private set; }
        public string Msg;
        public static void OpenWindow(PowerEditorMenuItemBase itemBase)
        {
            var window = GetWindow<DeleteItemWindow>();
            window.position = GUIHelper.GetEditorWindowRect().AlignCenter(400, 100);
            window.MenuItem = itemBase;
            window.init();
            window.ShowModal();
        }
        
        private void init()
        {
            Msg = MenuItem switch
            {
                PowerDataMenuItem => $"是否要删除File {MenuItem.Name}？",
                PFolderMenuItem   => $"是否要删除路径 {MenuItem.Name} 及路径下所有的文件？",
                _                 => "Msg?????"
            };
        }

        private void OnGUI()
        {
            EditorGUILayout.BeginVertical();
            
            EditorGUILayout.BeginVertical();
            EditorGUILayout.LabelField(Msg);
            EditorGUILayout.EndVertical();

            EditorGUILayout.BeginHorizontal();
            if (SirenixEditorGUI.Button("确 认", ButtonSizes.Medium))
            {
                MenuItem.Remove();
                AssetDatabase.DeleteAsset(MenuItem.Path);
                if (MenuItem is ICollectionMenuItem collectionMenuItem)
                {
                    collectionMenuItem.RemoveItem(MenuItem.Path, MenuItem is PFolderMenuItem);
                }
                Close();
            }
            
            if (SirenixEditorGUI.Button("取 消", ButtonSizes.Medium))
            {
                Close();
            }
            EditorGUILayout.EndHorizontal();
            EditorGUILayout.EndVertical();
        }
    }
}