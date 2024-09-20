﻿using Hono.Scripts.Battle;
using Sirenix.Utilities.Editor;
using UnityEditor;
using UnityEngine;

namespace Editor.AbilityEditor
{
    public class BuffDrawer : IExDrawer
    {
        private BuffData _data;

        public void LoadAsset(int id)
        {
            string path = AbilityEditorPath.BuffPath + "/" + id + ".asset";
            _data = AssetDatabase.LoadAssetAtPath<BuffData>(path);
            if (_data == null)
            {
                _data = ScriptableObject.CreateInstance<BuffData>();
                _data.id = id;

                AssetDatabase.CreateAsset(_data, path);
            }
        }

        public void Draw()
        {
            SirenixEditorGUI.BeginBox("Buff数据");
            _data.AddRule = (EApplicationRequirement)SirenixEditorFields.EnumDropdown("Buff添加规则", _data.AddRule);
            if (_data.AddRule == EApplicationRequirement.HasTags || _data.AddRule == EApplicationRequirement.NoTags)
            {
	            AbilityEditorHelper.DrawIntList(_data.FilterTags,"筛选tag",50);
            }
            _data.ReplaceRule = (EBuffReplaceRule)SirenixEditorFields.EnumDropdown("Buff替换规则", _data.ReplaceRule);
            _data.InitLayer = SirenixEditorFields.IntField("Buff初始层数",_data.InitLayer);
            _data.BuffDamageBasePer = SirenixEditorFields.IntField("Buff基础伤害万分比",_data.BuffDamageBasePer);
            SirenixEditorGUI.EndBox();
        }

        public void Save()
        {
            if (!_data) return;
            EditorUtility.SetDirty(_data);
            AssetDatabase.SaveAssets();
        }
    }
}