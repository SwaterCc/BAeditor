using System.Collections.Generic;
using Hono.Scripts.Battle;
using Sirenix.OdinInspector;
using Sirenix.Utilities.Editor;
using UnityEditor;
using UnityEditor.IMGUI.Controls;
using UnityEngine;

namespace Editor.AbilityEditor
{
    public class AbilityCycleDrawer
    {
        public AbilityView View { get; }
        public AbilityData Data { get; }
        public EAbilityCycle Cycle { get; }
        public AbilityNodeData CycleNode { get; }
        /// <summary>
        /// 周期View是否展开
        /// </summary>
        public bool CycleViewFoldout { get; set; }

        /// <summary>
        /// 树
        /// </summary>
        private readonly AbilityCycleTree _cycleTree;
        public AbilityCycleDrawer(AbilityView view, EAbilityCycle cycle, bool viewFoldoutShow = false)
        {
            View = view;
            Data = view.Data;
            Cycle = cycle;
            CycleViewFoldout = viewFoldoutShow;

            if (!Data.HeadNodeDict.TryGetValue(cycle, out var nodeId))
            {
                var cycleNodeData = (CycleNodeData)Data.GetNodeData(EAbilityNodeType.EAbilityCycle);
                cycleNodeData.cycleNodeData = cycle;
                cycleNodeData.ParentId = -1;

                Data.NodeDict.Add(cycleNodeData.NodeId, cycleNodeData);
                Data.HeadNodeDict.Add(Cycle, cycleNodeData.NodeId);
                CycleNode = cycleNodeData;
            }
            else
            {
                CycleNode = Data.NodeDict[nodeId];
            }

            _cycleTree = new AbilityCycleTree(Data, CycleNode);
        }
        
        public virtual void DrawCycle()
        {
            SirenixEditorGUI.BeginBox();
            var mainRect = GUIHelper.GetCurrentLayoutRect();
            SirenixEditorGUI.BeginBoxHeader();
            var headHeight = GUIHelper.GetCurrentLayoutRect().height;
            CycleViewFoldout = SirenixEditorGUI.Foldout(CycleViewFoldout, Cycle.ToString());
            if (SirenixEditorGUI.Button("展开树", ButtonSizes.Medium))
            {
                _cycleTree.ExpandAll();
            }

            SirenixEditorGUI.EndBoxHeader();
            if (CycleViewFoldout)
            {
                SirenixEditorGUI.BeginBox("编写逻辑");
                var boxRect = GUIHelper.GetCurrentLayoutRect();
                GUILayout.Box(" ", GUILayout.Height(_cycleTree.totalHeight), GUILayout.Width(boxRect.width)); //无所谓这个盒子，只是占位用的
                var treeRect = new Rect(boxRect.x, boxRect.y + headHeight, mainRect.width - 8, _cycleTree.totalHeight + 8);
                _cycleTree.OnGUI(treeRect);
                SirenixEditorGUI.EndBox();
            }

            SirenixEditorGUI.EndBox();
        }
        
    }
}