#region

using System.Collections.Generic;
using UnityEngine;

#endregion

namespace Hono.Scripts.Battle
{
    public class BuffData : ASerializableData
    {
        public EBuffReplaceRule ReplaceRule;
        public EApplicationRequirement AddRule;
        public int InitLayer = 1;
        public List<int> FilterTags = new List<int>();
        public int BuffDamageBasePer; //buff基础倍率万分比
        public int MaxLayer;
    }
}