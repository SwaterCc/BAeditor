﻿using System.Linq;
using Editor.AbilityEditor.SimpleWindow;
using Hono.Scripts.Battle;
using Sirenix.OdinInspector.Editor;
using Sirenix.Utilities;
using Sirenix.Utilities.Editor;
using UnityEditor;
using UnityEngine;

namespace Editor.AbilityEditor
{
    /// <summary>
    /// 战斗编辑器主窗口
    /// </summary>
    public class AbilityEditorMainWindow : OdinMenuEditorWindow
    {
        [MenuItem("战斗编辑器/打开Ability编辑器")]
        private static void OpenWindow()
        {
            var window = GetWindow<AbilityEditorMainWindow>();
            window.position = GUIHelper.GetEditorWindowRect().AlignCenter(1000, 800);
            window.titleContent = new GUIContent("Ability编辑器");
        }

        private const string MENU_SKILL = "Skill";
        private const string MENU_BUFF = "Buff";
        private const string MENU_BULLET = "Bullet";
        private const string MENU_OTHER = "Other";

        private const string ASSET_NAME_SKILL = "Skills";
        private const string ASSET_NAME_BUFF = "Buffs";
        private const string ASSET_NAME_BULLET = "Bullets";

        public static string SKILL_DATA_PATH = "Assets/Resources/BattleEditorData/Skill/";
        public static string BUFF_DATA_PATH = "Assets/Resources/BattleEditorData/Buff/";
        public static string BULLET_DATA_PATH = "Assets/Resources/BattleEditorData/Bullet/";

        public static string GetSavePath(EAbilityType type)
        {
            switch (type)
            {
                case EAbilityType.Skill:
                    return SKILL_DATA_PATH;
                case EAbilityType.Buff:
                    return BUFF_DATA_PATH;
                case EAbilityType.Bullet:
                    return BULLET_DATA_PATH;
            }

            return null;
        }

        private AbilityDataList _skills;
        private AbilityDataList _buffs;
        private AbilityDataList _bullets;

        protected override void Initialize()
        {
            _skills = LoadBattleAbilityConfigList(ASSET_NAME_SKILL);
            _buffs = LoadBattleAbilityConfigList(ASSET_NAME_BUFF);
            _bullets = LoadBattleAbilityConfigList(ASSET_NAME_BULLET);
        }

        public AbilityDataList GetDataList(EAbilityType type)
        {
            switch (type)
            {
                case EAbilityType.Skill:
                    return _skills;
                case EAbilityType.Buff:
                    return _bullets;
                case EAbilityType.Bullet:
                    return _bullets;
            }

            return null;
        }

        private AbilityDataList LoadBattleAbilityConfigList(string abilityTypeStr)
        {
            return AssetDatabase.LoadAssetAtPath($"Assets/Resources/BattleEditorData/{abilityTypeStr}.asset",
                typeof(AbilityDataList)) as AbilityDataList;
        }

        private void AddMenuItem(OdinMenuTree treeInstance, string rootMenu, AbilityDataList data)
        {
            if (treeInstance == null)
            {
                return;
            }

            foreach (var itemPair in data.Items)
            {
                var abilityData = LoadAbilitySerializable(data.abilityType, itemPair.Value.configId);
                if (abilityData != null)
                {
                    var abilityView = new AbilityView(abilityData);
                    treeInstance.Add($"{rootMenu}/{abilityView.GetOdinMenuTreeItemLabel()}", abilityView);
                }
            }
        }

        private AbilityData LoadAbilitySerializable(EAbilityType eAbilityType, int id)
        {
            AbilityData asset = null;
            switch (eAbilityType)
            {
                case EAbilityType.Skill:
                    asset = AssetDatabase.LoadAssetAtPath($"{SKILL_DATA_PATH}{id}.asset",
                        typeof(AbilityData)) as AbilityData;
                    break;
                case EAbilityType.Buff:
                    asset = AssetDatabase.LoadAssetAtPath($"{BUFF_DATA_PATH}{id}.asset",
                        typeof(AbilityData)) as AbilityData;
                    break;
                case EAbilityType.Bullet:
                    asset = AssetDatabase.LoadAssetAtPath($"{BULLET_DATA_PATH}{id}.asset",
                        typeof(AbilityData)) as AbilityData;
                    break;
            }

            return !asset ? null : asset;
        }

        protected override OdinMenuTree BuildMenuTree()
        {
            var treeInstance = new OdinMenuTree(false)
            {
                { MENU_SKILL, new BattleAbilityRootView(this, _skills), EditorIcons.Clouds },
                { MENU_BUFF, new BattleAbilityRootView(this, _buffs), EditorIcons.Clouds },
                { MENU_BULLET, new BattleAbilityRootView(this, _bullets), EditorIcons.Clouds }
            };


            AddMenuItem(treeInstance, MENU_SKILL, _skills);
            AddMenuItem(treeInstance, MENU_BUFF, _buffs);
            AddMenuItem(treeInstance, MENU_BULLET, _bullets);

            return treeInstance;
        }

        private void OnSelectionChange()
        {
            var selected = this.MenuTree.Selection.FirstOrDefault();
            if (selected?.Value is AbilityView abilityView)
            {
                abilityView.Save();
            }
        }
        

        protected override void OnDestroy()
        {
            var selected = this.MenuTree.Selection.FirstOrDefault();
            if (selected?.Value is AbilityView abilityView)
            {
                abilityView.Save();
            }
            base.OnDestroy();
        }

        protected override void OnBeginDrawEditors()
        {
            //绘制顶部创建按钮
            var selected = this.MenuTree.Selection.FirstOrDefault();
            if (selected == null)
            {
                return;
            }

            var toolbarHeight = this.MenuTree.Config.SearchToolbarHeight;

            SirenixEditorGUI.BeginHorizontalToolbar(toolbarHeight);
            {
                if (selected != null)
                {
                    GUILayout.Label(selected.Name);
                }

                if (SirenixEditorGUI.ToolbarButton(new GUIContent("创建Ability")))
                {
                    CreateAbilityWindow.OpenWindow(this);
                }

                if (selected.Value is AbilityView abilityView)
                {
                    if (SirenixEditorGUI.ToolbarButton(new GUIContent("打开变量窗口")))
                    {
                        ShowVariableWindow.OpenWindow(this, abilityView.Data);
                    }
                    
                    if (SirenixEditorGUI.ToolbarButton(new GUIContent("保存")))
                    {
                        abilityView.Save();
                    }
                }
                
            }
            SirenixEditorGUI.EndHorizontalToolbar();
        }
    }
}