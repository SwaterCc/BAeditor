using Hono.Scripts.Battle.Core;
using Newtonsoft.Json.Linq;

namespace Hono.Scripts.Battle
{
    public interface IComponentParamParser
    {
        ComponentCtorParams Parse(JToken token);
    }
}