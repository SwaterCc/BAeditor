using System.Threading;
using Cysharp.Threading.Tasks;
using Newtonsoft.Json.Linq;

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
        public Unit Unit { get; set; }

        protected ComponentCtorParams CtorParams { get; private set; }

        public virtual void Ctor(ComponentCtorParams ctorParams) { }
        
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

        public abstract void Recycle();
    }
    
    /// <summary>
    /// 组件工厂基类
    /// </summary>
    public abstract class UnitComponentFactory
    {
        public ComponentCtorParams UnCtorParams { get; set; }
        public abstract UnitComponent CreateComponent();
    }
    
    public class UnitComponentFactory<T> : UnitComponentFactory where T : UnitComponent, IGPoolObject, new()
    {
        public override UnitComponent CreateComponent()
        {
            T component = GPool<T>.Pool.Rent();
            component.Ctor(UnCtorParams);
            return component;
        }
    }
}