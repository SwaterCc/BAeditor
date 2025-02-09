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
            Data.AddRule = (EAddBlockRule)SirenixEditorFields.EnumDropdown("Buff添加规则", Data.AddRule);
            if (Data.AddRule == EAddBlockRule.HasTags || Data.AddRule == EAddBlockRule.NoTags)
            {
                //接入tag生态
	           // AbilityEditorTools.DrawIntList(_data.FilterTags,"筛选tag",50);
            }
            Data.ReplaceRule = (EBuffReplaceRule)SirenixEditorFields.EnumDropdown("Buff替换规则", Data.ReplaceRule);
            Data.MaxLayerNumber = SirenixEditorFields.IntField("Buff最大层数",       Data.MaxLayerNumber);
            SirenixEditorGUI.EndBox();
        }
    }
    
    public class BuffViewDrawer : AViewDrawer<BuffView> { }
}