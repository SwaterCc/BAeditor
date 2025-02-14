
using System;

namespace Hono.Scripts.Battle.Core
{
    [Flags]
    public enum EUnitComponentKey
    {
        None = 0,
        CombatComp = 1 << 0,
        HateComp = 1 << 1,
        ModelControllerComp = 1 << 2,
        MoveComp = 1 << 3
    }
}
