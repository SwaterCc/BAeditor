using System;
using Hono.Scripts.Battle;
using Sirenix.OdinInspector;
using Sirenix.OdinInspector.Editor;
using Sirenix.Utilities;
using Sirenix.Utilities.Editor;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Editor.AbilityEditor.SimpleWindow
{
    public class CreateItemWindow : OdinEditorWindow
    {
        public static CreateItemWindow OpenWindow(EAbilityType abilityType)
        {
            var window = CreateWindow<CreateItemWindow>();
            window.position = GUIHelper.GetEditorWindowRect().AlignCenter(400, 300);
            window.titleContent = new GUIContent("创建Item");
            return window;
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


        private bool sameFileCheck()
        {
			return true;
		}
        
        [Button("创 建")]
        public void Create()
        {
            _hasError = false;
            
            if (sameFileCheck())
            {
                _msg = "添加失败，id重复";
                _hasError = true;
                return;
            }

            AbilityData asset = CreateInstance<AbilityData>();

            // 设置默认数据
            asset.name = id.ToString();
            asset.id = id;
            asset.Type = createType;
            asset.Desc = desc;

            // 确定保存路径
            savePath = AssetDatabase.GenerateUniqueAssetPath(savePath);

            // 创建资产文件并保存
            AssetDatabase.CreateAsset(asset, savePath);
            AssetDatabase.SaveAssets();
            
            _main.ForceMenuTreeRebuild();

            Close();
        }
    }
}