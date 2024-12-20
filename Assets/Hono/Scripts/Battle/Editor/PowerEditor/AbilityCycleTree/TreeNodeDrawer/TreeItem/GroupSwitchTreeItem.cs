using Hono.Scripts.Battle;
using Hono.Scripts.Battle.Editor.AbilityEditor;
using Sirenix.Utilities.Editor;
using UnityEngine;

namespace Editor.AbilityEditor.TreeItem
{
    public class GroupSwitchTreeItem : ATreeItem<GroupSwitchNodeData>
    {
        public GroupSwitchTreeItem(AbilityCycleTree tree, AEditorTreeNode node) : base(tree, node) { }
        protected override ERightMenuState checkRightMenuState(MenuInfo info)
        {
            if (info.OperationType < ERightClickOperationType.AddChildOperation)
            {
                return ERightMenuState.NoShow;
            }

            return ERightMenuState.Enable;
        }

        protected override string getButtonText()
        {
            var text = "";
            if (Data.switchGroupNow)
            {
                text = "立刻切换Group到" + Data.nextGroupId;
            }
            else
            {
                text = "设置下个GroupId为：" + Data.nextGroupId;
            }

            return text;
        }
        
        protected override bool checkIsAllowMove(ATreeItem newParent)
        {
            if (newParent is AttrTreeItem or VariableTreeItem or ActionTreeItem)
            {
                return false;
            }

            return true;
        }


        protected override void OnBtnClicked(Rect btnRect)
        {
            ANodeSettingWindow.Open<GroupSwitchSettingWindow>(this);
        }
    }


    public class GroupSwitchSettingWindow : ANodeSettingWindow<GroupSwitchNodeData>
    {
        private AParamsField _nextGroupId;
        protected override void Init()
        {
            _nextGroupId = new AParamsField(TempData.nextGroupId, "Next Group Id", typeof(int));
        }

        protected override void Draw()
        {
            SirenixEditorGUI.BeginBox("设置Group切换：");
            TempData.switchGroupNow = PowerEditorUIHelper.BoolDropField("是否立刻切换Group：", TempData.switchGroupNow);
            _nextGroupId.Draw();
            SirenixEditorGUI.EndBox();
        }
    }
}