
using System;

namespace Hono.Scripts.Battle.Core
{
    public class CombatCompCtorParams : ComponentCtorParams
    {
        public bool AllowElementEffect { get; set; }
        public bool DisableBuffAdd { get; set; }
    }
}
