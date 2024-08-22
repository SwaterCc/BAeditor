using UnityEngine;

namespace BattleAbility
{
    /// <summary>
    /// 锁定打击点
    /// </summary>
    [CreateAssetMenu(menuName = "战斗编辑器/LockHitData")] 
    public class LockTargetHitData : HitData
    {
        /// <summary>
        /// 锁定目标
        /// </summary>
        public int[] TargetIds;
    }
}