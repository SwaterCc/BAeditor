namespace Hono.Scripts.Battle.Core
{
    public class WorldRoot : Unit
    {
        /// <summary>
        /// 特效组件
        /// </summary>
        public VFXComp VFXManager { get; }
        public WorldRoot()
        {
            VFXManager = addComponent(new VFXComp());
        }
        
        protected override void onTick(float dt) { }

        public override void Recycle() { }
    }
}