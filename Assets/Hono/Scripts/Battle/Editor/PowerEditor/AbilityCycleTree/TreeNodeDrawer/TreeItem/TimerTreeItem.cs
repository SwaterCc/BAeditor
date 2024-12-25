using Editor.AbilityEditor.TreeItemWindow;
using Hono.Scripts.Battle;
using Hono.Scripts.Battle.Base;
using Sirenix.OdinInspector;
using Sirenix.Utilities;
using Sirenix.Utilities.Editor;
using UnityEditor;
using UnityEngine;

namespace Editor.AbilityEditor.TreeItem
{
    public class TimerTreeItem : ATreeItem<TimerNodeData>
    {
        public TimerTreeItem(AbilityCycleTree tree, AEditorTreeNode data) : base(tree, data)
        {
            ButtonBackGroundColor = new Color(1f, 2.0f, 0.3f);
        }


        protected override ERightMenuState checkRightMenuState(MenuInfo info)
        {
            if (info.OperationType is 
                ERightClickOperationType.AddTimerChild or 
                ERightClickOperationType.AddGroupChild or 
                ERightClickOperationType.AddListenerChild)
            {
                return ERightMenuState.NoShow;
            }

            return ERightMenuState.Enable;
        }

        protected override string getButtonText()
        {
            return "计时器 首次调用间隔：" + Data.FirstInterval + " 间隔：" + Data.Interval + " 调用次数：" +
                   Data.MaxCount;
        }

        protected override bool checkIsAllowMove(ATreeItem newParent)
        {
            if (newParent is AttrTreeItem or VariableTreeItem or ActionTreeItem or TimerTreeItem or BranchGroupTreeItem)
            {
                return false;
            }

            if (newParent.HasParent<TimerTreeItem>())
            {
                return false;
            }
            
            return true;
        }
        
        protected override void OnBtnClicked(Rect btnRect)
        {
            ANodeSettingWindow.Open<TimerNodeDataWindow>(this);
        }
    }
}