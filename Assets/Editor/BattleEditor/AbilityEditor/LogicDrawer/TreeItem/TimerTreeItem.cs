using Hono.Scripts.Battle;
using Sirenix.OdinInspector;
using Sirenix.Utilities;
using Sirenix.Utilities.Editor;
using UnityEditor;
using UnityEngine;

namespace Editor.AbilityEditor.TreeItem
{
    public class TimerTreeItem : AbilityLogicTreeItem
    {
        private new TimerNodeData _nodeData;

        public TimerTreeItem(AbilityLogicTree tree, AbilityNodeData nodeData) : base(tree, nodeData)
        {
            _nodeData = (TimerNodeData)base._nodeData;
        }

        protected override void buildMenu()
        {
            _menu.AddItem(new GUIContent("创建节点/添加Action"), false,
                AddChild, (EAbilityNodeType.EAction));
            _menu.AddItem(new GUIContent("创建节点/添加If"), false,
                AddChild, (EAbilityNodeType.EBranchControl));
            _menu.AddItem(new GUIContent("创建节点/Set变量"), false,
                AddChild, (EAbilityNodeType.EVariableSetter));
            _menu.AddItem(new GUIContent("创建节点/SetAttr"), false,
                AddChild, (EAbilityNodeType.EAttrSetter));
            if (checkHasParent(EAbilityNodeType.EEvent))
            {
                _menu.AddItem(new GUIContent("创建节点/创建Event节点"), false,
                    AddChild, (EAbilityNodeType.EEvent));
            }

            if (checkHasParent(EAbilityNodeType.ERepeat))
            {
                _menu.AddItem(new GUIContent("创建节点/创建Repeat节点"), false,
                    AddChild, (EAbilityNodeType.ERepeat));
            }
        }

        protected override Color getButtonColor()
        {
            return new Color(0.2f, 1.0f, 0.3f);
        }

        protected override string getButtonText()
        {
            return string.IsNullOrEmpty(_nodeData.Desc) ? "计时器" : _nodeData.Desc;
        }

        protected override string getButtonTips()
        {
            return "计时器节点";
        }

        protected override void OnBtnClicked(Rect btnRect)
        {
            SettingWindow = BaseNodeWindow<TimerNodeDataWindow, TimerNodeData>.GetSettingWindow(_tree.TreeData,
                _nodeData,
                (nodeData) => _nodeData = nodeData);
            SettingWindow.position = new Rect(btnRect.x, btnRect.y, 740, 140);
            SettingWindow.Show();
        }
    }

    public class TimerNodeDataWindow : BaseNodeWindow<TimerNodeDataWindow, TimerNodeData>,
        IAbilityNodeWindow<TimerNodeData>
    {
        private ParameterField _firstInterval;
        private ParameterField _interval;
        private ParameterField _maxCount;

        protected override void onInit()
        {
            _firstInterval = new ParameterField(_nodeData.FirstInterval, "第一次触发间隔", typeof(float));
            _interval = new ParameterField(_nodeData.Interval, "触发间隔", typeof(float));
            _maxCount = new ParameterField(_nodeData.MaxCount, "触发次数", typeof(int));
        }

        private void OnGUI()
        {
            SirenixEditorGUI.BeginBox("配置计时器");
            _firstInterval.Draw();
            _interval.Draw();
            _maxCount.Draw();
            _nodeData.Desc = SirenixEditorFields.TextField("备注", _nodeData.Desc);
            if (SirenixEditorGUI.Button("保  存", ButtonSizes.Medium))
            {
                Save();
            }
            SirenixEditorGUI.EndBox();
        }
    }
}