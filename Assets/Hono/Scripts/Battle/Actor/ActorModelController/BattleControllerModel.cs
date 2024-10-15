using UnityEngine;

namespace Hono.Scripts.Battle
{
    public class BattleControllerModel : ActorModelController
    {
        public BattleControllerModel(Actor actor) : base(actor) { }

        protected override ModelSetup getModelSetup()
        {
            return new PreLoadModelSetup(EPreLoadGameObjectType.BattleRootModel);
        }

        public void OnEnterBattleGroundFirstTime()
        {
            getModelSetup().SetupModel(this, onLoadFinish);
            
            if (Model.TryGetComponent<ActorModel>(out var component))
            {
                component.ActorType = EActorType.BattleLevelController;
                component.ActorUid = Uid;
            }
        }

        private void onLoadFinish()
        {
            Model.transform.position = Vector3.zero;
            Model.transform.rotation = Quaternion.identity;
            Actor.SetAttr(ELogicAttr.AttrPosition, Vector3.zero, false);
            Actor.SetAttr(ELogicAttr.AttrRot, Quaternion.identity, false);
        }
    }
}