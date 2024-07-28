using System.Collections.Generic;
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
        private LogicTreeNodeDrawer _parent = null;
        public LogicTreeNodeDrawer Parent
        {
            get => _parent;
            set => _parent = value;
        }
        private List<LogicTreeNodeDrawer> _children = new();
        
        private BattleAbilitySerializableTree _serializableTree;
        private readonly BattleAbilitySerializableTree.TreeNode _treeNode;
        
        public LogicTreeNodeDrawer(BattleAbilitySerializableTree treeData,BattleAbilitySerializableTree.TreeNode treeNode)
        {
            _serializableTree = treeData;
            _treeNode = treeNode;
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
                    AddLogicTreeNodeWindow.OpenWindow(_treeNode.eNodeType);
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
            foreach (var child in _children)
            {
                child.Draw();
            }
            EditorGUI.indentLevel--;
        }

        //绘制节点自己的信息
        protected abstract void drawSelf();
    }
}