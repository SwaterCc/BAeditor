using System.Collections.Generic;
using Sirenix.OdinInspector;
using Sirenix.OdinInspector.Editor;
using Sirenix.Utilities;
using Sirenix.Utilities.Editor;
using UnityEditor;
using UnityEngine;

namespace Editor.AttrMaker
{
    public class CreateTemplateWindow : OdinEditorWindow
    {
        public static void Open(List<AttrTemplate> templatesDescList)
        {
            var window = GetWindow<CreateTemplateWindow>();
            window.position = GUIHelper.GetEditorWindowRect().AlignCenter(400, 150);
            window.titleContent = new GUIContent("创建模板");
            window.init(templatesDescList);
        }

        private List<AttrTemplate> _templates;

        private void init(List<AttrTemplate> templates)
        {
            _templates = templates;
        }

        [LabelText("是否基于其他模板创建")] public bool isCopyOther = true;

        [LabelText("Base模板")] [ShowIf("isCopyOther")] [ValueDropdown("_templates")]
        public AttrTemplate BaseTemplate;

        [LabelText("模板名")] public string Name;

        [Multiline(2)] [LabelText("模板描述")] public string Desc;

        [Button]
        public void Create()
        {
            AttrTemplate asset = CreateInstance<AttrTemplate>();

            // 设置默认数据
            asset.name = Name;
            asset.Desc = Desc;

            if (isCopyOther && BaseTemplate != null)
            {
                asset.AttrTemplateItems.AddRange(BaseTemplate.AttrTemplateItems);
            }

            // 确定保存路径
            string path = AttrMakerMainWindow.AttrTemplatesResourcePath; // 可以通过对话框选择路径
            path = path + "/" + asset.name + ".asset";
            path = AssetDatabase.GenerateUniqueAssetPath(path);

            // 创建资产文件并保存
            AssetDatabase.CreateAsset(asset, path);
            AssetDatabase.SaveAssets();
            
            Close();
        }
    }
}