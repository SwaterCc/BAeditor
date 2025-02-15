
using System;
using Newtonsoft.Json.Linq;

namespace Hono.Scripts.Battle.Core
{
    public class ModelControllerCompParamParser : IComponentParamParser
    {
        public ComponentCtorParams Parse(JToken token)
        {
            return new ModelControllerCompCtorParams
            {
                UnitModelType = token["UnitModelType"].Value<string>()
            };
        }
    }
}
