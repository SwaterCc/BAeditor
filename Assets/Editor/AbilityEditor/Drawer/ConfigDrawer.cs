using System.Reflection;
using Battle.Def;
using BattleAbility.Editor.BattleAbilityCustomAttribute;
using Editor.AbilityEditor;
using Sirenix.Utilities.Editor;
using UnityEditor;

namespace BattleAbility.Editor
{
    public class ConfigDrawer
    {
        private bool _baseFoldout = true;
        private BattleAbilityBaseConfig _config;

        public ConfigDrawer(BattleAbilityBaseConfig config)
        {
            _config = config;
        }
        
        public void DrawBaseConfig()
        {
            SirenixEditorGUI.BeginBox();
            SirenixEditorGUI.BeginBoxHeader();
            _baseFoldout = SirenixEditorGUI.Foldout(_baseFoldout, "基础配置");
            SirenixEditorGUI.EndBoxHeader();
            if (_baseFoldout)
            {
                SirenixEditorGUI.BeginVerticalList();
                //反射基类定义
                var baseFieldInfos = _config.GetType().BaseType.GetFields(BindingFlags.Public |
                                                                         BindingFlags.NonPublic |
                                                                         BindingFlags.Static |
                                                                         BindingFlags.DeclaredOnly |
                                                                         BindingFlags.Instance);
                foreach (var fieldInfo in baseFieldInfos)
                {
                    drawVerticalItem(fieldInfo);
                }


                foreach (var fieldInfo in _config.GetType().GetFields(BindingFlags.Public | BindingFlags.NonPublic |
                                                                     BindingFlags.Static | BindingFlags.DeclaredOnly |
                                                                     BindingFlags.Instance))
                {
                    drawVerticalItem(fieldInfo);
                }

                SirenixEditorGUI.EndVerticalList();
            }

            SirenixEditorGUI.EndBox();
        }

        private void drawVerticalItem(FieldInfo fieldInfo)
        {
            bool hasLabelTag = false;
            string label = "";
            BAEditorShowLabelTag.ELabeType labeType = BAEditorShowLabelTag.ELabeType.None;
            foreach (var attr in fieldInfo.GetCustomAttributes())
            {
                if (attr.GetType() == typeof(BAEditorShowLabelTag))
                {
                    var labelTag = attr as BAEditorShowLabelTag;
                    hasLabelTag = true;
                    if (labelTag == null) continue;
                    label = labelTag.LabelText;
                    labeType = labelTag.LabeType;
                    break;
                }
            }

            if (!hasLabelTag)
            {
                return;
            }

            //做分割
            SirenixEditorGUI.BeginListItem();
            SirenixEditorGUI.EndListItem();

            SirenixEditorGUI.BeginListItem();
            if (labeType == BAEditorShowLabelTag.ELabeType.List)
            {
                if (_config.GetAbilityType() == EAbilityType.Skill)
                {
                    var skillBase = (SkillBaseConfig)_config;

                    foreach (var attr in fieldInfo.GetCustomAttributes())
                    {
                        if (attr.GetType() == typeof(BAEditorCollectionItemInitTag))
                        {
                            var funcName = (attr as BAEditorCollectionItemInitTag).GetInitValueFuncName;
                            var methodInfo = skillBase.GetType().GetMethod(funcName);
                            if (methodInfo != null)
                            {
                                AbilityEditorHelper.DrawList<SkillBaseConfig.SkillCostInfo>(
                                    ref skillBase.CostResourceTypeWithValue, label,
                                    () => (SkillBaseConfig.SkillCostInfo)methodInfo.Invoke(skillBase, null), true);
                            }

                            break;
                        }
                    }
                }
            }
            else if (labeType == BAEditorShowLabelTag.ELabeType.Dict)
            {
                EditorGUILayout.LabelField("还没实现哦");
            }
            else
            {
                var afterValue = AbilityEditorHelper.DrawLabelAndUpdateValueByAttr(_config, fieldInfo, label, labeType);
                fieldInfo.SetValue(_config, afterValue);
            }

            SirenixEditorGUI.EndListItem();
        }
    }
}