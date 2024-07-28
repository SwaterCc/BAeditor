using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using Sirenix.OdinInspector.Editor;
using Sirenix.Utilities.Editor;
using UnityEditor;
using UnityEngine;
using SirenixEditorGUI = Sirenix.Utilities.Editor.SirenixEditorGUI;

namespace BattleAbility.Editor
{
    /// <summary>
    /// 用系列化的数据还原树状图
    /// </summary>
    public class LogicTreeDrawer
    {
        public readonly BattleAbilitySerializableTree TreeData;
        private bool _logicTreeFoldout = true;
        private readonly int _rootDrawerIdx = -1;
        private LogicTreeNodeDrawer _rootDrawer;
        private readonly LogicStageDrawer _parentDrawer;
        private List<LogicTreeNodeDrawer> _nodeDrawers = new List<LogicTreeNodeDrawer>();
        private int test = 1;

        private Vector2 _soroll = Vector2.zero;

        public LogicTreeDrawer(LogicStageDrawer parentDrawer, BattleAbilitySerializableTree treeData)
        {
            TreeData = treeData;
            _parentDrawer = parentDrawer;
            
        }

        /// <summary>
        /// 绘制树
        /// </summary>
        public void BuildTree()
        {
            SirenixEditorGUI.HorizontalLineSeparator();


            var mainBox = SirenixEditorGUI.BeginBox();
            SirenixEditorGUI.BeginBoxHeader();
            _logicTreeFoldout = SirenixEditorGUI.Foldout(_logicTreeFoldout, "(事件类型预览)");
            if (SirenixEditorGUI.Button("删除", ButtonSizes.Medium))
            {
                _parentDrawer.RemoveTree(this);
            }

            SirenixEditorGUI.EndBoxHeader();
            SirenixEditorGUI.BeginVerticalList();
            if (_logicTreeFoldout)
            {
                

                if (_rootDrawer == null && _rootDrawerIdx == -1)
                {
                    SirenixEditorGUI.BeginListItem();
                    if (Event.current.type == EventType.MouseDown)
                    {
                        var rect = GUIHelper.GetCurrentLayoutRect();
                        if (rect.Contains(Event.current.mousePosition) && Event.current.button == 1)
                        {
                            AddLogicTreeNodeWindow.OpenWindow(ENodeType.Event);
                        }
                    }
                    EditorGUILayout.LabelField("（右键点击这里创建事件节点）");
                    SirenixEditorGUI.EndListItem();
                }
                else
                {
                    //_rootDrawer.Draw();
                }

              
            }

            SirenixEditorGUI.EndVerticalList();
            SirenixEditorGUI.EndBox();
        }
    }
}