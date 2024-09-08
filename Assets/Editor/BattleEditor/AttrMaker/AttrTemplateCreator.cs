using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using Sirenix.OdinInspector.Editor;
using Sirenix.Utilities;
using Sirenix.Utilities.Editor;
using UnityEditor;
using UnityEngine;

namespace Editor.AttrMaker
{
    public class AttrTemplateDesc
    {
        public string Name;
        public string Desc;
        public AttrTemplate Asset;
    }

    public class AttrTemplateCreator
    {
        public List<AttrTemplateDesc> TemplatesDescList = new List<AttrTemplateDesc>();

        public AttrTemplateCreator(ref List<AttrTemplate> templates)
        {
            var files = AttrMakerMainWindow.LoadFolder(AttrMakerMainWindow.AttrTemplatesResourcePath);
            if (files != null)
            {
                foreach (var filePath in files)
                {
                    var tpl = AssetDatabase.LoadAssetAtPath<AttrTemplate>(AttrMakerMainWindow.AttrTemplatesResourcePath + "/" + filePath);
                    if (tpl != null)
                    {
                        templates.Add(tpl);
                        TemplatesDescList.Add(new AttrTemplateDesc() { Desc = tpl.Desc, Name = tpl.name, Asset = tpl });
                    }
                }
            }
        }
    }

    public class AttrTemplateCreatorDrawer : OdinValueDrawer<AttrTemplateCreator>
    {
        protected override void DrawPropertyLayout(GUIContent label)
        {
            var itemShowView = this.ValueEntry.SmartValue;
            SirenixEditorGUI.BeginBox("模板列表");
            SirenixEditorGUI.BeginVerticalList();
            foreach (var attrTemplateDesc in itemShowView.TemplatesDescList)
            {
                SirenixEditorGUI.BeginListItem();
                SirenixEditorGUI.BeginIndentedHorizontal();
                EditorGUILayout.LabelField(attrTemplateDesc.Name);
                EditorGUILayout.LabelField(attrTemplateDesc.Desc);
                SirenixEditorGUI.EndIndentedHorizontal();
                SirenixEditorGUI.EndListItem();
            }

            SirenixEditorGUI.EndVerticalList();
            SirenixEditorGUI.EndBox();
        }
    }
}