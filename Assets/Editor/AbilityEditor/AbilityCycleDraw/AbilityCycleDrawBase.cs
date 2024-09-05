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

        private AbilityLogicTreeDrawer _logicTreeDrawer;

        private TreeViewState _logicState;

        private bool _isFirst = true;
        
        public AbilityCycleDrawBase(EAbilityAllowEditCycle allowEditCycle, AbilityData data)
        {
            AllowEditCycle = allowEditCycle;
            Data = data;

            if (!data.HeadNodeDict.TryGetValue(allowEditCycle, out var nodeId))
            {
                var cycleNodeData = AbilityData.GetNodeData(data, EAbilityNodeType.EAbilityCycle);
                cycleNodeData.allowEditCycleNodeData = allowEditCycle;
                cycleNodeData.Parent = -1;

                data.NodeDict.Add(cycleNodeData.NodeId, cycleNodeData);
                data.HeadNodeDict.Add(AllowEditCycle, cycleNodeData.NodeId);
                CycleNode = cycleNodeData;
            }
            else
            {
                CycleNode = data.NodeDict[nodeId];
            }

            _logicState = new TreeViewState();
            _logicTreeDrawer = new AbilityLogicTreeDrawer(_logicState, data, CycleNode);
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
                SirenixEditorGUI.BeginBox();
                //画额外内容
                drawEx();
                SirenixEditorGUI.EndBox();

                SirenixEditorGUI.BeginBox("编写逻辑");
                var boxRect = GUIHelper.GetCurrentLayoutRect();
                //画逻辑树
                GUILayout.Box(" ", GUILayout.Height(GetHeight()), GUILayout.Width(boxRect.width)); //无所谓这个盒子，只是占位用的
                var treeRect = new Rect(boxRect.x, boxRect.y + headHeight, mainRect.width - 8, GetHeight() + 8);
                _logicTreeDrawer.OnGUI(treeRect);
                SirenixEditorGUI.EndBox();
            }

            SirenixEditorGUI.EndBox();
        }

        protected virtual void drawEx() { }

        private float GetHeight()
        {
            return _logicTreeDrawer.totalHeight;
        }
    }
}