using Sirenix.OdinInspector;
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
        private LogicTreeNodeDrawer _rootDrawer;
        private readonly LogicStageDrawer _parentDrawer;

        public LogicTreeDrawer(LogicStageDrawer parentDrawer, BattleAbilitySerializableTree treeData)
        {
            TreeData = treeData;
            _parentDrawer = parentDrawer;

            if (treeData.rootKey >= 0 && treeData.allNodes.Count > 0)
            {
                SetRootDrawer(new LogicTreeEventNodeDrawer(this, treeData.allNodes[treeData.rootKey]));
            }
        }

        public void SetRootDrawer(LogicTreeNodeDrawer rootDrawer)
        {
            _rootDrawer = rootDrawer;
        }

        /// <summary>
        /// 绘制树
        /// </summary>
        public void BuildTree()
        {
            SirenixEditorGUI.HorizontalLineSeparator();

            SirenixEditorGUI.BeginBox();
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
                if (_rootDrawer == null)
                {
                    SirenixEditorGUI.BeginListItem();
                    if (Event.current.type == EventType.MouseDown)
                    {
                        var rect = GUIHelper.GetCurrentLayoutRect();
                        if (rect.Contains(Event.current.mousePosition) && Event.current.button == 1)
                        {
                            LogicTreeNodeSelectionWindow.OpenWindow(this, null);
                        }
                    }

                    EditorGUILayout.LabelField("（右键点击这里创建事件节点）");
                    SirenixEditorGUI.EndListItem();
                }
                else
                {
                    _rootDrawer.Draw();
                }
            }

            SirenixEditorGUI.EndVerticalList();
            SirenixEditorGUI.EndBox();
        }
    }
}