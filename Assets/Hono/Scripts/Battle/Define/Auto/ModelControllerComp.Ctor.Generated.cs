
using System;

namespace Hono.Scripts.Battle.Core
{
    public partial class ModelControllerComp
    {
        public override void Ctor(ComponentCtorParams param)
        {
            if (param is ModelControllerCompCtorParams ctorParams)
            {
                this.UnitModelType = ctorParams.UnitModelType;
            }
        }
    }
}
