using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Serialization;

namespace Hono.Scripts.Battle
{
    public class BuffData : ScriptableObject, IAllowedIndexing
    {
        public int ID => id;
        public int id;
        public EBuffReplaceRule ReplaceRule;
        public EApplicationRequirement AddRule;
        public int InitLayer = 1;
        public List<int> FilterTags = new List<int>();
        public int BuffDamageBasePer; //buff基础倍率万分比
    }
}