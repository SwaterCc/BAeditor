using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using UnityEngine;

namespace Hono.Scripts.Battle
{
    [Serializable]
    public struct AttrDefineItem
    {
        [HorizontalGroup]
        [LabelText("属性类型")]
        [ValueDropdown("GetIntEnumValues")] 
        public string AttrType;
        [HorizontalGroup]
        [LabelText("属性名(属性值(不可重复))")]
        public string AttrName;
        [LabelText("属性值(不可重复)")]
        [HorizontalGroup]
        public int AttrID;
        [LabelText("属性描述)")]
        [HorizontalGroup]
        public string Desc;

        private static IEnumerable<ValueDropdownItem<string>> GetIntEnumValues()
        {
            yield return new ValueDropdownItem<string>("int", "int");
            yield return new ValueDropdownItem<string>("float", "float");
            yield return new ValueDropdownItem<string>("Vector3", "Vector3");
            yield return new ValueDropdownItem<string>("Quaternion", "Quaternion");
            yield return new ValueDropdownItem<string>("IntArray", "IntArray");
            yield return new ValueDropdownItem<string>("FloatArray", "FloatArray");
        }
    }
}