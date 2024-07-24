namespace BattleAbility
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
    /// 技能类型
    /// </summary>
    public enum ESkillType
    {
        Normal,
        Singing,
    }

    /// <summary>
    /// 战斗资源的类型
    /// </summary>
    public enum EBattleResourceType
    {
        
    }

    /// <summary>
    /// buff重复添加规则
    /// </summary>
    public enum EBuffAddRule
    {
        //同源替换
        //同源叠加
        //全叠加
        //全替换
    }

    /// <summary>
    /// 投射物
    /// </summary>
    public enum EBulletType
    {
        
    }
}