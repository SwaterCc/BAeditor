#region

using System;
using System.Runtime.CompilerServices;
using UnityEngine;

#endregion

namespace Hono.Scripts.Battle.Event
{
    public static class LootEventInfo
    {
        public static readonly EvtInfoField<float> LootUid = new();
    }
}