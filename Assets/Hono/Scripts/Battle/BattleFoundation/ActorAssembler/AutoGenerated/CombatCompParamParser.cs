
using System;
using Newtonsoft.Json.Linq;

namespace Hono.Scripts.Battle.Core
{
    public class CombatCompParamParser : IComponentParamParser
    {
        public ComponentCtorParams Parse(JToken token)
        {
            return new CombatCompCtorParams
            {
                AllowElementEffect = token["AllowElementEffect"].Value<bool>(),
                DisableBuffAdd = token["DisableBuffAdd"].Value<bool>()
            };
        }
    }
}
