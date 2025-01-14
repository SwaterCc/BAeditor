using System;
using System.Collections.Generic;
using Hono.Scripts.Battle;
using Sirenix.Utilities;
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
        public AEditorTreeNode Node { get; }

        /// <summary>
        /// 按钮颜色
        /// </summary>
        public Color ButtonBackGroundColor = Color.black;
        /// <summary>
        /// 按钮宽度
        /// </summary>
        public float ButtonWidth;
        /// <summary>
        /// 自适应宽度
        /// </summary>
        public bool AutoSize;
        /// <summary>
        /// 按钮字体对齐
        /// </summary>
        public TextAnchor ButtonTextAnchor;
        /// <summary>
        /// 右键菜单
        /// </summary>
        private readonly GenericMenu _menu;
        /// <summary>
        /// 按钮选中时颜色
        /// </summary>
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

        protected ATreeItem(AbilityCycleTree tree, AEditorTreeNode node) : base(node.Id)
        {
            Tree = tree;
            Node = node;
            ButtonTextAnchor = TextAnchor.MiddleLeft;

            _menu = new GenericMenu();
            //添加节点
            addMenu("添加节点/CallFunction", ERightClickOperationType.AddFunctionChild,    new FunctionNodeData());
            addMenu("添加节点/BranchGroup",  ERightClickOperationType.AddBranchGroupChild, new BranchGroupNodeData());
            addMenu("添加节点/Branch",       ERightClickOperationType.AddBranchChild,      new BranchNodeData());
            addMenu("添加节点/Listener",     ERightClickOperationType.AddListenerChild,    new ListenerNodeData());
            addMenu("添加节点/Group",        ERightClickOperationType.AddGroupChild,       new GroupNodeData());
            addMenu("添加节点/Timer",        ERightClickOperationType.AddTimerChild,       new TimerNodeData());
            addMenu("添加节点/Repeat",       ERightClickOperationType.AddRepeatChild,      new RepeatNodeData());
            addMenu("添加节点/Attr",         ERightClickOperationType.AddAttrChild,        new AttrModifyNodeData());
            addMenu("添加节点/Variable",     ERightClickOperationType.AddVariableChild,    new VariableNodeData());
            addMenu("添加节点/GroupSwitch",  ERightClickOperationType.AddGroupSwitchChild, new GroupSwitchNodeData());
            addMenu("添加节点/SendMsg",      ERightClickOperationType.AddMsgSendChild,     new MsgSendNodeData());
            //基础操作
            addMenu("复制",   ERightClickOperationType.Copy,       null);
            addMenu("粘贴",   ERightClickOperationType.Paste,      Node);
            addMenu("删除节点", ERightClickOperationType.RemoveSelf, Node);
        }

        /// <summary>
        /// 构建树
        /// </summary>
        protected void OnTreeBuild()
        {
            //根据数据构造树
            foreach (var editorNode in Node.Children)
            {
                var child = AbilityEditorUtility.CreateTreeItem(Tree, editorNode);
                AddChild(child);
                child.OnTreeBuild();
            }

            //Tree.SetExpanded(id, !children.IsNullOrEmpty());
        }

        private void addMenu(string label, ERightClickOperationType operation, object param)
        {
            var rightMenuInfo = new MenuInfo
            {
                Label = label,
                OperationType = operation,
                Param = param
            };

            switch (operation)
            {
                case ERightClickOperationType.AddFunctionChild:
                case ERightClickOperationType.AddBranchGroupChild:
                case ERightClickOperationType.AddBranchChild:
                case ERightClickOperationType.AddListenerChild:
                case ERightClickOperationType.AddGroupChild:
                case ERightClickOperationType.AddTimerChild:
                case ERightClickOperationType.AddRepeatChild:
                case ERightClickOperationType.AddVariableChild:
                case ERightClickOperationType.AddAttrChild:
                case ERightClickOperationType.AddGroupSwitchChild:
                case ERightClickOperationType.AddMsgSendChild:
                    rightMenuInfo.Function += data =>
                    {
                        Node.AddChild(new AEditorTreeNode((AbilityNodeData)data));
                        Tree.Reload();
                    };
                    break;
                case ERightClickOperationType.RemoveSelf:
                    rightMenuInfo.Function += _ =>
                    {
                        Node.RemoveSelfFromParent();
                        Tree.Reload();
                    };
                    break;
                case ERightClickOperationType.Copy:
                    rightMenuInfo.Function += _ =>
                    {
                        AbilityEditorUtility.SaveCopyItems(Tree.GetSelection());
                        Tree.Reload();
                    };
                    break;
                case ERightClickOperationType.Paste:
                    rightMenuInfo.Function += _ =>
                    {
                        Node.AddChildren(AbilityEditorUtility.GetCopyItems());
                        AbilityEditorUtility.ClearCopyCache();
                        Tree.Reload();
                    };
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
                if (menuInfo.OperationType == ERightClickOperationType.Paste)
                {
                    if (AbilityEditorUtility.GetCopyItems().IsNullOrEmpty())
                    {
                        state = ERightMenuState.Disable;
                    }
                }

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

        /// <summary>
        /// 尝试移动到指定父节点的某个位置
        /// </summary>
        /// <param name="newParent"></param>
        /// <param name="insertIdx"></param>
        public void TryMoveTo(ATreeItem newParent, int insertIdx)
        {
            if (checkIsAllowMove(newParent))
            {
                var nodeParent = Node.Parent;

                if (nodeParent == newParent.Node)
                {
                    var oldIndex = nodeParent.Children.IndexOf(Node);
                    var removeIdx = insertIdx < oldIndex ? oldIndex + 1 : oldIndex;
                    nodeParent.InsertChild(Node, insertIdx);
                    nodeParent.Children.RemoveAt(removeIdx);
                }
                else
                {
                    newParent.Node.InsertChild(Node, insertIdx);
                    nodeParent.RemoveChild(Node);
                }
            }
        }

        protected abstract bool checkIsAllowMove(ATreeItem newParent);

        protected abstract string getButtonText();
        protected abstract void OnBtnClicked(Rect btnRect);

        public void DrawItem(Rect lineRect)
        {
            var buttonText = getButtonText();
            if (string.IsNullOrEmpty(buttonText))
                buttonText = "未定义描述";

            if (!string.IsNullOrEmpty(Node.Data.desc))
            {
                buttonText = Node.Data.desc;
            }

            var bgColor = GUI.backgroundColor;
            GUI.backgroundColor = _nodeEditorWindowIsOpen ? _onNodeEditorWindowOpenColor : ButtonBackGroundColor;
            var buttonStyle = new GUIStyle(GUI.skin.button)
            {
                alignment = ButtonTextAnchor
            };
            var buttonContent = new GUIContent(buttonText, getButtonText());
            lineRect.width = ButtonWidth > 0 ? ButtonWidth : Mathf.Max(200f, buttonStyle.CalcSize(buttonContent).x);
            if (GUI.Button(lineRect, buttonContent, buttonStyle))
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

        protected ATreeItem(AbilityCycleTree tree, AEditorTreeNode node) : base(tree, node)
        {
            Data = (T)Node.Data;
        }
    }
}