using Sirenix.OdinInspector;
using UnityEngine;

namespace Hono.Scripts.Battle
{
    public class ActorModel : MonoBehaviour
    {
        [ReadOnly] public int ActorUid;
        [ReadOnly] public EActorType ActorType;

        protected ActorModelController ModelController { get; private set; }
    }
}