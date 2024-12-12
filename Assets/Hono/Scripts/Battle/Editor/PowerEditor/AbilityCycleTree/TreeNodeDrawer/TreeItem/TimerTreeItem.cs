using Hono.Scripts.Battle;
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

    public class TimerNodeDataWindow : ANodeSettingWindow<TimerNodeData>
    {
        private ParameterField _first;
        private ParameterField _interval;
        private ParameterField _maxCount;

        protected override void Init()
        {
            _first = new ParameterField(TempData.FirstInterval, "第一次触发间隔",      typeof(float));
            _interval = new ParameterField(TempData.Interval,   "触发间隔",         typeof(float));
            _maxCount = new ParameterField(TempData.MaxCount,   "触发次数(-1为无限次)", typeof(int));
        }

        protected override void Draw()
        {
            SirenixEditorGUI.BeginBox("配置计时器");
            _first.Draw();
            _interval.Draw();
            _maxCount.Draw();
            SirenixEditorGUI.EndBox();
        }
    }
}