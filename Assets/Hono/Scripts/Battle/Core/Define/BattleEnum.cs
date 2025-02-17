#region

using System;
using XLua;

#endregion

namespace Hono.Scripts.Battle
{
    public enum EActorType
    {
        NoInit = 0,
        Character = 1,
        Npc,
        Building,
    }

    /// <summary>
    /// 场景代理对象类型
    /// </summary>
    public enum EUOProxyType
    {
        ActorProxy = 1,
        TransformProxy = 11,
        CubeColliderProxy = 12,
        SphereColliderProxy = 13,
        CapsuleColliderProxy = 14,
        CubeTriggerProxy = 101,
        CylinderTriggerProxy = 102,
    }

    /// <summary>
    /// 代理对象的层级
    /// </summary>
    public enum EUOProxyLayerType
    {
        ActorLayer = 1,
        SceneObject = 2,
    }

    /// <summary>
    /// 战斗模式
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
    /// 技能修改逻辑
    /// </summary>
    public enum ESkillModifyType
    {
        SkillLevel = 1,
        SkillAttr = 2,
        SkillTag = 3,
        SkillEnergyCostAdd = 4,
        SkillEnergyCostPCT = 5,
    }

    /// <summary>
    /// 当前技能状态
    /// </summary>
    [Flags]
    public enum ESkillFlag
    {
        Empty = 0,
        NoLearn = 1,
        Executing = 2,
        Disable = 4,
        EnergyNotEnough = 8,
        CD = 16,
        Occupied = 32,
    }

    /// <summary>
    /// Actor初始化状态
    /// </summary>
    [Flags]
    public enum EUnitFlag
    {
        Empty = 0,
        Move = 1,
        Attack = 2,
        Stiff = 4,
    }

    /// <summary>
    /// 世界状态
    /// </summary>
    public enum EWorldState
    {
        /// <summary>
        /// 
        /// </summary>
        Empty = 0,

        /// <summary>
        /// 数据加载
        /// </summary>
        Loading = 1,

        /// <summary>
        /// 战略定制状态（第一次大地图展开）
        /// </summary>
        Ready,

        /// <summary>
        /// 俯视角游玩状态
        /// </summary>
        Gaming,

        /// <summary>
        /// 战略地图布置状态
        /// </summary>
        StrategicMap,

        /// <summary>
        /// 结算
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
        ListenerInfo,
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

    /// <summary>
    /// ability 指令类型
    /// </summary>
    public enum EAbilityCommandType
    {
        //永久修改
        Permanent,
        /// <summary>
        /// 当ability执行的到end阶段时回退(当Ability删除时该类型的指令也会被删除)
        /// </summary>
        UndoWhenAbilityEndCycle,
        /// <summary>
        /// 当ability删除时回退
        /// </summary>
        UndoWhenAbilityRemove,
    }

    /// <summary>
    /// 对变量进行操作
    /// </summary>
    public enum EVariableOperationType
    {
        Reset,
        Add,
        Sub,
        Reverse,
    }

    /// <summary>
    /// 循环节点操作
    /// </summary>
    public enum ERepeatNodeOperationType
    {
        Repeat,
        ETraverseList,
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
        ///    动作
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
        /// 目标
        /// </summary>
        Targeted,
        /// <summary>
        /// 坐标
        /// </summary>
        Position,
        /// <summary>
        /// 面向方向
        /// </summary>
        Front,
        /// <summary>
        /// 自定义方向
        /// </summary>
        Direction,
        /// <summary>
        /// 自定义
        /// </summary>
        Custom,
    }

    /// <summary>
    ///     位移类型
    /// </summary>
    public enum EBulletMotionType
    {
        //直线
        Liner,

        //环绕
        Around,
    }

    /// <summary>
    /// cd模式
    /// </summary>
    public enum EEnterCDType
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
    /// 伤害来源标记
    /// </summary>
    [LuaCallCSharp]
    public enum EDamageSourceType
    {
        /// <summary>
        /// 无来源，说明是直接调用了伤害接口
        /// </summary>
        None = 1,
        Skill = 2,
        Buff = 3,
    }

    public enum ECombatEnergyField
    {
        CurrentValue,

        MaxValueAdd,
        MaxValuePCT,

        EnergyGetWhenKillEnemyAdd,
        EnergyGetWhenKillEnemyPCT,

        EnergyGetWhenSkillHitAdd,
        EnergyGetWhenSkillHitPCT,

        EnergyGetWhenBeHitAdd,
        EnergyGetWhenBeHitPCT,

        EnergyGetWhenIdleAdd,
        EnergyGetWhenIdlePCT,
    }

    /// <summary>
    /// 添加阻断规则
    /// 当该buff存在的时候会阻止指定的buff再次添加
    /// </summary>
    public enum EBuffAddBlockRule
    {
        None,
        BlockByTags,
        BlockById,
    }

    /// <summary>
    /// buff重复添加规则（替换or叠层）
    /// </summary>
    public enum EBuffAddRule
    {
        /// <summary>
        /// 同源同IdBuff覆盖
        /// </summary>
        SameSourceOverride,

        /// <summary>
        /// 同源同IdBuff叠加
        /// </summary>
        SameSourceLayering,

        /// <summary>
        /// 同Id全叠加
        /// </summary>
        AllLayering,

        /// <summary>
        /// 同Id全覆盖
        /// </summary>
        AllOverride,
    }

    /// <summary>
    /// 添加成功后的行为
    /// 移除指定tag的buff
    /// 移除指定id的buff
    /// </summary>
    public enum EBuffAddSuccessBehave
    {
        RemoveBuffsByTag,
        RemoveBuffsById
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
        Faction, //相对阵营
        ActorType,
    }

    public enum EFilterFunctionType
    {
        None,
        Random = 1,
        HighestHp = 10,
        LeastHp,
        Far = 30,
        Near,
        ControlPlayer = 40,
    }

    /// <summary>
    ///     阵营关系
    /// </summary>
    public enum EFactionRelationship
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
}