using System;

namespace Battle
{
    [Serializable]
    public class ResCheckItem
    {
        public EBattleResourceType ResourceType;
        public int Flag;
        public float Cost;
    }
}