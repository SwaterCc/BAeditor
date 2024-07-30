using System;
using System.Linq;
using EditorData.BattleEditorData;
using Sirenix.OdinInspector.Editor;
using Sirenix.Utilities;
using Sirenix.Utilities.Editor;
using UnityEditor;
using UnityEngine;

namespace BattleAbility.Editor
{
    /// <summary>
    /// 战斗编辑器主窗口
    /// </summary>
    public class BattleAbilityEditorMainWindow : OdinMenuEditorWindow
    {
        [MenuItem("战斗编辑器/打开技能编辑器")]
        private static void OpenWindow()
        {
            var window = GetWindow<BattleAbilityEditorMainWindow>();
            window.position = GUIHelper.GetEditorWindowRect().AlignCenter(1000, 800);
            window.titleContent = new GUIContent("战斗编辑器");
        }

        private OdinMenuTree _treeInstance;

        private const string MENU_SKILL = "Skill";
        private const string MENU_BUFF = "Buff";
        private const string MENU_BULLET = "Bullet";

        private const string ASSET_NAME_SKILL = "Skills";
        private const string ASSET_NAME_BUFF = "Buffs";
        private const string ASSET_NAME_BULLET = "Bullets";

        private const string SKILL_DATA_PATH = "Assets/EditorData/BattleEditorData/Config/Skill/";
        private const string BUFF_DATA_PATH = "Assets/EditorData/BattleEditorData/Config/Buff/";
        private const string BULLET_DATA_PATH = "Assets/EditorData/BattleEditorData/Config/Bullet/";

        private BattleAbilityConfigItemList _skills;
        private BattleAbilityConfigItemList _buffs;
        private BattleAbilityConfigItemList _bullets;

        protected override void Initialize()
        {
            _skills = LoadBattleAbilityConfigList(ASSET_NAME_SKILL);
            _buffs = LoadBattleAbilityConfigList(ASSET_NAME_BUFF);
            _bullets = LoadBattleAbilityConfigList(ASSET_NAME_BULLET);
        }

        private BattleAbilityConfigItemList LoadBattleAbilityConfigList(string abilityTypeStr)
        {
            return AssetDatabase.LoadAssetAtPath($"Assets/EditorData/BattleEditorData/Config/{abilityTypeStr}.asset",
                typeof(BattleAbilityConfigItemList)) as BattleAbilityConfigItemList;
            ;
        }

        private void AddMenuItem(string rootMenu, BattleAbilityConfigItemList data)
        {
            if (_treeInstance == null)
            {
                return;
            }

            foreach (var itemPair in data.Items)
            {
                //测试用
                var battleAbilityData = LoadBattleAbilitySerializable(data.abilityType, itemPair.Value.configId);
                if (data != null)
                {
                    var battleSHowView =
                        new BattleAbilityItemView(battleAbilityData.baseConfig, battleAbilityData.stageDatas);
                    _treeInstance.Add($"{rootMenu}/{battleSHowView.GetOdinMenuTreeItemLabel()}",
                        battleSHowView);
                }
            }
        }

        private BattleAbilityData LoadBattleAbilitySerializable(EAbilityType eAbilityType, int id)
        {
            BattleAbilityData asset = null;
            switch (eAbilityType)
            {
                case EAbilityType.Skill:
                    asset = AssetDatabase.LoadAssetAtPath($"{SKILL_DATA_PATH}{id}.asset",
                        typeof(BattleAbilityData)) as BattleAbilityData;
                    break;
                case EAbilityType.Buff:
                    asset = AssetDatabase.LoadAssetAtPath($"{BUFF_DATA_PATH}{id}.asset",
                        typeof(BattleAbilityData)) as BattleAbilityData;
                    break;
                case EAbilityType.Bullet:
                    asset = AssetDatabase.LoadAssetAtPath($"{BULLET_DATA_PATH}{id}.asset",
                        typeof(BattleAbilityData)) as BattleAbilityData;
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(eAbilityType), eAbilityType, null);
            }
            return !asset ? null : asset;
        }

        protected override OdinMenuTree BuildMenuTree()
        {
            if (_treeInstance == null)
            {
                _treeInstance = new OdinMenuTree(true)
                {
                    { MENU_SKILL, null, EditorIcons.Clouds },
                    { MENU_BUFF, null, EditorIcons.Clouds },
                    { MENU_BULLET, null, EditorIcons.Clouds }
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
            var toolbarHeight = this.MenuTree.Config.SearchToolbarHeight;

            SirenixEditorGUI.BeginHorizontalToolbar(toolbarHeight);
            {
                if (selected != null)
                {
                    GUILayout.Label(selected.Name);
                }

                if (SirenixEditorGUI.ToolbarButton(new GUIContent("Create Item")))
                {
                }

                if (SirenixEditorGUI.ToolbarButton(new GUIContent("Create Character")))
                {
                }
            }
            EditorGUILayout.LabelField($"mousePOs {Event.current.mousePosition}");
            SirenixEditorGUI.EndHorizontalToolbar();
        }
    }
}