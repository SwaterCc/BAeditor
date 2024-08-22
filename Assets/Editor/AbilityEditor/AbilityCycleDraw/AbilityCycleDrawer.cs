using System;
using System.Collections.Generic;
using Battle;
using Battle.Skill;
using Sirenix.OdinInspector;
using Sirenix.Utilities;
using Sirenix.Utilities.Editor;
using UnityEditor;
using UnityEngine;

namespace Editor.AbilityEditor
{
    public class OnPreAwardCheckDrawer : AbilityCycleDrawBase
    {
        public OnPreAwardCheckDrawer(EAbilityCycleType cycleType, AbilityData data) : base(cycleType, data) { }

        protected override bool getDefaultFoldout()
        {
            return Data.Type == EAbilityType.Buff;
        }

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


        public OnPreExecuteCheckDrawer(EAbilityCycleType cycleType, AbilityData data) : base(cycleType, data) { }

        protected override bool getDefaultFoldout()
        {
            return Data.Type == EAbilityType.Skill;
        }

        protected override void drawEx()
        {
            if (Data.Type == EAbilityType.Skill)
            {
                DrawResList(Data, "SkillResCheck", "设置技能释放检测");
            }
        }
    }

    public class OnInitDrawer : AbilityCycleDrawBase
    {
        private SpecializationDataTemplate _template = null;

        public OnInitDrawer(EAbilityCycleType cycleType, AbilityData data) : base(cycleType, data)
        {
            //TODO 需要实现
            switch (Data.Type)
            {
                case EAbilityType.Skill:
                    _template = AssetDatabase.LoadAssetAtPath(
                        "Assets/AbilityRes/BattleEditorData/SkillSpecializatioTmp.asset",
                        typeof(SpecializationDataTemplate)) as SpecializationDataTemplate;
                    break;
                case EAbilityType.Buff:
                    _template = AssetDatabase.LoadAssetAtPath(
                        "Assets/AbilityRes/BattleEditorData/BuffSpecializationTmp.asset",
                        typeof(SpecializationDataTemplate)) as SpecializationDataTemplate;
                    break;
                case EAbilityType.Bullet:
                    _template = AssetDatabase.LoadAssetAtPath(
                        "Assets/AbilityRes/BattleEditorData/BulletSpecializationTmp.asset",
                        typeof(SpecializationDataTemplate)) as SpecializationDataTemplate;
                    break;
            }
        }

        protected override void drawEx()
        {
            if (_template == null) return;
            foreach (var field in _template.fieldList)
            {
                var fieldType = Type.GetType(field.TypeStr);
                if (!Data.SpecializationData.TryGetValue(field.Name, out var value))
                {
                    Data.SpecializationData.Add(field.Name, fieldType?.InstantiateDefault(true));
                }

                EditorGUIUtility.labelWidth = 90;

                Data.SpecializationData[field.Name] = AbilityEditorHelper.DrawLabelByType(fieldType, field.Desc, value);
            }
        }
    }

    public class OnPreExecuteDrawer : AbilityCycleDrawBase
    {
        private EResCostType _resCostType;
        private bool _hasType;

        public OnPreExecuteDrawer(EAbilityCycleType cycleType, AbilityData data) : base(cycleType, data) { }

        protected override bool getDefaultFoldout() => false;

        protected override void drawEx()
        {
            if (Data.SpecializationData.TryGetValue("ResCostType", out var costType))
            {
                _hasType = true;
                _resCostType = (EResCostType)costType;
            }
            else
            {
                _hasType = false;
            }

            if (Data.Type == EAbilityType.Skill && _hasType && _resCostType == EResCostType.BeforeExecute)
            {
                Foldout = true;
                DrawResList(Data, "SkillResCost", "设置技能消耗：");
            }
        }
    }

    public class OnExecutingDrawer : AbilityCycleDrawBase
    {
        public OnExecutingDrawer(EAbilityCycleType cycleType, AbilityData data) : base(cycleType, data) { }
        protected override bool getDefaultFoldout() => true;
    }

    public class OnEndExecuteDrawer : AbilityCycleDrawBase
    {
        private EResCostType _resCostType;
        private bool _hasType;

        public OnEndExecuteDrawer(EAbilityCycleType cycleType, AbilityData data) : base(cycleType, data) { }

        protected override bool getDefaultFoldout() => false;

        protected override void drawEx()
        {
            if (Data.SpecializationData.TryGetValue("ResCostType", out var costType))
            {
                _hasType = true;
                _resCostType = (EResCostType)costType;
            }
            else
            {
                _hasType = false;
            }

            if (Data.Type == EAbilityType.Skill && _hasType && _resCostType == EResCostType.AfterExecute)
            {
                Foldout = true;
                DrawResList(Data, "SkillResCost", "设置技能消耗：");
            }
        }
    }
}