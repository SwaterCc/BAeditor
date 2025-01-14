using System;
using System.Collections.Generic;
using Hono.Scripts.Battle;
using Hono.Scripts.Battle.AbilitySystem;
using Sirenix.OdinInspector;
using Sirenix.Utilities.Editor;
using UnityEditor;
using UnityEngine;

namespace Editor.AbilityEditor.TreeItemWindow
{
    public class MsgSendSettingWindow: ANodeSettingWindow<MsgSendNodeData>
    {
        private AParamsField _actorUid;
        private List<AParamsField> _paramsFields;
        
        protected override void Init()
        {
            _actorUid = new AParamsField(TreeItem, TempData.actorUid, "发送给:", typeof(int));
            _paramsFields = new List<AParamsField>();
            foreach (AParams param in TempData.values)
            {
                var type = Type.GetType(param.paramCastType);
                if (type == null)
                {
                    Debug.LogError("参数类型转换失败");
                    continue;
                }

                _paramsFields.Add(new AParamsField(TreeItem, param, "", type));
            }
        }

        protected override void Draw()
        {
            SirenixEditorGUI.BeginBox("发送消息");
            TempData.msgKey = SirenixEditorFields.TextField("消息Key", TempData.msgKey);
            _actorUid.Draw();
            if (SirenixEditorGUI.Button("添加参数",ButtonSizes.Medium))
            {
                TempData.msgParamKeys.Add("");
                _paramsFields.Add(new AParamsField(TreeItem, new AParams(), "", typeof(object)));
            }

            SirenixEditorGUI.BeginVerticalList();
            for (int i = 0; i < _paramsFields.Count; i++)
            {
                SirenixEditorGUI.BeginListItem();
                EditorGUILayout.BeginHorizontal();
                TempData.msgParamKeys[i] = SirenixEditorFields.TextField("msgParamKey:", TempData.msgParamKeys[i]);
                _paramsFields[i].Draw();
                EditorGUILayout.EndHorizontal();
                SirenixEditorGUI.EndListItem();
            }
            SirenixEditorGUI.EndVerticalList();
            SirenixEditorGUI.EndBox();
        }
    }
}