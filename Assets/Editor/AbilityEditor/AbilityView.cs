using System;
using System.Collections.Generic;
using Hono.Scripts.Battle;
using Sirenix.OdinInspector.Editor;
using Sirenix.Utilities;
using Sirenix.Utilities.Editor;
using UnityEditor;
using UnityEngine;

namespace Editor.AbilityEditor
{
    /// <summary>
    /// 能力配置界面 （基础配置 + 逻辑配置）
    /// </summary>
    public class AbilityView
    {
        /// <summary>
        /// 能力基础数据
        /// </summary>
        public AbilityData Data;

        private static FuncShowInfoList _funcShowInfoList;

        public static FuncShowInfoList FuncShowInfoList
        {
            get
            {
                if (_funcShowInfoList == null)
                {
                    _funcShowInfoList = AssetDatabase.LoadAssetAtPath("Assets/Resources/AbilityRes/funcCache.asset",
                        typeof(FuncShowInfoList)) as FuncShowInfoList;
                }

                return _funcShowInfoList;
            }
        }

        public AbilityView(AbilityData baseConfig)
        {
            Data = baseConfig;
        }

        public string GetOdinMenuTreeItemLabel()
        {
            return $"{Data.ConfigId}->{Data.Name}";
        }

        public static void UpdateGroupId(AbilityData data)
        {
            foreach (var pair in data.NodeDict)
            {
                var node = pair.Value;

                int parentId = node.Parent;
                while (parentId > 0)
                {
                    var parentNode = data.NodeDict[parentId];
                    if (parentNode.NodeType == EAbilityNodeType.EGroup)
                    {
                        node.BelongGroupId = parentNode.GroupNodeData.GroupId;
                        break;
                    }

                    parentId = parentNode.Parent;
                }
            }
        }
        
        public void Save()
        {
            AbilityView.UpdateGroupId(Data);
            EditorUtility.SetDirty(Data);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }
    }

    public class AbilityViewDrawer : OdinValueDrawer<AbilityView>
    {
        private Dictionary<EAbilityAllowEditCycle, AbilityCycleDrawBase> _cycleDrawer = new();

        private bool _logicMainFoldout = true;

        private Vector2 _scrollViewPos = Vector2.zero;

        private Dictionary<EAbilityAllowEditCycle, string> _cycleDesc;

        protected override void Initialize()
        {
            _cycleDrawer = new Dictionary<EAbilityAllowEditCycle, AbilityCycleDrawBase>();
        }

        private AbilityCycleDrawBase getDrawer(EAbilityAllowEditCycle type, AbilityData data)
        {
            switch (type)
            {
                case EAbilityAllowEditCycle.OnInit:
                    return new OnInitDrawer(EAbilityAllowEditCycle.OnInit, data);

                case EAbilityAllowEditCycle.OnPreExecuteCheck:
                    return new OnPreExecuteCheckDrawer(EAbilityAllowEditCycle.OnPreExecuteCheck, data);

                case EAbilityAllowEditCycle.OnPreExecute:
                    return new OnPreExecuteDrawer(EAbilityAllowEditCycle.OnPreExecute, data);

                case EAbilityAllowEditCycle.OnExecuting:
                    return new OnExecutingDrawer(EAbilityAllowEditCycle.OnExecuting, data);

                case EAbilityAllowEditCycle.OnEndExecute:
                    return new OnEndExecuteDrawer(EAbilityAllowEditCycle.OnEndExecute, data);
            }

            return null;
        }
        
        protected override void DrawPropertyLayout(GUIContent label)
        {
            var itemShowView = this.ValueEntry.SmartValue;
            _scrollViewPos = GUILayout.BeginScrollView(_scrollViewPos, false, true);

            SirenixEditorGUI.BeginBox("基础数据");
            EditorGUIUtility.labelWidth = 100;
            SirenixEditorFields.IntField("配置ID", itemShowView.Data.ConfigId);
            itemShowView.Data.Name = SirenixEditorFields.TextField("Name", itemShowView.Data.Name);
            itemShowView.Data.Desc = SirenixEditorFields.TextField("Desc", itemShowView.Data.Desc);
            itemShowView.Data.IconPath = SirenixEditorFields.TextField("IconPath", itemShowView.Data.IconPath);
            //tag需要工具
            EditorGUILayout.LabelField("Tag需要工具");
            itemShowView.Data.DefaultStartGroupId = SirenixEditorFields.IntField("默认开始阶段", itemShowView.Data.DefaultStartGroupId);
            itemShowView.Data.PreCheckerVarName = SirenixEditorFields.TextField("检测阶段变量名", itemShowView.Data.PreCheckerVarName);
            SirenixEditorGUI.EndBox();
            
            
            
            foreach (EAbilityAllowEditCycle cycle in Enum.GetValues(typeof(EAbilityAllowEditCycle)))
            {
                if(cycle == EAbilityAllowEditCycle.OnReady) continue;
                
                if (!_cycleDrawer.TryGetValue(cycle, out var drawer))
                {
                    drawer = getDrawer(cycle, itemShowView.Data);
                    if (drawer != null)
                        _cycleDrawer.Add(cycle, drawer);
                }

                drawer?.DrawCycle();
            }

            GUILayout.EndScrollView();
        }
    }
}