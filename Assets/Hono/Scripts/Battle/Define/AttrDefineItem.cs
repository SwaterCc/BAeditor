using System;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using UnityEngine;

namespace Hono.Scripts.Battle
{
    [Serializable]
    public struct AttrDefineItem
    {
        [LabelText("属性变量名")] [ReadOnly] [HorizontalGroup]
        public string AttrName;

        [LabelText("属性类型")] [ReadOnly] [HorizontalGroup]
        public string AttrType;

        //TODO:会存在大量的拆箱操作，后续需要优化 
        [OdinSerialize] [LabelText("属性值")] [HorizontalGroup]
        public object AttrValue;
    }
}