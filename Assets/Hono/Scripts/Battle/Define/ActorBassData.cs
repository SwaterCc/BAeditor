using System.Collections.Generic;
using Sirenix.OdinInspector;

namespace Hono.Scripts.Battle
{
    public class ActorBassData : SerializedScriptableObject
    {
        public int id;
        public EActorType ActorType;
        public float Speed;
        public int ModelId;
        public List<int> OwnerSkills = new List<int>();
        public List<int> OwnerBuffs = new List<int>();
        public List<int> OwnerAbility = new List<int>();
    }
}