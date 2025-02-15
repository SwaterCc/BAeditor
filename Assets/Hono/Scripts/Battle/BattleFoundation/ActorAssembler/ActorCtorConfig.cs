using System;
using System.Collections.Generic;
using Hono.Scripts.Battle.Core;
using Hono.Scripts.Battle.Core.Base;
using Newtonsoft.Json.Linq;

namespace Hono.Scripts.Battle.Core
{
    /// <summary>
    /// 解析出的结构数据
    /// </summary>
    public partial class ActorCtorConfig
    {
        /// <summary>
        /// Actor基础类型
        /// </summary>
        public EActorType ActorType { get; private set; }

        /// <summary>
        /// 基础属性表Id
        /// </summary>
        public int BaseAttrTableId { get; private set; }

        /// <summary>
        /// 加载UnitObjectProxy
        /// </summary>
        public bool IsLoadUnityObjectProxy { get; private set; }

        /// <summary>
        /// 代理类型
        /// </summary>
        public EUnityObjectProxyType ProxyType { get; private set; }

        /// <summary>
        /// 头像Icon
        /// </summary>
        public string RPGIcon { get; private set; }

        /// <summary>
        /// 初始Tag
        /// </summary>
        public List<int> Tags = new();

        /// <summary>
        /// 拥有的Ability列表
        /// </summary>
        public List<ActorCtorAbilityInfo> AbilityList = new();

        /// <summary>
        /// 组件工厂对象
        /// </summary>
        public readonly List<UnitComponentFactory> Factories = new();
    }
    
    public class ActorCtorAbilityInfo
    {
        public int AbilityId;
        public bool Execute;
    }
}