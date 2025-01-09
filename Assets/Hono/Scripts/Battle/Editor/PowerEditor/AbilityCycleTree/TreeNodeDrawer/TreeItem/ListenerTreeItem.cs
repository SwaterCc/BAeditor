using System;
using System.Collections.Generic;
using Editor.AbilityEditor.TreeItemWindow;
using Editor.BattleEditor.AbilityEditor;
using Hono.Scripts.Battle;
using Hono.Scripts.Battle.Base;
using Hono.Scripts.Battle.Event;
using Sirenix.Utilities.Editor;
using UnityEditor;
using UnityEngine;

namespace Editor.AbilityEditor.TreeItem
{
    /// <summary>
    /// 监听节点，仅允许放置在Cycle节点下，仅在监听到事件或消息时执行其子节点
    /// </summary>
    public class ListenerTreeItem : ATreeItem<ListenerNodeData>
    {
        public ListenerTreeItem(AbilityCycleTree tree, AEditorTreeNode data) : base(tree, data)
        {
            ButtonBackGroundColor = new Color(0.4f, 1.8f, 1.5f);
        }

        protected override ERightMenuState checkRightMenuState(MenuInfo info)
        {
            if (info.OperationType is ERightClickOperationType.AddListenerChild or ERightClickOperationType.AddGroupChild)
            {
                return ERightMenuState.NoShow;
            }

            return ERightMenuState.Enable;
        }

        protected override bool checkIsAllowMove(ATreeItem newParent)
        {
            if (newParent is not CycleTreeItem)
            {
                return false;
            }

            return true;
        }

        protected override string getButtonText()
        {
            string text = "";
            if (Data.isEvent)
            {
                text = "等待事件：" + Enum.GetName(typeof(EEventType), Data.eventType);
            }
            else
            {
                var msgName = string.IsNullOrEmpty(Data.msgName) ? "未定义" : Data.msgName;
                text = "等待消息：" + msgName;
            }

            return text;
        }


        protected override void OnBtnClicked(Rect btnRect)
        {
            ANodeSettingWindow.Open<ListenerSettingWindow>(this);
        }
    }
}