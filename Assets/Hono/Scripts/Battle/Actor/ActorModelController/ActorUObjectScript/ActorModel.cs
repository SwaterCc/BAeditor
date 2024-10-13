using System;
using Hono.Scripts.Battle.Event;
using Sirenix.OdinInspector;
using System.Collections.Generic;
using UnityEngine;

namespace Hono.Scripts.Battle
{
    public class ActorModel : MonoBehaviour
    {
        [ReadOnly] public int ActorUid;
        [ReadOnly] public EActorType ActorType;

        protected ActorModelController ModelController { get; private set; }

        public void BindModelController(ActorModelController modelController)
        {
            ModelController = modelController;
        }
    }
}