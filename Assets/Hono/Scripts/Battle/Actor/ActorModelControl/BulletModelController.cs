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
            public PreLoadModelSetup(ActorModelController modelController) : base(modelController) { }
            public override void SetupModel()
            {
                throw new NotImplementedException();
            }
        }
    }
   
}