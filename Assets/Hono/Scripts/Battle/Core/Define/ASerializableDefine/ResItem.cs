#region

using System;
using System.Collections.Generic;
using UnityEngine.Serialization;

#endregion

namespace Hono.Scripts.Battle
{
    [Serializable]
    public class ResItems
    {
        public List<ResItem> Items = new();
    }

    [Serializable]
    public class ResItem
    {
        public EBattleResourceType resourceType;
        public int param1;
        public int param2;
    }
}