using UnityEngine;

namespace Hono.Scripts.Battle
{
    public class BattleControllerModel : ActorModelController
    {
        protected override ModelSetup getModelSetup()
        {
            return null;
        }

        protected override void onEnterScene()
        {
            Model = Object.Instantiate(GameObjectPreLoadMgr.Instance[EPreLoadGameObjectType.BattleRootModel]);
            
            if (Model.TryGetComponent<ActorModel>(out var component))
            {
                component.ActorType = EActorType.BattleLevelController;
                component.ActorUid = Uid;
            }
        }

        protected override void onModelLoadComplete()
        {
            Model.transform.position = Vector3.zero;
            Model.transform.rotation = Quaternion.identity;
            Actor.SetAttr(ELogicAttr.AttrPosition, Vector3.zero, false);
            Actor.SetAttr(ELogicAttr.AttrRot, Quaternion.identity, false);
        }

        protected override void RecycleSelf() { }
    }
}