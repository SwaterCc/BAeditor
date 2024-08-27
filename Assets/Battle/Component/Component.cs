namespace Battle
{
    public enum EShowComponentType
    {
        Transform,
        Count,
    }

    public enum ELogicComponentType
    {
        HitBoxComp,
        BeHurtComp,
        Count,
    }

    public interface IAsyncLoad
    {
        public void AsyncLoad();
        public void OnLoadedFinish();
    }

    public abstract class AShowComponent
    {
        protected ActorShow _actorShow;

        protected AShowComponent(ActorShow show)
        {
            _actorShow = show;
        }

        public abstract EShowComponentType GetCompType();
        public abstract void Init();
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

    public abstract class ALogicComponent
    {
        protected ActorLogic _actorLogic;

        protected ALogicComponent(ActorLogic logic)
        {
            _actorLogic = logic;
        }

        public abstract ELogicComponentType GetCompType();
        public abstract void Init();
        public virtual void Reset() { }

        /// <summary>
        /// 逻辑帧
        /// </summary>
        public void Tick(float dt)
        {
            onTick(dt);
        }

        protected abstract void onTick(float dt);
    }
}