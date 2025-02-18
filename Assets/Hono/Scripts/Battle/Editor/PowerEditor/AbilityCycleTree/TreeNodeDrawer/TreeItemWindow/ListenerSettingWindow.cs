using System;
using System.Collections.Generic;
using System.Linq;
using Editor.BattleEditor.AbilityEditor;
using Hono.Scripts.Battle;
using Hono.Scripts.Battle.AbilitySystem;
using Hono.Scripts.Battle.Base;
using Hono.Scripts.Battle.Editor.AbilityEditor;
using Hono.Scripts.Battle.Event;
using Sirenix.OdinInspector;
using Sirenix.Utilities.Editor;
using UnityEditor;
using UnityEngine;

namespace Editor.AbilityEditor.TreeItemWindow
{
    public class ListenerSettingWindow : ANodeSettingWindow<ListenerNodeData>
    {
        private readonly List<AParamsField> _parameterFields = new List<AParamsField>();
        private EEventType _curEvent;

        protected override void Init()
        {
            _parameterFields.Clear();
            _curEvent = TempData.eventType;

            if (!string.IsNullOrEmpty(TempData.getCheckerFunc.funcName))
            {
                var funcInfo = AbilityFuncInfoCache.EventBindInfoLookup[TempData.eventType].GetCheckerFuncInfo;

                for (var index = 0; index < TempData.getCheckerFunc.funcParams.Count; index++)
                {
                    AParams param = TempData.getCheckerFunc.funcParams[index];
                    var paramInfo = funcInfo.ParamInfos[index];
                    _parameterFields.Add(new AParamsField(TreeItem, param, paramInfo.ParamName, paramInfo.ParamType));
                }
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
            TempData.eventType = (EEventType)SirenixEditorFields.EnumDropdown(TempData.eventType);
            PowerEditorUIHelper.DrawSimpleField(ref TempData.eventInterval, "事件触发间隔", TempData.eventInterval);
            PowerEditorUIHelper.DrawSimpleField(ref TempData.isGlobalEvtListener, "是否监听世界事件",
                                                TempData.isGlobalEvtListener);

            if (string.IsNullOrEmpty(TempData.getCheckerFunc.funcName))
            {
                if (!AbilityFuncInfoCache.EventBindInfoLookup.TryGetValue(TempData.eventType, out var bind))
                {
                    EditorGUILayout.LabelField("该事件没有检查器");
                    return;
                }

                if (SirenixEditorGUI.Button("添加检查器", ButtonSizes.Medium))
                {
                    TempData.getCheckerFunc.funcName = bind.GetCheckerFuncInfo.FuncName;
                    TempData.getCheckerFunc.paramType = EParamType.Function;
                    TempData.getCheckerFunc.paramCastType = bind.GetCheckerFuncInfo.ReturnType.Name;
                    foreach (var paramInfo in bind.GetCheckerFuncInfo.ParamInfos)
                    {
                        var param = new AParams();
                        param.paramType = EParamType.Simple;
                        TempData.getCheckerFunc.funcParams ??= new List<AParams>();
                        TempData.getCheckerFunc.funcParams.Add(param);
                        _parameterFields.Add(new AParamsField(TreeItem, param, paramInfo.ParamName, paramInfo.ParamType));
                    }
                }
            }
            
            if (_curEvent != TempData.eventType)
            {
                TempData.getCheckerFunc = new AParams();
                _parameterFields.Clear();
                _curEvent = TempData.eventType;
            }
            
            foreach (var field in _parameterFields)
            {
                field.Draw();
            }
        }
    }
}