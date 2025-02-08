#region

using System.Collections.Generic;
using UnityEngine;

#endregion

namespace Hono.Scripts.Battle
{
    public class BuffData : ASerializableData
    {
        /// <summary>
        /// 同Id Buff行为
        /// </summary>
        public EBuffReplaceRule ReplaceRule;
        /// <summary>
        /// Buff添加规则
        /// </summary>
        public EApplicationRequirement AddRule;
        /// <summary>
        /// Buff删除规则
        /// </summary>
        public EBuffRemoveType RemoveType;
        public List<int> FilterTags = new List<int>();
        public int MaxLayerNumber;
        public float MaxDuration;
    }
    
    
    
    //1.添加相同buff时检测
        //无限制
        //存在相同Id时允许添加
        //存在不相Id时允许添加
        //存在相同Tag时允许添加
        //存在不同Tag时允许添加
        //存在相同来源时允许添加
        //存在不同来源时允许添加
        
    //2.添加后的行为
        //1.叠层
        //2.替换
}