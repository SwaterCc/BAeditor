#region

using System;
using System.Collections.Generic;

#endregion

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