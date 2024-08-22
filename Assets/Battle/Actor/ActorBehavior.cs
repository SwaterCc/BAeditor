using System;
using Battle.Auto;
using UnityEngine;

namespace Battle
{
    public class ActorBehavior : MonoBehaviour
    {
        private Actor _actor;

        public Actor Actor => _actor;

        private void OnEnable()
        {
            _actor = new Actor();
            BattleManager.Instance.Add(_actor);
        }

        private void Update()
        {
            var pos = _actor.GetAttrCollection().GetAttr(EAttributeType.Position);
            ((SimpleAttribute<Vector3>)pos).Set(transform.position);
        }

        private void OnDestroy() { }
    }
}