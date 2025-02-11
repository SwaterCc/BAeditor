namespace Hono.Scripts.Battle.Core
{
    /// <summary>
    /// 组件构造函数抽象基类
    /// </summary>
    public abstract class ComponentCtorParams { }

    /// <summary>
    /// 组件基类
    /// </summary>
    public abstract class UnitComponent
    {
        private bool _isFirst = true;

        public Unit Unit { get; set; }

        protected ComponentCtorParams CtorParams { get; }

        protected UnitComponent(ComponentCtorParams ctorParams)
        {
            CtorParams = ctorParams;
        }

        public abstract void Init();

        public void Tick(float dt)
        {
            if (_isFirst)
            {
                beforeTick();
                _isFirst = false;
            }

            onTick(dt);
        }

        /// <summary>
        /// 第一次Tick之前运行
        /// </summary>
        protected virtual void beforeTick() { }

        protected virtual void onTick(float dt) { }

        public void Clear()
        {
            _isFirst = true;
            onClear();
        }

        protected abstract void onClear();
    }
}