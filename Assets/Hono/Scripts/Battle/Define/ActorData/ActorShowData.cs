using UnityEngine;

namespace Hono.Scripts.Battle
{
    public class ActorShowData : ScriptableObject,IAllowedIndexing
    {
        public int ID => id;
        public int id;
        public EActorShowType ShowType;
        public int ModelId;
        public int InitAttrId;
    }
}