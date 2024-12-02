using Hono.Scripts.Battle;
using Sirenix.Utilities.Editor;
using UnityEditor;
using UnityEngine;

namespace Editor.AbilityEditor
{
    public class BuffView : AView<BuffData>
    {
        public override void Draw()
        {
            SirenixEditorGUI.BeginBox("Buff数据");
            Data.AddRule = (EApplicationRequirement)SirenixEditorFields.EnumDropdown("Buff添加规则", Data.AddRule);
            if (Data.AddRule == EApplicationRequirement.HasTags || Data.AddRule == EApplicationRequirement.NoTags)
            {
	           // AbilityEditorTools.DrawIntList(_data.FilterTags,"筛选tag",50);
            }
            Data.ReplaceRule = (EBuffReplaceRule)SirenixEditorFields.EnumDropdown("Buff替换规则", Data.ReplaceRule);
            Data.InitLayer = SirenixEditorFields.IntField("Buff初始层数",Data.InitLayer);
            Data.BuffDamageBasePer = SirenixEditorFields.IntField("Buff基础伤害万分比",Data.BuffDamageBasePer);
            SirenixEditorGUI.EndBox();
        }
    }
    
    public class BuffViewDrawer : AViewDrawer<BuffView> { }
}