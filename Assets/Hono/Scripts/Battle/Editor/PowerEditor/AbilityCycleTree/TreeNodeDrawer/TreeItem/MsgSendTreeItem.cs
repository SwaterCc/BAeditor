using Editor.AbilityEditor.TreeItemWindow;
using Hono.Scripts.Battle;
using Hono.Scripts.Battle.AbilitySystem;
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

            for (var index = 0; index < Data.values.Count; index++)
            {
                AParams param = Data.values[index];
                msgParams +=  Data.msgParamKeys[index]+":" + param + "|" ;
            }

            return $"发送Msg Key:{key} 给ActorUid:{Data.actorUid}, 参数列表| {msgParams}";
        }

        protected override void OnBtnClicked(Rect btnRect)
        {
            ANodeSettingWindow.Open<MsgSendSettingWindow>(this);
        }
    }
}