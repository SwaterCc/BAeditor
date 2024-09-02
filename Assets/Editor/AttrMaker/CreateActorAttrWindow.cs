using System;
using System.Collections.Generic;
using Hono.Scripts.Battle;
using Sirenix.OdinInspector;
using Sirenix.OdinInspector.Editor;
using Sirenix.Utilities;
using Sirenix.Utilities.Editor;
using UnityEditor;
using UnityEngine;

namespace Editor.AttrMaker
{
    public class CreateActorAttrWindow : OdinEditorWindow
    {
        public static void Open(List<AttrTemplate> templatesDescList)
        {
            var window = GetWindow<CreateActorAttrWindow>();
            window.position = GUIHelper.GetEditorWindowRect().AlignCenter(400, 150);
            window.titleContent = new GUIContent("创建模板");
            window.init(templatesDescList);
        }

        private List<AttrTemplate> _templates;

        private void init(List<AttrTemplate> templates)
        {
            _templates = templates;
        }
      
        [InfoBox("$_msg", InfoMessageType.Error,"HasError")]
        
        [LabelText("Base模板")] [ValueDropdown("_templates")]
        public AttrTemplate BaseTemplate;

        [LabelText("模板ID")] public int Id;

        [Multiline(2)] [LabelText("模板描述")] public string Desc;
        
        private string _msg;

        public bool HasError()
        {
            return _hasError;
        }
        
        private bool _hasError = false;
        
        [Button]
        public void Create()
        {
            AttrDefine asset = CreateInstance<AttrDefine>();

            // 设置默认数据
            asset.name = Id.ToString();
            asset.id = Id;
            asset.desc = Desc;

            if (BaseTemplate != null)
            {
                foreach (var templateAttr in BaseTemplate.AttrTemplateItems)
                {
                    var item = new AttrDefineItem()
                    {
                        AttrName = templateAttr.AttrName,
                        AttrType = templateAttr.AttrType,
                    };
                    
                    var valueType = Type.GetType(templateAttr.AttrType);
                    if (valueType == null)
                    {
                        _msg = $"获取type{templateAttr.AttrType}失败";
                        _hasError = true;
                        return;
                    }

                    if (templateAttr.AttrDefaultValue != null)
                    {
                        item.AttrValue = templateAttr.AttrDefaultValue;
                    }
                    else
                    {
                        item.AttrValue = valueType.InstantiateDefault(true);
                        asset.AttrDefineItems.Add(item);
                    }
                    
                    asset.AttrDefineItems.Add(item);
                }
            }
            else
            {
                _msg = "模板是空的";
                _hasError = true;
            }

            
            if(_hasError) return;
            
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