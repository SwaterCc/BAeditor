using System.Collections.Generic;
using Battle;
using Battle.Def;
using BattleAbility.Editor.BattleAbilityCustomAttribute;

namespace BattleAbility.Editor
{
    /// <summary>
    /// 能力的基础数据，抽象基类，具体的数据由子类实现
    ///
    /// </summary>
    public abstract class BattleAbilityBaseConfig
    {
        [BAEditorShowLabelTag("ID(如果ID小于零视为没有初始化)", BAEditorShowLabelTag.ELabeType.Int32)]
        public int ConfigId = -1;

        [BAEditorShowLabelTag("能力名")] public string Name = "NoName";

        [BAEditorShowLabelTag("能力描述")] public string Desc = "NoInit";

        [BAEditorShowLabelTag("Icon路径")]
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
            [BAEditorShowLabelTag("资源消耗类型", BAEditorShowLabelTag.ELabeType.Enum)]
            public EBattleResourceType CostType;

            [BAEditorShowLabelTag("消耗资源的ID", BAEditorShowLabelTag.ELabeType.Int32)]
            public int CostResourceId;

            [BAEditorShowLabelTag("消耗资源的数量", BAEditorShowLabelTag.ELabeType.Int32)]
            public int CostValue;
        }

        [BAEditorShowLabelTag("技能类型", BAEditorShowLabelTag.ELabeType.Enum)]
        public ESkillType SkillType;

        [BAEditorShowLabelTag("技能CD", BAEditorShowLabelTag.ELabeType.Long)]
        public long SkillCd;

        [BAEditorShowLabelTag("技能消耗-(消耗资源,消耗数量)", BAEditorShowLabelTag.ELabeType.List)]
        [BAEditorCollectionItemInitTag("GetNewSkillCostInfo")]
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
        [BAEditorShowLabelTag("Buff生命时长(-1为无限)", BAEditorShowLabelTag.ELabeType.Long)]
        public long BuffLife;
        [BAEditorShowLabelTag("Buff添加规则", BAEditorShowLabelTag.ELabeType.Enum)]
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
        [BAEditorShowLabelTag("子弹类型", BAEditorShowLabelTag.ELabeType.Enum)]
        public EBulletType bulletType;

        public override EAbilityType GetAbilityType()
        {
            return EAbilityType.Bullet;
        }
    }
}