using System;
using System.Collections.Generic;
using Hono.Scripts.Battle;
using Sirenix.OdinInspector.Editor;
using Sirenix.Utilities.Editor;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

namespace Editor.AbilityEditor
{
    /// <summary>
    /// Ability界面
    /// </summary>
    public class AbilityView : AView<AbilityData>
    {
        private List<AbilityCycleDrawer> _cycleDrawers;

        public AbilityView()
        {
            _cycleDrawers = new()
            {
                new AbilityCycleDrawer(this, EAbilityCycle.Init),
                new AbilityCycleDrawer(this, EAbilityCycle.PreExecute),
                new AbilityCycleDrawer(this, EAbilityCycle.Executing),
                new AbilityCycleDrawer(this, EAbilityCycle.EndExecute),
            };
        }

        protected override void onInit(OdinDrawer odinDrawer)
        {
            var drawer = (AbilityViewDrawer)odinDrawer;
            drawer.EnableScrollView = true;
        }

        public override void Draw()
        {
            SirenixEditorGUI.BeginBox("基础数据");
            EditorGUIUtility.labelWidth = 100;
            EditorGUILayout.LabelField($"AbilityID:{Data.id}");
            Data.desc = SirenixEditorFields.TextField("Ability描述", Data.desc);
            Data.DefaultStartGroupId = SirenixEditorFields.IntField("默认开始阶段", Data.DefaultStartGroupId);
            SirenixEditorGUI.EndBox();

            foreach (var cycleDrawer in _cycleDrawers)
            {
                cycleDrawer.DrawCycle();
            }
        }

        public override void Save() { }
    }

    public class AbilityViewDrawer : AViewDrawer<AbilityView> { }
}