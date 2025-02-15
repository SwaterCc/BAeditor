
using System;

namespace Hono.Scripts.Battle.Core
{
    public partial class HateComp
    {
        public override void Ctor(ComponentCtorParams param)
        {
            if (param is HateCompCtorParams ctorParams)
            {
                this.GetHateTargetType = ctorParams.GetHateTargetType;
            }
        }
    }
}
