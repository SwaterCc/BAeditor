using UnityEngine;

namespace Hono.Scripts.Battle
{
    public abstract class ActorInput
    {
        protected ActorLogic Logic { get; }
        
        public Vector3 MoveInputValue { get; protected set; }

        protected ActorInput(ActorLogic logic)
        {
            Logic = logic;
        }

        public void Init()
        {
            onInit();
        }

        protected virtual void onInit() { }

        public void Tick(float dt)
        {
            onTick(dt);
        }

        protected abstract void onTick(float dt);
    }


    public class NoInput : ActorInput
    {
        public NoInput(ActorLogic logic) : base(logic) { }
        protected override void onTick(float dt) { }
    }
}