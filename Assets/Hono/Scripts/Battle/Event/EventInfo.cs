using Hono.Scripts.Battle.Tools;

namespace Hono.Scripts.Battle.Event
{
    public interface IEventInfo
    {
        public void PushInVarCollection(VarCollection collection);
        public void ClearInVarCollection(VarCollection collection);
    }
    
    public class HitEventInfo : IEventInfo
    {
        
        /// <summary>
        /// 属于哪个角色
        /// </summary>
        public int ActorUid;

        /// <summary>
        /// 属于哪个能力
        /// </summary>
        public int AbilityUId;

        /// <summary>
        /// 伤害id
        /// </summary>
        public DamageResults DamageResults;

        public void PushInVarCollection(VarCollection collection)
        {
            throw new System.NotImplementedException();
        }

        public void ClearInVarCollection(VarCollection collection)
        {
            throw new System.NotImplementedException();
        }
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

        public void PushInVarCollection(VarCollection collection)
        {
            throw new System.NotImplementedException();
        }

        public void ClearInVarCollection(VarCollection collection)
        {
            throw new System.NotImplementedException();
        }
    }

    public class SkillUsedEventInfo : IEventInfo
    {
        public int SkillId;
        public int ActorUid;
        public void PushInVarCollection(VarCollection collection)
        {
            throw new System.NotImplementedException();
        }

        public void ClearInVarCollection(VarCollection collection)
        {
            throw new System.NotImplementedException();
        }
    }
}