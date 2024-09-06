using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Hono.Scripts.Battle
{
    public class BuffData : ScriptableObject, IAllowedIndexing
    {
        public int ID => id;
        public int id;
        public EBuffAddRule AddRule;
        public int InitLayer = 1;
        public int BuffDamageBasePer; //buff基础倍率万分比
    }
}