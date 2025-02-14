using System;

namespace Hono.Scripts.Battle.Core.Base
{
    /// <summary>
    /// json格式文件导出字段，放置在UnitComponent子类上，导出后可以在componentList下，和componentCtor下自动补全组件名
    /// </summary>
    [AttributeUsage(AttributeTargets.Class)]
    public class JsonUnitComponent : Attribute { }
    
    /// <summary>
    /// json格式文件导出字段，放置在UnitComponent子类字段上，componentCtor中配置组件参数时下产生输入补全，
    /// 并自动生成被标记特性的参数 组成的ComponentCtorParams的子类，创建一个新的.cs文件将其写入
    /// </summary>
    [AttributeUsage(AttributeTargets.Field)]
    public class JsonUnitComponentParam : Attribute { }
}