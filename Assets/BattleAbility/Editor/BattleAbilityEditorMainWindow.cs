using System.Linq;
using EditorData.BattleEditorData;
using Sirenix.OdinInspector;
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
                typeof(BattleAbilityConfigItemList)) as BattleAbilityConfigItemList;;
        }

        private void AddTreeItem(string rootMenu,BattleAbilityConfigItemList data)
        {
            if (_treeInstance == null)
            {
                return;
            }
            
            foreach (var itemPair in data.Items)
            {
                _treeInstance.Add($"{rootMenu}/{itemPair.Value.ConfigId}",null);
            }
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
            
            AddTreeItem(MENU_SKILL,_skills);
            AddTreeItem(MENU_BUFF,_buffs);
            AddTreeItem(MENU_BULLET,_bullets);
            
            return _treeInstance;
        }

        protected override void OnBeginDrawEditors()
        {
            //绘制顶部栏数据
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
            SirenixEditorGUI.EndHorizontalToolbar();
        }
    }
}