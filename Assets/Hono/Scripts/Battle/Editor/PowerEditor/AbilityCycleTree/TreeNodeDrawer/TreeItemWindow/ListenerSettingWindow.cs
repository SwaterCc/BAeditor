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
        private List<AParamsField> _parameterFields;
        private EEventType _curEvent;

        protected override void Init()
        {
            _parameterFields = new List<AParamsField>();
            _curEvent = TempData.eventType;
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

            if (TempData.Checker == null)
            {
                if (!AbilityFuncInfoCache.EventBindInfoLookup.TryGetValue(TempData.eventType, out var bind))
                {
                    EditorGUILayout.LabelField("该事件没有检查器");
                    return;
                }

                if (SirenixEditorGUI.Button("添加检查器", ButtonSizes.Medium))
                {
                    TempData.Checker = (IEventChecker)Activator.CreateInstance(bind.CheckerType);
                }
            }
            else
            {
                if (SirenixEditorGUI.Button("配置事件检查器", ButtonSizes.Large))
                {
                    SerializableOdinWindow.Open(TempData.Checker);
                }
            }
        }
    }
}