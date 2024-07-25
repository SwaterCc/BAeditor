using System;
using System.Collections.Generic;
using BattleAbility.Editor.BattleAbilityCustomAttribute;
using Sirenix.OdinInspector;
using UnityEngine.Serialization;

namespace BattleAbility.Editor
{
    /// <summary>
    /// 能力的基础数据，抽象基类，具体的数据由子类实现
    ///
    /// </summary>
    public abstract class BattleAbilityBaseConfig
    {
        [BattleAbilityLabelTagEditor("ID(如果ID小于零视为没有初始化)", BattleAbilityLabelTagEditor.ELabeType.Int32)]
        public int ConfigId = -1;

        [BattleAbilityLabelTagEditor("能力名")] public string Name = "NoName";

        [BattleAbilityLabelTagEditor("能力描述")] public string Desc = "NoInit";

        [BattleAbilityLabelTagEditor("Icon路径")]
        public string IconPath = "";

        //预留标签
        public int Tag = -1;

        /// <summary>
        /// 配置归属类型
        /// </summary>
        public virtual EAbilityType GetAbilityType()
        {
            return EAbilityType.UnInit;
        }
    }

    public class SkillBaseConfig : BattleAbilityBaseConfig
    {
        public class SkillCostInfo
        {
            [BattleAbilityLabelTagEditor("资源消耗类型", BattleAbilityLabelTagEditor.ELabeType.Enum)]
            public EBattleResourceType CostType;

            [BattleAbilityLabelTagEditor("消耗资源的ID", BattleAbilityLabelTagEditor.ELabeType.Int32)]
            public int CostResourceId;

            [BattleAbilityLabelTagEditor("消耗资源的数量", BattleAbilityLabelTagEditor.ELabeType.Int32)]
            public int CostValue;
        }

        [BattleAbilityLabelTagEditor("技能类型", BattleAbilityLabelTagEditor.ELabeType.Enum)]
        public ESkillType SkillType;

        [BattleAbilityLabelTagEditor("技能CD", BattleAbilityLabelTagEditor.ELabeType.Long)]
        public long SkillCd;

        [BattleAbilityLabelTagEditor("技能消耗-(消耗资源,消耗数量)", BattleAbilityLabelTagEditor.ELabeType.List)]
        [BattleAbilityDrawerCollectionEditor("GetNewSkillCostInfo")]
        public List<SkillCostInfo> CostResourceTypeWithValue = new();

        public override EAbilityType GetAbilityType()
        {
            return EAbilityType.Skill;
        }

        public SkillCostInfo GetNewSkillCostInfo()
        {
            return new SkillCostInfo() { CostType = EBattleResourceType.Energy, CostResourceId = 0, CostValue = 0 };
        }
    }

    public class BuffBaseConfig : BattleAbilityBaseConfig
    {
        public long BuffLife;
        public EBuffAddRule AddRule;

        public override EAbilityType GetAbilityType()
        {
            return EAbilityType.Buff;
        }
    }

    /// <summary>
    /// 投射物，涵盖了子弹与虚拟体的结合体
    /// </summary>
    public class BulletBaseConfig : BattleAbilityBaseConfig
    {
        public new EAbilityType ConfigType => EAbilityType.Bullet;
        public EBulletType bulletType;

        public override EAbilityType GetAbilityType()
        {
            return EAbilityType.Bullet;
        }
    }
}