using Editor.AbilityEditor.TreeItemWindow;
using Hono.Scripts.Battle;
using UnityEngine;

namespace Editor.AbilityEditor.TreeItem
{
    public class MsgSendTreeItem : ATreeItem<MsgSendNodeData>
    {
        public MsgSendTreeItem(AbilityCycleTree tree, AEditorTreeNode node) : base(tree, node)
        {
            ButtonBackGroundColor = new Color(0.5f, 1f, 0.3f);
        }
        protected override ERightMenuState checkRightMenuState(MenuInfo info)
        {
            //禁止添加子节点
            if (info.OperationType < ERightClickOperationType.AddChildOperation)
            {
                return ERightMenuState.NoShow;
            }

            return ERightMenuState.Enable;
        }

        protected override bool checkIsAllowMove(ATreeItem newParent)
        {
            if (newParent is AttrModifyTreeItem or VariableTreeItem or FunctionTreeItem or BranchGroupTreeItem)
            {
                return false;
            }
 
            return true;
        }

        protected override string getButtonText()
        {
            string key = string.IsNullOrEmpty(Data.msgKey) ? "未设置" : Data.msgKey;

            string msgParams = "";
            
            foreach (var param in Data.values)
            {
                msgParams += param + ",";
            }

            return $"SendMsg {key} to actor:{Data.actorUid}, param:{msgParams}";
        }

        protected override void OnBtnClicked(Rect btnRect)
        {
            ANodeSettingWindow.Open<MsgSendSettingWindow>(this);
        }
    }
}