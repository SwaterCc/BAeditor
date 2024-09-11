using UnityEngine;

namespace Hono.Scripts.Battle
{
    public enum ELogicAttr
    {
        //Base
        AttrUid,
        AttrSourceActorUid,
        AttrSourceAbilityConfigId,
        SourceAbilityType,
        AttrRot,
        AttrPosition,
        AttrBaseSpeed,
        AttrAttackTargetUids,
        AttrMoveTargetUid,
        AttrFaction, 
        //HP
        
        AttrHp = 10000,
        AttrHpPer = 10001,
        AttrMaxHp = 10010,
        AttrMaxHpTotal = 10011,
        AttrMaxHpAdd = 10012,
        AttrMaxHpExAdd = 10013,
        AttrMaxHpPer = 10014,
        AttrMaxHpExPer = 10015,
        //MP
        AttrMp = 10020,
        AttrMpPer = 10021,
        AttrMaxMp = 10030,
        AttrMaxMpTotal = 10031,
        AttrMaxMpAdd = 10032,
        AttrMaxMpExAdd = 10033,
        AttrMaxMpPer = 10034,
        AttrMaxMpExPer = 10035,
        //攻击力
        AttrAttack = 12100,
        AttrAttackTotal = 12101,
        AttrAttackAdd = 12102,
        AttrAttackExAdd = 12103,
        AttrAttackPer = 12104,
        AttrAttackExPer = 12105,
        //防御力
        AttrDefense = 12110,
        AttrDefenseTotal = 12111,
        AttrDefenseAdd = 12112,
        AttrDefenseExAdd = 12113,
        AttrDefensePer = 12114,
        AttrDefenseExPer = 12115,
        //护甲穿透值
        AttrIgnoreDefense = 12120,
        AttrIgnoreDefenseTotal = 12121,
        AttrIgnoreDefenseAdd = 12122,
        AttrIgnoreDefenseExAdd = 12123,
        AttrIgnoreDefensePer = 12124,
        AttrIgnoreDefenseExPer = 12125,
        //护甲穿透万分比
        AttrIgnoreDefensePCT = 12130,
        AttrIgnoreDefensePCTTotal = 12131,
        AttrIgnoreDefensePCTAdd = 12132,
        AttrIgnoreDefensePCTExAdd = 12133,
        AttrIgnoreDefensePCTPer = 12134,
        AttrIgnoreDefensePCTExPer = 12135,
        //暴击率
        AttrCrit = 12140,
        AttrCritTotal = 12141,
        AttrCritAdd = 12142,
        AttrCritExAdd = 12143,
        AttrCritPer = 12144,
        AttrCritExPer = 12145,
        //暴击伤害
        AttrCritDamage = 12150,
        AttrCritDamageTotal = 12151,
        AttrCritDamageAdd = 12152,
        AttrCritDamageExAdd = 12153,
        AttrCritDamagePer = 12154,
        AttrCritDamageExPer = 12155,
        //治疗强度
        AttrHealIntensity = 12200,
        AttrHealIntensityTotal = 12201,
        AttrHealIntensityAdd = 12202,
        AttrHealIntensityExAdd = 12203,
        AttrHealIntensityPer = 12204,
        AttrHealIntensityExPer = 12205,
        //治疗效果
        AttrHeal = 13010,
        AttrHealTotal = 13011,
        AttrHealAdd = 13012,
        AttrHealExAdd = 13013,
        AttrHealPer = 13014,
        AttrHealExPer = 13015,
        //被治疗效果
        AttrHealed = 13020,
        AttrHealedTotal = 13021,
        AttrHealedAdd = 13022,
        AttrHealedExAdd = 13023,
        AttrHealedPer = 13024,
        AttrHealedExPer = 13025,
        //攻击速度
        AttrAttackSpeedPCT = 12500,
        AttrAttackSpeedPCTTotal = 12501,
        AttrAttackSpeedPCTAdd = 12502,
        AttrAttackSpeedPCTExAdd = 12503,
        AttrAttackSpeedPCTPer = 12504,
        AttrAttackSpeedPCTExPer = 12505,
        //全属性抗性穿透
        AttrElementPenPCT = 14000,
        AttrElementPenPCTTotal = 14001,
        AttrElementPenPCTAdd = 14002,
        AttrElementPenPCTExAdd = 14003,
        AttrElementPenPCTPer = 14004,
        AttrElementPenPCTExPer = 14005,
        //全属性抗性
        AttrElementRedPCT = 14010,
        AttrElementRedPCTTotal = 14011,
        AttrElementRedPCTAdd = 14012,
        AttrElementRedPCTExAdd = 14013,
        AttrElementRedPCTPer = 14014,
        AttrElementRedPCTExPer = 14015,
        //物理属性抗性穿透
        AttrElementPhysicalPenPCT = 14020,
        AttrElementPhysicalPenPCTTotal = 14021,
        AttrElementPhysicalPenPCTAdd = 14022,
        AttrElementPhysicalPenPCTExAdd = 14023,
        AttrElementPhysicalPenPCTPer = 14024,
        AttrElementPhysicalPenPCTExPer = 14025,
        //物理属性抗性
        AttrElementPhysicalRedPCT = 14030,
        AttrElementPhysicalRedPCTTotal = 14031,
        AttrElementPhysicalRedPCTAdd = 14032,
        AttrElementPhysicalRedPCTExAdd = 14033,
        AttrElementPhysicalRedPCTPer = 14034,
        AttrElementPhysicalRedPCTExPer = 14035,
        //元素属性抗性穿透
        AttrElementMagicPenPCT = 14040,
        AttrElementMagicPenPCTTotal = 14041,
        AttrElementMagicPenPCTAdd = 14042,
        AttrElementMagicPenPCTExAdd = 14043,
        AttrElementMagicPenPCTPer = 14044,
        AttrElementMagicPenPCTExPer = 14045,
        //元素属性抗性
        AttrElementMagicRedPCT = 14050,
        AttrElementMagicRedPCTTotal = 14051,
        AttrElementMagicRedPCTAdd = 14052,
        AttrElementMagicRedPCTExAdd = 14053,
        AttrElementMagicRedPCTPer = 14054,
        AttrElementMagicRedPCTExPer = 14055,
        //直接减伤
        AttrDmgRed = 15300,
        AttrDmgRedTotal = 15301,
        AttrDmgRedAdd = 15302,
        AttrDmgRedExAdd = 15303,
        AttrDmgRedPer = 15304,
        AttrDmgRedExPer = 15305,
        //近距离减伤
        AttrDmgRedNear = 16020,
        AttrDmgRedNearTotal = 16021,
        AttrDmgRedNearAdd = 16022,
        AttrDmgRedNearExAdd = 16023,
        AttrDmgRedNearPer = 16024,
        AttrDmgRedNearExPer = 16025,
        //远距离减伤
        AttrDmgRedFar = 16120,
        AttrDmgRedFarTotal = 16121,
        AttrDmgRedFarAdd = 16122,
        AttrDmgRedFarExAdd = 16123,
        AttrDmgRedFarPer = 16124,
        AttrDmgRedFarExPer = 16125,
        //子弹类型减伤
        AttrDmgRedBullet = 16220,
        AttrDmgRedBulletTotal = 16221,
        AttrDmgRedBulletAdd = 16222,
        AttrDmgRedBulletExAdd = 16223,
        AttrDmgRedBulletPer = 16224,
        AttrDmgRedBulletExPer = 16225,
        //近战类型减伤
        AttrDmgRedMelee = 16320,
        AttrDmgRedMeleeTotal = 16321,
        AttrDmgRedMeleeAdd = 16322,
        AttrDmgRedMeleeExAdd = 16323,
        AttrDmgRedMeleePer = 16324,
        AttrDmgRedMeleeExPer = 16325,
        //BUFF类型减伤
        AttrDmgRedBuff = 16420,
        AttrDmgRedBuffTotal = 16421,
        AttrDmgRedBuffAdd = 16422,
        AttrDmgRedBuffExAdd = 16423,
        AttrDmgRedBuffPer = 16424,
        AttrDmgRedBuffExPer = 16425,
        //普通敌人强度减伤
        AttrDmgRedVsNormalEnemy = 16720,
        AttrDmgRedVsNormalEnemyTotal = 16721,
        AttrDmgRedVsNormalEnemyAdd = 16722,
        AttrDmgRedVsNormalEnemyExAdd = 16723,
        AttrDmgRedVsNormalEnemyPer = 16724,
        AttrDmgRedVsNormalEnemyExPer = 16725,
        //精英敌人强度减伤
        AttrDmgRedVsEliteEnemy = 16820,
        AttrDmgRedVsEliteEnemyTotal = 16821,
        AttrDmgRedVsEliteEnemyAdd = 16822,
        AttrDmgRedVsEliteEnemyExAdd = 16823,
        AttrDmgRedVsEliteEnemyPer = 16824,
        AttrDmgRedVsEliteEnemyExPer = 16825,
        //健康时减伤（当前生命≥70%）
        AttrDmgRedHealthy = 17020,
        AttrDmgRedHealthyTotal = 17021,
        AttrDmgRedHealthyAdd = 17022,
        AttrDmgRedHealthyExAdd = 17023,
        AttrDmgRedHealthyPer = 17024,
        AttrDmgRedHealthyExPer = 17025,
        //不健康时减伤（当前生命＜30%）
        AttrDmgRedNonHealthy = 17120,
        AttrDmgRedNonHealthyTotal = 17121,
        AttrDmgRedNonHealthyAdd = 17122,
        AttrDmgRedNonHealthyExAdd = 17123,
        AttrDmgRedNonHealthyPer = 17124,
        AttrDmgRedNonHealthyExPer = 17125,
        AttrEntityLevel =60010,
    }
}