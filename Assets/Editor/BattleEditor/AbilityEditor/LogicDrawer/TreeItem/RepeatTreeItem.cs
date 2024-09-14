
using System;
using Hono.Scripts.Battle;
using Sirenix.OdinInspector;
using Sirenix.Utilities.Editor;
using UnityEditor;
using UnityEngine;

namespace Editor.AbilityEditor.TreeItem
{
    public class RepeatTreeItem : AbilityLogicTreeItem
    {
        public RepeatTreeItem(int id, int depth, string name) : base(id, depth, name) { }
        public RepeatTreeItem(AbilityNodeData nodeData) : base(nodeData) { }
      
        protected override Color getButtonColor()
        {
            return Color.grey;
        }

        protected override string getButtonText()
        {
            var maxCount = new ParameterMaker();
            ParameterMaker.Init(maxCount,NodeData.RepeatNodeData.MaxRepeatCount);
            
            return "循环次数："+maxCount.ToString();
        }

        protected override string getItemEffectInfo()
        {
            return "循环节点，循环指定次数";
        }

        protected override void OnBtnClicked()
        {
            SettingWindow = RepeatNodeDataWindow.GetWindow(NodeData);
            SettingWindow.Show();
            SettingWindow.Focus();
        }
    }

    public class RepeatNodeDataWindow : BaseNodeWindow<RepeatNodeDataWindow>, IWindowInit
    {
        private ParameterMaker _maxCount;
        
        protected override void onInit()
        {
            _maxCount = new ParameterMaker();
            ParameterMaker.Init(_maxCount,NodeData.RepeatNodeData.MaxRepeatCount);
        }

        private void OnGUI()
        {
            SirenixEditorGUI.BeginBox("设置循环次数");
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("循环次数 int");
            _maxCount.Draw();
            EditorGUILayout.EndHorizontal();
            
            if (SirenixEditorGUI.Button("保  存", ButtonSizes.Medium))
            {
                Save();
            }
            SirenixEditorGUI.EndBox();
        }

        private void Save()
        {
            NodeData.RepeatNodeData.MaxRepeatCount = _maxCount.ToArray();
            Close();
        }
    }
}