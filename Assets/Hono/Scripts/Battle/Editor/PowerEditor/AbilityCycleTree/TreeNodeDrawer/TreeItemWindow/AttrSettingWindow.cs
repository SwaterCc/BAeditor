using System;
using System.Linq;
using Hono.Scripts.Battle;
using Hono.Scripts.Battle.Base;
using Sirenix.OdinInspector;
using Sirenix.Utilities.Editor;
using UnityEditor;
using UnityEngine;

namespace Editor.AbilityEditor.TreeItemWindow
{
     public class AttrSettingWindow : ANodeSettingWindow<AttrModifyNodeData>
    {
        private AParamsField _value;
        private AttrDropdown _attrDropdown;

        protected override void Init()
        {
            _value = new AParamsField<RefInt>(TempData.value, "属性值：");
            _attrDropdown = new AttrDropdown(TempData.value);
        }

        protected override void Draw()
        {
            SirenixEditorGUI.BeginBox("设置属性");
            string attrName ="";
            if (Enum.GetName(typeof(EAttrType), TempData.attrType) == null)
            {
                attrName = "未设置";
            }
            else
            {
                attrName = Enum.GetName(typeof(EAttrType), TempData.attrType);
            }
            
            EditorGUILayout.LabelField("当前属性：" + attrName);
            
            if (SirenixEditorGUI.Button("选择属性", ButtonSizes.Medium))
            {
                _attrDropdown.Show(GUILayoutUtility.GetRect(100,100,300,300));
            }

            _value?.Draw();
            TempData.modifyType = (EAttrModifyType)SirenixEditorFields.EnumDropdown("属性修改方式：", TempData.modifyType);
            SirenixEditorGUI.EndBox();
        }
    }
}