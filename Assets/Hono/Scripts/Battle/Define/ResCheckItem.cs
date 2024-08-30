using System;

namespace Hono.Scripts.Battle
{
    [Serializable]
    public class ResCheckItem
    {
        public EBattleResourceType ResourceType;
        public int Flag;
        public float Cost;
    }
}