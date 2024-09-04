using System;
using System.Collections.Generic;
using UnityEngine.Serialization;

namespace Hono.Scripts.Battle
{
    [Serializable]
    public class ResItems
    {
        public List<ResItem> Items = new();
    }

    [Serializable]
    public struct ResItem
    {
        public EBattleResourceType ResourceType;
        public int ResId;
        public int Value;
    }
}