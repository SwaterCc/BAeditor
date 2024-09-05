using System;
using System.Collections.Generic;
using Hono.Scripts.Battle;
using Sirenix.OdinInspector.Editor;
using Sirenix.Utilities;
using Sirenix.Utilities.Editor;
using UnityEditor;
using UnityEngine;
using UnityEngine.Serialization;

namespace Editor.AbilityEditor
{
    public interface IExDrawer
    {
        public void LoadAsset(int id);
        public void Draw();
        public void Save();
    }
    
    /// <summary>
    /// 能力配置界面 （基础配置 + 逻辑配置）
    /// </summary>
    public class AbilityView
    {
        /// <summary>
        /// 能力基础数据
        /// </summary>
        public AbilityData AbilityData;

        public IExDrawer ExDrawer;
        public AbilityView(AbilityData abilityAbilityData)
        {
            AbilityData = abilityAbilityData;
            switch (AbilityData.Type)
            {
                case EAbilityType.Skill:
                    ExDrawer = new SkillDrawer();
                    break;
                case EAbilityType.Buff:
                    ExDrawer = new BuffDrawer();
                    break;
            }
            ExDrawer?.LoadAsset(AbilityData.ConfigId);
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
            ExDrawer?.Save();
            AbilityView.UpdateGroupId(AbilityData);
            EditorUtility.SetDirty(AbilityData);
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
            SirenixEditorFields.IntField("配置ID", itemShowView.AbilityData.ConfigId);
            itemShowView.AbilityData.Name = SirenixEditorFields.TextField("Name", itemShowView.AbilityData.Name);
            itemShowView.AbilityData.Desc = SirenixEditorFields.TextField("Desc", itemShowView.AbilityData.Desc);
            
            //tag需要工具
            AbilityEditorHelper.DrawIntList(itemShowView.AbilityData.Tags,"Tags(后续需要新的工具)",38);
            itemShowView.AbilityData.DefaultStartGroupId = SirenixEditorFields.IntField("默认开始阶段", itemShowView.AbilityData.DefaultStartGroupId);
            itemShowView.AbilityData.PreCheckerVarName = SirenixEditorFields.TextField("检测阶段变量名", itemShowView.AbilityData.PreCheckerVarName);
            SirenixEditorGUI.EndBox();
            
            itemShowView.ExDrawer?.Draw();
            
            foreach (EAbilityAllowEditCycle cycle in Enum.GetValues(typeof(EAbilityAllowEditCycle)))
            {
                if(cycle == EAbilityAllowEditCycle.OnReady) continue;
                
                if (!_cycleDrawer.TryGetValue(cycle, out var drawer))
                {
                    drawer = getDrawer(cycle, itemShowView.AbilityData);
                    if (drawer != null)
                        _cycleDrawer.Add(cycle, drawer);
                }

                drawer?.DrawCycle();
            }

            GUILayout.EndScrollView();
        }
    }
}