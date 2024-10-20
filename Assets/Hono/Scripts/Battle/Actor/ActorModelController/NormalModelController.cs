using UnityEngine;

namespace Hono.Scripts.Battle
{
    public class NormalModelController : ActorModelController, IPoolObject
    {
        private readonly AsyncLoadModelSetup _modelSetup = new();
        
        protected override ModelSetup getModelSetup()
        {
            return _modelSetup;
        }

        protected override void RecycleSelf()
        {
            AObjectPool<NormalModelController>.Pool.Recycle(this);
        }

        protected override void onEnterScene()
        {
            Model.transform.position = Actor.Pos;
            Model.transform.rotation = Actor.Rot;
        }

        public void OnRecycle() { }
    }
}