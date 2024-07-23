namespace BattleAbility.Editor
{
    /// <summary>
    /// 能力配置的归属类型
    /// </summary>
    public enum EAbilityType
    {
        UnInit,
        Skill,
        Buff,
        Bullet,
    }
    
    /// <summary>
    /// 能力的基础数据，抽象基类，具体的数据由子类实现
    /// </summary>
    public abstract class BattleAbilityBaseConfig
    {
        /// <summary>
        /// 配置归属类型
        /// </summary>
        public EAbilityType ConfigType;
        
        /// <summary>
        /// 能力名字
        /// </summary>
        public string Name;
        
        /// <summary>
        /// 能力介绍
        /// </summary>
        public string Desc;
    }

    public class SkillBaseConfig : BattleAbilityBaseConfig
    {
        
    }

    public class BuffBaseConfig : BattleAbilityBaseConfig
    {
        
    }

    public class BulletBaseConfig : BattleAbilityBaseConfig
    {
        
    }
}