using System;
using System.Collections.Generic;
using Hono.Scripts.Battle;
using UnityEditor;
using UnityEditor.IMGUI.Controls;
using UnityEngine;

namespace Editor.AbilityEditor
{
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
        /// 按钮颜色
        /// </summary>
        public Color ButtonBackGroundColor;
        /// <summary>
        /// 按钮宽度
        /// </summary>
        public float ButtonWidth;
        /// <summary>
        /// 按钮字体对齐
        /// </summary>
        public TextAnchor ButtonTextAnchor;
        /// <summary>
        /// 右键菜单
        /// </summary>
        private readonly GenericMenu _menu;
        private readonly GUIStyle _buttonStyle;
        private readonly Color _onNodeEditorWindowOpenColor = new Color(2, 2, 2);
        private bool _nodeEditorWindowIsOpen;

        /// <summary>
        /// 菜单状态
        /// </summary>
        public enum ERightMenuState
        {
            Enable,
            Disable,
            NoShow,
        }

        public class MenuInfo
        {
            public ERightClickOperationType OperationType;
            public string Label;
            public GenericMenu.MenuFunction2 Function;
            public object Param;
        }

        private readonly Dictionary<ERightClickOperationType, MenuInfo> _rightMenuItems = new();

        public new ATreeItem parent => (ATreeItem)base.parent;

        protected ATreeItem(AbilityCycleTree tree, ATreeEditorNode editorNode) : base(editorNode.Id)
        {
            Tree = tree;
            EditorNode = editorNode;

            ButtonWidth = 200f;

            _buttonStyle = new GUIStyle(GUI.skin.button)
            {
                alignment = TextAnchor.MiddleLeft,
            };

            _menu = new GenericMenu();
            //添加节点
            addMenu("添加节点/Action",      ERightClickOperationType.AddActionChild,      new ActionNodeData());
            addMenu("添加节点/BranchGroup", ERightClickOperationType.AddBranchGroupChild, new BranchGroupNodeData());
            addMenu("添加节点/Branch",      ERightClickOperationType.AddBranchChild,      new BranchNodeData());
            addMenu("添加节点/Listener",    ERightClickOperationType.AddListenerChild,    new ListenerNodeData());
            addMenu("添加节点/Group",       ERightClickOperationType.AddGroupChild,       new GroupNodeData());
            addMenu("添加节点/Timer",       ERightClickOperationType.AddTimerChild,       new TimerNodeData());
            addMenu("添加节点/Repeat",      ERightClickOperationType.AddRepeatChild,      new RepeatNodeData());
            addMenu("添加节点/Variable",    ERightClickOperationType.AddVariableChild,    new VariableNodeData());
            addMenu("添加节点/Attr",        ERightClickOperationType.AddAttrChild,        new AttrNodeData());
            //基础操作
            addMenu("复制",   ERightClickOperationType.Copy,       null);
            addMenu("粘贴",   ERightClickOperationType.Paste,      EditorNode);
            addMenu("删除节点", ERightClickOperationType.RemoveSelf, EditorNode);
        }

        /// <summary>
        /// 构建树
        /// </summary>
        public void BuildTree()
        {
            //根据数据构造树
            foreach (var editorNode in EditorNode.Children)
            {
                var childItem = ATreeItemExtensions.CreateTreeItem(Tree, editorNode);
                children.Add(childItem);
                childItem.BuildTree();
            }
        }

        protected void addMenu(string label, ERightClickOperationType operation, object param)
        {
            var rightMenuInfo = new MenuInfo
            {
                Label = label,
                OperationType = operation,
                Param = param
            };

            switch (operation)
            {
                case ERightClickOperationType.AddActionChild:
                case ERightClickOperationType.AddBranchGroupChild:
                case ERightClickOperationType.AddBranchChild:
                case ERightClickOperationType.AddListenerChild:
                case ERightClickOperationType.AddGroupChild:
                case ERightClickOperationType.AddTimerChild:
                case ERightClickOperationType.AddRepeatChild:
                case ERightClickOperationType.AddVariableChild:
                case ERightClickOperationType.AddAttrChild:
                    rightMenuInfo.Function += data => EditorNode.AddChild(new ATreeEditorNode((AbilityNodeData)data));
                    break;
                case ERightClickOperationType.RemoveSelf:
                    rightMenuInfo.Function += _ => EditorNode.RemoveSelfFromParent();
                    break;
                case ERightClickOperationType.Copy:
                    rightMenuInfo.Function += _ => AbilityCycleTreeUtility.SaveCopyItems(Tree.GetSelection());
                    break;
                case ERightClickOperationType.Paste:
                    rightMenuInfo.Function += _ => EditorNode.AddChildren(AbilityCycleTreeUtility.GetCopyItems());
                    break;
            }

            _rightMenuItems.Add(operation, rightMenuInfo);
        }

        protected MenuInfo addCustomMenu(string label,
            ERightClickOperationType operationType,
            GenericMenu.MenuFunction2 func,
            object param)
        {
            var rightMenuInfo = new MenuInfo
            {
                Label = label,
                Function = func,
                Param = param,
                OperationType = operationType
            };
            if (!_rightMenuItems.TryAdd(operationType, rightMenuInfo))
            {
                Debug.LogError($"尝试添加重复的操作{operationType}");
            }

            return rightMenuInfo;
        }

        protected abstract ERightMenuState checkRightMenuState(MenuInfo info);

        public void ShowRightMenu()
        {
            foreach (var pMenuInfo in _rightMenuItems)
            {
                var menuInfo = pMenuInfo.Value;
                var state = checkRightMenuState(menuInfo);
                switch (state)
                {
                    case ERightMenuState.Enable:
                        _menu.AddItem(new GUIContent(menuInfo.Label), false, menuInfo.Function, menuInfo.Param);
                        break;
                    case ERightMenuState.Disable:
                        _menu.AddDisabledItem(new GUIContent(menuInfo.Label));
                        break;
                }
            }

            _menu.ShowAsContext();
        }


        protected abstract string getButtonText();
        protected abstract void OnBtnClicked(Rect btnRect);

        public void DrawItem(Rect lineRect)
        {
            lineRect.width = ButtonWidth;
            var buttonText = getButtonText();

            if (string.IsNullOrEmpty(buttonText)) buttonText = "未定义描述";
            if (buttonText.Length > 70)
            {
                buttonText = buttonText.Substring(0, 70);
                buttonText += "...";
            }

            var bgColor = GUI.backgroundColor;
            GUI.backgroundColor = _nodeEditorWindowIsOpen ? _onNodeEditorWindowOpenColor : ButtonBackGroundColor;
            if (GUI.Button(lineRect, new GUIContent(buttonText, getButtonText()), _buttonStyle))
            {
                var btnRect = EditorGUIUtility.GetMainWindowPosition();

                if (Event.current.button == 0)
                {
                    OnBtnClicked(btnRect);
                }
            }

            GUI.backgroundColor = bgColor;
        }

        public void OnItemEditWindowOpen()
        {
            _nodeEditorWindowIsOpen = true;
        }

        public void OnItemEditWindowClose()
        {
            _nodeEditorWindowIsOpen = false;
        }
    }

    public abstract class ATreeItem<T> : ATreeItem where T : AbilityNodeData
    {
        public readonly T Data;

        protected ATreeItem(AbilityCycleTree tree, ATreeEditorNode editorNode) : base(tree, editorNode)
        {
            Data = (T)EditorNode.Data;
        }
    }
}