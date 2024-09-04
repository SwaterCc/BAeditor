using System.Collections.Generic;
using UnityEngine;

namespace Hono.Scripts.Battle
{
    public class ActorPrototypeData : ScriptableObject,IAllowedIndexing
    {
        public int ID => id;
        public int id;
        public float Speed;
        public int ShowConfigId;
        public int LogicConfigId;
        
    }
}