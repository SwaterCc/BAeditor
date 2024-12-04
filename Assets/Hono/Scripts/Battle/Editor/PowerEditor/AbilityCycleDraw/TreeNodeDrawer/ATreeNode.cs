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
        public string ToolTip;
        public float Width;
        public GUIStyle ButtonStyle;
    }

    public abstract class ATreeNode : TreeViewItem
    {
        public AbilityCycleTree Tree { get; }
        public EAbilityNodeType NodeType { get; }
        public AbilityNodeData NodeData { get; private set; }
       
        /// <summary>
        /// 按钮样式
        /// </summary>
        protected readonly NodeStyle Style;
        /// <summary>
        /// 右键菜单
        /// </summary>
        private readonly GenericMenu _menu;

        protected ATreeNode(AbilityCycleTree tree)
        {
            
        }
        
        protected ATreeNode(AbilityCycleTree tree, AbilityNodeData nodeData) : base(nodeData.NodeId)
        {
            Tree = tree;
            NodeData = nodeData;
            _menu = new GenericMenu();

            Style = new NodeStyle()
            {
                Color = Color.black,
                Label = "null",
                ToolTip = "",
                Width = 456f,

                ButtonStyle = new GUIStyle(GUI.skin.button)
                {
                    alignment = TextAnchor.MiddleLeft
                }
            };
        }

        #region 右键菜单绘制

        protected abstract void buildMenu(GenericMenu menu);

        public void ShowMenu()
        {
            buildMenu(_menu);

            _menu.ShowAsContext();
        }

        #endregion

        #region 按钮绘制

        protected abstract void OnBtnClicked(Rect btnRect);

        public virtual void DrawItem(Rect lineRect)
        {
            var bgColor = GUI.backgroundColor;

            /*if (SettingWindow != null)
            {
                GUI.backgroundColor = new Color(2, 2, 2);
            }
            else
            {
                GUI.backgroundColor = Style.Color;
            }*/

            lineRect.width = Style.Width;
            var buttonText = Style.Label;

            if (string.IsNullOrEmpty(buttonText)) buttonText = "未定义描述";
            if (buttonText.Length > 70)
            {
                buttonText = buttonText.Substring(0, 70);
                buttonText += "...";
            }

            if (NodeData.NodeType != EAbilityNodeType.EAbilityCycle)
                buttonText = $"<{NodeData.NodeId}>" + buttonText;

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

        #endregion
        
        /// <summary>
        /// 序列化该节点
        /// </summary>
        public void Serialize()
        {
            if (NodeData == null)
            {
                NodeData = AbilityDataExtensions.CreateNode()
            }
            OnSerialize();
            foreach (var child in children)
            {
                if (child is ATreeNode aTreeNode)
                {
                    aTreeNode.Serialize();
                }
            }
        }

        protected abstract void OnSerialize();
    }

    public abstract class ATreeNode<T> : ATreeNode where T : AbilityNodeData
    {
        public new T NodeData { get; private set; }

        protected ATreeNode(AbilityCycleTree tree, AbilityNodeData nodeData) : base(tree, nodeData)
        {
            NodeData = nodeData == null ? null : (T)base.NodeData;
        }
    }
}