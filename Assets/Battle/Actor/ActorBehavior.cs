using UnityEngine;

namespace Battle
{
    public class ActorBehavior : MonoBehaviour
    {
        private Actor _actor;

        public Actor Actor => _actor;

        private void Awake()
        {
            _actor = new Actor();
            _actor.Init();
            BattleManager.Instance.Add(_actor);
        }
        
        private void OnDestroy() { }
    }
}