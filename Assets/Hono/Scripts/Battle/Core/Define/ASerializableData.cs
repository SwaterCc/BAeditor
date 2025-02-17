using Sirenix.OdinInspector;

namespace Hono.Scripts.Battle
{
    public abstract class ASerializableData : SerializedScriptableObject
    {
        public int id;
        public string fileName;
        public string desc;
        public string path;
    }

    public interface IIncludeAbility
    {
        public void AddAbilityData();
    }
}