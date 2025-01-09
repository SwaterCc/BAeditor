using System;
using System.Collections.Generic;
using System.Linq;
using Editor.BattleEditor.AbilityEditor;
using Hono.Scripts.Battle;
using Hono.Scripts.Battle.AbilitySystem;
using Hono.Scripts.Battle.Base;
using Hono.Scripts.Battle.Event;
using Sirenix.Utilities.Editor;
using UnityEditor;
using UnityEngine;

namespace Editor.AbilityEditor.TreeItemWindow
{
    public class ListenerSettingWindow : ANodeSettingWindow<ListenerNodeData>
    {
        private List<AParamsField> _parameterFields;
        private EEventType _curEvent;

        protected override void Init()
        {
            _parameterFields = new List<AParamsField>();
            _curEvent = TempData.eventType;

            if (!AbilityFuncInfoCache.EventCheckerDict.TryGetValue(TempData.eventType, out var value))
            {
                return;
            }

            if (!AbilityFuncInfoCache.TryGetFuncInfo(value.CreateFuncName, out var funcInfo))
            {
                return;
            }

            if (!string.IsNullOrEmpty(TempData.GetChecker.funcName))
            {
                for (int index = 0; index < TempData.GetChecker.funcParams.Count; index++)
                {
                    AParams aParameter = TempData.GetChecker.funcParams[index];
                    if (funcInfo.ParamInfos.Count <= index) continue;


                    var field = new AParamsField(TreeItem, aParameter, funcInfo.ParamInfos[index].ParamName,
                                                 funcInfo.ParamInfos[index].ParamType);
                    _parameterFields.Add(field);
                }
            }
        }

        private void initParameter()
        {
            if (!AbilityFuncInfoCache.EventCheckerDict.TryGetValue(TempData.eventType, out var value))
            {
                return;
            }

            if (!AbilityFuncInfoCache.TryGetFuncInfo(value.CreateFuncName, out var funcInfo))
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

                AParamsField param = new(TreeItem, parameter, paramInfo.ParamName, paramInfo.ParamType);
                _parameterFields.Add(param);
            }
        }

        protected override void Draw()
        {
            SirenixEditorGUI.BeginBox();

            TempData.isEvent = SirenixEditorFields.Dropdown(new GUIContent("选择类型："), TempData.isEvent,
                                                            new[] { true, false },
                                                            new[] { "事件", "消息" });

            if (TempData.isEvent)
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
            TempData.msgName = SirenixEditorFields.TextField("消息Key：", TempData.msgName);
        }

        private void showEvent()
        {
            TempData.eventType = SirenixEditorFields.Dropdown(new GUIContent("事件类型"),
                                                              TempData.eventType,
                                                              AbilityFuncInfoCache.EventCheckerDict.Keys.ToList());

            if (_curEvent != TempData.eventType)
            {
                initParameter();
                _curEvent = TempData.eventType;
            }

            if (_curEvent == EEventType.NoInit || string.IsNullOrEmpty(TempData.GetChecker.funcName))
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

                EditorGUILayout.EndVertical();
                SirenixEditorGUI.EndBox();
            }
        }
    }
}