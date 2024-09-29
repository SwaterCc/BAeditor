using System.Collections.Generic;
using Editor.AbilityEditor.SimpleWindow;
using Hono.Scripts.Battle;
using Sirenix.OdinInspector;
using Sirenix.Utilities.Editor;
using UnityEditor;
using UnityEngine;

namespace Editor.AbilityEditor
{
    public class SkillDrawer : IExDrawer
    {
        private SkillData _data;
        private bool _selectSelf;

        public void LoadAsset(int id)
        {
            string path = AbilityEditorPath.SkillPath + "/" + id + ".asset";
            _data = AssetDatabase.LoadAssetAtPath<SkillData>(path);
            if (_data == null)
            {
                _data = ScriptableObject.CreateInstance<SkillData>();
                _data.SkillId = id;

                AssetDatabase.CreateAsset(_data, path);
            }

            _selectSelf = _data.SkillTargetType == ESkillTargetType.Self;
        }

        public void Draw()
        {
            if (!_data) return;
            float oldWidth = EditorGUIUtility.labelWidth;
            EditorGUIUtility.labelWidth = 140;
            SirenixEditorGUI.BeginBox("技能数据");
            SirenixEditorGUI.HorizontalLineSeparator();
            _data.SkillType = (ESkillType)SirenixEditorFields.EnumDropdown("技能类型", _data.SkillType);
            SirenixEditorGUI.HorizontalLineSeparator();
            _data.SelectSelf = EditorGUILayout.Toggle("技能选择自己", _data.SelectSelf);
            SirenixEditorGUI.HorizontalLineSeparator();
            if (!_data.SelectSelf) {
	            if (SirenixEditorGUI.Button("技能索敌筛选器",ButtonSizes.Medium))
	            {
		            FilterSettingWindow.Open(ref _data.CustomFilter);
	            }
            }
            else {
	            _data.CustomFilter = FilterSettingTmp.SelfSetting;
            }
            SirenixEditorGUI.HorizontalLineSeparator();
            _data.ForceFaceTarget = EditorGUILayout.Toggle("是否转向技能目标", _data.ForceFaceTarget);
            SirenixEditorGUI.HorizontalLineSeparator();
            _data.PriorityATK = SirenixEditorFields.IntField("技能打断优先级", _data.PriorityATK);
            SirenixEditorGUI.HorizontalLineSeparator();
            _data.PriorityDEF = SirenixEditorFields.IntField("技能抗打断优先级", _data.PriorityDEF);
            SirenixEditorGUI.HorizontalLineSeparator();
            _data.EcdMode = (ECDMode)SirenixEditorFields.EnumDropdown("CD启动时机", _data.EcdMode);
            SirenixEditorGUI.HorizontalLineSeparator();
            _data.SkillCD = SirenixEditorFields.FloatField("技能CD", _data.SkillCD);
            SirenixEditorGUI.HorizontalLineSeparator();
            drawResList(_data.SkillResCheck,"技能释放前检测");
            SirenixEditorGUI.HorizontalLineSeparator();
            _data.CostType = (EResCostType)SirenixEditorFields.EnumDropdown("战斗资源扣除时机", _data.CostType);
            SirenixEditorGUI.HorizontalLineSeparator();
            drawResList(_data.SkillResCost,"扣除资源配置");
           
            SirenixEditorGUI.EndBox();
            EditorGUIUtility.labelWidth = oldWidth;
            
            _data.MaxTargetCount = _data.CustomFilter.MaxTargetCount;
        }

        public void Save()
        {
            if (_data == null)
            {
                return;
            }

            EditorUtility.SetDirty(_data);
            AssetDatabase.SaveAssets();
        }

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
    }
}