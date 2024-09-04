using System.Collections.Generic;
using UnityEngine;

namespace Hono.Scripts.Battle
{
    public class ActorLogicData : ScriptableObject ,IAllowedIndexing
    {
        public int ID => id;
        public int id;
        public EActorLogicType logicType;
        public int initAttrId;
        public List<int> ownerSkills = new List<int>();
        public List<int> ownerBuffs = new List<int>();
        public List<int> ownerOtherAbility = new List<int>();
    }
}