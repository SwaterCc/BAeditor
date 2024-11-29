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
            SirenixEditorGUI.BeginBox("技能数据");

            float oldWidth = EditorGUIUtility.labelWidth;
            EditorGUIUtility.labelWidth = 140;
            SirenixEditorGUI.BeginVerticalList();
            
            Data.skillIcon = PowerEditorUIHelper.DrawIconField(Data.skillIcon, 50f);
            
            PowerEditorUIHelper.DrawSimpleField(ref Data.skillName, "技能名",  Data.skillName);
            PowerEditorUIHelper.DrawSimpleField(ref Data.skillDesc, "技能描述", Data.skillDesc);
            
            PowerEditorUIHelper.DrawSimpleField(ref Data.skillType,       "技能类型",   Data.skillType);
            PowerEditorUIHelper.DrawSimpleField(ref Data.skillTargetType, "目标选择类型", Data.skillTargetType);
            PowerEditorUIHelper.DrawSimpleField(ref Data.enterCdType,     "进入cd时机", Data.enterCdType);
            PowerEditorUIHelper.DrawSimpleField(ref Data.speedOfRotateToTarget, "转向释放方向的速度",
                                                Data.speedOfRotateToTarget);


            /*SirenixEditorGUI.HorizontalLineSeparator();
            Data.ForceFaceTarget = EditorGUILayout.Toggle("是否转向技能目标", Data.ForceFaceTarget);
            SirenixEditorGUI.HorizontalLineSeparator();
            Data.PriorityATK = SirenixEditorFields.IntField("技能打断优先级", Data.PriorityATK);
            SirenixEditorGUI.HorizontalLineSeparator();
            Data.PriorityDEF = SirenixEditorFields.IntField("技能抗打断优先级", Data.PriorityDEF);
            SirenixEditorGUI.HorizontalLineSeparator();
            Data.enterCdType = (EEnterCdType)SirenixEditorFields.EnumDropdown("CD启动时机", Data.enterCdType);
            SirenixEditorGUI.HorizontalLineSeparator();
            Data.SkillCD = SirenixEditorFields.FloatField("技能CD", Data.SkillCD);
            SirenixEditorGUI.HorizontalLineSeparator();
            drawResList(Data.skillResCheck, "技能释放前检测");
            SirenixEditorGUI.HorizontalLineSeparator();
            Data.costTimingType =
                (EResCostTimingType)SirenixEditorFields.EnumDropdown("战斗资源扣除时机", Data.costTimingType);
            SirenixEditorGUI.HorizontalLineSeparator();
            drawResList(Data.skillResCost, "扣除资源配置");*/
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