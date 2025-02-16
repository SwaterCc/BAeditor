using System;
using System.Collections.Generic;
using System.Reflection;
using System.Reflection.Emit;
using System.Threading;
using Cysharp.Threading.Tasks;
using Hono.Scripts.Battle.Core.Base;
using Newtonsoft.Json.Linq;

namespace Hono.Scripts.Battle.Core
{
    /// <summary>
    /// 组件构造函数抽象基类
    /// </summary>
    public abstract class UnitCompCtorParams { }
    
    /// <summary>
    /// 组件基类
    /// </summary>
    public abstract class UnitComponent
    {
        public Unit Unit { get; set; }
        
        public abstract void Init();

        public virtual void Ctor(UnitCompCtorParams ctorParams){}
        
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
}