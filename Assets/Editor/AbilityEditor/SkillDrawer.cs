using System.Collections.Generic;
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
        }

        public void Draw()
        {
            if (!_data) return;
            SirenixEditorGUI.BeginBox("技能数据");
            _data.SkillType = (ESkillType)SirenixEditorFields.EnumDropdown("技能类型", _data.SkillType);
            _data.SkillTargetType = (ESkillTargetType)SirenixEditorFields.EnumDropdown("技能目标类型", _data.SkillTargetType);
            _data.SkillRange = SirenixEditorFields.FloatField("技能范围", _data.SkillRange);
            _data.PriorityATK = SirenixEditorFields.IntField("技能打断优先级", _data.PriorityATK);
            _data.PriorityDEF = SirenixEditorFields.IntField("技能抗打断优先级", _data.PriorityDEF);
            _data.EcdMode = (ECDMode)SirenixEditorFields.EnumDropdown("CD启动时机", _data.EcdMode);
            _data.SkillCD = SirenixEditorFields.FloatField("技能CD", _data.SkillCD);
            drawResList(_data.SkillResCheck,"技能释放前检测");
            _data.CostType = (EResCostType)SirenixEditorFields.EnumDropdown("战斗资源扣除时机", _data.CostType);
            drawResList(_data.SkillResCheck,"扣除资源配置");
            _data.ForceFaceTarget = EditorGUILayout.Toggle("是否转向技能目标", _data.ForceFaceTarget);
            _data.SkillDamageBasePer = SirenixEditorFields.IntField("技能基础倍率万分比", _data.SkillDamageBasePer);
            _data.UseCustomFilter = EditorGUILayout.Toggle("是否使用自定义筛选器", _data.UseCustomFilter);
            if (_data.UseCustomFilter)
            {
                EditorGUILayout.LabelField("自定义筛选器编辑器绘制还未实现");
            }
            EditorGUILayout.LabelField("Tag绘制未实现");
            SirenixEditorGUI.EndBox();
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
        }
    }
}