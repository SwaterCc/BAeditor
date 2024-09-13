
using System.Collections.Generic;

using Hono.Scripts.Battle;
using Hono.Scripts.Battle.Event;
using Hono.Scripts.Battle.Tools.CustomAttribute;
using Sirenix.OdinInspector;
using Sirenix.Utilities.Editor;
using UnityEditor;
using UnityEngine;

namespace Editor.AbilityEditor.TreeItem
{
    public class EventTreeItem : AbilityLogicTreeItem
    {
        public EventTreeItem(int id, int depth, string name) : base(id, depth, name) { }
        public EventTreeItem(AbilityLogicTree tree, AbilityNodeData nodeData) : base(tree,nodeData) { }

        protected override Color getButtonColor()
        {
            return new Color(0.4f, 0.8f, 0.5f);
        }

        protected override string getButtonText()
        {
            return _nodeData.EventNodeData.EventType.ToString();
        }

        protected override string getButtonTips()
        {
            return "监听指定事件回调，与执行顺序无关";
        }

        protected override void OnBtnClicked()
        {
            SettingWindow = EventNodeDataWindow.GetWindow(_nodeData);
            SettingWindow.Show();
            SettingWindow.Focus();
        }
    }

    public class EventNodeDataWindow : BaseNodeWindow<EventNodeDataWindow>, IWindowInit
    {
        private static readonly Dictionary<EBattleEventType, string> _eventCheckerDict =
            new Dictionary<EBattleEventType, string>();
        
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

        private ParameterMaker _func;
        private EBattleEventType _curType;
        private string _varName;
        private string _desc;
        
        protected override void onInit()
        {
            initDict();
            _func = new ParameterMaker();
            _curType = _nodeData.EventNodeData.EventType;
            _varName = _nodeData.EventNodeData.CaptureVarName;
            ParameterMaker.Init(_func, _nodeData.EventNodeData.CreateCheckerFunc);
        }

        private void Save()
        {
            _nodeData.EventNodeData.EventType = _curType;
            _nodeData.EventNodeData.CreateCheckerFunc = _func.ToArray();
            _nodeData.EventNodeData.CaptureVarName = _varName;
            _nodeData.EventNodeData.Desc = _desc;
            Close();
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

            _varName = SirenixEditorFields.TextField("回调变量名", _varName);
            _desc = SirenixEditorFields.TextField("备注", _desc);

            if (SirenixEditorGUI.Button("保  存", ButtonSizes.Medium))
            {
                Save();
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