namespace Hono.Scripts.Battle.Core
{
    //Actor需要一个基础模型，通常是一个Character,如果该单位为部位或者组件，从设计层面上不会主动移动，则会使用Collider为其赋予碰撞体积
    public class ActorModelController
    {
        /// <summary>
        /// 演出效果管理器，加载演出模型，收集挂点
        /// </summary>
        public PerformanceEffectController PEController { get; }
    }
}