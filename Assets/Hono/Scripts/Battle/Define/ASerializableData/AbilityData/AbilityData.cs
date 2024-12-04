#region

using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using Hono.Scripts.Battle.Base;
using Hono.Scripts.Battle.Event;
using Hono.Scripts.Battle.Tools;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using UnityEngine.Serialization;

#endregion

namespace Hono.Scripts.Battle
{
    public class AbilityData : ASerializableData
    {
        /// <summary>
        /// 节点id计数器
        /// </summary>
        private int _nodeIdCounter;
        
        /// <summary>
        /// 默认起始Group
        /// </summary>
        public int defaultStartGroupId = -1;

        /// <summary>
        /// 头节点字典
        /// </summary>
        [OdinSerialize] 
        public Dictionary<EAbilityCycle, int> HeadNodeDict = new();

        /// <summary>
        /// 存储所有数据
        /// </summary>
        [Searchable] 
        [OdinSerialize] 
        public Dictionary<int, AbilityNodeData> NodeDict = new();

        public AbilityData()
        {
            this.InitCycleHead(EAbilityCycle.Init);
            this.InitCycleHead(EAbilityCycle.PreExecute);
            this.InitCycleHead(EAbilityCycle.Executing);
            this.InitCycleHead(EAbilityCycle.EndExecute);
        }
        
        public int GenNodeId()
        {
            return ++_nodeIdCounter;
        }
    }
}