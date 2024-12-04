using System;
using System.Collections.Generic;
using Hono.Scripts.Battle;
using Sirenix.OdinInspector;
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
        private class AbilityCycleDrawer
        {
            private bool _cycleViewFoldout;
            private readonly AbilityCycleTree _cycleTree;
            public string Label { get; set; }
            
            public AbilityCycleDrawer(AbilityView view, EAbilityCycle cycle, bool viewFoldoutShow = false)
            {
                _cycleViewFoldout = viewFoldoutShow;
                _cycleTree = new AbilityCycleTree(view, cycle);
            }

            public void DrawCycleTree()
            {
                SirenixEditorGUI.BeginBox();
                var mainRect = GUIHelper.GetCurrentLayoutRect();
                SirenixEditorGUI.BeginBoxHeader();
                var headHeight = GUIHelper.GetCurrentLayoutRect().height;
                var cycleLabel = string.IsNullOrEmpty(Label)
                    ? _cycleTree.Head.NodeData.ToString()
                    : Label;
                _cycleViewFoldout = SirenixEditorGUI.Foldout(_cycleViewFoldout, cycleLabel);
                if (SirenixEditorGUI.Button("展开", ButtonSizes.Medium))
                {
                    _cycleTree.ExpandAll();
                }

                SirenixEditorGUI.EndBoxHeader();
                if (_cycleViewFoldout)
                {
                    var boxRect = GUIHelper.GetCurrentLayoutRect();
                    GUILayout.Box(" ", GUILayout.Height(_cycleTree.totalHeight), GUILayout.Width(boxRect.width)); //无所谓这个盒子，只是占位用的
                    var treeRect = new Rect(boxRect.x, boxRect.y + headHeight, mainRect.width - 8, _cycleTree.totalHeight + 8);
                    _cycleTree.OnGUI(treeRect);
                }

                SirenixEditorGUI.EndBox();
            }
        }
        
        private readonly List<AbilityCycleDrawer> _cycleDrawers;

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
            Data.defaultStartGroupId = SirenixEditorFields.IntField("默认开始阶段", Data.defaultStartGroupId);
            SirenixEditorGUI.EndBox();

            foreach (var cycleDrawer in _cycleDrawers)
            {
                cycleDrawer.DrawCycleTree();
            }
        }
    }

    public class AbilityViewDrawer : AViewDrawer<AbilityView> { }
}