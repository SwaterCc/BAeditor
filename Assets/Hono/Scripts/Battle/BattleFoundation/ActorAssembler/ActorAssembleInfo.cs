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
    public partial class ActorAssembleInfo
    {
        public class ActorCtorAbilityInfo
        {
            public int AbilityId;
            public bool Execute;
        }
        
        /// <summary>
        /// Actor基础类型
        /// </summary>
        public EActorType ActorType { get; private set; }

        /// <summary>
        /// 基础属性表Id
        /// </summary>
        public int BaseAttrTableId { get; private set; }
        
        /// <summary>
        /// Model表Id
        /// </summary>
        public int ModelId { get; private set;}
        
        /// <summary>
        /// 初始Tag
        /// </summary>
        public List<int> Tags = new();

        /// <summary>
        /// 拥有的Ability列表
        /// </summary>
        public List<ActorCtorAbilityInfo> AbilityList = new();

        /// <summary>
        /// 组件位
        /// </summary>
        public EUnitComponentKey UnitComponentBits { get; private set; }

        /// <summary>
        /// 组件工厂对象
        /// </summary>
        public readonly Dictionary<EUnitComponentKey, UnitCompCtorParams> UnitCompCtorParams = new();
    }
}