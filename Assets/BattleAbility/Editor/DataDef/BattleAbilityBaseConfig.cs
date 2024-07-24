using System;
using System.Collections.Generic;
using UnityEngine.Serialization;

namespace BattleAbility.Editor
{
    /// <summary>
    /// 能力的基础数据，抽象基类，具体的数据由子类实现
    /// </summary>
    [Serializable]
    public abstract class BattleAbilityBaseConfig
    {
        /// <summary>
        /// 配置归属类型
        /// </summary>
        public EAbilityType ConfigType => EAbilityType.UnInit;
        
        /// <summary>
        /// 配置id
        /// </summary>
        public int ConfigId = -1;

        /// <summary>
        /// 能力名字
        /// </summary>
        public string Name = "NoName";

        /// <summary>
        /// 能力介绍
        /// </summary>
        public string Desc = "NoInit";

        //预留标签
        public int Tag = -1;
    }

    [Serializable]
    public class SkillBaseConfig : BattleAbilityBaseConfig
    {
        public new EAbilityType ConfigType => EAbilityType.Skill;
        public ESkillType SkillType;
        public long SkillCd;
        public Dictionary<EBattleResourceType,int> CostResourceTypeWithValue;
        public string AnimPath;
    }

    [Serializable]
    public class BuffBaseConfig : BattleAbilityBaseConfig
    {
        public new EAbilityType ConfigType => EAbilityType.Buff;
        public long BuffLife;
        public EBuffAddRule AddRule;
    }

    /// <summary>
    /// 投射物，涵盖了子弹与虚拟体的结合体
    /// </summary>
    [Serializable]
    public class BulletBaseConfig : BattleAbilityBaseConfig
    {
        public new EAbilityType ConfigType => EAbilityType.Bullet;
        public EBulletType bulletType;
    }
}