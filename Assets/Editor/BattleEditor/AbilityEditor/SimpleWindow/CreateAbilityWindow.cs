using System;
using Hono.Scripts.Battle;
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

        [InfoBox("Ability的Id有分段\n 1-9999 是非特化Ability \n 10001-19999 是技能 \n 20001-29999 是buff \n 30001-39999 是子弹 \n 40001-49999 是GameMode")]
        public int id;

        public string abilityName;

        public string desc;

        private bool _hasError;

        private bool hasError()
        {
            return _hasError;
        }
        
        [InfoBox("$_msg", InfoMessageType.Error, "hasError")]
        private string _msg;


        private bool abilityIdCheck()
        {
            switch (createType)
            {
                case EAbilityType.Skill:
                    return id is > 10000 and < 20000;
                case EAbilityType.Buff:
                    return id is > 20000 and < 30000;
                case EAbilityType.Bullet:
                    return id is > 30000 and < 40000;
                case EAbilityType.GameMode:
                    return id is > 40000 and < 50000;
                case EAbilityType.Other:
                    return id is > 0 and < 10000;
            }

            return false;
        }
        
        [Button("创 建")]
        public void Create()
        {
            _hasError = false;
            var list = _main.AbilityPathWithDatas[createType];
            if (list == null)
            {
                _msg = "获取列表失败";
                _hasError = true;
                return;
            }

            if (!abilityIdCheck())
            {
                _msg = "id不服合规范！";
                _hasError = true;
                return;
            }

            var item = new AbilityConfigItem()
            {
                configId = id,
                name = abilityName,
                desc = desc
            };

            string savePath = _main.AbilityFolders[createType] + "/" + id + ".asset";
            
            if (list.ContainsKey(savePath))
            {
                _msg = "添加失败，id重复";
                _hasError = true;
                return;
            }

            AbilityData asset = CreateInstance<AbilityData>();

            // 设置默认数据
            asset.name = item.configId.ToString();
            asset.ConfigId = item.configId;
            asset.Type = createType;
            asset.Name = abilityName;
            asset.Desc = desc;

            // 确定保存路径
            savePath = AssetDatabase.GenerateUniqueAssetPath(savePath);

            // 创建资产文件并保存
            AssetDatabase.CreateAsset(asset, savePath);
            AssetDatabase.SaveAssets();
                
            _main.Reload(createType);
            _main.ForceMenuTreeRebuild();

            Close();
        }
    }
}