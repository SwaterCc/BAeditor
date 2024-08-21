using System;
using AbilityRes;
using Battle;
using BattleAbility.Editor;
using Sirenix.OdinInspector;
using Sirenix.OdinInspector.Editor;
using Sirenix.Utilities;
using Sirenix.Utilities.Editor;
using UnityEditor;
using UnityEngine;

namespace Editor.AbilityEditor.SimpleWindow
{
    public class CreateAbilityWindow : OdinEditorWindow
    {
        public static void OpenWindow(AbilityEditorMainWindow main)
        {
            var window = GetWindow<CreateAbilityWindow>();
            window.position = GUIHelper.GetEditorWindowRect().AlignCenter(400, 300);
            window.titleContent = new GUIContent("创建Ability");
            window.init(main);
        }

        private AbilityEditorMainWindow _main;

        private void init(AbilityEditorMainWindow mainWindow)
        {
            _main = mainWindow;
        }

        [Title("创建类型")] [EnumToggleButtons] public EAbilityType createType;

        public int id;

        public string abilityName;

        public string desc;

        private bool _hasError;

        private bool hasError()
        {
            return _hasError;
        }

        [InfoBox("$msg", InfoMessageType.Error, "hasError")]
        private string msg;

        [Button("创 建")]
        public void Create()
        {
            _hasError = false;
            var list = _main.GetDataList(createType);
            if (list == null)
            {
                msg = "获取列表失败";
                _hasError = true;
                return;
            }

            var item = new AbilityConfigItem()
            {
                configId = id,
                name = abilityName,
                desc = desc
            };

            if (!list.Items.TryAdd(id, item))
            {
                msg = "添加失败，id重复";
                _hasError = true;
            }
            else
            {
                AbilityData asset = CreateInstance<AbilityData>();

                // 设置默认数据
                asset.name = item.configId.ToString();
                asset.ConfigId = item.configId;
                asset.Type = createType;
                asset.Name = abilityName;
                asset.Desc = desc;

                // 确定保存路径
                string path = AbilityEditorMainWindow.GetSavePath(createType); // 可以通过对话框选择路径
                path = path + asset.name + ".asset";
                path = AssetDatabase.GenerateUniqueAssetPath(path);

                // 创建资产文件并保存
                AssetDatabase.CreateAsset(asset, path);
                AssetDatabase.SaveAssets();
            }

            EditorUtility.SetDirty(list);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
            
            if (!_hasError)
            {
                Close();
            }
        }
    }
}