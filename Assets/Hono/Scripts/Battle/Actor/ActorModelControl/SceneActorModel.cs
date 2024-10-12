using System;
using UnityEngine;

namespace Hono.Scripts.Battle
{
    public class SceneActorModel : MonoBehaviour
    {
        public int StaticActorUid;
        public EActorType ActorType;

        private void Awake()
        {
            ActorManager.Instance.CreateSceneActor(this);
        }
    }
}