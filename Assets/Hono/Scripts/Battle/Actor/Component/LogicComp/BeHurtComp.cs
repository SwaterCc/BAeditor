
using Hono.Scripts.Battle.Event;
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

            public void OnBeHurt(HitInfo hitInfo)
            {
                Actor.SetAttr(ELogicAttr.AttrHp, (int)(Actor.GetAttr<int>(ELogicAttr.AttrHp) - hitInfo.FinalDamageValue),
	                false);
                Debug.Log($"当前血量{Actor.GetAttr<int>(ELogicAttr.AttrHp)}");
            }
        }
    }
}