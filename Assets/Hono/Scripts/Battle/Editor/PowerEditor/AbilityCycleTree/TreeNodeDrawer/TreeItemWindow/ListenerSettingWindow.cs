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

          
            _parameterFields.Clear();
            foreach (var paramInfo in funcInfo.ParamInfos)
            {
                var parameter = new AParams();
             

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

            
        }
    }
}