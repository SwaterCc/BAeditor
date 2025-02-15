
using System;
using Newtonsoft.Json.Linq;

namespace Hono.Scripts.Battle.Core
{
    public class HateCompParamParser : IComponentParamParser
    {
        public ComponentCtorParams Parse(JToken token)
        {
            return new HateCompCtorParams
            {
                GetHateTargetType = token["GetHateTargetType"].Value<int>()
            };
        }
    }
}
