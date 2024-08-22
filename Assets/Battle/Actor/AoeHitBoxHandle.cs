using System;
using System.Collections.Generic;
using UnityEngine;

namespace Battle
{
    public class AoeHitBoxHandle : MonoBehaviour
    {
        public Collider box;

        private List<IBeHurt> _hitActors = new List<IBeHurt>(16);

        public List<IBeHurt> GetHitList() => _hitActors;

        
        
        public void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent<ActorBehavior>(out var actorBehavior))
                if (!_hitActors.Contains(actorBehavior.Actor))
                    _hitActors.Add(actorBehavior.Actor);
        }

        public void OnTriggerExit(Collider other)
        {
            if (other.TryGetComponent<ActorBehavior>(out var actorBehavior))
            {
                if (_hitActors.Contains(actorBehavior.Actor))
                    _hitActors.Remove(actorBehavior.Actor);
            }
        }
    }
}