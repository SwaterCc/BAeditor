using System;
using System.Collections.Generic;
using Battle;
using Battle.Def;
using Battle.Tools;
using Editor.AbilityEditor;
using Sirenix.OdinInspector;
using Sirenix.OdinInspector.Editor;
using Sirenix.Utilities.Editor;
using UnityEngine;

namespace BattleAbility.Editor
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

        private static CommonUtility.IdGenerator _idGenerator;

        public static CommonUtility.IdGenerator IdGenerator
        {
            get { return _idGenerator ??= CommonUtility.GetIdGenerator(); }
            set => _idGenerator = value;
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


            foreach (EAbilityCycleType cycle in Enum.GetValues(typeof(EAbilityCycleType)))
            {
                if (!_cycleDrawer.TryGetValue(cycle, out var drawer))
                {
                    drawer = getDrawer(cycle, itemShowView.Data);
                    if (drawer != null)
                        _cycleDrawer.Add(cycle, drawer);
                    else
                        throw new Exception("ssssssssssssssssssss");
                }

                drawer.DrawCycle();
            }

            GUILayout.EndScrollView();
        }
    }
}