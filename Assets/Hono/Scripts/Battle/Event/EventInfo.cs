using System.Reflection;

namespace Hono.Scripts.Battle.Event
{
    public interface IEventInfo
    {
        //消息来源
        
        public void SetFieldsInAbilityVariables(Ability ability);
        public void ClearFields(Ability ability);
    }

    
    
    public class HitInfo : IEventInfo
    {
        /// <summary>
        /// 伤害来源Actor
        /// </summary>
        public int SourceActorId;

        /// <summary>
        /// 伤害来源AbilityUid
        /// </summary>
        public int SourceAbilityUId;

        /// <summary>
        /// 伤害来源AbilityConfig
        /// </summary>
        public int SourceAbilityConfigId;

        /// <summary>
        /// 属于哪个能力类型
        /// </summary>
        public int SourceAbilityType;

        /// <summary>
        /// 最终伤害
        /// </summary>
        public float FinalDamageValue;

        /// <summary>
        /// 是否暴击
        /// </summary>
        public bool IsCritical;

        /// <summary>
        /// 最终冲击力
        /// </summary>
        public float ImpactValue;

        /// <summary>
        /// 伤害是否免疫
        /// </summary>
        public bool IsImmunity;

        /// <summary>
        /// 是否杀死目标
        /// </summary>
        public bool IsKillTarget;

        public void ParseDamageResult(DamageResults results)
        {
            ImpactValue = results.ImpactValue;
            IsCritical = results.IsCritical;
            FinalDamageValue = results.DamageValue;
        }

        public void SetFieldsInAbilityVariables(Ability ability)
        {
            foreach (var fieldInfo in GetType().GetFields(BindingFlags.Public | BindingFlags.Instance))
            {
                var name = "EventInfo:" + fieldInfo.Name;
                ability.Variables.Set(name, fieldInfo.GetValue(this));
            }
        }

        public void ClearFields(Ability ability)
        {
            foreach (var fieldInfo in GetType().GetFields(BindingFlags.Public | BindingFlags.Instance))
            {
                var name = "EventInfo:" + fieldInfo.Name;
                ability.Variables.Delete(name);
            }
        }
    }


    public class SkillUsedEventInfo : IEventInfo
    {
        public int SkillId;
        public int ActorUid;
        public void SetFieldsInAbilityVariables(Ability ability) { }

        public void ClearFields(Ability ability) { }
    }
}