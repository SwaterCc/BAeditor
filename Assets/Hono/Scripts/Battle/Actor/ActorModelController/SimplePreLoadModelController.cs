namespace Hono.Scripts.Battle
{
    public class SimplePreLoadModelController : ActorModelController, IPoolObject
    {
        private readonly PreLoadModelSetup _modelSetup = new();

        public void Init(Actor actor, EPreLoadGameObjectType objectType)
        {
            base.Init(actor);
            _modelSetup.Init(objectType);
        }

        protected override ModelSetup getModelSetup()
        {
            return _modelSetup;
        }

        protected override void RecycleSelf()
        {
            AObjectPool<SimplePreLoadModelController>.Pool.Recycle(this);
        }

        public void OnRecycle() { }
    }
}