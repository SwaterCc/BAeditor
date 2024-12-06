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
        private OdinMenuTree _treeInstance;
        private List<PMenuRootItem> _rootItems = new List<PMenuRootItem>();

        protected override void OnEnable()
        {
            base.OnEnable();
            
            _treeInstance = new OdinMenuTree(true);
            _treeInstance.Config.DrawSearchToolbar = true;

            _rootItems = new List<PMenuRootItem>()
            {
                new SkillMenuRoot(_treeInstance, "Skill"),
                new BuffMenuRoot(_treeInstance, "Buff"),
                new BulletMenuRoot(_treeInstance, "Bullet"),
                new AbilityRoot(_treeInstance),
            };

            _treeInstance.MenuItems.AddRange(_rootItems);

            foreach (var root in _rootItems)
            {
                root.BuildTree();
            }
        }

        protected override OdinMenuTree BuildMenuTree()
        {
            return _treeInstance;
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