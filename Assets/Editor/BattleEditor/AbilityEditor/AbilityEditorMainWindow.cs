using System;
using System.Collections.Generic;
using System.Linq;
using Editor.AbilityEditor.SimpleWindow;
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
            window.position = GUIHelper.GetEditorWindowRect().AlignCenter(1000, 800);
            window.titleContent = new GUIContent("Ability编辑器");

            foreach (var eType in Enum.GetValues(typeof(EAbilityType)))
            {
                window.Reload((EAbilityType)eType);
            }
        }

        private readonly Dictionary<EAbilityType, string> _menuNames = new()
        {
            { EAbilityType.Other, "Other" },
            { EAbilityType.Skill, "Skill" },
            { EAbilityType.Buff, "Buff" },
            { EAbilityType.Bullet, "Bullet" },
            { EAbilityType.GameMode, "GameMode" }
        };

        private readonly Dictionary<EAbilityType, string> _abilityFolders = new()
        {
            { EAbilityType.Other, AbilityEditorPath.AbilityOtherPath },
            { EAbilityType.Skill, AbilityEditorPath.AbilitySkillPath },
            { EAbilityType.Buff, AbilityEditorPath.AbilityBuffPath },
            { EAbilityType.Bullet, AbilityEditorPath.AbilityBulletPath },
            { EAbilityType.GameMode, AbilityEditorPath.AbilityGameModePath }
        };

        public Dictionary<EAbilityType, string> AbilityFolders => _abilityFolders;
        

        private readonly Dictionary<EAbilityType, Dictionary<string, AbilityData>> _abilityPathWithDatas = new()
        {
            { EAbilityType.Other, new Dictionary<string, AbilityData>() },
            { EAbilityType.Skill, new Dictionary<string, AbilityData>() },
            { EAbilityType.Buff, new Dictionary<string, AbilityData>() },
            { EAbilityType.Bullet, new Dictionary<string, AbilityData>() },
            { EAbilityType.GameMode, new Dictionary<string, AbilityData>() }
        };

        public Dictionary<EAbilityType, Dictionary<string, AbilityData>> AbilityPathWithDatas => _abilityPathWithDatas;

        public static string[] LoadFolder(string folderPath)
        {
            // 获取指定文件夹路径下所有 .asset 文件的 GUID 数组
            string[] assetGUIDs = AssetDatabase.FindAssets("t:object", new[] { folderPath });

            // 创建一个字符串数组来存储文件名
            string[] assetFileNames = new string[assetGUIDs.Length];

            for (int i = 0; i < assetGUIDs.Length; i++)
            {
                // 使用 GUID 获取每个资产的路径
                string assetPath = AssetDatabase.GUIDToAssetPath(assetGUIDs[i]);

                // 检查文件是否为 .asset 文件
                if (assetPath.EndsWith(".asset"))
                {
                    // 获取文件名（带扩展名）
                    assetFileNames[i] = assetPath;
                }
            }

            return assetFileNames;
        }

        public void Reload(EAbilityType abilityType)
        {
            _abilityPathWithDatas[abilityType].Clear();
            var paths = LoadFolder(_abilityFolders[abilityType]);
            foreach (var fullPath in paths)
            {
                var data = AssetDatabase.LoadAssetAtPath<AbilityData>(fullPath);
                _abilityPathWithDatas[abilityType].Add(fullPath, data);
            }
            
        }

        protected override OdinMenuTree BuildMenuTree()
        {
            var treeInstance = new OdinMenuTree(false);
           
            foreach (var pMenuName in _menuNames)
            {
                treeInstance.Add(pMenuName.Value,
                    new BattleAbilityRootView(this, pMenuName.Key, _abilityPathWithDatas[pMenuName.Key]),
                    SdfIconType.BoxSeam);

                foreach (var abilityDataPair in _abilityPathWithDatas[pMenuName.Key])
                {
                    string itemName = pMenuName.Value + "/" + abilityDataPair.Value.ConfigId +
                                      $"({abilityDataPair.Value.Desc})";
                    treeInstance.Add(itemName, new AbilityView(abilityDataPair.Value), SdfIconType.CaretRight);
                }
            }

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
            MenuTree.DrawSearchToolbar();
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
                        ShowVariableWindow.OpenWindow(this, abilityView.AbilityData);
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