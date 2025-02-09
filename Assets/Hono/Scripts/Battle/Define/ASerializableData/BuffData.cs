#region

using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

#endregion

namespace Hono.Scripts.Battle
{
    public class BuffData : ASerializableData
    {
        public struct BuffAddBehaveInfo
        {
            public int Id;
            public int Count;
        }
        /// <summary>
        /// Buff添加规则
        /// </summary>
        public EBuffAddBlockRule blockRule;
        /// <summary>
        /// 阻断tag
        /// </summary>
        public List<int> blockTags = new();
        /// <summary>
        /// 阻断id
        /// </summary>
        public List<int> blockBuffIds = new();
        /// <summary>
        /// 同Id Buff添加规则
        /// </summary>
        public EBuffAddRule addRule;
        /// <summary>
        /// 添加成功的规则
        /// </summary>
        public EBuffAddSuccessBehave addSuccessBehave;
        /// <summary>
        /// 清理指定tag的buff数量
        /// </summary>
        public List<int> removeBuffByTags = new();
        /// <summary>
        /// id删除数量（-1为全部）
        /// </summary>
        public int tagRemoveCount;
        /// <summary>
        /// 清理指定id的buff数量
        /// </summary>
        public List<int> removeBuffByIds = new();
        /// <summary>
        /// id删除数量（-1为全部）
        /// </summary>
        public int idRemoveCount;
        /// <summary>
        /// 最大buff层数
        /// </summary>
        public int maxLayerNumber;
        /// <summary>
        /// 持续时间(-1为跟随Unit生命周期)
        /// </summary>
        public float duration;
        /// <summary>
        /// 叠层时再次执行逻辑
        /// </summary>
        public bool runAgainWhenLayering;
        /// <summary>
        /// 叠层时叠加持续时长
        /// </summary>
        public bool lifeAddWhenLayering;
        /// <summary>
        /// buff本身拥有的Tag
        /// </summary>
        public List<int> buffTags = new();
    }
}