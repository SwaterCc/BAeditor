using Hono.Scripts.Battle.Scene;
using UnityEngine;

namespace Hono.Scripts.Battle
{
    public class BattleController : ActorLogic
    {
        private VFXComp _vfxComp;
        public VFXComp VFXComp => _vfxComp;

        public BattleControllerModel ModelController => (BattleControllerModel)Actor.ModelController;
        
        protected override void OnInit() { }

        protected override void constructComponents()
        {
            _vfxComp = new VFXComp(this);
            addComponent(_vfxComp);
        }

        protected override void RecycleSelf() { }

        public void RunAbility(int abilityConfigId)
        {
            if (abilityConfigId <= 0)
            {
                return;
            }

            AbilityController.AwardAbility(abilityConfigId, true);
        }

        public void RemoveAbility(int abilityConfigId)
        {
            if (abilityConfigId <= 0)
            {
                return;
            }

            AbilityController.RemoveAbility(abilityConfigId);
        }
    }
}