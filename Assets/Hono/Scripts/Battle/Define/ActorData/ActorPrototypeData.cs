using System.Collections.Generic;
using UnityEngine;

namespace Hono.Scripts.Battle
{
    [CreateAssetMenu(menuName = "战斗编辑器/Actor/ActorPrototypeData")] 
    public class ActorPrototypeData : ScriptableObject,IAllowedIndexing
    {
        public int ID => id;
        public int id;
        public int ShowConfigId;
        public int LogicConfigId;
        public EActorType ActorType;
    }
}