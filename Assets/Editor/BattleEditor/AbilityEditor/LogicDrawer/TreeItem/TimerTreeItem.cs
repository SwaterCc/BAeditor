
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
        public TimerTreeItem(int id, int depth, string name) : base(id, depth, name) { }
        public TimerTreeItem(AbilityNodeData nodeData) : base(nodeData) { }

        protected override Color getButtonColor()
        {
            return new Color(0.2f, 1.0f, 0.3f);
        }

        protected override string getButtonText()
        {
            return string.IsNullOrEmpty(NodeData.TimerNodeData.Desc)?"计时器":NodeData.TimerNodeData.Desc;
        }

        protected override string getItemEffectInfo()
        {
            return "计时器节点";
        }

        protected override void OnBtnClicked()
        {
            SettingWindow = TimerNodeDataWindow.GetWindow(NodeData);
            SettingWindow.Show();
            SettingWindow.Focus();
        }
    }
    
    public class TimerNodeDataWindow : BaseNodeWindow<TimerNodeDataWindow>, IWindowInit
    {
        private ParameterMaker _firstInterval;
        private ParameterMaker _interval;
        private ParameterMaker _maxCount;
        private string _desc;
        
        protected override void onInit()
        {
            _firstInterval = new ParameterMaker();
            ParameterMaker.Init(_firstInterval,NodeData.TimerNodeData.FirstInterval);
            _interval = new ParameterMaker();
            ParameterMaker.Init(_interval,NodeData.TimerNodeData.Interval);
            _maxCount = new ParameterMaker();
            ParameterMaker.Init(_maxCount,NodeData.TimerNodeData.MaxCount);
        }

        public override Rect GetPos()
        {
            return GUIHelper.GetEditorWindowRect().AlignCenter(400, 120);
        }

        private void OnGUI()
        {
            SirenixEditorGUI.BeginBox("配置计时器");
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("第一次调用间隔 float");
            _firstInterval.Draw();
            EditorGUILayout.EndHorizontal();
            
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("调用间隔 float");
            _interval.Draw();
            EditorGUILayout.EndHorizontal();
            
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("最大调用次数 int");
            _maxCount.Draw();
            EditorGUILayout.EndHorizontal();

            _desc = SirenixEditorFields.TextField("备注", _desc);
            
            if (SirenixEditorGUI.Button("保  存", ButtonSizes.Medium))
            {
                Save();
            }
            SirenixEditorGUI.EndBox();
        }
        
        private void Save()
        {
            NodeData.TimerNodeData.FirstInterval = _firstInterval.ToArray();
            NodeData.TimerNodeData.Interval = _interval.ToArray();
            NodeData.TimerNodeData.MaxCount = _maxCount.ToArray();
            NodeData.TimerNodeData.Desc = _desc;
            Close();
        }
    }
    
}