namespace Hono.Scripts.Battle.Event
{
    public interface IEventInfo
    {
        public void SetFieldsInAbilityVariables(Ability ability);
        public void ClearFields(Ability ability);
    }
    
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
        public DamageResults DamageResults;

        public void SetFieldsInAbilityVariables(Ability ability)
        {
            
        }

        public void ClearFields(Ability ability)
        {
            
        }
    }
    

    public class SkillUsedEventInfo : IEventInfo
    {
        public int SkillId;
        public int ActorUid;
        public void SetFieldsInAbilityVariables(Ability ability)
        {
            
        }

        public void ClearFields(Ability ability)
        {
            
        }
    }
}