using System.Collections.Generic;
using System.Linq;
using Editor.AbilityEditor.SimpleWindow;
using Editor.BattleEditor.AbilityEditor;
using Hono.Scripts.Battle.Editor;
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
    public class PowerEditorMainWindow : OdinMenuEditorWindow
    {
        protected override OdinMenuTree BuildMenuTree()
        {
            var treeInstance = new OdinMenuTree(true);
            treeInstance.Config.DrawSearchToolbar = true;

            List<PowerMenuItemBase> rootItems = new List<PowerMenuItemBase>()
            {
                new SkillRootMenu(treeInstance, "Skill"),
                //new BuffRootMenu(treeInstance, "Buff"),
                //new BulletRootMenu(treeInstance, "Bullet"),
                //new AbilityRootMenu(treeInstance),
            };

            treeInstance.MenuItems.AddRange(rootItems);

            foreach (var root in rootItems)
            {
                root.BuildTree();
            }

            return treeInstance;
        }

        protected override void OnDestroy() { }

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

            var parent = selected.Parent;
            string selectLabel = selected.Name;
            while (parent != null)
            {
                selectLabel = parent.Name + "->" + selectLabel;
                parent = parent.Parent;
            }

            EditorGUILayout.LabelField(selectLabel);

            if (selected.Value is AbilityView abilityView)
            {
                if (SirenixEditorGUI.ToolbarButton(new GUIContent("保存")))
                {
                    abilityView.Save();
                }
            }

            SirenixEditorGUI.EndHorizontalToolbar();
        }
    }
}