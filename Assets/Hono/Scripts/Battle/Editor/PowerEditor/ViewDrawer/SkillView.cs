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

        private AbilityView _abilityView;

        public override void Load(string path)
        {
            base.Load(path);
            if (Data.abilityId > 0)
            {
                _abilityView.Load(BattleEditorPath.AbilityRootPath + "/" + Data.abilityId + ".asset");
            }
        }

        #region 资源列表绘制

        /// <summary>
        /// 绘制资源列表
        /// </summary>
        /// <param name="resItemsList"></param>
        /// <param name="desc"></param>
        private void drawResList(List<ResItems> resItemsList, string desc)
        {
            float oldw = EditorGUIUtility.labelWidth;
            int removeIdx = -1;
            ResItems removeList = null;

            EditorGUILayout.LabelField(desc);
            SirenixEditorGUI.BeginVerticalList();

            foreach (var resItems in resItemsList)
            {
                SirenixEditorGUI.BeginListItem();
                EditorGUILayout.BeginHorizontal();
                if (GUILayout.Button("删除", GUILayout.Width(42)))
                {
                    removeList = resItems;
                }

                EditorGUILayout.LabelField("条件组：", GUILayout.Width(48));
                EditorGUILayout.BeginVertical();
                for (var index = 0; index < resItems.Items.Count; index++)
                {
                    var item = resItems.Items[index];
                    EditorGUILayout.BeginHorizontal();
                    if (GUILayout.Button("-", GUILayout.Width(22)))
                    {
                        removeIdx = index;
                    }

                    EditorGUIUtility.labelWidth = 70;
                    item.ResourceType =
                        (EBattleResourceType)SirenixEditorFields.EnumDropdown("消耗资源类型", item.ResourceType,
                                                                              GUILayout.Width(180));
                    EditorGUIUtility.labelWidth = 50;
                    item.ResId = SirenixEditorFields.IntField("资源Id", item.ResId);
                    item.Value = SirenixEditorFields.IntField("消耗数量", item.Value);
                    EditorGUILayout.EndHorizontal();
                }

                EditorGUILayout.EndVertical();

                if (GUILayout.Button("+", GUILayout.Width(22)))
                {
                    resItems.Items.Add(new ResItem());
                }

                EditorGUILayout.EndHorizontal();
                SirenixEditorGUI.EndListItem();
                if (removeIdx != -1)
                {
                    resItems.Items.RemoveAt(removeIdx);
                    removeIdx = -1;
                }
            }

            if (SirenixEditorGUI.Button("+", ButtonSizes.Medium))
            {
                resItemsList.Add(new ResItems());
            }

            SirenixEditorGUI.EndVerticalList();

            if (removeList != null)
            {
                resItemsList.Remove(removeList);
            }

            EditorGUIUtility.labelWidth = oldw;
        }

        #endregion

        private void DrawSkillView()
        {
            float oldWidth = EditorGUIUtility.labelWidth;
            EditorGUIUtility.labelWidth = 140;

            SirenixEditorGUI.BeginBox("技能信息");
            SirenixEditorGUI.BeginVerticalList();
            Data.skillIcon = PowerEditorUIHelper.DrawIconField(Data.skillIcon, 50f);
            PowerEditorUIHelper.DrawSimpleField(ref Data.fileName,      "技能名",  Data.fileName,      true);
            PowerEditorUIHelper.DrawSimpleField(ref Data.desc,          "技能描述", Data.desc,          true);
            PowerEditorUIHelper.DrawSimpleField(ref Data.skillType,     "技能类型", Data.skillType,     true);
            PowerEditorUIHelper.DrawSimpleField(ref Data.skillDuration, "技能时长", Data.skillDuration, true);
            SirenixEditorGUI.EndVerticalList();
            SirenixEditorGUI.EndBox();

            SirenixEditorGUI.BeginBox("技能目标选择");
            SirenixEditorGUI.BeginVerticalList();
            PowerEditorUIHelper.DrawSimpleField(ref Data.skillTargetType, "目标选择类型", "指向性技能与非指向性技能",
                                                Data.skillTargetType, true);
            EditorGUILayout.LabelField("一级过滤：对当前技能选出的目标进行筛选，指向性技能会在选择时生效，aoe技能则会影响存入的技能目标列表");
            //PowerEditorUIHelper.DrawSimpleField(ref Data.hitCeiling, "命中数量上限", "会随技能等级表成长", Data.hitCeiling, true);
            PowerEditorUIHelper.DrawSimpleField(ref Data.castRange, "允许释放范围", "会随技能等级表成长", Data.castRange, true);
            /*if (Data.skillTargetType == ESkillTargetSelectType.NoTargetedSkill)
            {
                EditorGUILayout.LabelField("非指向技能选择范围配置");
            }*/

            /*PowerEditorUIHelper.DrawSimpleField(ref Data.speedOfRotateToTarget, "转向释放方向的速度",
                                                Data.speedOfRotateToTarget, true);*/
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
            drawResList(Data.skillResCheck, "资源检查");
            //PowerEditorUIHelper.DrawSimpleField(ref Data.costTimingType, "资源扣除时机", Data.costTimingType, true);
            drawResList(Data.skillResCost, "资源扣除");
            SirenixEditorGUI.EndVerticalList();
            SirenixEditorGUI.EndBox();

            EditorGUIUtility.labelWidth = oldWidth;
        }

        public override void Draw()
        {
            SirenixEditorGUI.BeginHorizontalToolbar();
            if (SirenixEditorGUI.ToolbarTab(_isSkillDrawerTab, "技能配置"))
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
    }

    public class SkillViewDrawer : AViewDrawer<SkillView> { }
}