using System.Collections.Generic;
using Sirenix.OdinInspector;
using Sirenix.OdinInspector.Editor;
using Sirenix.Utilities.Editor;
using UnityEngine;

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
        /// 阶段列表数据
        /// </summary>
        public List<BattleAbilityLogicStage> StageDatas;

        public BattleAbilityItemView(BattleAbilityBaseConfig baseConfig, List<BattleAbilityLogicStage> stageDatas)
        {
            BaseConfig = baseConfig;
            StageDatas = stageDatas;
        }

        public string GetOdinMenuTreeItemLabel()
        {
            return $"{BaseConfig.ConfigId}->{BaseConfig.Name}";
        }
    }

    public class BattleAbilityItemViewDrawer : OdinValueDrawer<BattleAbilityItemView>
    {
        private ConfigDrawer _configDrawer;
        private readonly List<LogicStageDrawer> _stageDrawers = new();
        private bool _logicMainFoldout = true;
        private Vector2 _scrollViewPos = Vector2.zero;
        private LogicStageDrawer _removeObj = null;
        protected override void Initialize()
        {
            var itemShowView = this.ValueEntry.SmartValue;
            _configDrawer ??= new ConfigDrawer(itemShowView.BaseConfig);
            foreach (var stage in itemShowView.StageDatas)
            {
                _stageDrawers.Add(new LogicStageDrawer(this,stage));
            }
        }

        protected override void DrawPropertyLayout(GUIContent label)
        {
            var itemShowView = this.ValueEntry.SmartValue;
            _scrollViewPos = GUILayout.BeginScrollView(_scrollViewPos, false, true);

            _configDrawer.DrawBaseConfig();

            SirenixEditorGUI.BeginBox();
            SirenixEditorGUI.EndBox();
             
            SirenixEditorGUI.BeginBox();
            SirenixEditorGUI.BeginBoxHeader();
            _logicMainFoldout = SirenixEditorGUI.Foldout(_logicMainFoldout, "阶段逻辑");
            SirenixEditorGUI.EndBoxHeader();
            if (_logicMainFoldout)
            {
                foreach (var stageDrawer in _stageDrawers)
                {
                    stageDrawer.DrawStage();
                }

                if (SirenixEditorGUI.Button("添加阶段", ButtonSizes.Medium))
                {
                    var newStageData = new BattleAbilityLogicStage();
                    itemShowView.StageDatas.Add(newStageData);
                    _stageDrawers.Add(new LogicStageDrawer(this,newStageData));
                }
            }

            if (_removeObj != null)
            {
                itemShowView.StageDatas.Remove(_removeObj.StageData);
                _stageDrawers.Remove(_removeObj);
                _removeObj = null;
            }
            SirenixEditorGUI.EndBox();
            GUILayout.EndScrollView();
        }

        public void removeStage(LogicStageDrawer removeDrawer)
        {
            _removeObj = removeDrawer;
        }
    }
}