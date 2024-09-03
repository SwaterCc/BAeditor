using UnityEngine;

namespace Hono.Scripts.Battle
{
    public class ActorShowData : ScriptableObject
    {
        public int id;
        public EActorShowType ShowType;
        public int ModelId;
        public int InitAttrId;
    }
}