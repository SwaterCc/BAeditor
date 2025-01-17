namespace Hono.Scripts.Battle
{
    public abstract class AComponent
    {
        public Actor Self { get; set; }

        public abstract void onInit();

        public void Tick(float dt)
        {
            onTick(dt);
        }

        protected virtual void onTick(float dt) { }

        public abstract AComponent Clone();

        public abstract void Clear();
    }
}