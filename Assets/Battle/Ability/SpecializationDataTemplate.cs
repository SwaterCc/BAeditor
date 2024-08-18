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
        public string Name = "";
        public string Desc = "";
        public SpecializationDataType Type;
    }

    [CreateAssetMenu(menuName = "战斗编辑器/SpecializationDataTemplate")] 
    public class SpecializationDataTemplate : SerializedScriptableObject
    {
        public List<SpecializationData> fieldList = new List<SpecializationData>();
    }
}