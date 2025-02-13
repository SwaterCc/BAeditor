namespace Hono.Scripts.Battle.Core
{
    public class WorldRoot : Unit
    {
        /// <summary>
        /// 特效组件
        /// </summary>
        public VFXComp VFXComp { get; }
        public WorldRoot()
        {
            VFXComp = addComponent(new VFXComp(null));
        }
        
        protected override void onTick(float dt) { }

        public override void Recycle() { }
    }
}