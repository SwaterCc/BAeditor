namespace Hono.Scripts.Battle.Event
{
    public interface IEventInfo { }


    public class HitEventInfo : IEventInfo
    {
        /// <summary>
        /// 打击点配置ID
        /// </summary>
        public int HitConfigId;
        
        /// <summary>
        /// 属于哪个角色
        /// </summary>
        public int ActorId;

        /// <summary>
        /// 属于哪个能力
        /// </summary>
        public int AbilityUId;

        /// <summary>
        /// 伤害id
        /// </summary>
        public int DamageId;
    }
    
    public class MotionEventInfo : IEventInfo
    {
        /// <summary>
        /// 属于哪个角色
        /// </summary>
        public int ActorId;
        
        /// <summary>
        /// 位移id
        /// </summary>
        public int MotionId;
    }
    
}