using System.Collections.Generic;
using Sirenix.OdinInspector;
using Sirenix.Utilities.Editor;
using UnityEngine;

namespace BattleAbility.Editor
{
    /// <summary>
    /// 后续用组合模式优化
    /// </summary>
    public class LogicStageDrawer
    {
        private bool _mainFoldout = true;
        private BattleAbilityItemViewDrawer _parent;
        public readonly BattleAbilityLogicStage StageData;
        private List<LogicTreeDrawer> _treeDrawers = new();
        private LogicTreeDrawer _removeObj = null;
        public LogicStageDrawer(BattleAbilityItemViewDrawer parent,BattleAbilityLogicStage stageData)
        {
            _parent = parent;
            StageData = stageData;
            foreach (var treeData in stageData.SerializableTrees)
            {
                _treeDrawers.Add(new LogicTreeDrawer(this,treeData));
            }
        }
        
        public void DrawStage()
        {
            SirenixEditorGUI.BeginBox();
            SirenixEditorGUI.BeginBoxHeader();
            _mainFoldout = SirenixEditorGUI.Foldout(_mainFoldout, $"阶段{StageData.stageId}");
            if(SirenixEditorGUI.Button("删除", ButtonSizes.Medium))
            {
                _parent.removeStage(this);
            }
            SirenixEditorGUI.EndBoxHeader();
            if (_mainFoldout)
            {
                foreach (var fieldInfo in StageData.GetType().GetFields())
                {
                    var attrValue = BattleAbilitEditorHelper.GetFiledLabelAndType(fieldInfo);
                    BattleAbilitEditorHelper.DrawLabelAndUpdateValueByAttr(StageData, fieldInfo, attrValue.Item1,
                        attrValue.Item2);
                }
                
                GUILayout.Space(10);
                foreach (var treeDrawer in _treeDrawers)
                {
                    treeDrawer.BuildTree();
                    GUILayout.Space(6.18f);
                }
                if(SirenixEditorGUI.Button("新建节点树",ButtonSizes.Medium))
                {
                    var newTreeData = new BattleAbilitySerializableTree();
                    StageData.SerializableTrees.Add(newTreeData);
                    _treeDrawers.Add(new LogicTreeDrawer(this,newTreeData));
                }

                if (_removeObj != null)
                {
                    StageData.SerializableTrees.Remove(_removeObj.TreeData);
                    _treeDrawers.Remove(_removeObj);
                    _removeObj = null;
                }
            }
            SirenixEditorGUI.EndBox();
        }

        public void RemoveTree(LogicTreeDrawer treeDrawer)
        {
            _removeObj = treeDrawer;
        }
    }
}