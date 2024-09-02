using System;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using UnityEngine;

namespace Hono.Scripts.Battle
{
    [Serializable]
    public class AttrDefineItem
    {
        [LabelText("属性变量名")] [ReadOnly] public string AttrName;

        [LabelText("属性类型")] [ReadOnly] public string AttrType;

        //TODO:会存在大量的拆箱操作，后续需要优化 
        [OdinSerialize]
        public object AttrValue;
    }
}