namespace Hono.Scripts.Battle.Core
{
    public struct Buff
    {
        /// <summary>
        /// 唯一id
        /// </summary>
        public int Uid;
        /// <summary>
        /// 配置id
        /// </summary>
        public int Id;
        /// <summary>
        /// 关联的Ability
        /// </summary>
        public int AbilityId;
        /// <summary>
        /// 标记当前 Buff 是否有效
        /// </summary>
        public bool IsValid;
        /// <summary>
        /// 当前buffLayer
        /// </summary>
        public int LayerCount;
        /// <summary>
        /// 当前 Buff 层数
        /// </summary>
        public int SourceUnitUid; // Buff 来源角色 ID
        /// <summary>
        /// Buff 所属角色 ID
        /// </summary>
        public int BelongActorUid;
        /// <summary>
        /// 持续时间
        /// </summary>
        public float TimeLeft;
        /// <summary>
        /// 永久buff（跟随buff持有者的全程生命周期）
        /// </summary>
        public bool IsPermanent;
        
        public static bool operator ==(Buff a, Buff b)
        {
            return a.Id == b.Id && a.SourceUnitUid == b.SourceUnitUid &&
                   a.BelongActorUid == b.BelongActorUid;
        }

        public static bool operator !=(Buff a, Buff b)
        {
            return !(a == b);
        }
    }
}