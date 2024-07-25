using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using BattleAbility.Editor.BattleAbilityCustomAttribute;
using Sirenix.OdinInspector;
using Sirenix.OdinInspector.Editor;
using Sirenix.Serialization;
using Sirenix.Utilities.Editor;
using UnityEditor;
using UnityEngine;
using Object = System.Object;

namespace BattleAbility.Editor
{
    /// <summary>
    /// 能力配置界面 （基础配置 + 逻辑配置）
    /// </summary>
    public class BattleAbilityItemView
    {
        /// <summary>
        /// 能力基础数据
        /// </summary>
        public readonly BattleAbilityBaseConfig BaseConfig;

        /// <summary>
        /// 逻辑序列化数据
        /// </summary>
        public List<BattleAbilityLogicStage> LogicDatas;

        public BattleAbilityItemView(BattleAbilityBaseConfig baseConfig, List<BattleAbilityLogicStage> logicDatas)
        {
            BaseConfig = baseConfig;
            LogicDatas = logicDatas;
        }

        public string GetOdinMenuTreeItemLabel()
        {
            return $"{BaseConfig.ConfigId}->{BaseConfig.Name}";
        }
    }

    public class BattleAbilityItemViewDrawer : OdinValueDrawer<BattleAbilityItemView>
    {
        private BattleAbilityConfigDrawer _configDrawer;
        private BattleAbilityLogicTreeDrawer _treeDrawer;

        protected override void DrawPropertyLayout(GUIContent label)
        {
            var itemShowView = this.ValueEntry.SmartValue;
            
            _configDrawer ??= new BattleAbilityConfigDrawer(itemShowView.BaseConfig);
            _treeDrawer ??= new BattleAbilityLogicTreeDrawer(itemShowView.LogicDatas);
            
            GUILayout.BeginScrollView(Vector2.zero, false, true);

            _configDrawer.DrawBaseConfig();
            _treeDrawer.BuildTree();
           
            GUILayout.EndScrollView();
        }

        
    }
}