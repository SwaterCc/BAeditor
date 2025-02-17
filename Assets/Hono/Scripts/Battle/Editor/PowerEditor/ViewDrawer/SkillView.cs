using System;
using System.Collections.Generic;
using Editor.AbilityEditor.SimpleWindow;
using Hono.Scripts.Battle;
using Hono.Scripts.Battle.Editor.AbilityEditor;
using Sirenix.OdinInspector;
using Sirenix.Utilities.Editor;
using UnityEditor;
using UnityEngine;

namespace Editor.AbilityEditor
{
    public class SkillView : AView<SkillData>
    {
        private bool _isSkillDrawerTab = true;
        private bool _isAbilityDrawerTab;

        private readonly AbilityView _abilityView = new();

        public override void Load(string path)
        {
            base.Load(path);
            _abilityView.Data = Data.SkillAbility;
        }

        protected override void onInit()
        {
            _abilityView.Init();
        }

        private void DrawSkillView()
        {
            float oldWidth = EditorGUIUtility.labelWidth;
            EditorGUIUtility.labelWidth = 140;

            SirenixEditorGUI.BeginBox("技能基础数据");
            SirenixEditorGUI.BeginVerticalList();
            EditorGUILayout.LabelField($"技能ID {Data.id}");
            Data.skillIcon = PowerEditorUIHelper.DrawIconField(Data.skillIcon, 50f);
            PowerEditorUIHelper.DrawSimpleField(ref Data.desc,             "技能描述",        Data.desc,             true);
            PowerEditorUIHelper.DrawSimpleField(ref Data.skillType,        "技能类型",        Data.skillType,        true);
            PowerEditorUIHelper.DrawSimpleField(ref Data.skillDuration,    "技能时长",        Data.skillDuration,    true);
            PowerEditorUIHelper.DrawSimpleField(ref Data.rotToTarget,      "释放前转向技能目标方向", Data.rotToTarget,      true);
            PowerEditorUIHelper.DrawSimpleField(ref Data.isExclusive,      "是否为独占技能",     Data.isExclusive,      true);
            PowerEditorUIHelper.DrawSimpleField(ref Data.disableMoveInput, "禁用移动输入",      Data.disableMoveInput, true);
            PowerEditorUIHelper.DrawIntList(Data.tags, "技能Tags", 50, 60);
            SirenixEditorGUI.EndVerticalList();
            SirenixEditorGUI.EndBox();

            SirenixEditorGUI.BeginBox("冷却时间");
            SirenixEditorGUI.BeginVerticalList();
            PowerEditorUIHelper.DrawSimpleField(ref Data.enterCdType, "进入cd时机", Data.enterCdType, true);
            PowerEditorUIHelper.DrawSimpleField(ref Data.skillCd,     "基础Cd",   Data.skillCd,     true);
            SirenixEditorGUI.EndVerticalList();
            SirenixEditorGUI.EndBox();
            
            SirenixEditorGUI.BeginBox("冷却时间");
            SirenixEditorGUI.BeginVerticalList();
            SirenixEditorGUI.BeginListItem();
            drawResList(Data.skillResCheck, "资源检查");
            SirenixEditorGUI.EndListItem();
            SirenixEditorGUI.BeginListItem();
            EditorGUILayout.LabelField("");
            SirenixEditorGUI.EndListItem();
            SirenixEditorGUI.BeginListItem();
            drawResList(Data.skillResCost, "资源扣除");
            SirenixEditorGUI.EndListItem();
            SirenixEditorGUI.EndVerticalList();
            SirenixEditorGUI.EndBox();
            
            SirenixEditorGUI.BeginBox("技能选敌");
            SirenixEditorGUI.BeginVerticalList();
            PowerEditorUIHelper.DrawSimpleField(ref Data.skillTargetType, "目标选择类型", Data.skillTargetType, true);
            switch (Data.skillTargetType)
            {
                case ESkillTargetSelectType.Custom:
                    EditorGUILayout.LabelField("自定义模式，在Ability中设置选敌逻辑");
                    break;
                case ESkillTargetSelectType.Front:
                case ESkillTargetSelectType.Direction:
                case ESkillTargetSelectType.Position:
                    if (SirenixEditorGUI.Button("范围选择形状", ButtonSizes.Large))
                    {
                        SerializableOdinWindow.Open(Data.selectRangeShape);
                    }
                    break;
                case ESkillTargetSelectType.Targeted:
                    if (SirenixEditorGUI.Button("目标选择条件配置", ButtonSizes.Large))
                    {
                        SerializableOdinWindow.Open(Data.targetFilter);
                    }
                    break;
            }
            PowerEditorUIHelper.DrawSimpleField(ref Data.castRange, "施法范围", Data.castRange, true);
            SirenixEditorGUI.EndVerticalList();
            SirenixEditorGUI.EndBox();

            EditorGUIUtility.labelWidth = oldWidth;
        }

        public override void Draw()
        {
            SirenixEditorGUI.BeginHorizontalToolbar();
            if (SirenixEditorGUI.ToolbarTab(_isSkillDrawerTab, "Skill配置"))
            {
                _isSkillDrawerTab = true;
                _isAbilityDrawerTab = false;
            }

            if (SirenixEditorGUI.ToolbarTab(_isAbilityDrawerTab, "Ability"))
            {
                _isAbilityDrawerTab = true;
                _isSkillDrawerTab = false;
            }

            SirenixEditorGUI.EndHorizontalToolbar();

            if (_isSkillDrawerTab)
            {
                DrawSkillView();
            }

            if (_isAbilityDrawerTab)
            {
                _abilityView?.Draw();
            }
        }
        
        #region 资源列表绘制

        /// <summary>
        /// 绘制资源列表
        /// </summary>
        /// <param name="resItems"></param>
        /// <param name="desc"></param>
        private void drawResList(ResItems resItems, string desc)
        {
            float oldw = EditorGUIUtility.labelWidth;
            int removeIdx = -1;

            EditorGUILayout.LabelField(desc);
            SirenixEditorGUI.BeginVerticalList();
            for (var index = 0; index < resItems.Items.Count; index++)
            {
                SirenixEditorGUI.BeginListItem();
                var item = resItems.Items[index];
                EditorGUILayout.BeginHorizontal();
                if (GUILayout.Button("-", GUILayout.Width(22)))
                {
                    removeIdx = index;
                }

                EditorGUIUtility.labelWidth = 70;
                item.resourceType =
                    (EBattleResourceType)SirenixEditorFields.EnumDropdown("消耗资源类型", item.resourceType,
                                                                          GUILayout.Width(180));
                EditorGUIUtility.labelWidth = 50;
                item.param1 = SirenixEditorFields.IntField("Id",   item.param1);
                item.param2 = SirenixEditorFields.IntField("消耗数量", item.param2);
                EditorGUILayout.EndHorizontal();
                SirenixEditorGUI.EndListItem();
            }
            
            if (GUILayout.Button("+", GUILayout.Width(22)))
            {
                resItems.Items.Add(new ResItem());
            }
            
            if (removeIdx != -1)
            {
                resItems.Items.RemoveAt(removeIdx);
            }
            
            SirenixEditorGUI.EndVerticalList();
            
            EditorGUIUtility.labelWidth = oldw;
        }

        #endregion

    }

    public class SkillViewDrawer : AViewDrawer<SkillView> { }
}