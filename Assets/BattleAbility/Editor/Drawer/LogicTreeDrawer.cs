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
        public BattleAbilitySerializableTree TreeData;
        private bool _logicTreeFoldout = true;
        private int _rootDrawerIdx = -1;
        private LogicTreeNodeDrawer _rootDrawer;
        private LogicStageDrawer _parentDrawer;
        private List<LogicTreeNodeDrawer> _nodeDrawers = new List<LogicTreeNodeDrawer>();
        private int test = 1;
        public LogicTreeDrawer(LogicStageDrawer parentDrawer,BattleAbilitySerializableTree treeData)
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
            if(SirenixEditorGUI.Button("删除", ButtonSizes.Medium))
            {
                _parentDrawer.RemoveTree(this);
            }
            SirenixEditorGUI.EndBoxHeader();
            SirenixEditorGUI.BeginVerticalList();
            if (_logicTreeFoldout)
            {
                SirenixEditorGUI.BeginListItem();
                
                if (Event.current.type == EventType.MouseDown)
                {
                    var rect = GUIHelper.GetCurrentLayoutRect();
                    if (rect.Contains(Event.current.mousePosition) && Event.current.button == 1)
                    {
                        Debug.Log($"curent KeyCode {Event.current.keyCode}");
                       AddLogicTreeNodeWindow.OpenWindow();
                    }
                }
              
                if (_rootDrawer == null && _rootDrawerIdx == -1)
                {
                    SirenixEditorGUI.BeginIndentedHorizontal(GUILayout.Width(120));
                    EditorGUILayout.LabelField("+",GUILayout.Width(15));
                    SirenixEditorGUI.Button("添加事件", ButtonSizes.Medium);
                    
                    SirenixEditorGUI.EndIndentedHorizontal();
                }
                SirenixEditorGUI.EndListItem();
               
             
            }
            SirenixEditorGUI.EndVerticalList();
            SirenixEditorGUI.EndBox();

        }
    }
}