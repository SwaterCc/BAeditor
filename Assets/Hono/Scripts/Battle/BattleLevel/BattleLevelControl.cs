namespace Hono.Scripts.Battle
{
    public class BattleController : ActorLogic, IAPoolObject
    {
        private VFXComp _vfxComp;
        public VFXComp VFXComp => _vfxComp;

        public BattleController()
        {
            _vfxComp = addComponent(new VFXComp(this));
        }

        public override void RecycleLogicObject()
        {
            APool<BattleController>.Pool.Recycle(this);
        }
    }
}