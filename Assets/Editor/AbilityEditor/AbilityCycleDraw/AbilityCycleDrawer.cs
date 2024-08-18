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
        public OnPreAwardCheckDrawer(EAbilityCycleType type, AbilityData data, bool foldout = true) : base(type, data,
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


        public OnPreExecuteCheckDrawer(EAbilityCycleType type, AbilityData data, bool foldout = true) : base(type, data,
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

                SirenixEditorGUI.BeginVerticalList();

                foreach (var items in checkList)
                {
                    SirenixEditorGUI.BeginListItem();
                    EditorGUILayout.BeginHorizontal();
                    if (GUILayout.Button("删除组", GUILayout.Width(28)))
                    {
                        _removeList = items;
                    }

                    EditorGUILayout.LabelField("条件组：", GUILayout.Width(28));
                    foreach (var item in items)
                    {
                        if (GUILayout.Button("-", GUILayout.Width(8)))
                        {
                            _removeItem = item;
                        }

                        item.ResourceType = (EBattleResourceType)EditorGUILayout.EnumPopup("资源类型", item.ResourceType);
                        item.Flag = SirenixEditorFields.IntField("特征值", item.Flag);
                        item.Cost = SirenixEditorFields.FloatField("消耗值", item.Cost);
                    }

                    if (SirenixEditorGUI.Button("+", ButtonSizes.Medium))
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

        public OnInitDrawer(EAbilityCycleType type, AbilityData data, bool foldout = true) : base(type, data, foldout)
        {
            //TODO 需要实现
            switch (Data.Type)
            {
                case EAbilityType.Skill:
                    break;
                case EAbilityType.Buff:
                    break;
                case EAbilityType.Bullet:
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

                Data.SpecializationData[field.Name] = AbilityEditorHelper.DrawLabelByType(field.Type, label, value);
            }
        }
    }

    public class OnPreExecuteDrawer : AbilityCycleDrawBase
    {
        public OnPreExecuteDrawer(EAbilityCycleType type, AbilityData data, bool foldout = true) : base(type, data,
            foldout) { }
    }

    public class OnExecutingDrawer : AbilityCycleDrawBase
    {
        public OnExecutingDrawer(EAbilityCycleType type, AbilityData data, bool foldout = true) : base(type, data,
            foldout) { }
    }

    public class OnEndExecuteDrawer : AbilityCycleDrawBase
    {
        public OnEndExecuteDrawer(EAbilityCycleType type, AbilityData data, bool foldout = true) : base(type, data,
            foldout) { }
    }
}