using Cysharp.Threading.Tasks;

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
        /// <summary>
        /// 仅允许应用在UnitComponent及其衍生类上
        /// </summary>
        protected internal interface IAsyncLoadTask
        {
            /// <summary>
            /// 加载接口
            /// </summary>
            public UniTask LoadTask();
        }
        
        public Unit Unit { get; set; }

        protected ComponentCtorParams CtorParams { get; }

        protected UnitComponent(ComponentCtorParams ctorParams)
        {
            CtorParams = ctorParams;
        }

        public abstract void Init();

        public void Tick(float dt)
        {
            onTick(dt);
        }

        /// <summary>
        /// 第一次Tick之前运行,父类为空函数
        /// </summary>
        public virtual void BeforeTick() { }

        protected virtual void onTick(float dt) { }

        public void Clear()
        {
            onClear();
        }

        protected abstract void onClear();
    }
}