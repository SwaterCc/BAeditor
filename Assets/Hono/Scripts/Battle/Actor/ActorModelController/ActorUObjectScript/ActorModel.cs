#region

using Sirenix.OdinInspector;
using UnityEngine;

#endregion

namespace Hono.Scripts.Battle
{
    public class ActorModel : MonoBehaviour
    {
        public int ActorUid;
        [ReadOnly] public EActorType ActorType;

        protected ActorModelController ModelController { get; private set; }
    }
}