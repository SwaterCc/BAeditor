
using System;
using Newtonsoft.Json.Linq;

namespace Hono.Scripts.Battle.Core
{
    public class MoveCompParamParser : IComponentParamParser
    {
        public ComponentCtorParams Parse(JToken token)
        {
            return new MoveCompCtorParams
            {

            };
        }
    }
}
