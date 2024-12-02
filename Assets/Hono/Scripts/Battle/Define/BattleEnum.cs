#region

using System;

#endregion

namespace Hono.Scripts.Battle
{
    public enum EActorType
    {
        BattleLevelController,
        Pawn,
        Monster,
        Building,
        Bullet,
        HitBox,
        Loot,
        MonsterGenerator,
        TriggerBox,
        //TeamDefaultBirthPoint,
        //TeamRefreshPoint,
    }

    /// <summary>
    ///     战斗模式
    /// </summary>
    public enum EBattleModeType
    {
        Normal = 1,
        War = 2,
        Rogue = 3,
    }

    /// <summary>
    ///     回合结算目标条件
    /// </summary>
    public enum ERoundTargetType
    {
        FactionId,
        SpecialUid,
        Tag,
    }

    /// <summary>
    ///     回合结算条件
    /// </summary>
    public enum ERoundConditionType
    {
        //生存
        Survival,

        //死亡
        Death,
    }

    /// <summary>
    ///     能力配置的归属类型
    /// </summary>
    public enum EAbilityType
    {
        Skill,
        Buff,
        Bullet,
        GameMode,
        Other,
    }

    /// <summary>
    ///     战斗玩法状态
    /// </summary>
    public enum EBattleStateType
    {
        /// <summary>
        ///     未在游玩,
        /// </summary>
        NoGaming,

        /// <summary>
        ///     编队
        /// </summary>
        BuildTeams,

        /// <summary>
        ///     加载战场
        /// </summary>
        LoadBattleGround,

        /// <summary>
        ///     游玩
        /// </summary>
        Playing,

        /// <summary>
        ///     结算
        /// </summary>
        Score,
    }

    /// <summary>
    ///     战斗中的波次状态
    /// </summary>
    public enum ERoundState
    {
        /// <summary>
        ///     未执行
        /// </summary>
        NoRunning,

        /// <summary>
        ///     准备期
        /// </summary>
        Ready,

        /// <summary>
        ///     运行期
        /// </summary>
        Running,

        /// <summary>
        ///     成功结算期(清理期,只有通过该回合才会进入，回合失败会直接重开或者结算战斗)
        /// </summary>
        SuccessScoring,

        /// <summary>
        ///     失败结算期
        /// </summary>
        FailedScoring,
    }

    public enum EParamType
    {
        Simple,
        Function,
        Variable,
        Attr,
    }

    /// <summary>
    /// Tag查询范围
    /// </summary>
    public enum ETagSearchRange
    {
        Actor,
        Ability,
        All,
    }

    public enum EBuildingType
    {
        EDefenseTower = 1,
        EBarrack = 2,
    }

    /// <summary>
    /// 战利品掉落规则
    /// </summary>
    public enum ELootDropRule
    {
        NoDrop,
        KillCount,
        KillSpecialTag,
    }

    /// <summary>
    /// 傷害元素類型
    /// </summary>
    public enum EDamageElementType
    {
        Physical = 1,
        Magic,
    }

    /// <summary>
    /// 伤害类型
    /// </summary>
    public enum EDamageType
    {
        Normal = 1,
        Percent,
        Dot,
        Health,
    }

    public enum EPawnTeamMemberStateType
    {
        Empty,
        Normal,
        Dead,
    }
    
    public enum ESelectPosType
    {
        Self,
        Target,
    }

    /// <summary>
    ///     Ability可编辑周期
    /// </summary>
    public enum EAbilityCycle
    {
        /// <summary>
        /// 未激活，通常是在池子里
        /// </summary>
        NoActive = 0,

        /// <summary>
        /// 能力初始化
        /// </summary>
        Init = 1,

        /// <summary>
        /// 预启动
        /// </summary>
        PreExecute = 2,

        /// <summary>
        /// 执行中
        /// </summary>
        Executing = 3,

        /// <summary>
        /// 结束
        /// </summary>
        EndExecute = 4,
    }

    /// <summary>
    ///     怪物生成器的行为
    /// </summary>
    public enum EMonsterGenBehave
    {
        Summon,
        Pause,
        Resume,
        Clear,
    }

    public enum EGenMonsterActionType
    {
        Stay,
        AttackPlayer
    }

    /// <summary>
    ///     技能类型
    /// </summary>
    public enum ESkillType
    {
        /// <summary>
        /// 普通攻击
        /// </summary>
        NormalAttack,
        /// <summary>
        /// 武器技能
        /// </summary>
        WeaponSkill,
        /// <summary>
        /// 终极技能
        /// </summary>
        UltimateSkill,
        /// <summary>
        /// 被动技能
        /// </summary>
        PassiveSkill,
        /// <summary>
        /// 效果技能（定义暂时不明，没有和被动技能做出区别）
        /// </summary>
        EffectSkill,
    }

    /// <summary>
    ///     战利品功能
    /// </summary>
    public enum ELootFunctionType
    {
        SkillLevelUp = 1,
        SkillLearn = 2,
        AttrChange = 3,
        BuffAdd = 4,
    }

