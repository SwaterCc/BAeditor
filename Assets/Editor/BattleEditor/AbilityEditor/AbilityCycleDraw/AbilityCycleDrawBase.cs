using System.Collections.Generic;
using Hono.Scripts.Battle;
using Sirenix.OdinInspector;
using Sirenix.Utilities.Editor;
using UnityEditor;
using UnityEditor.IMGUI.Controls;
using UnityEngine;

namespace Editor.AbilityEditor
{
    public abstract class AbilityCycleDrawBase
    {
        public bool Foldout = false;

        public EAbilityAllowEditCycle AllowEditCycle;

        public AbilityData Data;

        public AbilityNodeData CycleNode;

        private AbilityLogicTree _logicTree;

        private TreeViewState _logicState;

        private bool _isFirst = true;
        
        public AbilityCycleDrawBase(EAbilityAllowEditCycle allowEditCycle, AbilityData data)
        {
            AllowEditCycle = allowEditCycle;
            Data = data;

            if (!data.HeadNodeDict.TryGetValue(allowEditCycle, out var nodeId))
            {
                var cycleNodeData = (EditorCycleNodeData)AbilityData.GetNodeData(data, EAbilityNodeType.EAbilityCycle);
                cycleNodeData.AllowEditCycleNodeData = allowEditCycle;
                cycleNodeData.ParentId = -1;

                data.NodeDict.Add(cycleNodeData.NodeId, cycleNodeData);
                data.HeadNodeDict.Add(AllowEditCycle, cycleNodeData.NodeId);
                CycleNode = cycleNodeData;
            }
            else
            {
                CycleNode = data.NodeDict[nodeId];
            }

            _logicState = new TreeViewState();
            _logicTree = new AbilityLogicTree(_logicState, data, CycleNode);
        }

        protected virtual bool getDefaultFoldout()
        {
            return true;
        }

        public virtual void DrawCycle()
        {
            if (_isFirst)
            {
                Foldout = getDefaultFoldout();
                _isFirst = false;
            }
            
            SirenixEditorGUI.BeginBox();
            var mainRect = GUIHelper.GetCurrentLayoutRect();
            SirenixEditorGUI.BeginBoxHeader();
            var headHeight = GUIHelper.GetCurrentLayoutRect().height;
            Foldout = SirenixEditorGUI.Foldout(Foldout, AllowEditCycle.ToString());
            SirenixEditorGUI.EndBoxHeader();
            if (Foldout)
            {
                //画额外内容
                drawEx();
                SirenixEditorGUI.BeginBox("编写逻辑");
                var boxRect = GUIHelper.GetCurrentLayoutRect();
                //画逻辑树
                GUILayout.Box(" ", GUILayout.Height(GetHeight()), GUILayout.Width(boxRect.width)); //无所谓这个盒子，只是占位用的
                var treeRect = new Rect(boxRect.x, boxRect.y + headHeight, mainRect.width - 8, GetHeight() + 8);
                _logicTree.OnGUI(treeRect);
                SirenixEditorGUI.EndBox();
            }

            SirenixEditorGUI.EndBox();
        }

        protected virtual void drawEx() { }

        private float GetHeight()
        {
            return _logicTree.totalHeight;
        }
    }
}