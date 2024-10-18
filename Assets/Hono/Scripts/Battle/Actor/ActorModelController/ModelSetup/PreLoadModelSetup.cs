using Cysharp.Threading.Tasks;
using System;
using UnityEngine;
using UnityEngine.AddressableAssets;
using Object = UnityEngine.Object;

namespace Hono.Scripts.Battle
{
    public partial class ActorModelController
    {
        public class PreLoadModelSetup : ModelSetup
        {
            private readonly EPreLoadGameObjectType _objectType;
            
            public PreLoadModelSetup(EPreLoadGameObjectType objectType)
            {
                _objectType = objectType;
            }

            public override void SetupModel(ActorModelController modelController, Action loadComplete = null)
            {
                modelController._model = GameObjectPreLoadMgr.Instance[_objectType];
                
                modelController.IsModelLoadFinish = true;
                
                loadComplete?.Invoke();
            }
        }
    }
   
}