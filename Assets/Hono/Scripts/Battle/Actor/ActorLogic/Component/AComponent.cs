namespace Hono.Scripts.Battle
{
    public partial class ActorLogic
    {
        public abstract class AComponent
        {
            public ActorLogic ActorLogic { get; }
            public Actor Self => ActorLogic.Self;

            protected AComponent(ActorLogic logic)
            {
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