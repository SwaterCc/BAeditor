using Battle;
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
            return
                $"Timer First:{NodeData.TimerNodeData.FirstInterval}s,Interval:{NodeData.TimerNodeData.Interval}s,Count:{NodeData.TimerNodeData.MaxCount}";
        }

        protected override void OnBtnClicked()
        {
            TimerNodeDataWindow.Open(NodeData);
        }
    }
    
    public class TimerNodeDataWindow : BaseNodeWindow<TimerNodeDataWindow>, IWindowInit
    {
        private ParameterMaker _firstInterval;
        private ParameterMaker _interval;
        private ParameterMaker _maxCount;
        
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

            SirenixEditorGUI.EndBox();
            
        }
    }
    
}