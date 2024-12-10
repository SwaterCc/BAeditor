using System.IO;
using Editor.AbilityEditor.SimpleWindow;
using Sirenix.OdinInspector;
using Sirenix.OdinInspector.Editor;
using Sirenix.Utilities;
using Sirenix.Utilities.Editor;
using UnityEditor;
using UnityEngine;

namespace Hono.Scripts.Battle.Editor.PowerEditor.SimpleWindow
{
    public class CreateFolderWindow : OdinEditorWindow
    {
        public PowerEditorMenuItemBase MenuItem { get; private set; }

        public static void OpenWindow(PowerEditorMenuItemBase itemBase)
        {
            var window = GetWindow<CreateFolderWindow>();
            window.position = GUIHelper.GetEditorWindowRect().AlignCenter(400, 100);
            window.MenuItem = itemBase;
            window.ShowModal();
        }

        [LabelText("路径名:")]
        public string folderName;
        
        private string _msg;

        [HideInInspector]
        public bool HasError;

        private bool checkHasRepeat()
        {
            var files = Directory.GetDirectories(MenuItem.Path);
            foreach (var path in files)
            {
                var fileName = Path.GetFileName(path);
                if (fileName == folderName)
                {
                    return true;
                }
            }

            return false;
        }

        [Button("创 建")]
        public void Create()
        {
            if (checkHasRepeat())
            {
                _msg = $"{folderName} 命名重复";
                HasError = true;
                return;
            }

            AssetDatabase.CreateFolder(MenuItem.Path, folderName);
            if (MenuItem is ICollectionMenuItem collectionMenuItem)
            {
                collectionMenuItem.AddItem(MenuItem.Path + "/" + folderName, true);
            }
            Close();
            MenuItem.MenuTree.UpdateMenuTree();
        }
    }
}