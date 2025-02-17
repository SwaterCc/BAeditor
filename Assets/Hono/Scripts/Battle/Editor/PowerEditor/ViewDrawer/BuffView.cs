using Hono.Scripts.Battle;
using Hono.Scripts.Battle.Editor.AbilityEditor;
using Sirenix.Utilities.Editor;
using UnityEditor;
using UnityEngine;

namespace Editor.AbilityEditor
{
    public class BuffView : AView<BuffData>
    {
        private bool _isBuffDrawerTab = true;
        private bool _isAbilityDrawerTab;
        private readonly AbilityView _abilityView = new();

        public override void Load(string path)
        {
            base.Load(path);
            _abilityView.Data = Data.BuffAbility;
        }

        protected override void onInit()
        {
            _abilityView.Init();
        }

        private void drawBuff()
        {
            SirenixEditorGUI.BeginBox("Buff数据");
            PowerEditorUIHelper.DrawSimpleField(ref Data.blockRule, "添加阻断规则", Data.blockRule, true);
            
            if (Data.blockRule == EBuffAddBlockRule.BlockById)
            {
                PowerEditorUIHelper.DrawIntList(Data.blockBuffIds, "阻断Ids", 50);
            }
            
            if (Data.blockRule == EBuffAddBlockRule.BlockByTags)
            {
                PowerEditorUIHelper.DrawIntList(Data.blockTags, "阻断Tags", 50);
            }

            PowerEditorUIHelper.DrawSimpleField(ref Data.addRule,          "buff重复添加规则", Data.addRule,          true);
            PowerEditorUIHelper.DrawSimpleField(ref Data.addSuccessBehave, "buff添加成功行为", Data.addSuccessBehave, true);
            if (Data.addSuccessBehave == EBuffAddSuccessBehave.RemoveBuffsById)
            {
                PowerEditorUIHelper.DrawIntList(Data.removeBuffByIds, "清理指定id的buff", 100);
                PowerEditorUIHelper.DrawSimpleField(ref Data.idRemoveCount, "清理指定id的buff数量", Data.idRemoveCount, true);
            }
            if(Data.addSuccessBehave == EBuffAddSuccessBehave.RemoveBuffsByTag)
            {
                PowerEditorUIHelper.DrawIntList(Data.removeBuffByTags, "清理指定tag的buff", 100);
                PowerEditorUIHelper.DrawSimpleField(ref Data.tagRemoveCount, "清理指定tag的buff数量", Data.tagRemoveCount,
                                                    true);
            }

            PowerEditorUIHelper.DrawSimpleField(ref Data.maxLayerNumber, "最大叠层数量",     Data.maxLayerNumber, true);
            PowerEditorUIHelper.DrawSimpleField(ref Data.duration,       "持续时长(-1永久)", Data.duration,       true);
            PowerEditorUIHelper.DrawIntList(Data.buffTags, "buff本身的Tags", 100);
            PowerEditorUIHelper.DrawSimpleField(ref Data.runAgainWhenLayering, "叠层后再次执行Ability",
                                                Data.runAgainWhenLayering, true);
            PowerEditorUIHelper.DrawSimpleField(ref Data.lifeAddWhenLayering, "持续时长可叠加", Data.lifeAddWhenLayering,
                                                true);
            SirenixEditorGUI.EndBox();
        }

        public override void Draw()
        {
            SirenixEditorGUI.BeginHorizontalToolbar();
            if (SirenixEditorGUI.ToolbarTab(_isBuffDrawerTab, "Buff配置"))
            {
                _isBuffDrawerTab = true;
                _isAbilityDrawerTab = false;
            }

            if (SirenixEditorGUI.ToolbarTab(_isAbilityDrawerTab, "Ability"))
            {
                _isAbilityDrawerTab = true;
                _isBuffDrawerTab = false;
            }

            SirenixEditorGUI.EndHorizontalToolbar();

            if (_isBuffDrawerTab)
            {
                drawBuff();
            }

            if (_isAbilityDrawerTab)
            {
                _abilityView?.Draw();
            }
        }
    }

    public class BuffViewDrawer : AViewDrawer<BuffView> { }
}