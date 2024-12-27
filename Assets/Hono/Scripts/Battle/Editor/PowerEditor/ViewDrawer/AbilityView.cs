using System;
using System.Collections.Generic;
using Hono.Scripts.Battle;
using Sirenix.OdinInspector;
using Sirenix.OdinInspector.Editor;
using Sirenix.Utilities.Editor;
using Unity.VisualScripting;
using UnityEditor;
using UnityEditor.IMGUI.Controls;
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
            private readonly TreeViewState _treeViewState;
            public string Label { get; set; }

            public AbilityCycleDrawer(AbilityView view, EAbilityCycle cycle, bool viewFoldoutShow = true)
            {
                _cycleViewFoldout = viewFoldoutShow;
                _treeViewState = new TreeViewState();
                _cycleTree = new AbilityCycleTree(_treeViewState, view, cycle);
            }

            public void DrawCycleTree()
            {
                SirenixEditorGUI.BeginBox();
                var mainRect = GUIHelper.GetCurrentLayoutRect();
                SirenixEditorGUI.BeginBoxHeader();
                var headHeight = GUIHelper.GetCurrentLayoutRect().height;
                var cycleLabel = string.IsNullOrEmpty(Label)
                    ? _cycleTree.Head.Data.ToString()
                    : Label;
                _cycleViewFoldout = SirenixEditorGUI.Foldout(_cycleViewFoldout, cycleLabel);
                if (SirenixEditorGUI.Button("展开", ButtonSizes.Medium))
                {
                    _cycleTree.ExpandAll();
                }

                SirenixEditorGUI.EndBoxHeader();
                if (_cycleViewFoldout)
                {
                    GUILayout.Box(" ", GUILayout.Height(_cycleTree.totalHeight),
                                  GUILayout.Width(mainRect.width)); //无所谓这个盒子，只是占位用的
                    var treeRect = new Rect(mainRect.x, mainRect.y + headHeight + 2, mainRect.width,
                                            _cycleTree.totalHeight + 8);
                    _cycleTree.OnGUI(treeRect);
                }

                SirenixEditorGUI.EndBox();
            }
        }

        private List<AbilityCycleDrawer> _cycleDrawers;


        protected override void onInit()
        {
            _cycleDrawers = new List<AbilityCycleDrawer>
            {
                new(this, EAbilityCycle.Init) { Label = "初始化" },
                new(this, EAbilityCycle.PreExecute) { Label = "预启动" },
                new(this, EAbilityCycle.Executing) { Label = "执行" },
                new(this, EAbilityCycle.EndExecute) { Label = "结束" },
            };
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
                EditorGUILayout.Space(10);
            }
        }
    }

    public class AbilityViewDrawer : AViewDrawer<AbilityView> { }
}