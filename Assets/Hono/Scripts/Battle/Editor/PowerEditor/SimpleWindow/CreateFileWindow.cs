using System;
using System.IO;
using Hono.Scripts.Battle;
using Sirenix.OdinInspector;
using Sirenix.OdinInspector.Editor;
using Sirenix.Utilities;
using Sirenix.Utilities.Editor;
using Hono.Scripts.Battle.Editor;
using UnityEditor;
using UnityEngine;

namespace Editor.AbilityEditor.SimpleWindow
{
    public class CreateFileWindow : OdinEditorWindow
    {
        public PowerEditorMenuItemBase MenuItem { get; private set; }

        public static void OpenWindow(PowerEditorMenuItemBase itemBase)
        {
            var window = GetWindow<CreateFileWindow>();
            window.position = GUIHelper.GetEditorWindowRect().AlignCenter(400, 100);
            window.MenuItem = itemBase;
            window.init();
        }

        private void init()
        {
            string label = "";

            if (MenuItem.Root is SkillRootItem)
            {
                label = "正在创建技能：";
            }
            else if (MenuItem.Root is BuffMenuRoot)
            {
                label = "正在创建BUFF：";
            }
            else if (MenuItem.Root is BulletMenuRoot)
            {
                label = "正在创建Bullet：";
            }
            else
            {
                label = "正在创建Ability：";
            }

            this.titleContent = new GUIContent(label);
        }

        [InfoBox("$_msg", InfoMessageType.Error, "HasError")]
        public int id;

        private string _msg;

        [HideInInspector]
        public bool HasError;

        private bool checkHasRepeat(string newFileName)
        {
            var files = Directory.GetFiles(MenuItem.Path, "*.asset");
            foreach (var path in files)
            {
                var fileName = Path.GetFileName(path);
                if (fileName == newFileName)
                {
                    return true;
                }
            }

            return false;
        }

        [Button("创 建")]
        public void Create()
        {
            var newFileName = id + ".asset";
            var newPath = MenuItem.Path + "/" + newFileName;

            if (checkHasRepeat(newFileName))
            {
                _msg = $"Id {id} 存在重复文件";
                HasError = true;
                return;
            }

            ASerializableData data = MenuItem.Root switch
            {
                SkillRootItem  => CreateInstance<SkillData>(),
                BuffMenuRoot   => CreateInstance<BuffData>(),
                BulletMenuRoot => CreateInstance<BulletData>(),
                _              => CreateInstance<AbilityData>()
            };

            data.name = id.ToString();
            data.id = id;

            AssetDatabase.CreateAsset(data, newPath);
            Close();
            MenuItem.MenuTree.UpdateMenuTree();
        }
    }
}