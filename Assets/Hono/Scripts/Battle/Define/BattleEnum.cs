using System;

namespace Hono.Scripts.Battle
{
    public enum EActorType
    {
        Pawn,
        Monster,
        Building,
        HitBox,
    }
    
    public enum EActorLogicType
    {
        Pawn,
        Monster,
        Building,
        HitBox,
    }

    public enum EActorShowType
    {
        LogicTest,
        Pawn,
        Monster,
        Building,
    }

    /// <summary>
    /// 能力配置的归属类型
    /// </summary>
    public enum EAbilityType
    {
        Skill,
        Buff,
        Bullet,
        GameMode,
        Other,
    }

    public enum EDamageElementType {
        Physical = 1,
        Magic,
    }
    
    public enum EDamageType
    {
        Normal = 1,
        Percent,
        Dot,
        Health,
    }

    /// <summary>
    /// 当前Ability执行到哪一步了
    /// </summary>
    public enum EAbilityState
    {
        Init,
        Ready,
        PreExecute,
        Executing,
        EndExecute,
        Error,
    }

    public enum ESelectPosType
    {
        Self,
        Target,
    }

    /// <summary>
    /// Ability可编辑周期
    /// </summary>
    public enum EAbilityAllowEditCycle
    {
        /// <summary>
        /// 能力初始化
        /// </summary>
        OnInit,

        /// <summary>
        /// 初始化完成，且未有执行指令时的等待周期，不可编辑（目前）
        /// </summary>
        OnReady,

        /// <summary>
        /// 能力执行前检测
        /// </summary>
        OnPreExecuteCheck,

        /// <summary>
        /// 预启动
        /// </summary>
        OnPreExecute,

        /// <summary>
        /// 运行
        /// </summary>
        OnExecuting,

        /// <summary>
        /// 结束
        /// </summary>
        OnEndExecute,
    }

    /// <summary>
    /// 能力节点类型
    /// </summary>
    public enum EAbilityNodeType
    {
        /// <summary>
        /// 生命周期节点
        /// </summary>
        EAbilityCycle,

        /// <summary>
        /// 注册事件触发
        /// </summary>
        EEvent,

        /// <summary>
        /// 分支控制节点
        /// </summary>
        EBranchControl,

        /// <summary>
        /// 变量控制
        /// </summary>
        EVariableControl,

        /// <summary>
        /// 遍历操作
        /// </summary>
        ERepeat,

        /// <summary>
        /// 任务，动作
        /// </summary>
        EAction,

        /// <summary>
        /// 等待节点
        /// </summary>
        ETimer,

        /// <summary>
        /// 阶段节点
        /// </summary>
        EGroup
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
    /// 技能目标类型
    /// </summary>
    public enum ESkillTargetType
    {
        Enemy,
        Self,
        Friendly,
        Team,
        Group,
        All
    }

    /// <summary>
    /// cd模式
    /// </summary>
    public enum ECDMode
    {
        BeforeExecute,
        AfterExecute
    }

    /// <summary>
    /// 资源消耗时机
    /// </summary>
    public enum EResCostType
    {
        BeforeExecute,
        AfterExecute
    }

    /// <summary>
    /// 战斗资源的类型
    /// </summary>
    public enum EBattleResourceType
    {
        Energy,
        Item,
        Buff,
    }

    /// <summary>
    /// buff重复添加规则
    /// </summary>
    public enum EBuffAddRule
    {
        //同源替换
        SameSourceReplace,

        //同源叠加
        SameSourceAdd,

        //全叠加
        Add,

        //非同源替换
        OnlyOne,
    }

    /// <summary>
    /// 投射物
    /// </summary>
    public enum EBulletType { }


    /// <summary>
    /// 技能编辑器中配置的临时变量生效范围
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
    /// 打击点类型
    /// </summary>
    public enum EHitType
    {
        Aoe,
        Single,
    }

    public enum ECheckBoxShapeType
    {
        /// <summary>
        /// 立方体
        /// </summary>
        Cube,

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
    /// 范围检测方式
    /// </summary>
    public enum ECheckBoxBehaveType
    {
        /// <summary>
        /// 预设触发器
        /// </summary>
        Trigger,
        /// <summary>
        /// 射线检测，适合一帧的瞬时检测
        /// </summary>
        RayCast,
    }

    /// <summary>
    /// 战斗位移的类型
    /// </summary>
    public enum EBattleSpecialMotionType { }

    /// <summary>
    /// 比较结果方式
    /// </summary>
    public enum ECompareResType
    {
        Less,
        LessAndEqual,
        Equal,
        More,
        MoreAndEqual
    }

    public enum EFilterRangeType
    {
        Range,
        Tag,
        ActorState,
        AbilityID,
        Faction
    }

    public enum EVariableOperationType
    {
        /// <summary>
        /// 创建变量
        /// </summary>
        Create,

        /// <summary>
        /// 修改变量
        /// </summary>
        Change,
    }

    /// <summary>
    /// 属性修改操作类别
    /// </summary>
    public enum EAttrCommandType
    {
        Add,
        Override,
    }

    /// <summary>
    /// 阵营关系
    /// </summary>
    public enum EFactionType
    {
        /// <summary>
        /// 中立
        /// </summary>
        Neutrality,
        /// <summary>
        /// 友善
        /// </summary>
        Friendly,
        /// <summary>
        /// 敌对
        /// </summary>
        Enemy, 
    }
    
    public enum ERepeatOperationType
    {
        //仅重复执行
        OnlyRepeat,

        //数值循环
        NumberLoop,
    }

    [Flags]
    public enum EFuncCacheFlag
    {
        OnlyCache = 0,
        Variable = 1 << 0,
        Action = 1 << 1,
        Branch = 1 << 2,
    }

    /// <summary>
    /// 状态机枚举
    /// </summary>
    public enum EActorState
    {
        Idle,
        Move,
        Battle,
        Stiff,
        Death
    }
}