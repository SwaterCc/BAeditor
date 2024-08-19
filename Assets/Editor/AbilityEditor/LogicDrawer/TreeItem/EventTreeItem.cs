using System;
using System.Collections.Generic;
using System.Reflection;
using Battle;
using Battle.Def;
using Battle.Event;
using Battle.Tools.CustomAttribute;
using Sirenix.Utilities;
using Sirenix.Utilities.Editor;
using UnityEditor;
using UnityEngine;

namespace Editor.AbilityEditor.TreeItem
{
    public class EventTreeItem : AbilityLogicTreeItem
    {
        public EventTreeItem(int id, int depth, string name) : base(id, depth, name) { }
        public EventTreeItem(AbilityNodeData nodeData) : base(nodeData) { }

        protected override Color getButtonColor()
        {
            return new Color(0.4f, 0.8f, 0.5f);
        }

        protected override string getButtonText()
        {
            return NodeData.EventNodeData.EventType.ToString();
        }

        protected override void OnBtnClicked()
        {
            EventNodeDataWindow.Open(NodeData);
        }
    }

    public class EventNodeDataWindow : BaseNodeWindow<EventNodeDataWindow>, IWindowInit
    {
        private static readonly Dictionary<EBattleEventType, string> _eventCheckerDict =
            new Dictionary<EBattleEventType, string>();


        private ParameterNode _func;
        private EBattleEventType _curType;

        private static void initDict()
        {
            if (_eventCheckerDict.Count != 0) return;

            foreach (var field in typeof(EBattleEventType).GetFields())
            {
                object[] attributes = field.GetCustomAttributes(typeof(EventCheckerBinder), false);

                if (attributes.Length > 0)
                {
                    foreach (var attribute in attributes)
                    {
                        if (attribute is EventCheckerBinder binder)
                        {
                            // 获取枚举值
                            var enumValue = (EBattleEventType)field.GetValue(null);
                            _eventCheckerDict.Add(enumValue, binder.CreateFunc);
                        }
                    }
                }
            }
        }

        protected override void onInit()
        {
            initDict();
            
            _func = new ParameterNode();
            _func.Parse(NodeData.EventNodeData.CreateCheckerFunc, 0);
            _curType = NodeData.EventNodeData.EventType;
        }

        private void OnGUI()
        {
            SirenixEditorGUI.BeginBox();

            var type = (EBattleEventType)SirenixEditorFields.EnumDropdown("事件类型", _curType);
            if (type != _curType)
            {
               
                _curType = type;
                if (!_eventCheckerDict.TryGetValue(type, out var value))
                {
                    SirenixEditorGUI.BeginBox("参数设置");
                    EditorGUILayout.LabelField("该事件暂时不支持配置");
                    SirenixEditorGUI.EndBox();
                    return;
                }

                _func.Create(new Parameter() { FuncName = value, IsFunc = true });
            }

            SirenixEditorGUI.BeginBox("参数设置");
            bool isFirst = true;
            EditorGUILayout.BeginVertical();
            foreach (var node in _func.FuncParameter)
            {
                if (isFirst)
                {
                    isFirst = false;
                    continue;
                }
                node.Draw();
            }
            EditorGUILayout.EndVertical();
            SirenixEditorGUI.EndBox();
        }

        public override GUIContent GetWindowName()
        {
            return new GUIContent("事件节点");
        }
    }
}