namespace Hono.Scripts.Battle.Core
{
    /// <summary>
    /// 世界级Ability介入接口
    /// </summary>
    public class WorldRoot : Unit {
        public WorldRoot()
        {
            Uid = World.Current.GetUid();
        }
        
        protected override void onTick(float dt) { }

        public override void Recycle() { }
    }
}