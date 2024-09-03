namespace Hono.Scripts.Battle
{
    public partial class ActorLogic
    {
        public class DragMoveComp : ALogicComponent
        {
            public DragMoveComp(ActorLogic logic) : base(logic) { }
            public override void Init() { }

            protected override void onTick(float dt) { }
        }
    }
}