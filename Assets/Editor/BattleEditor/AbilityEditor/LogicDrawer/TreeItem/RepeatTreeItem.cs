
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
        public RepeatTreeItem(AbilityLogicTree tree, AbilityNodeData nodeData) : base(tree,nodeData) { }
      
        protected override Color getButtonColor()
        {
            return Color.grey;
        }

        protected override string getButtonText()
        {
            var maxCount = new ParameterMaker();
            ParameterMaker.Init(maxCount,_nodeData.RepeatNodeData.MaxRepeatCount);
            
            return "循环次数："+maxCount.ToString();
        }

        protected override string getButtonTips()
        {
            return "循环节点，循环指定次数";
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
            menu.AddItem(new GUIContent("创建节点/创建Timer节点"), false,
                AddNode, EAbilityNodeType.ETimer);
            return menu;
        }

        protected override void OnBtnClicked()
        {
            SettingWindow = RepeatNodeDataWindow.GetWindow(_nodeData);
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
            ParameterMaker.Init(_maxCount,_nodeData.RepeatNodeData.MaxRepeatCount);
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
            _nodeData.RepeatNodeData.MaxRepeatCount = _maxCount.ToArray();
            Close();
        }
    }
}