namespace Hono.Scripts.Battle
{
    public class TeamDefaultBirthPointLogic : ActorLogic, IPoolObject
    {
        protected override void RecycleSelf()
        {
            AObjectPool<TeamDefaultBirthPointLogic>.Pool.Recycle(this);
        }

        public void OnRecycle() { }
    }
}