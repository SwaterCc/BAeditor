using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using BattleAbility.Editor.BattleAbilityCustomAttribute;
using Sirenix.OdinInspector;
using Sirenix.OdinInspector.Editor;
using Sirenix.Serialization;
using Sirenix.Utilities.Editor;
using UnityEditor;
using UnityEngine;
using Object = System.Object;

namespace BattleAbility.Editor
{
    /// <summary>
    /// 能力配置界面 （基础配置 + 逻辑配置）
    /// </summary>
    public class BattleAbilityItemView
    {
        /// <summary>
        /// 能力基础数据
        /// </summary>
        public readonly BattleAbilityBaseConfig BaseConfig;

        /// <summary>
        /// 逻辑树对象
        /// </summary>
        public BattleAbilityLogicTreeView LogicTreeView;

        public BattleAbilityItemView(BattleAbilityBaseConfig baseConfig, BattleAbilitySerializableTree serializableTree)
        {
            BaseConfig = baseConfig;
            LogicTreeView = new BattleAbilityLogicTreeView(serializableTree);
        }

        public string GetOdinMenuTreeItemLabel()
        {
            return $"{BaseConfig.ConfigId}->{BaseConfig.Name}";
        }
    }

    public class BattleAbilityItemViewDrawer : OdinValueDrawer<BattleAbilityItemView>
    {
        private bool _baseFoldout = true;
        private bool _logicFoldout = true;

        protected override void DrawPropertyLayout(GUIContent label)
        {
            var itemShowView = this.ValueEntry.SmartValue;

            GUILayout.BeginScrollView(Vector2.zero, false, true);

            drawBaseConfig(itemShowView.BaseConfig);

            SirenixEditorGUI.BeginBox();
            SirenixEditorGUI.EndBox();

            SirenixEditorGUI.BeginBox();
            SirenixEditorGUI.BeginBoxHeader();
            _logicFoldout = SirenixEditorGUI.Foldout(_logicFoldout, "逻辑开发");
            SirenixEditorGUI.EndBoxHeader();
            if (_logicFoldout)
            {
            }

            SirenixEditorGUI.EndBox();

            GUILayout.EndScrollView();
        }

        private void drawBaseConfig(BattleAbilityBaseConfig config)
        {
            SirenixEditorGUI.BeginBox();
            SirenixEditorGUI.BeginBoxHeader();
            _baseFoldout = SirenixEditorGUI.Foldout(_baseFoldout, "基础配置");
            SirenixEditorGUI.EndBoxHeader();
            if (_baseFoldout)
            {
                SirenixEditorGUI.BeginVerticalList();
                //反射基类定义
                var baseFieldInfos = config.GetType().BaseType.GetFields(BindingFlags.Public |
                                                                         BindingFlags.NonPublic |
                                                                         BindingFlags.Static |
                                                                         BindingFlags.DeclaredOnly |
                                                                         BindingFlags.Instance);
                foreach (var fieldInfo in baseFieldInfos)
                {
                    drawVerticalItem(ref config, fieldInfo);
                }


                foreach (var fieldInfo in config.GetType().GetFields(BindingFlags.Public | BindingFlags.NonPublic |
                                                                     BindingFlags.Static | BindingFlags.DeclaredOnly |
                                                                     BindingFlags.Instance))
                {
                    drawVerticalItem(ref config, fieldInfo);
                }

                SirenixEditorGUI.EndVerticalList();
            }

            SirenixEditorGUI.EndBox();
        }

        private void drawVerticalItem(ref BattleAbilityBaseConfig config, FieldInfo fieldInfo)
        {
            bool hasLabelTag = false;
            string label = "";
            BattleAbilityLabelTagEditor.ELabeType labeType = BattleAbilityLabelTagEditor.ELabeType.None;
            foreach (var attr in fieldInfo.GetCustomAttributes())
            {
                if (attr.GetType() == typeof(BattleAbilityLabelTagEditor))
                {
                    var labelTag = attr as BattleAbilityLabelTagEditor;
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
            if (labeType == BattleAbilityLabelTagEditor.ELabeType.List)
            {
                if (config.GetAbilityType() == EAbilityType.Skill)
                {
                    var skillBase = (SkillBaseConfig)config;

                    foreach (var attr in fieldInfo.GetCustomAttributes())
                    {
                        if (attr.GetType() == typeof(BattleAbilityDrawerCollectionEditor))
                        {
                            var funcName = (attr as BattleAbilityDrawerCollectionEditor).GetInitValueFuncName;
                            var methodInfo = skillBase.GetType().GetMethod(funcName);
                            if (methodInfo != null)
                            {
                                BattleAbilitEditorHelper.DrawList<SkillBaseConfig.SkillCostInfo>(
                                    ref skillBase.CostResourceTypeWithValue, label,
                                    () => (SkillBaseConfig.SkillCostInfo)methodInfo.Invoke(skillBase, null), true);
                            }

                            break;
                        }
                    }
                }
            }
            else if (labeType == BattleAbilityLabelTagEditor.ELabeType.Dict)
            {
                EditorGUILayout.LabelField("还没实现哦");
            }
            else
            {
                var afterValue = BattleAbilitEditorHelper.DrawLabelAndUpdateValueByAttr(config, fieldInfo, label, labeType);
                fieldInfo.SetValue(config, afterValue);
            }

            SirenixEditorGUI.EndListItem();
        }
        
    }
}