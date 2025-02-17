using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

namespace Hono.Scripts.Battle
{
    public class SkillData : ASerializableData,IIncludeAbility
    {
        /// <summary>
        /// 技能icon路径
        /// </summary>
        public string skillIcon;
        /// <summary>
        /// 技能的持续时长 -1为跟随ability结束
        /// </summary>
        public float skillDuration;
        /// <summary>
        /// 技能类型
        /// </summary>
        public ESkillType skillType;
        /// <summary>
        /// 进入cd时机
        /// </summary>
        public EEnterCDType enterCdType;
        /// <summary>
        /// 技能Cd
        /// </summary>
        public float skillCd;
        /// <summary>
        /// 释放时资源检测列表
        /// </summary>
        public ResItems skillResCheck = new();
        /// <summary>
        /// 释放时资源消耗列表
        /// </summary>
        public ResItems skillResCost = new();
        /// <summary>
        /// 是否转向技能方向
        /// </summary>
        public bool rotToTarget;
        /// <summary>
        /// 技能的Tag
        /// </summary>
        public List<int> tags = new();
        /// <summary>
        /// 独占技能只能有一个在运行
        /// </summary>
        public bool isExclusive;
        /// <summary>
        /// 禁用移动（仅输入移动，Action移动不受影响）
        /// </summary>
        public bool disableMoveInput;
        /// <summary>
        /// 技能Ability数据
        /// </summary>
        [SerializeField]
        private AbilityData skillAbility;
        public AbilityData SkillAbility => skillAbility;
        ///////////////////////////指示器相关///////////////////////////
        /// <summary>
        /// 技能可释放区域范围（施法范围）
        /// </summary>
        public float castRange;
        /// <summary>
        /// 技能目标选择范围
        /// </summary>
        public ESkillTargetSelectType skillTargetType;
        /// <summary>
        /// 目标选择条件（指示器）
        /// </summary>
        public ConditionFilterSetting targetFilter = new();
        /// <summary>
        /// 非指向技能选择范围配置（指示器）
        /// </summary>
        public RangeFilterSetting rangeFilter = new();
        
        public void AddAbilityData()
        {
            if (skillAbility == null)
            {
                skillAbility = CreateInstance<AbilityData>();
                skillAbility.name = "skillAbility"; // 设置子资产名称
                skillAbility.fileName = fileName;
                skillAbility.id = id;
                skillAbility.path = path;
                skillAbility.abilityBelongType = EAbilityBelongType.Skill;
#if UNITY_EDITOR
                UnityEditor.AssetDatabase.AddObjectToAsset(skillAbility, this);
                UnityEditor.EditorUtility.SetDirty(this);
                UnityEditor.AssetDatabase.SaveAssets();
#endif
            }
        }
    }
}