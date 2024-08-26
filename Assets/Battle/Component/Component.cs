namespace Battle
{
    public enum EShowComponentType
    {
        Model,
        Count,
    }
    
    public enum ELogicComponentType
    {
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
        public abstract void Init();

        public virtual void Reset() { }

        public abstract EShowComponentType GetCompType();

        /// <summary>
        /// 渲染帧
        /// </summary>
        public virtual void Update(float dt) { }

        public virtual void OnEnterScene() { }

        public virtual void OnExitScene() { }
    }

    public abstract class ALogicComponent
    {
        public abstract void Init();

        public virtual void Reset() { }

        public abstract ELogicComponentType GetCompType();

        /// <summary>
        /// 逻辑帧
        /// </summary>
        public virtual void Tick(float dt) { }

        public virtual void OnEnterScene() { }

        public virtual void OnExitScene() { }
    }
}