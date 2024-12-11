using System;
using System.Collections.Generic;
using Hono.Scripts.Battle;
using Sirenix.OdinInspector;
using Sirenix.Utilities.Editor;
using UnityEditor;
using UnityEngine;

namespace Editor.AbilityEditor.TreeItem
{
    public class GroupTreeItem : ATreeItem<GroupNodeData>
    {
        public GroupTreeItem(AbilityCycleTree tree, AEditorTreeNode data) : base(tree, data)
        {
            ButtonBackGroundColor = new Color(0, 0.5f, 1.5f);
        }

        protected override ERightMenuState checkRightMenuState(MenuInfo info)
        {
            throw new NotImplementedException();
        }

        protected override string getButtonText()
        {
            return $"GroupID <{Data.groupId}> ";
        }

        protected override void OnBtnClicked(Rect btnRect)
        {
            ANodeSettingWindow.Open<GroupSettingWindow>(this);
        }
    }

    public class GroupSettingWindow : ANodeSettingWindow<GroupNodeData>
    {
        protected override void Init() { }

        protected override void Draw()
        {
            SirenixEditorGUI.BeginBox("设置Group");

            TempData.groupId = SirenixEditorFields.IntField("阶段Id", TempData.groupId);
            TempData.autoNext = EditorGUILayout.Toggle("阶段Id", TempData.autoNext);
            if (TempData.autoNext)
            {
                TempData.defaultNextGroupId = SirenixEditorFields.IntField("默认下一阶段Id", TempData.defaultNextGroupId);
            }

            SirenixEditorGUI.EndBox();
        }
    }
}