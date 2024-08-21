using System.Collections.Generic;
using Battle;
using BattleAbility.Editor;
using Editor.AbilityEditor.AbilityNodeDraw;
using Sirenix.OdinInspector;
using Sirenix.Utilities.Editor;
using TreeEditor;
using UnityEditor;
using UnityEditor.IMGUI.Controls;
using UnityEngine;

namespace Editor.AbilityEditor
{
    public abstract class AbilityCycleDrawBase
    {
        public bool Foldout;
        
        public EAbilityCycleType CycleType;
        
        public AbilityData Data;
      
        public AbilityNodeData CycleNode;
        
        private AbilityLogicTreeDrawer _logicTreeDrawer;

        private TreeViewState _logicState;
        
        public AbilityCycleDrawBase(EAbilityCycleType cycleType, AbilityData data, bool foldout = true)
        {
            CycleType = cycleType;
            Data = data;
            Foldout = foldout;

            if (!data.HeadNodeDict.TryGetValue(cycleType, out var nodeId))
            {
                var cycleNodeData = AbilityData.GetNodeData(data,EAbilityNodeType.EAbilityCycle);
                cycleNodeData.CycleNodeData = cycleType;
                cycleNodeData.Parent = -1;
                
                data.NodeDict.Add(cycleNodeData.NodeId,cycleNodeData);
                data.HeadNodeDict.Add(CycleType, cycleNodeData.NodeId);
                CycleNode = cycleNodeData;
            }
            else
            {
                CycleNode = data.NodeDict[nodeId];
            }

            _logicState = new TreeViewState();
            _logicTreeDrawer = new AbilityLogicTreeDrawer(_logicState, data, CycleNode);
        }
        
        public virtual void DrawCycle()
        {
            SirenixEditorGUI.BeginBox();
            var mainRect = GUIHelper.GetCurrentLayoutRect();
            SirenixEditorGUI.BeginBoxHeader();
            var headHeight = GUIHelper.GetCurrentLayoutRect().height;
            Foldout = SirenixEditorGUI.Foldout(Foldout, CycleType.ToString());
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
                GUILayout.Box(" ", GUILayout.Height(GetHeight()),GUILayout.Width(boxRect.width)); //无所谓这个盒子，只是占位用的
                var treeRect = new Rect(boxRect.x, boxRect.y + headHeight, mainRect.width - 8, GetHeight() + 8);
                _logicTreeDrawer.OnGUI(treeRect);
                SirenixEditorGUI.EndBox();
            }

            SirenixEditorGUI.EndBox();
        }
        
        protected virtual void drawEx() { }

        private float GetHeight()
        {
            //var count = _logicTreeDrawer.ro.ChildrenIds.Count > 0 ? CycleNode.ChildrenIds.Count : 1;
            return _logicTreeDrawer.totalHeight;
        }
    }
}