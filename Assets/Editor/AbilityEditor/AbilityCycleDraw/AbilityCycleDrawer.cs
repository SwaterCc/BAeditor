using System;
using System.Collections.Generic;
using Battle;
using Battle.Def;
using Battle.Skill;
using Sirenix.OdinInspector;
using Sirenix.Utilities.Editor;
using UnityEditor;
using UnityEngine;

namespace Editor.AbilityEditor
{
    public class OnPreAwardCheckDrawer : AbilityCycleDrawBase
    {
        public OnPreAwardCheckDrawer(EAbilityCycleType cycleType, AbilityData data, bool foldout = true) : base(cycleType, data,
            foldout) { }

        protected override void drawEx()
        {
            if (Data.Type == EAbilityType.Buff)
            {
                if (!Data.SpecializationData.TryGetValue("AddRules", out var rules))
                {
                    Data.SpecializationData.Add("AddRules", EBuffAddRule.OnlyOne);
                }

                Data.SpecializationData["AddRules"] =
                    SirenixEditorFields.EnumDropdown((EBuffAddRule)Data.SpecializationData["AddRules"]);
            }
        }
    }

    public class OnPreExecuteCheckDrawer : AbilityCycleDrawBase
    {
        private ResCheckItem _removeItem;
        private List<ResCheckItem> _removeList;


        public OnPreExecuteCheckDrawer(EAbilityCycleType cycleType, AbilityData data, bool foldout = true) : base(cycleType, data,
            foldout) { }

        protected override void drawEx()
        {
            if (Data.Type == EAbilityType.Skill)
            {
                if (!Data.SpecializationData.TryGetValue("SkillResCheck", out var resCheck))
                {
                    Data.SpecializationData.Add("SkillResCheck", new List<List<ResCheckItem>>());
                }

                var checkList = (List<List<ResCheckItem>>)Data.SpecializationData["SkillResCheck"];
                EditorGUILayout.LabelField("设置技能检测条件：");
                SirenixEditorGUI.BeginVerticalList();

                foreach (var items in checkList)
                {
                    SirenixEditorGUI.BeginListItem();
                    EditorGUILayout.BeginHorizontal();
                    if (GUILayout.Button("删除", GUILayout.Width(42)))
                    {
                        _removeList = items;
                    }

                    EditorGUILayout.LabelField("条件组：", GUILayout.Width(48));
                    EditorGUILayout.BeginVertical();
                    foreach (var item in items)
                    {
                        EditorGUILayout.BeginHorizontal();
                        if (GUILayout.Button("-", GUILayout.Width(22)))
                        {
                            _removeItem = item;
                        }
                        EditorGUIUtility.labelWidth = 70;
                        item.ResourceType =
                            (EBattleResourceType)SirenixEditorFields.EnumDropdown("消耗类型",item.ResourceType,
                                GUILayout.Width(180));
                        EditorGUIUtility.labelWidth = 50;
                        item.Flag = SirenixEditorFields.IntField("特征值", item.Flag);
                        item.Cost = SirenixEditorFields.FloatField("消耗值", item.Cost);
                        EditorGUILayout.EndHorizontal();
                    }

                    EditorGUILayout.EndVertical();

                    if (GUILayout.Button("+", GUILayout.Width(22)))
                    {
                        items.Add(new ResCheckItem());
                    }

                    EditorGUILayout.EndHorizontal();
                    SirenixEditorGUI.EndListItem();
                    if (_removeItem != null)
                    {
                        items.Remove(_removeItem);
                        _removeItem = null;
                    }
                }

                if (SirenixEditorGUI.Button("+", ButtonSizes.Medium))
                {
                    checkList.Add(new List<ResCheckItem>());
                }

                SirenixEditorGUI.EndVerticalList();

                if (_removeList != null)
                {
                    checkList.Remove(_removeList);
                    _removeList = null;
                }
            }
        }
    }

    public class OnInitDrawer : AbilityCycleDrawBase
    {
        private SpecializationDataTemplate _template = null;

        public OnInitDrawer(EAbilityCycleType cycleType, AbilityData data, bool foldout = true) : base(cycleType, data, foldout)
        {
            //TODO 需要实现
            switch (Data.Type)
            {
                case EAbilityType.Skill:
                    _template =  AssetDatabase.LoadAssetAtPath("Assets/Editor/EditorData/BattleEditorData/SkillSpecializatioTmp.asset",
                        typeof(SpecializationDataTemplate)) as SpecializationDataTemplate;
                    break;
                case EAbilityType.Buff:
                    _template =  AssetDatabase.LoadAssetAtPath("Assets/Editor/EditorData/BattleEditorData/BuffSpecializatioTmp.asset",
                        typeof(SpecializationDataTemplate)) as SpecializationDataTemplate;
                    break;
                case EAbilityType.Bullet:
                    _template =  AssetDatabase.LoadAssetAtPath("Assets/Editor/EditorData/BattleEditorData/BulletSpecializatioTmp.asset",
                        typeof(SpecializationDataTemplate)) as SpecializationDataTemplate;
                    break;
            }
        }

        protected override void drawEx()
        {
            if (_template == null) return;
            foreach (var field in _template.fieldList)
            {
                if (!Data.SpecializationData.TryGetValue(field.Name, out var value))
                {
                    Data.SpecializationData.Add(field.Name, default);
                }

                string label = field.Desc.Length == 0 ? field.Name : field.Desc;
                EditorGUIUtility.labelWidth = 90;
             
                Data.SpecializationData[field.Name] = AbilityEditorHelper.DrawLabelByType(Type.GetType(field.TypeStr), field.Desc, Activator.CreateInstance(Type.GetType(field.TypeStr)));
            }
        }
    }

    public class OnPreExecuteDrawer : AbilityCycleDrawBase
    {
        public OnPreExecuteDrawer(EAbilityCycleType cycleType, AbilityData data, bool foldout = true) : base(cycleType, data,
            foldout) { }
    }

    public class OnExecutingDrawer : AbilityCycleDrawBase
    {
        public OnExecutingDrawer(EAbilityCycleType cycleType, AbilityData data, bool foldout = true) : base(cycleType, data,
            foldout) { }
    }

    public class OnEndExecuteDrawer : AbilityCycleDrawBase
    {
        public OnEndExecuteDrawer(EAbilityCycleType cycleType, AbilityData data, bool foldout = true) : base(cycleType, data,
            foldout) { }
    }
}