namespace Hono.Scripts.Battle
{
    public interface IAsyncLoad
    {
        public void AsyncLoad();
        public void OnLoadedFinish();
    }

    public partial class ActorShow
    {
        public abstract class AShowComponent
        {
            protected ActorShow _actorShow;

            protected AShowComponent(ActorShow show)
            {
                _actorShow = show;
            }

            public abstract void Init();

            public abstract void Load();

            public abstract void LoadFinish();

            public virtual void Reset() { }

            /// <summary>
            /// 渲染帧
            /// </summary>
            public void Update(float dt)
            {
                onUpdate(dt);
            }

            protected abstract void onUpdate(float dt);
        }
    }

    public partial class ActorLogic
    {
        public abstract class ALogicComponent
        {
            protected ActorLogic _actorLogic;

            protected ALogicComponent(ActorLogic logic)
            {
                _actorLogic = logic;
            }

            public abstract void Init();
            public virtual void Reset() { }

            /// <summary>
            /// 逻辑帧
            /// </summary>
            public void Tick(float dt)
            {
                onTick(dt);
            }

            protected virtual void onTick(float dt) { }
        }
    }
}