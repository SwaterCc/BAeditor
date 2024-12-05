using System;
using Hono.Scripts.Battle;
using UnityEditor;
using UnityEditor.IMGUI.Controls;
using UnityEngine;

namespace Editor.AbilityEditor
{
    public class NodeStyle
    {
        public Color Color;
        public string Label;
        public float Width;
        public GUIStyle ButtonStyle;
    }
    
    public abstract class ATreeNode : TreeViewItem
    {
        /// <summary>
        /// ability周期树
        /// </summary>
        public AbilityCycleTree Tree { get; }

        /// <summary>
        /// 节点类型
        /// </summary>
        public EAbilityNodeType NodeType { get; }

        /// <summary>
        /// ability节点数据
        /// </summary>
        public AbilityNodeData Data { get; }

        /// <summary>
        /// 按钮样式
        /// </summary>
        protected readonly NodeStyle Style;
        /// <summary>
        /// 右键菜单
        /// </summary>
        private readonly GenericMenu _menu;

        public new ATreeNode parent => (ATreeNode)base.parent;

        protected ATreeNode(AbilityCycleTree tree, AbilityNodeData data) : base(data.NodeId)
        {
            Tree = tree;
            Data = data;
            NodeType = data.NodeType;
            Style = new NodeStyle()
            {
                Color = Color.black,
                Label = "null",
                Width = 456f,

                ButtonStyle = new GUIStyle(GUI.skin.button)
                {
                    alignment = TextAnchor.MiddleLeft
                }
            };
            _menu = new GenericMenu();
        }

        /// <summary>
        /// 构建树
        /// </summary>
        public void BuildTree()
        {
            //根据数据构造树
            foreach (var nodeId in Data.ChildrenIds)
            {
                var childData = Tree.TreeData.NodeDict[nodeId];
                var childItem = ATreeNodeExtensions.GetNode(Tree, childData);
                children.Add(childItem);
                childItem.BuildTree();
            }
        }

        public void AddChild(ATreeNode child)
        {
            
        }

        public void RemoveChild(object removeChild)
        {
            RemoveChild((ATreeNode)removeChild);
        }
        
        protected void RemoveChild(ATreeNode removeChild)
        {
            
        }
        
        protected abstract void OnCopy(ATreeNode parentNode,int idx);
        protected abstract void OnMove(ATreeNode parentNode);
        
        protected abstract GenericMenu buildRightMenu(GenericMenu menu);
        public void ShowRightMenu()
        {
            buildRightMenu(_menu).ShowAsContext();
        }

        protected abstract void OnBtnClicked(Rect btnRect);

        public void DrawItem(Rect lineRect)
        {
            lineRect.width = Style.Width;
            var buttonText = Style.Label;

            if (string.IsNullOrEmpty(buttonText)) buttonText = "未定义描述";
            if (buttonText.Length > 70)
            {
                buttonText = buttonText.Substring(0, 70);
                buttonText += "...";
            }

            var bgColor = GUI.backgroundColor;
            GUI.backgroundColor = Style.Color;
            if (GUI.Button(lineRect, new GUIContent(buttonText, Style.Label), Style.ButtonStyle))
            {
                var btnRect = EditorGUIUtility.GetMainWindowPosition();

                if (Event.current.button == 0)
                {
                    OnBtnClicked(btnRect);
                }
            }

            GUI.backgroundColor = bgColor;
        }
    }

    public abstract class ATreeNode<T> : ATreeNode where T : AbilityNodeData
    {
        public new T Data { get; private set; }

        protected ATreeNode(AbilityCycleTree tree, AbilityNodeData data) : base(tree, data)
        {
            Data = data == null ? null : (T)base.Data;
        }
    }
}