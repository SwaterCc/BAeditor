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

        public static void DrawResList(AbilityData data, string key, string desc)
        {
            /*if (!data.SpecializationData.TryGetValue(key, out var resCheck))
            {
                resCheck = new List<List<ResItem>>();
                data.SpecializationData.Add(key, new List<List<ResItem>>());
            }*/

            /*
            ResItem removeItem = null;
            List<ResItem> removeList = null;


            var checkList = (List<List<ResItem>>)resCheck;
            EditorGUILayout.LabelField(desc);
            SirenixEditorGUI.BeginVerticalList();

            foreach (var items in checkList)
            {
                SirenixEditorGUI.BeginListItem();
                EditorGUILayout.BeginHorizontal();
                if (GUILayout.Button("删除", GUILayout.Width(42)))
                {
                    removeList = items;
                }

                EditorGUILayout.LabelField("条件组：", GUILayout.Width(48));
                EditorGUILayout.BeginVertical();
                foreach (var item in items)
                {
                    EditorGUILayout.BeginHorizontal();
                    if (GUILayout.Button("-", GUILayout.Width(22)))
                    {
                        removeItem = item;
                    }

                    EditorGUIUtility.labelWidth = 70;
                    item.ResourceType =
                        (EBattleResourceType)SirenixEditorFields.EnumDropdown("消耗类型", item.ResourceType,
                            GUILayout.Width(180));
                    EditorGUIUtility.labelWidth = 50;
                    item.Flag = SirenixEditorFields.IntField("特征值", item.Flag);
                    item.Cost = SirenixEditorFields.FloatField("消耗值", item.Cost);
                    EditorGUILayout.EndHorizontal();
                }

                EditorGUILayout.EndVertical();

                if (GUILayout.Button("+", GUILayout.Width(22)))
                {
                    items.Add(new ResItem());
                }

                EditorGUILayout.EndHorizontal();
                SirenixEditorGUI.EndListItem();
                if (removeItem != null)
                {
                    items.Remove(removeItem);
                    removeItem = null;
                }
            }

            if (SirenixEditorGUI.Button("+", ButtonSizes.Medium))
            {
                checkList.Add(new List<ResItem>());
            }

            SirenixEditorGUI.EndVerticalList();

            if (removeList != null)
            {
                checkList.Remove(removeList);
            }*/
        }
    }
}