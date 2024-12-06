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
    
    public abstract class ATreeItem : TreeViewItem
    {
        /// <summary>
        /// ability周期树
        /// </summary>
        public AbilityCycleTree Tree { get; }

        /// <summary>
        /// ability节点数据
        /// </summary>
        public ATreeEditorNode EditorNode { get; }

        /// <summary>
        /// 按钮样式
        /// </summary>
        protected readonly NodeStyle Style;
        /// <summary>
        /// 右键菜单
        /// </summary>
        private readonly GenericMenu _menu;

        public new ATreeItem parent => (ATreeItem)base.parent;

        protected ATreeItem(AbilityCycleTree tree, ATreeEditorNode editorNode) : base(editorNode.Id)
        {
            Tree = tree;
            EditorNode = editorNode;
          
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
            foreach (var nodeId in EditorNode.Children)
            {
                var childData = Tree.TreeData.NodeDict[nodeId];
                var childItem = ATreeItemExtensions.CreateTreeItem(Tree, childData);
                children.Add(childItem);
                childItem.BuildTree();
            }
        }

        public void AddChild(ATreeItem child)
        {
            
        }

        public void RemoveChild(object removeChild)
        {
            RemoveChild((ATreeItem)removeChild);
        }
        
        protected void RemoveChild(ATreeItem removeChild)
        {
            
        }
        
        protected abstract void OnCopy(ATreeItem parentItem,int idx);
        protected abstract void OnMove(ATreeItem parentItem);
        
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
}