
using System.Collections.Generic;
using Editor.BattleEditor.AbilityEditor;
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
        private new EventNodeData _nodeData;

        public EventTreeItem(AbilityLogicTree tree, AbilityNodeData nodeData) : base(tree, nodeData)
        {
            _nodeData = (EventNodeData)base._nodeData;
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
           
            if (!checkHasParent(EAbilityNodeType.ERepeat))
            {
                _menu.AddItem(new GUIContent("创建节点/创建Repeat节点"), false,
                    AddChild, (EAbilityNodeType.ERepeat));
            }
            
            if (!checkHasParent(EAbilityNodeType.EGroup))
            {
                _menu.AddItem(new GUIContent("创建节点/创建Stage节点"), false,
                    AddChild, (EAbilityNodeType.EGroup));
            }

            if (!checkHasParent(EAbilityNodeType.ETimer))
            {
                _menu.AddItem(new GUIContent("创建节点/创建Timer节点"), false,
                    AddChild, (EAbilityNodeType.ETimer));
            }
        }

        protected override Color getButtonColor()
        {
            return new Color(0.4f, 0.8f, 0.5f);
        }

        protected override string getButtonText()
        {
            return _nodeData.EventType.ToString();
        }

        protected override string getButtonTips()
        {
            return "监听指定事件回调，与执行顺序无关";
        }

        protected override void OnBtnClicked(Rect btnRect)
        {
            SettingWindow = BaseNodeWindow<EventNodeDataWindow, EventNodeData>.GetSettingWindow(_tree.TreeData,
                _nodeData,
                (nodeData) => _nodeData = nodeData);
            SettingWindow.position = new Rect(btnRect.x, btnRect.y, 740, 240);
            SettingWindow.Show();
        }
    }

    public class EventNodeDataWindow : BaseNodeWindow<EventNodeDataWindow,EventNodeData>, IAbilityNodeWindow<EventNodeData>
    {
        private static readonly Dictionary<EBattleEventType, string> _eventCheckerDict = new();
        
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

        private List<ParameterField> _parameterFields;
        private EBattleEventType _curEvent;
        protected override void onInit()
        {
            initDict();
            _curEvent = _nodeData.EventType;
            _parameterFields = new List<ParameterField>();
        }

        private void initParameter()
        {
            if (!_eventCheckerDict.TryGetValue(_nodeData.EventType, out var value))
            {
                return;
            }
            if (!AbilityFunctionHelper.TryGetFuncInfo(value, out var funcInfo))
            {
                return;
            }

            _nodeData.CreateChecker.ParameterType = EParameterType.Function;
            _nodeData.CreateChecker.FuncName = value;
            _nodeData.CreateChecker.FuncParams ??= new List<Parameter>();
            _nodeData.CreateChecker.FuncParams.Clear();
            foreach (var paramInfo in funcInfo.ParamInfos)
            {
                var parameter = new Parameter();
                _nodeData.CreateChecker.FuncParams.Add(parameter);
                var param = new ParameterField(parameter, paramInfo.ParamName, paramInfo.ParamType);
                _parameterFields.Add(param);
            }
        }
        
        private void OnGUI()
        {
            SirenixEditorGUI.BeginBox();

            _nodeData.EventType = (EBattleEventType)SirenixEditorFields.EnumDropdown("事件类型", _nodeData.EventType);


            if (_curEvent == EBattleEventType.NoInit || string.IsNullOrEmpty(_nodeData.CreateChecker.FuncName))
            {
                EditorGUILayout.LabelField("未初始化，请选择事件类型");
            }
            else
            {
                if (_curEvent != _nodeData.EventType)
                {
                     initParameter();
                }

                if (_parameterFields.Count == 0)
                {
                    EditorGUILayout.LabelField("未找到参数");
                }
                else
                {
                    SirenixEditorGUI.BeginBox("参数设置");
                    
                    EditorGUILayout.BeginVertical();
                    
                    foreach (var parameterField in _parameterFields)
                    {
                        parameterField.Draw();
                    }
                    
                    _nodeData.Desc = SirenixEditorFields.TextField("备注",  _nodeData.Desc );

                    if (SirenixEditorGUI.Button("保  存", ButtonSizes.Medium))
                    {
                        Save();
                    }
                    EditorGUILayout.EndVertical();
                    SirenixEditorGUI.EndBox();
                }
            }
      
            SirenixEditorGUI.EndBox();
        }
    }
}