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


        private ParameterMaker _func;
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
            
            _func = new ParameterMaker();
            _curType = NodeData.EventNodeData.EventType;
            ParameterMaker.Init(_func, NodeData.EventNodeData.CreateCheckerFunc);
        }

        private void OnDestroy()
        {
            NodeData.EventNodeData.CreateCheckerFunc = _func.ToArray();
        }

        private void OnGUI()
        {
            SirenixEditorGUI.BeginBox();

            var type = (EBattleEventType)SirenixEditorFields.EnumDropdown("事件类型", _curType);
            
            if (!_eventCheckerDict.TryGetValue(type, out var value))
            {
                SirenixEditorGUI.BeginBox("参数设置");
                EditorGUILayout.LabelField("该事件暂时不支持配置");
                SirenixEditorGUI.EndBox();
                SirenixEditorGUI.EndBox();
                return;
            }
            
            if (type != _curType)
            {
                _curType = type;
                _func.CreateFuncParam(value);
            }

            SirenixEditorGUI.BeginBox("参数设置");
            bool isFirst = true;
            EditorGUILayout.BeginVertical();
            foreach (var node in _func.FuncParams)
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
            SirenixEditorGUI.EndBox();
        }

        public override GUIContent GetWindowName()
        {
            return new GUIContent("事件节点");
        }
    }
}