    /// <summary>
    ///     能力节点类型
    /// </summary>
    public enum EAbilityNodeType
    {
        /// <summary>
        ///     生命周期节点
        /// </summary>
        EAbilityCycle,

        /// <summary>
        ///     注册事件触发
        /// </summary>
        EEvent,

        /// <summary>
        ///     分支控制节点
        /// </summary>
        EBranchControl,

        /// <summary>
        ///     变量设置
        /// </summary>
        EVariableSetter,

        /// <summary>
        ///     属性设置节点
        /// </summary>
        EAttrSetter,

        /// <summary>
        ///     遍历操作
        /// </summary>
        ERepeat,

        /// <summary>
        ///     任务，动作
        /// </summary>
        EAction,

        /// <summary>
        ///     等待节点
        /// </summary>
        ETimer,

        /// <summary>
        ///     阶段节点
        /// </summary>
        EGroup,
    }

    /// <summary>
    /// 技能目标选择方式
    /// </summary>
    public enum ESkillTargetSelectType
    {
        /// <summary>
        /// 指向技能
        /// </summary>
        TargetedSkill,
        /// <summary>
        /// 非指向技能
        /// </summary>
        NoTargetedSkill,
    }

    /// <summary>
    ///     位移类型
    /// </summary>
    public enum EMotionType
    {
        //直线
        Liner,

        //环绕
        Around,

        //抛物线
        Parabola,
    }

    /// <summary>
    /// cd模式
    /// </summary>
    public enum EEnterCdType
    {
        BeforeExecute,
        AfterExecute
    }

    /// <summary>
    ///     资源消耗时机
    /// </summary>
    public enum EResCostTimingType
    {
        BeforeExecute,
        AfterExecute
    }

    /// <summary>
    ///     战斗资源的类型
    /// </summary>
    public enum EBattleResourceType
    {
        Energy,
        Item,
        Buff,
    }

    /// <summary>
    ///     buff重复添加规则（替换or叠层）
    /// </summary>
    public enum EBuffReplaceRule
    {
        //同源替换，非同源叠加
        SameSourceReplace,

        //同源叠加，非同源替换
        SameSourceAdd,

        //全叠加，仅保留第一个buff
        Add,

        //仅保留最后一个buff无叠加
        OnlyOne,
    }

    /// <summary>
    ///     添加规则
    /// </summary>
    public enum EApplicationRequirement
    {
        HasTags,
        NoTags,
    }

    /// <summary>
    ///     技能编辑器中配置的临时变量生效范围
    /// </summary>
    public enum EVariableRange
    {
        //战场全局变量
        Battleground,

        //仅角色
        Actor,

        //单个能力
        Ability,
    }

    /// <summary>
    ///     打击点类型
    /// </summary>
    public enum EHitType
    {
        Aoe,
        Single,
    }

    public enum ECheckBoxShapeType
    {
        /// <summary>
        ///     立方体
        /// </summary>
        Cube,

        /// <summary>
        ///     球
        /// </summary>
        Sphere,
    }

    /// <summary>
    ///     范围检测方式
    /// </summary>
    public enum ECheckBoxBehaveType
    {
        /// <summary>
        ///     预设触发器
        /// </summary>
        Trigger,

        /// <summary>
        ///     射线检测，适合一帧的瞬时检测
        /// </summary>
        RayCast,
    }

    /// <summary>
    ///     比较结果方式
    /// </summary>
    public enum ECompareResType
    {
        Less,
        LessAndEqual,
        Equal,
        More,
        MoreAndEqual
    }

    public enum EFilterConditionType
    {
        Tag,
        ActorState,
        AbilityID,
        Faction,
        ActorType,
    }

    public enum EFilterFunctionType
    {
        None,
        Random = 1,
        HighestHp = 10,
        LeastHp,
        HighestMp = 20,
        LeastMp,
        Far = 30,
        Near,
        ControlPlayer = 40,
    }
    
    /// <summary>
    ///     阵营关系
    /// </summary>
    public enum EFactionType
    {
        /// <summary>
        ///     中立
        /// </summary>
        Neutrality,

        /// <summary>
        ///     友善
        /// </summary>
        Friendly,

        /// <summary>
        ///     敌对
        /// </summary>
        Enemy,
    }

    /// <summary>
    ///     状态机枚举
    /// </summary>
    public enum EActorStateType
    {
        Empty,
        Idle,
        Move,
        Attack,
        Stiff,
        Death
    }

    /// <summary>
    /// Actor运行标签
    /// </summary>
    public enum EActorFlag
    {
        UnInit,
        Ready,
        Active,
    }

    public enum ECalculateType
    {
        Add,
        Subtract,
        Multiply,
        Divide,
    }

    public enum EPathType
    {
        CSV,
        Ability,
        Skill,
        Buff,
        Bullet,
    }

    public enum EParamValueType
    {
        Any,
        Int,
        Float,
        Bool,
        String,
        IntList,
        Enum,
        Vector3,
        Object,
        Custom,
    }
}