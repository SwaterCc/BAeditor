using Battle.Def;
using Sirenix.OdinInspector;
using Sirenix.Utilities.Editor;
using Unity.VisualScripting;
using UnityEditor;
using UnityEditor.IMGUI.Controls;
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
      
        private readonly LogicStageDrawer _parentDrawer;
        private LogicTreeView _treeView;
        [SerializeField]
        private TreeViewState _treeViewState;

        public LogicTreeDrawer(LogicStageDrawer parentDrawer, BattleAbilitySerializableTree treeData)
        {
            TreeData = treeData;
            _parentDrawer = parentDrawer;
            _treeViewState = new TreeViewState();
           
            if (treeData.rootKey >= 0 && treeData.allNodes.Count > 0)
            {
                CreateLogicTreeView(treeData.allNodes[treeData.rootKey]);
            }
        }

        public void CreateLogicTreeView(BattleAbilitySerializableTree.TreeNode eventNode)
        {
            _treeView = new LogicTreeView(_treeViewState, TreeData);
        }

        private float GetHeight()
        {
            ///36是树的行高，2是树节点的间隔
            return TreeData.allNodes.Count * (36 + 1) + 1;
        }

        /// <summary>
        /// 绘制树
        /// </summary>
        public void BuildTree()
        {
            SirenixEditorGUI.HorizontalLineSeparator();

            SirenixEditorGUI.BeginBox();
            var mainRect = GUIHelper.GetCurrentLayoutRect();
            SirenixEditorGUI.BeginBoxHeader();
            var headHeight = GUIHelper.GetCurrentLayoutRect().height;
            _logicTreeFoldout = SirenixEditorGUI.Foldout(_logicTreeFoldout, "(事件类型预览)");
            if (SirenixEditorGUI.Button("删除", ButtonSizes.Medium))
            {
                _parentDrawer.RemoveTree(this);
            }

            SirenixEditorGUI.EndBoxHeader();
            
            GUILayout.BeginVertical();
            if (_logicTreeFoldout)
            {
                if (_treeView == null)
                {
                    SirenixEditorGUI.BeginListItem();
                    if (Event.current.type == EventType.MouseDown)
                    {
                        var rect = GUIHelper.GetCurrentLayoutRect();
                        if (rect.Contains(Event.current.mousePosition))
                        {
                            Event.current.Use();
                            var newEventNode = BattleAbilitySerializableTree.GetNode(TreeData, ENodeType.Event);
                            newEventNode.depth = 0;
                            newEventNode.parentKey = -1;
                            TreeData.rootKey = newEventNode.nodeId;
                            CreateLogicTreeView(newEventNode);
                        }
                    }

                    EditorGUILayout.LabelField("（点击这里创建事件节点）");
                    SirenixEditorGUI.EndListItem();
                }
                else
                {
                    GUILayout.Box("asdasd", GUILayout.Height(GetHeight())); //无所谓这个盒子，只是占位用的
                    var boxRect = GUIHelper.GetCurrentLayoutRect();
                    var treeRect = new Rect(boxRect.x, boxRect.y + headHeight - 23f, mainRect.width - 8, GetHeight());
                    _treeView.OnGUI(treeRect);
                }
            }

            GUILayout.EndVertical();
            SirenixEditorGUI.EndBox();
        }
    }
}