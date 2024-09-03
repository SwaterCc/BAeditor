using UnityEditor;
using UnityEngine;

namespace Hono.Scripts.Battle
{
    public class BuffData : ScriptableObject
    {
        public EBuffAddRule AddRule;
        public int InitLayer = 1;
        public int BuffDamageBasePer;//buff基础倍率万分比
    }
}