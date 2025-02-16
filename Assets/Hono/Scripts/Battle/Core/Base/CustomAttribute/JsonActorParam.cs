using System;

namespace Hono.Scripts.Battle.Core.Base
{
    /// <summary>
    /// json格式文件导出字段，放置在UnitComponent子类上，导出后可以在componentList下，和componentCtor下自动补全组件名,Desc是组件功能的具体描述
    /// </summary>
    [AttributeUsage(AttributeTargets.Field)]
    public class JsonActorParam : Attribute
    {
        public string Desc;

        public JsonActorParam(string desc = "")
        {
            Desc = desc;
        }
    }

    
    /// <summary>
    /// json格式文件导出字段，放置在UnitComponent子类上，导出后可以在componentList下，和componentCtor下自动补全组件名,Desc是组件功能的具体描述
    /// </summary>
    [AttributeUsage(AttributeTargets.Class)]
    public class JsonUnitComponent : Attribute
    {
        public string Desc;

        public JsonUnitComponent(string desc = "")
        {
            Desc = desc;
        }
    }

    /// <summary>
    /// json格式文件导出字段，放置在UnitCompCtorParams子类上，json ComponentCtor key 中配置 与ComponentType.Name相同的组件参数时产生输入补全,
    /// </summary>
    [AttributeUsage(AttributeTargets.Class)]
    public class JsonUnitCompCtorParams : Attribute
    {
        public Type ComponentType;

        public JsonUnitCompCtorParams(Type componentType)
        {
            ComponentType = componentType;
        }
    }

    /// <summary>
    /// json格式文件导出字段，放置在UnitComponent子类字段上，componentCtor中配置组件参数时下产生输入补全，ParamDesc是UnitCompCtorParams子类字段的描述
    /// </summary>
    [AttributeUsage(AttributeTargets.Field)]
    public class JsonUnitCompCtorParam : Attribute
    {
        public string ParamDesc;

        public JsonUnitCompCtorParam(string paramDesc = "")
        {
            ParamDesc = paramDesc;
        }
    }
}