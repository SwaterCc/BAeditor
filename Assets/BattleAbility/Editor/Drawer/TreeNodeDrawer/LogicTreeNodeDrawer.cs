using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using Sirenix.Utilities.Editor;
using UnityEditor;
using UnityEngine;

namespace BattleAbility.Editor
{
    /// <summary>
    /// 绘制树节点的抽象类
    /// </summary>
    public abstract class LogicTreeNodeDrawer
    {
        //private 
        private readonly LogicTreeNodeDrawer _parent;
        
        private List<LogicTreeNodeDrawer> _childrenDrawer = new();

        private BattleAbilitySerializableTree _serializableTree;
        private readonly BattleAbilitySerializableTree.TreeNode _nodeData;

        public LogicTreeNodeDrawer(BattleAbilitySerializableTree treeData,
            BattleAbilitySerializableTree.TreeNode nodeData, LogicTreeNodeDrawer parent)
        {
            _serializableTree = treeData;
            _nodeData = nodeData;
            _parent = parent;
            foreach (var idx in _nodeData.childIds)
            {
                if (_serializableTree.allNodes.TryGetValue(idx, out var node))
                {
                    LogicTreeNodeDrawer drawer = null;
                    switch (node.eNodeType)
                    {
                        case ENodeType.Event:
                            drawer = new LogicTreeEventNodeDrawer(_serializableTree, node);
                            break;
                        case ENodeType.Condition:
                            drawer = new LogicTreeConditionNodeDrawer(_serializableTree, node, this);
                            break;
                        case ENodeType.Action:
                            drawer = new LogicTreeActionNodeDrawer(_serializableTree, node, this);
                            break;
                        case ENodeType.Variable:
                            drawer = new LogicTreeVariableNodeDrawer(_serializableTree, node, this);
                            break;
                    }
                    _childrenDrawer.Add(drawer);
                }
            }
        }

        protected void treeButton(string btnText, float buttonWidth, Action action)
        {
            if (GUILayout.Button(btnText, GUILayout.Width(buttonWidth)))
            {
                if (Event.current.button == 0)
                {
                    action.Invoke();
                }
            }
        }

        public void Draw()
        {
            SirenixEditorGUI.BeginListItem();
            if (Event.current.type == EventType.MouseDown)
            {
                var rect = GUIHelper.GetCurrentLayoutRect();
                if (rect.Contains(Event.current.mousePosition) && Event.current.button == 1)
                {
                    //点击当前Item创建添加节点页面
                    AddLogicTreeNodeWindow.OpenWindow(_nodeData.eNodeType);
                }
            }

            EditorGUILayout.BeginHorizontal();
            if (_parent == null)
            {
                //头节点
                EditorGUILayout.LabelField("+");
            }
            else
            {
                EditorGUILayout.LabelField("∟");
            }

            drawSelf();
            EditorGUILayout.EndHorizontal();
            EditorGUILayout.LabelField("(点击右键，添加对应节点)");
            SirenixEditorGUI.EndListItem();

            EditorGUI.indentLevel++;
            foreach (var child in _childrenDrawer)
            {
                child.Draw();
            }

            EditorGUI.indentLevel--;
        }

        //绘制节点自己的信息
        protected abstract void drawSelf();
    }
}