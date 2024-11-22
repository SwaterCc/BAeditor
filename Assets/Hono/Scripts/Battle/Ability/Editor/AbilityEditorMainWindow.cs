using System;
using System.Collections.Generic;
using System.Linq;
using Editor.AbilityEditor.SimpleWindow;
using Editor.BattleEditor.AbilityEditor;
using Hono.Scripts.Battle;
using Sirenix.OdinInspector;
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
            window.position = GUIHelper.GetEditorWindowRect().AlignCenter(1600, 900);
            window.titleContent = new GUIContent("Ability编辑器");

            AbilityFunctionHelper.Init();
        }

        private readonly Dictionary<EAbilityType, string> _menuNames = new()
        {
            { EAbilityType.Skill, "Skill" },
            { EAbilityType.Buff, "Buff" },
            { EAbilityType.Bullet, "Bullet" },
            { EAbilityType.Other, "Other" },
        };

        protected override OdinMenuTree BuildMenuTree()
        {
            var treeInstance = new OdinMenuTree(false);
            treeInstance.Config.DrawSearchToolbar = true;
            //setting页面
            treeInstance.AddAssetAtPath("Setting", "Assets/BattleData/Ability");
            
            
            /*foreach (var pMenuName in _menuNames)
            {
                treeInstance.Add(pMenuName.Value,
                    new AItemRootView(this, pMenuName.Key, _abilityPathWithDatas[pMenuName.Key]),
                    SdfIconType.BoxSeam);

                foreach (var abilityDataPair in _abilityPathWithDatas[pMenuName.Key])
                {
                    string itemName = pMenuName.Value + "/" + abilityDataPair.Value.id +
                                      $"({abilityDataPair.Value.Desc})";
                    treeInstance.Add(itemName, new AbilityView(abilityDataPair.Value), SdfIconType.CaretRight);
                }
            }*/
            
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
            /*
            var selected = this.MenuTree.Selection.FirstOrDefault();
            if (selected?.Value is AbilityView abilityView)
            {
                abilityView.Save();
            }
            */
            
            base.OnDestroy();
            AbilityViewDrawer.CopyDataList = null;
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