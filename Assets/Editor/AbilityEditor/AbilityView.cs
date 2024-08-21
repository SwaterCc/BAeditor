using System;
using System.Collections.Generic;
using AbilityRes;
using Battle;
using BattleAbility.Editor;
using Sirenix.OdinInspector.Editor;
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
                    _funcShowInfoList = AssetDatabase.LoadAssetAtPath("Assets/Editor/EditorData/funcCache.asset",
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
    }

    public class AbilityViewDrawer : OdinValueDrawer<AbilityView>
    {
        private Dictionary<EAbilityCycleType, AbilityCycleDrawBase> _cycleDrawer = new();

        private bool _logicMainFoldout = true;

        private Vector2 _scrollViewPos = Vector2.zero;

        private Dictionary<EAbilityCycleType, string> _cycleDesc;

        protected override void Initialize()
        {
            _cycleDrawer = new Dictionary<EAbilityCycleType, AbilityCycleDrawBase>();
        }

        private AbilityCycleDrawBase getDrawer(EAbilityCycleType type, AbilityData data)
        {
            switch (type)
            {
                case EAbilityCycleType.OnPreAwardCheck:
                    return new OnPreAwardCheckDrawer(EAbilityCycleType.OnPreAwardCheck, data);
                case EAbilityCycleType.OnInit:
                    return new OnInitDrawer(EAbilityCycleType.OnInit, data);

                case EAbilityCycleType.OnPreExecuteCheck:
                    return new OnPreExecuteCheckDrawer(EAbilityCycleType.OnPreExecuteCheck, data);

                case EAbilityCycleType.OnPreExecute:
                    return new OnPreExecuteDrawer(EAbilityCycleType.OnPreExecute, data);

                case EAbilityCycleType.OnExecuting:
                    return new OnExecutingDrawer(EAbilityCycleType.OnExecuting, data);

                case EAbilityCycleType.OnEndExecute:
                    return new OnEndExecuteDrawer(EAbilityCycleType.OnEndExecute, data);
            }

            return null;
        }

        protected override void DrawPropertyLayout(GUIContent label)
        {
            var itemShowView = this.ValueEntry.SmartValue;
            _scrollViewPos = GUILayout.BeginScrollView(_scrollViewPos, false, true);

            SirenixEditorGUI.BeginBox();
            
            
            SirenixEditorGUI.EndBox();
            
            foreach (EAbilityCycleType cycle in Enum.GetValues(typeof(EAbilityCycleType)))
            {
                if(cycle == EAbilityCycleType.OnReady) continue;
                
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