
using UnityEngine;

namespace Hono.Scripts.Battle
{
    public partial class ActorLogic
    {
        public class BeHurtComp : ALogicComponent
        {
            public BeHurtComp(ActorLogic logic) : base(logic) { }

            public override void Init() { }


            protected override void onTick(float dt) { }

            public void OnBeHurt(DamageResults damageResults)
            {
                //Debug.Log($"造成伤害{damageResults.DamageValue}");
            }
        }
    }
}