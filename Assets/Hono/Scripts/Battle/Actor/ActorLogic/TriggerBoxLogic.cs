namespace Hono.Scripts.Battle
{
    public class TriggerBoxLogic : ActorLogic, IPoolObject
    {
        protected override void RecycleSelf()
        {
            AObjectPool<TriggerBoxLogic>.Pool.Recycle(this);
        }

        public void OnRecycle() { }
    }
}