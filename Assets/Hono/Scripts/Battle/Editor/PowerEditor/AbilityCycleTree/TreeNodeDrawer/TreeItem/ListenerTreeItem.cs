using System;
using System.Collections.Generic;
using Editor.BattleEditor.AbilityEditor;
using Hono.Scripts.Battle;
using Hono.Scripts.Battle.Base;
using Hono.Scripts.Battle.Event;
using Sirenix.Utilities.Editor;
using UnityEditor;
using UnityEngine;

namespace Editor.AbilityEditor.TreeItem
{
    /// <summary>
    /// 监听节点，仅允许放置在Cycle节点下，仅在监听到事件或消息时执行其子节点
    /// </summary>
    public class ListenerTreeItem : ATreeItem<ListenerNodeData>
    {
        public ListenerTreeItem(AbilityCycleTree tree, AEditorTreeNode data) : base(tree, data)
        {
            ButtonBackGroundColor = new Color(0.4f, 1.8f, 1.5f);
        }

        protected override ERightMenuState checkRightMenuState(MenuInfo info)
        {
            if (info.OperationType is ERightClickOperationType.AddListenerChild or ERightClickOperationType.AddGroupChild)
            {
                return ERightMenuState.NoShow;
            }

            return ERightMenuState.Enable;
        }

        protected override bool checkIsAllowMove(ATreeItem newParent)
        {
            if (newParent is not CycleTreeItem)
            {
                return false;
            }

            return true;
        }

        protected override string getButtonText()
        {
            string text = "";
            if (Data.IsEvent)
            {
                text = "等待事件：" + Enum.GetName(typeof(EBattleEventType), Data.EventType);
            }
            else
            {
                var msgName = string.IsNullOrEmpty(Data.MsgName) ? "未定义" : Data.MsgName;
                text = "等待消息：" + msgName;
            }

            return text;
        }


        protected override void OnBtnClicked(Rect btnRect)
        {
            ANodeSettingWindow.Open<ListenerSettingWindow>(this);
        }
    }

    public class ListenerSettingWindow : ANodeSettingWindow<ListenerNodeData>
    {
        private List<AParamsField> _parameterFields;
        private EBattleEventType _curEvent;

        protected override void Init()
        {
            _parameterFields = new List<AParamsField>();
            _curEvent = TempData.EventType;

            if (!AbilityFunctionHelper.EventCheckerDict.TryGetValue(TempData.EventType, out var value))
            {
                return;
            }

            if (!AbilityFunctionHelper.TryGetFuncInfo(value.CreateFuncName, out var funcInfo))
            {
                return;
            }

            if (!string.IsNullOrEmpty(TempData.GetChecker.funcName))
            {
                for (int index = 0; index < TempData.GetChecker.funcParams.Count; index++)
                {
                    /*AParams aParameter = TempData.GetChecker.funcParams[index];
                    if (funcInfo.ParamInfos.Count <= index) continue;
                    // 获取泛型类的类型
                    Type genericClassType = typeof(AParamsField<>);
                    // 为泛型类指定具体类型参数，例如 typeof(int)
                    Type constructedType = genericClassType.MakeGenericType(paramInfo.ParamType);
                    object param = Activator.CreateInstance(constructedType, parameter, paramInfo.ParamName);
                    _parameterFields.Add(new AParamsField(aParameter, funcInfo.ParamInfos[index].ParamName));*/
                }
            }
        }

        private void initParameter()
        {
            if (!AbilityFunctionHelper.EventCheckerDict.TryGetValue(TempData.EventType, out var value))
            {
                return;
            }

            if (!AbilityFunctionHelper.TryGetFuncInfo(value.CreateFuncName, out var funcInfo))
            {
                return;
            }

            TempData.GetChecker.paramType = EParamType.Function;
            TempData.GetChecker.funcName = value.CreateFuncName;
            TempData.GetChecker.funcParams ??= new List<AParams>();
            TempData.GetChecker.funcParams.Clear();
            _parameterFields.Clear();
            foreach (var paramInfo in funcInfo.ParamInfos)
            {
                var parameter = new AParams();
                TempData.GetChecker.funcParams.Add(parameter);
                // 获取泛型类的类型
                Type genericClassType = typeof(AParamsField<>);
                // 为泛型类指定具体类型参数，例如 typeof(int)
                Type constructedType = genericClassType.MakeGenericType(paramInfo.ParamType);
                object param = Activator.CreateInstance(constructedType, parameter, paramInfo.ParamName);
             
                _parameterFields.Add((AParamsField)param);
            }
        }

        protected override void Draw()
        {
            SirenixEditorGUI.BeginBox();

            TempData.IsEvent = SirenixEditorFields.Dropdown(new GUIContent("选择类型："), TempData.IsEvent,
                                                            new[] { true, false },
                                                            new[] { "事件", "消息" });

            if (TempData.IsEvent)
            {
                showEvent();
            }
            else
            {
                showMsg();
            }

            SirenixEditorGUI.EndBox();
        }

        private void showMsg()
        {
            TempData.MsgName = SirenixEditorFields.TextField("消息Key：", TempData.MsgName);
        }

        private void showEvent()
        {
            TempData.EventType = SirenixEditorFields.Dropdown(new GUIContent("事件类型"),
                                                              TempData.EventType, AbilityFunctionHelper.AllowEvent);

            if (_curEvent != TempData.EventType)
            {
                initParameter();
                _curEvent = TempData.EventType;
            }

            if (_curEvent == EBattleEventType.NoInit || string.IsNullOrEmpty(TempData.GetChecker.funcName))
            {
                EditorGUILayout.LabelField("未初始化，请选择事件类型");
            }
            else
            {
                SirenixEditorGUI.BeginBox("参数设置");

                EditorGUILayout.BeginVertical();

                foreach (var parameterField in _parameterFields)
                {
                    parameterField.Draw();
                }

                TempData.Desc = SirenixEditorFields.TextField("备注", TempData.Desc);

                EditorGUILayout.EndVertical();
                SirenixEditorGUI.EndBox();
            }
        }
    }
}