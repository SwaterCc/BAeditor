
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
        public TimerTreeItem(AbilityLogicTree tree, AbilityNodeData nodeData) : base(tree,nodeData) { }

        protected override Color getButtonColor()
        {
            return new Color(0.2f, 1.0f, 0.3f);
        }

        protected override string getButtonText()
        {
            return string.IsNullOrEmpty(_nodeData.TimerNodeData.Desc)?"计时器":_nodeData.TimerNodeData.Desc;
        }

        protected override string getButtonTips()
        {
            return "计时器节点";
        }

        public override GenericMenu GetGenericMenu()
        {
            var menu = new GenericMenu();
            menu.AddItem(new GUIContent("创建节点/添加Action节点"), false,
                AddNode, EAbilityNodeType.EAction);
            menu.AddItem(new GUIContent("创建节点/添加分支节点"), false,
                AddNode,  EAbilityNodeType.EBranchControl);
            menu.AddItem(new GUIContent("创建节点/创建变量控制节点"), false,
                AddNode, EAbilityNodeType.EVariableControl);
            menu.AddItem(new GUIContent("创建节点/创建Event节点"), false,
                AddNode, EAbilityNodeType.EEvent);
            menu.AddItem(new GUIContent("创建节点/创建Repeat节点"), false,
                AddNode, EAbilityNodeType.ERepeat);
            return menu;
        }

        protected override void OnBtnClicked()
        {
            SettingWindow = TimerNodeDataWindow.GetWindow(_nodeData);
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
            ParameterMaker.Init(_firstInterval,_nodeData.TimerNodeData.FirstInterval);
            _interval = new ParameterMaker();
            ParameterMaker.Init(_interval,_nodeData.TimerNodeData.Interval);
            _maxCount = new ParameterMaker();
            ParameterMaker.Init(_maxCount,_nodeData.TimerNodeData.MaxCount);
        }

        public override Rect GetPos()
        {
            return GUIHelper.GetEditorWindowRect().AlignCenter(740, 120);
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
            _nodeData.TimerNodeData.FirstInterval = _firstInterval.ToArray();
            _nodeData.TimerNodeData.Interval = _interval.ToArray();
            _nodeData.TimerNodeData.MaxCount = _maxCount.ToArray();
            _nodeData.TimerNodeData.Desc = _desc;
            Close();
        }
    }
    
}