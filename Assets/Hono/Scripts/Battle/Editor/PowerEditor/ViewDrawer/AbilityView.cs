using System;
using System.Collections.Generic;
using Hono.Scripts.Battle;
using Sirenix.OdinInspector.Editor;
using Sirenix.Utilities.Editor;
using UnityEditor;
using UnityEngine;

namespace Editor.AbilityEditor
{
    /// <summary>
    /// Ability界面
    /// </summary>
    public class AbilityView : AView
    {
        /// <summary>
        /// 能力基础数据
        /// </summary>
        public AbilityData AbilityData;
        
        
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
                        node.BelongGroupId = ((GroupNodeData)parentNode).groupId;
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

        public override void Load(string path)
        {
            
        }

        public override void Draw()
        {
           
        }

        public override void Save()
        {
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
                
                await AssetManager.Instance.ReloadAsset<AbilityData>(AbilityData.id);

                switch (AbilityData.Type)
                {
                    case EAbilityType.Skill:
                        await AssetManager.Instance.ReloadAsset<SkillData>(AbilityData.id);
                        break;
                    case EAbilityType.Buff:
                        await AssetManager.Instance.ReloadAsset<BuffData>(AbilityData.id);
                        break;
                    case EAbilityType.Bullet:
                        await AssetManager.Instance.ReloadAsset<BulletData>(AbilityData.id);
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
        private Dictionary<EAbilityCycle, AbilityCycleDrawBase> _cycleDrawer = new();

        private Vector2 _scrollViewPos = Vector2.zero;

        private Dictionary<EAbilityCycle, string> _cycleDesc;
        
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
            _cycleDrawer = new Dictionary<EAbilityCycle, AbilityCycleDrawBase>();
            AbilityData = ValueEntry.SmartValue.AbilityData;
            VarCollector.SetAbilityData(AbilityData);
            VarCollector.RefreshAllVariable();
        }

        private AbilityCycleDrawBase getDrawer(EAbilityCycle type, AbilityData data)
        {
            switch (type)
            {
                case EAbilityCycle.Init:
                    return new OnInitDrawer(EAbilityCycle.Init, data);
                case EAbilityCycle.PreExecute:
                    return new OnPreExecuteDrawer(EAbilityCycle.PreExecute, data);
                case EAbilityCycle.Executing:
                    return new OnExecutingDrawer(EAbilityCycle.Executing, data);
                case EAbilityCycle.EndExecute:
                    return new OnEndExecuteDrawer(EAbilityCycle.EndExecute, data);
            }

            return null;
        }
        
        protected override void DrawPropertyLayout(GUIContent label)
        {
            var itemShowView = this.ValueEntry.SmartValue;
            _scrollViewPos = GUILayout.BeginScrollView(_scrollViewPos, false, true);

            SirenixEditorGUI.BeginBox("基础数据");
            EditorGUIUtility.labelWidth = 100;
            SirenixEditorFields.IntField("配置ID", itemShowView.AbilityData.id);
            itemShowView.AbilityData.Desc = SirenixEditorFields.TextField("Desc", itemShowView.AbilityData.Desc);
            
            //tag需要工具
            //AbilityEditorTools.DrawIntList(itemShowView.AbilityData.Tags,"Tags(后续需要新的工具)",38);
            itemShowView.AbilityData.DefaultStartGroupId = SirenixEditorFields.IntField("默认开始阶段", itemShowView.AbilityData.DefaultStartGroupId);
            SirenixEditorGUI.EndBox();
            
            foreach (EAbilityCycle cycle in Enum.GetValues(typeof(EAbilityCycle)))
            {
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