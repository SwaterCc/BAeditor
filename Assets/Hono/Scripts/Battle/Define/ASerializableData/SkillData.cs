using System.Collections.Generic;
using UnityEngine.Serialization;

namespace Hono.Scripts.Battle
{
    public class SkillData : ASerializableData
    {
        /// <summary>
        /// 技能icon路径
        /// </summary>
        public string skillIcon;
        /// <summary>
        /// 技能类型
        /// </summary>
        public ESkillType skillType;
        /// <summary>
        /// 进入cd时机
        /// </summary>
        public EEnterCdType enterCdType;
        /// <summary>
        /// 技能Cd
        /// </summary>
        public float skillCd;
        /// <summary>
        /// 资源消耗时机
        /// </summary>
        public EResCostTimingType costTimingType;
        /// <summary>
        /// 释放时资源检测列表
        /// </summary>
        public List<ResItems> skillResCheck = new();
        /// <summary>
        /// 释放时资源消耗列表
        /// </summary>
        public List<ResItems> skillResCost = new();
        /// <summary>
        /// 转向技能释放方向的速度
        /// </summary>
        public float speedOfRotateToTarget;
        /// <summary>
        /// 技能目标选择范围
        /// </summary>
        public ESkillTargetSelectType skillTargetType;
        /// <summary>
        /// 一级过滤，对当前技能选出的目标进行筛选，指向性技能会在选择时生效，aoe技能则会影响存入的技能目标列表
        /// </summary>
        public ConditionFilterSetting firstFilter = new();
        /// <summary>
        /// 命中目标数量上限
        /// </summary>
        public int hitCeiling;
        /// <summary>
        /// 技能可释放区域范围
        /// </summary>
        public float skillRange;
        /// <summary>
        /// 非指向技能选择范围配置
        /// </summary>
        public CheckBoxData selectRangeShape = new();
        /// <summary>
        /// 技能的持续时长 -1为跟随ability结束
        /// </summary>
        public float skillDuration;
        /// <summary>
        /// 关联的Ability
        /// </summary>
        public int abilityId;
    }
}