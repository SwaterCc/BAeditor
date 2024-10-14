using System;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Hono.Scripts.Battle
{
    public partial class ActorModelController
    {
        public class SceneModelSetup : ModelSetup
        {
            private readonly SceneActorModel _sceneActorModel;

            public SceneModelSetup(SceneActorModel sceneActorModel)
            {
                _sceneActorModel = sceneActorModel;
            }
            
            public override void SetupModel(ActorModelController modelController, Action loadComplete = null)
            {
                modelController._model = _sceneActorModel.gameObject;
                modelController.IsModelLoadFinish = true;
                modelController.IsSceneModel = true;
                modelController.Actor.SetAttr(ELogicAttr.AttrPosition, _sceneActorModel.transform.position, false);
                modelController.Actor.SetAttr(ELogicAttr.AttrRot, _sceneActorModel.transform.rotation, false);
                _sceneActorModel.OnModelSetupFinish(modelController.Actor);
            }
        }
    }
}