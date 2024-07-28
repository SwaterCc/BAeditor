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

    public enum EStageTag
    {
        
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
        Energy,
        Buff,
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

    /// <summary>
    /// 技能编辑器中配置的临时变量生效范围
    /// </summary>
    public enum ELocalValueRange
    {
        //战场全局变量
        Battleground,
        //仅角色
        Char,
        //单个能力
        Ability,
        //单个阶段
        Stage,
    }
    
    public enum ENodeType
    {
        /// <summary>
        /// 事件节点
        /// </summary>
        Event,

        /// <summary>
        /// 逻辑判定节点
        /// </summary>
        Condition,

        /// <summary>
        /// 具体动作
        /// </summary>
        Action,

        /// <summary>
        /// 创建变量
        /// </summary>
        Variable,
    }
}