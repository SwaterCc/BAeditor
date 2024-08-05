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
    /// 标签系统是否要做统一
    /// </summary>
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
    /// 能力逻辑当前处于什么阶段
    /// </summary>
    public enum EBattleAbilityState
    {
        /// <summary>
        /// 未初始化
        /// </summary>
        UnInit,
        /// <summary>
        /// 准备就绪还没调用RUN
        /// </summary>
        Ready,
        /// <summary>
        /// 阶段开始
        /// </summary>
        StageStart,
        /// <summary>
        /// 阶段运行中
        /// </summary>
        StageRunning,
        /// <summary>
        /// 阶段结束
        /// </summary>
        StageEnd,
        /// <summary>
        /// 能力完成运行
        /// </summary>
        LogicEnd,
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

    /// <summary>
    /// 战斗逻辑树节点类型
    /// </summary>
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
        
        /// <summary>
        /// 遍历
        /// </summary>
        Foreach,
    }

    /// <summary>
    /// 打击点类型
    /// </summary>
    public enum EHitType
    {
        Aoe,
        LockTarget
    }

    /// <summary>
    /// Aoe打击点范围形状类型
    /// </summary>
    public enum EHitAreaType
    {
        /// <summary>
        /// 矩形
        /// </summary>
        Rect,

        /// <summary>
        /// 球
        /// </summary>
        Sphere,

        /// <summary>
        /// 圆柱
        /// </summary>
        Cylinder,

        /// <summary>
        /// 扇形
        /// </summary>
        Sector
    }

    /// <summary>
    /// 战斗位移的类型
    /// </summary>
    public enum EBattleSpecialMotionType
    {
    }

    /// <summary>
    /// 战斗事件节点的枚举类型
    /// </summary>
    public enum EBattleEventNodeType
    {
        /// <summary>
        /// 当前阶段开始
        /// </summary>
        StageBegin,

        /// <summary>
        /// 当前阶段结束
        /// </summary>
        StageEnd,

        /// <summary>
        /// 指定打击点命中
        /// </summary>
        Hit,
        
        /// <summary>
        /// 位移开始
        /// </summary>
        SpecialMotionBegin,
        
        /// <summary>
        /// 位移结束
        /// </summary>
        SpecialMotionEnd,
        
        /// <summary>
        /// 定时器
        /// </summary>
        Timer
    }

    /// <summary>
    /// 比较方式
    /// </summary>
    public enum ECompareType
    {
        
    }
}