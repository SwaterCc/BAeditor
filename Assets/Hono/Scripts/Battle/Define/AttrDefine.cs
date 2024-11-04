#region

using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using Sirenix.Serialization;

#endregion

namespace Hono.Scripts.Battle
{
    public class AttrDefine : SerializedScriptableObject
    {
        public int id;
        public string desc;
        [NonSerialized] [OdinSerialize] public List<AttrDefineItem> AttrDefineItems = new List<AttrDefineItem>();
    }
}