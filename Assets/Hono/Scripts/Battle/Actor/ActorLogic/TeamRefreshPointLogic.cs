namespace Hono.Scripts.Battle
{
    public class TeamRefreshPointLogic : ActorLogic ,IPoolObject
    {
        protected override void RecycleSelf()
        {
            AObjectPool<TeamRefreshPointLogic>.Pool.Recycle(this);
        }

        public void OnRecycle() { }
    }
}