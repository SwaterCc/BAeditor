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
            _value = new AParamsField(TreeItem, TempData.value, "修改属性值：", typeof(RefInt));
            _attrDropdown = new AttrDropdown((type => TempData.attrType = type));
        }

        protected override void Draw()
        {
            SirenixEditorGUI.BeginBox("修改属性（Modify）");
            string attrName ="";
            if (Enum.GetName(typeof(EAttrType), TempData.attrType) == null)
            {
                attrName = "未选择属性";
            }
            else
            {
                attrName = Enum.GetName(typeof(EAttrType), TempData.attrType);
            }
            
            if (SirenixEditorGUI.Button(attrName, ButtonSizes.Medium))
            {
                _attrDropdown.Show(GUILayoutUtility.GetRect(100,100,300,300));
            }

            _value.Draw();
            TempData.commandType = (EAbilityCommandType)SirenixEditorFields.EnumDropdown("属性修改方式：", TempData.commandType);
            SirenixEditorGUI.EndBox();
        }
    }
}