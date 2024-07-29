using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using Sirenix.Utilities.Editor;
using UnityEditor;
using UnityEditor.Graphs;
using UnityEngine;
using Object = UnityEngine.Object;

namespace BattleAbility.Editor
{
    /// <summary>
    /// 绘制树节点的抽象类
    /// </summary>
    public abstract class LogicTreeNodeDrawer
    {
        public static LogicTreeNodeDrawer CreateNodeDrawerAndAddToTree(LogicTreeDrawer treeDrawer,
            LogicTreeNodeDrawer parent,
            ENodeType eNodeType)
        {
            var nodeData = new BattleAbilitySerializableTree.TreeNode();
            nodeData.nodeId = GetHashId(typeof(BattleAbilitySerializableTree.TreeNode)); //临时测试实现，后续改为可控生成id
            treeDrawer.TreeData.allNodes.Add(nodeData.nodeId, nodeData);

            LogicTreeNodeDrawer drawer = null;
            switch (eNodeType)
            {
                case ENodeType.Event:
                    nodeData.eNodeType = ENodeType.Event;
                    drawer = new LogicTreeEventNodeDrawer(treeDrawer, nodeData);
                    break;
                case ENodeType.Condition:
                    nodeData.eNodeType = ENodeType.Condition;
                    drawer = new LogicTreeConditionNodeDrawer(treeDrawer, nodeData, parent);
                    break;
                case ENodeType.Action:
                    nodeData.eNodeType = ENodeType.Action;
                    drawer = new LogicTreeActionNodeDrawer(treeDrawer, nodeData, parent);
                    break;
                case ENodeType.Variable:
                    nodeData.eNodeType = ENodeType.Variable;
                    drawer = new LogicTreeVariableNodeDrawer(treeDrawer, nodeData, parent);
                    break;
            }

            if (parent != null)
            {
                parent._nodeData.childIds.Add(nodeData.nodeId);
                parent._childrenDrawer.Add(drawer);
            }

            return drawer;
        }

        private static int count = 1;

        public static int GetHashId(Type type)
        {
            //还没实现
            return count++;
        }

        private readonly LogicTreeDrawer _treeDrawer;
        private readonly LogicTreeNodeDrawer _parent;

        private readonly List<LogicTreeNodeDrawer> _childrenDrawer = new();

        private BattleAbilitySerializableTree _serializableTree;
        private readonly BattleAbilitySerializableTree.TreeNode _nodeData;

        protected LogicTreeNodeDrawer(LogicTreeDrawer treeDrawer,
            BattleAbilitySerializableTree.TreeNode nodeData, LogicTreeNodeDrawer parent)
        {
            _treeDrawer = treeDrawer;
            _serializableTree = treeDrawer.TreeData;
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
                            drawer = new LogicTreeEventNodeDrawer(_treeDrawer, node);
                            break;
                        case ENodeType.Condition:
                            drawer = new LogicTreeConditionNodeDrawer(_treeDrawer, node, this);
                            break;
                        case ENodeType.Action:
                            drawer = new LogicTreeActionNodeDrawer(_treeDrawer, node, this);
                            break;
                        case ENodeType.Variable:
                            drawer = new LogicTreeVariableNodeDrawer(_treeDrawer, node, this);
                            break;
                    }

                    _childrenDrawer.Add(drawer);
                }
            }
        }

        public LogicTreeNodeDrawer GetParent() => _parent;

        public int GetNodeKey() => _nodeData.nodeId;
        
        protected void treeButton(string btnText, Color btnColor, Action action = null, float buttonWidth = 250f)
        {
            var bgColor = GUI.backgroundColor;
            GUI.backgroundColor = btnColor;
            if (GUILayout.Button(btnText, GUILayout.Width(buttonWidth)))
            {
                if (Event.current.button == 0)
                {
                    action?.Invoke();
                }
            }
            GUI.backgroundColor = bgColor;
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
                    LogicTreeNodeSelectionWindow.OpenWindow(_treeDrawer, this);
                }
            }

            EditorGUILayout.BeginHorizontal();
            if (_parent == null)
            {
                //头节点
                EditorGUILayout.LabelField("+", GUILayout.Width(10));
            }
            else
            {
                EditorGUILayout.LabelField("∟", GUILayout.Width(10 + EditorGUI.indentLevel * 20));
            }

            drawSelf();
            EditorGUILayout.LabelField("(点击右键，添加对应节点)");
            EditorGUILayout.EndHorizontal();
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


        public void RemoveChild(LogicTreeNodeDrawer nodeDrawer)
        {
            _parent._nodeData.childIds.Remove(nodeDrawer._nodeData.nodeId);
            _treeDrawer.TreeData.allNodes.Remove(nodeDrawer._nodeData.nodeId);
            _childrenDrawer.Remove(nodeDrawer);
        }

        public void RemoveSelf()
        {
            if (_parent != null)
            {
                _parent.RemoveChild(this);
            }
            else
            {
                _treeDrawer.TreeData.allNodes.Remove(_nodeData.nodeId);
                _treeDrawer.SetRootDrawer(null);
            }
            //删除父节点中自己的引用
            //删除数据索引
        }

        public void RemoveAllChildren()
        {
            foreach (var drawer in _childrenDrawer)
            {
                RemoveChild(drawer);
            }
        }
    }
}