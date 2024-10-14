using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Hono.Scripts.Battle
{
    
    public class TriggerBoxModel : SceneActorModel
    {
        [LabelText("拥有的Ability")]
        public List<int> AbilityIds = new();

        [LabelText("默认为开启状态")]
        public bool IsDefaultOpen;
      
        private readonly Dictionary<int,float> _stayDicts = new(32);
        private TriggerBoxModelController _triggerBoxController;
        protected override void onInit()
        {
            ActorType = EActorType.TriggerBox;
            
            if (gameObject.TryGetComponent<Collider>(out var triggerComp))
            {
                triggerComp.isTrigger = true;
            }
        }

        public override void OnModelSetupFinish(Actor actor)
        {
            _triggerBoxController = (TriggerBoxModelController)ModelController;
            _triggerBoxController.SetActive(IsDefaultOpen);
        }

        private void OnTriggerEnter(Collider other)
        {
            if (ModelController == null) return;
            if (other.TryGetComponent<ActorModel>(out var actorModel))
            {
                _triggerBoxController.OnTriggerEnter(actorModel.ActorUid);
                _stayDicts.TryAdd(actorModel.ActorUid, 0);
            }
        }

        private void Update()
        {
            if (ModelController == null) return;
            
            foreach (var pair in _stayDicts)
            {
                _stayDicts[pair.Key] += Time.deltaTime;
                if (_stayDicts[pair.Key] > 0.2f)
                {
                    _triggerBoxController.OnTriggerStay(pair.Key);
                }
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (ModelController == null) return;
            if (other.TryGetComponent<ActorModel>(out var actorModel))
            {
                _triggerBoxController.OnTriggerExit(actorModel.ActorUid);
                _stayDicts.Remove(actorModel.ActorUid);
            }
        }
    }
}