using System;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Hono.Scripts.Battle
{
    public partial class ActorModelController
    {
        public class SceneModelSetup : ModelSetup
        {
            private SceneActorModel _sceneActorModel;

            public void Init(SceneActorModel sceneActorModel)
            {
                _sceneActorModel = sceneActorModel;
            }

            protected override void OnLoadModel()
            {
                _gameObject = _sceneActorModel.gameObject;
                _sceneActorModel.OnModelSetupFinish(_modelController.Actor);
                _modelController.Actor.SetAttr(ELogicAttr.AttrPosition, _sceneActorModel.transform.position, false);
                _modelController.Actor.SetAttr(ELogicAttr.AttrRot, _sceneActorModel.transform.rotation, false);
            }

            protected override void OnUnInit()
            {
                _sceneActorModel = null;
            }
        }
    }
}