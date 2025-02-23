using Newtonsoft.Json.Linq;

namespace Hono.Scripts.Battle
{
    public abstract class ConfigTable
    {
        public abstract ConfigTableRow GetRowBase(int id);
        public abstract void ParseJsonObject(JObject tableJObject);
    }
}