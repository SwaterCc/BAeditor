using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using UnityEngine;

namespace Editor.AttrMaker
{
    public class AttrTemplate : SerializedScriptableObject
    {
        public string Desc;
        [NonSerialized]
        [OdinSerialize]
        public List<AttrTemplateItem> AttrTemplateItems = new List<AttrTemplateItem>();
    }
}