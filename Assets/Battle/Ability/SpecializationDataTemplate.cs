using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using Unity.VisualScripting;
using UnityEngine;

namespace Battle.Skill
{
    public enum SpecializationDataType
    {
        Int,
        Long,
        Float,
        Bool,
        String,
        Enum,
        Custom
    }

    public class SpecializationData
    {
        [LabelText("命名")] public string Name = "";
        [LabelText("描述")] public string Desc = "";
        [LabelText("值类型")] public SpecializationDataType DataType;

        public string TypeStr;

        public string EnumName;

        public void UpdateTypeStr()
        {
            switch (DataType)
            {
                case SpecializationDataType.Int:
                    TypeStr = typeof(int).ToString();
                    break;
                case SpecializationDataType.Long:
                    TypeStr = typeof(long).ToString();
                    break;
                case SpecializationDataType.Float:
                    TypeStr = typeof(float).ToString();
                    break;
                case SpecializationDataType.Bool:
                    TypeStr = typeof(bool).ToString();
                    break;
                case SpecializationDataType.String:
                    TypeStr = typeof(string).ToString();
                    break;
                case SpecializationDataType.Custom:
                case SpecializationDataType.Enum:
                    if (!string.IsNullOrEmpty(TypeStr))
                        tryParse();
                    break;
            }
        }

        [Button("尝试转换下")]
        private void tryParse()
        {
            TypeStr = Type.GetType(EnumName) + ", " + Type.GetType(EnumName)?.Assembly;
        }
    }

    [CreateAssetMenu(menuName = "战斗编辑器/SpecializationDataTemplate")]
    public class SpecializationDataTemplate : SerializedScriptableObject
    {
        public List<SpecializationData> fieldList = new List<SpecializationData>();

        [Button("刷一下")]
        public void Refresh()
        {
            foreach (var VARIABLE in fieldList)
            {
                VARIABLE.UpdateTypeStr();
            }
        }
    }
}