#region

using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;

#endregion

namespace Hono.Scripts.Battle
{
    public class LootActorModel : ActorModel
    {
        [ReadOnly] public Collider Trigger;

        private LootLogic _logic;
        private bool _isUsed;
        public TextMeshPro Desc;

        private void Awake()
        {
            ActorType = EActorType.Loot;

            if (!Trigger.isTrigger)
            {
                Trigger.isTrigger = true;
            }

            Trigger.includeLayers = BattleConstValue.ActorPawn;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (_logic == null)
            {
                var actor = ActorManager.Instance.GetActor(ActorUid);
                if (actor == null)
                {
                    return;
                }

                _logic = (LootLogic)actor.Logic;
            }

            if (_isUsed) return;

            if (other.TryGetComponent<ActorModel>(out var otherModel))
            {
                _logic.OnPawnPickUp(otherModel.ActorUid);
                _isUsed = true;
            }
        }
    }
}