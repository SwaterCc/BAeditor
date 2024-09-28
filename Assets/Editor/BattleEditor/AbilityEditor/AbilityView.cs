﻿using System;
using System.Collections.Generic;
using Editor.BattleEditor.AbilityEditor;
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
                case EAbilityType.Bullet:
                    ExDrawer = new BulletDrawer();
                    break;
            }
            ExDrawer?.LoadAsset(AbilityData.ConfigId);
        }
        
        public static void UpdateGroupId(AbilityData data)
        {
            foreach (var pair in data.NodeDict)
            {
                var node = pair.Value;

                int parentId = node.ParentId;
                int count = 0;
                while (parentId > 0)
                {
                    var parentNode = data.NodeDict[parentId];
                    if (parentNode.NodeType == EAbilityNodeType.EGroup)
                    {
                        node.BelongGroupId = ((GroupNodeData)parentNode).GroupId;
                        break;
                    }

                    parentId = parentNode.ParentId;

                    if (count++ > 1000) {
	                    //保底
	                    Debug.LogWarning($"有ID错误节点{parentNode.NodeId}");
	                    break;
                    }
                }
            }
        }
        
        public void Save()
        {
            ExDrawer?.Save();
            UpdateGroupId(AbilityData);
            EditorUtility.SetDirty(AbilityData);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
            rtReload();
        }

        private async void rtReload()
        {
            if (EditorApplication.isPlaying)
            {
#if UNITY_EDITOR
                if(!DebugMode.Instance.AutoReloadAsset) return;
                
                await AssetManager.Instance.ReloadAsset<AbilityData>(AbilityData.ConfigId);

                switch (AbilityData.Type)
                {
                    case EAbilityType.Skill:
                        await AssetManager.Instance.ReloadAsset<SkillData>(AbilityData.ConfigId);
                        break;
                    case EAbilityType.Buff:
                        await AssetManager.Instance.ReloadAsset<BuffData>(AbilityData.ConfigId);
                        break;
                    case EAbilityType.Bullet:
                        await AssetManager.Instance.ReloadAsset<BulletData>(AbilityData.ConfigId);
                        break;
                    case EAbilityType.GameMode:
                        break;
                    case EAbilityType.Other:
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }

                AssetManager.Instance.CallReloadHandles();
#endif
            }
        }
    }

    public class AbilityViewDrawer : OdinValueDrawer<AbilityView>
    {
        private Dictionary<EAbilityAllowEditCycle, AbilityCycleDrawBase> _cycleDrawer = new();

        private Vector2 _scrollViewPos = Vector2.zero;

        private Dictionary<EAbilityAllowEditCycle, string> _cycleDesc;
        
        public static readonly VarCollector VarCollector = new();

        public static AbilityData AbilityData { get; private set; }

        public static AbilityNodeData BeforeClick { get; private set; }

        public static void NodeBtnClick(in AbilityNodeData clickData)
        {
            if (clickData.NodeType != EAbilityNodeType.EAbilityCycle)
            {
                BeforeClick = clickData;
            }
        }

        public static List<AbilityNodeData> CopyDataList = null;
        
        protected override void Initialize()
        {
            _cycleDrawer = new Dictionary<EAbilityAllowEditCycle, AbilityCycleDrawBase>();
            AbilityData = ValueEntry.SmartValue.AbilityData;
            VarCollector.SetAbilityData(AbilityData);
            VarCollector.RefreshAllVariable();
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