
using System;

namespace Hono.Scripts.Battle.Core
{
    public partial class CombatComp
    {
        public override void Ctor(ComponentCtorParams param)
        {
            if (param is CombatCompCtorParams ctorParams)
            {
                this.AllowElementEffect = ctorParams.AllowElementEffect;
                this.DisableBuffAdd = ctorParams.DisableBuffAdd;
            }
        }
    }
}
