using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using Sirenix.OdinInspector.Editor;
using Sirenix.Utilities.Editor;
using UnityEngine;

namespace BattleAbility.Editor
{
    /// <summary>
    /// 用系列化的数据还原树状图
    /// </summary>
    public class BattleAbilityLogicTreeDrawer
    {
        private List<BattleAbilityLogicStage> _stageDatas;
        private bool _logicFoldout = true;

        private List<LogicTreeStageDrawer> _stageDrawers = new();
        public BattleAbilityLogicTreeDrawer(List<BattleAbilityLogicStage> stageDatas)
        {
            _stageDatas = stageDatas;
        }

        /// <summary>
        /// 绘制树
        /// </summary>
        public void BuildTree()
        {
            SirenixEditorGUI.BeginBox();
            SirenixEditorGUI.EndBox();

            SirenixEditorGUI.BeginBox();
            SirenixEditorGUI.BeginBoxHeader();
            _logicFoldout = SirenixEditorGUI.Foldout(_logicFoldout, "逻辑开发");
            SirenixEditorGUI.EndBoxHeader();
            if (_logicFoldout)
            {
                foreach (var stageDrawer in _stageDrawers)
                {
                    stageDrawer.DrawStage();
                }

                if (SirenixEditorGUI.Button("添加阶段",ButtonSizes.Medium))
                {
                    _stageDatas.Add(new BattleAbilityLogicStage());
                }
            }
            SirenixEditorGUI.EndBox();

        }
    }
}