using System;
using System.Linq;
using Battle;
using Battle.Def;
using BattleAbility.Editor;
using Editor.AbilityEditor.SimpleWindow;
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

        private OdinMenuTree _treeInstance;

        private const string MENU_SKILL = "Skill";
        private const string MENU_BUFF = "Buff";
        private const string MENU_BULLET = "Bullet";
        private const string MENU_OTHER = "Other";

        private const string ASSET_NAME_SKILL = "Skills";
        private const string ASSET_NAME_BUFF = "Buffs";
        private const string ASSET_NAME_BULLET = "Bullets";

        public static string SKILL_DATA_PATH = "Assets/EditorData/BattleEditorData/Config/Skill/";
        public static string BUFF_DATA_PATH = "Assets/EditorData/BattleEditorData/Config/Buff/";
        public static string BULLET_DATA_PATH = "Assets/EditorData/BattleEditorData/Config/Bullet/";

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
            return AssetDatabase.LoadAssetAtPath($"Assets/EditorData/BattleEditorData/Config/{abilityTypeStr}.asset",
                typeof(AbilityDataList)) as AbilityDataList;
            ;
        }

        private void AddMenuItem(string rootMenu, AbilityDataList data)
        {
            if (_treeInstance == null)
            {
                return;
            }

            foreach (var itemPair in data.Items)
            {
                var abilityData = LoadAbilitySerializable(data.abilityType, itemPair.Value.configId);
                if (data != null)
                {
                    var abilityView = new AbilityView(abilityData);
                    _treeInstance.Add($"{rootMenu}/{abilityView.GetOdinMenuTreeItemLabel()}", abilityView);
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
            if (_treeInstance == null)
            {
                _treeInstance = new OdinMenuTree(true)
                {
                    { MENU_SKILL, new BattleAbilityRootView(_skills), EditorIcons.Clouds },
                    { MENU_BUFF, new BattleAbilityRootView(_buffs), EditorIcons.Clouds },
                    { MENU_BULLET, new BattleAbilityRootView(_bullets), EditorIcons.Clouds }
                };
            }

            AddMenuItem(MENU_SKILL, _skills);
            AddMenuItem(MENU_BUFF, _buffs);
            AddMenuItem(MENU_BULLET, _bullets);

            return _treeInstance;
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
                        ShowVariableWindow.OpenWindow(this,abilityView.Data);
                    }
                }
                
                
            }
            SirenixEditorGUI.EndHorizontalToolbar();
        }
    }
}