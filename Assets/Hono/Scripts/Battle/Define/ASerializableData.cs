using Sirenix.OdinInspector;

namespace Hono.Scripts.Battle
{
    public abstract class ASerializableData : SerializedScriptableObject
    {
        public int id;
        public string path;
    }
}