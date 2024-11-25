namespace Hono.Scripts.Battle
{
    public partial class ActorLogic
    {
        public abstract class AComponent
        {
            public Actor Self { get; }
            public ActorLogic ActorLogic { get; }

            protected AComponent(ActorLogic logic)
            {
                Self = logic.Self;
                ActorLogic = logic;
            }

            public abstract void Init();

            public virtual void EnterScene() { }

            public void Tick(float dt)
            {
                onTick(dt);
            }

            protected virtual void onTick(float dt) { }

            public abstract void Clear();
        }
    }
}