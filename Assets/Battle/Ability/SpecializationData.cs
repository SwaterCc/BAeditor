using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using Unity.VisualScripting;
using UnityEngine;

namespace Battle.Skill
{
    public class SpecializationData: SerializedScriptableObject
    {
        public string Name;
        public string Type;
        public object Value;
    }

    [CreateAssetMenu(menuName = "战斗编辑器/CreateSpecializationTemplate")] 
    public class SpecializationDataTemplate : SerializedScriptableObject
    {
        public List<SpecializationData> _list = new List<SpecializationData>();
    }
}