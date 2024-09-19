using System;
using System.Collections.Generic;
using UnityEngine;

namespace Hono.Scripts.Battle
{
//Auto::AttrMaker
    public enum ELogicAttr
    {
              AttrUid = 1,
      AttrSourceActorUid = 11,
      AttrSourceAbilityConfigId = 12,
      SourceAbilityType = 13,
      AttrRot = 14,
      AttrPosition = 15,
      AttrBaseSpeed = 16,
      AttrAttackTargetUids = 17,
      AttrMoveTargetUid = 18,
      AttrFaction = 19,
      AttrUnselectable = 20,
      AttrInvincible = 21,
      AttrHp = 10000,
      AttrHpPer = 10001,
      AttrMaxHp = 10010,
      AttrMaxHpTotal = 10011,
      AttrMaxHpAdd = 10012,
      AttrMaxHpExAdd = 10013,
      AttrMaxHpPer = 10014,
      AttrMaxHpExPer = 10015,
      AttrMp = 10020,
      AttrMpPer = 10021,
      AttrMaxMp = 10030,
      AttrMaxMpTotal = 10031,
      AttrMaxMpAdd = 10032,
      AttrMaxMpExAdd = 10033,
      AttrMaxMpPer = 10034,
      AttrMaxMpExPer = 10035,
      AttrShield = 10040,
      AttrShieldPer = 10041,
      AttrAttack = 12100,
      AttrAttackTotal = 12101,
      AttrAttackAdd = 12102,
      AttrAttackExAdd = 12103,
      AttrAttackPer = 12104,
      AttrAttackExPer = 12105,
      AttrDefense = 12110,
      AttrDefenseTotal = 12111,
      AttrDefenseAdd = 12112,
      AttrDefenseExAdd = 12113,
      AttrDefensePer = 12114,
      AttrDefenseExPer = 12115,
      AttrIgnoreDefense = 12120,
      AttrIgnoreDefenseTotal = 12121,
      AttrIgnoreDefenseAdd = 12122,
      AttrIgnoreDefenseExAdd = 12123,
      AttrIgnoreDefensePer = 12124,
      AttrIgnoreDefenseExPer = 12125,
      AttrIgnoreDefensePCT = 12130,
      AttrIgnoreDefensePCTTotal = 12131,
      AttrIgnoreDefensePCTAdd = 12132,
      AttrIgnoreDefensePCTExAdd = 12133,
      AttrIgnoreDefensePCTPer = 12134,
      AttrIgnoreDefensePCTExPer = 12135,
      AttrCrit = 12140,
      AttrCritTotal = 12141,
      AttrCritAdd = 12142,
      AttrCritExAdd = 12143,
      AttrCritPer = 12144,
      AttrCritExPer = 12145,
      AttrCritDamage = 12150,
      AttrCritDamageTotal = 12151,
      AttrCritDamageAdd = 12152,
      AttrCritDamageExAdd = 12153,
      AttrCritDamagePer = 12154,
      AttrCritDamageExPer = 12155,
      AttrHealIntensity = 12200,
      AttrHealIntensityTotal = 12201,
      AttrHealIntensityAdd = 12202,
      AttrHealIntensityExAdd = 12203,
      AttrHealIntensityPer = 12204,
      AttrHealIntensityExPer = 12205,
      AttrHeal = 13010,
      AttrHealTotal = 13011,
      AttrHealAdd = 13012,
      AttrHealExAdd = 13013,
      AttrHealPer = 13014,
      AttrHealExPer = 13015,
      AttrHealed = 13020,
      AttrHealedTotal = 13021,
      AttrHealedAdd = 13022,
      AttrHealedExAdd = 13023,
      AttrHealedPer = 13024,
      AttrHealedExPer = 13025,
      AttrAttackSpeedPCT = 12500,
      AttrAttackSpeedPCTTotal = 12501,
      AttrAttackSpeedPCTAdd = 12502,
      AttrAttackSpeedPCTExAdd = 12503,
      AttrAttackSpeedPCTPer = 12504,
      AttrAttackSpeedPCTExPer = 12505,
      AttrElementPenPCT = 14000,
      AttrElementPenPCTTotal = 14001,
      AttrElementPenPCTAdd = 14002,
      AttrElementPenPCTExAdd = 14003,
      AttrElementPenPCTPer = 14004,
      AttrElementPenPCTExPer = 14005,
      AttrElementRedPCT = 14010,
      AttrElementRedPCTTotal = 14011,
      AttrElementRedPCTAdd = 14012,
      AttrElementRedPCTExAdd = 14013,
      AttrElementRedPCTPer = 14014,
      AttrElementRedPCTExPer = 14015,
      AttrElementPhysicalPenPCT = 14020,
      AttrElementPhysicalPenPCTTotal = 14021,
      AttrElementPhysicalPenPCTAdd = 14022,
      AttrElementPhysicalPenPCTExAdd = 14023,
      AttrElementPhysicalPenPCTPer = 14024,
      AttrElementPhysicalPenPCTExPer = 14025,
      AttrElementPhysicalRedPCT = 14030,
      AttrElementPhysicalRedPCTTotal = 14031,
      AttrElementPhysicalRedPCTAdd = 14032,
      AttrElementPhysicalRedPCTExAdd = 14033,
      AttrElementPhysicalRedPCTPer = 14034,
      AttrElementPhysicalRedPCTExPer = 14035,
      AttrElementMagicPenPCT = 14040,
      AttrElementMagicPenPCTTotal = 14041,
      AttrElementMagicPenPCTAdd = 14042,
      AttrElementMagicPenPCTExAdd = 14043,
      AttrElementMagicPenPCTPer = 14044,
      AttrElementMagicPenPCTExPer = 14045,
      AttrElementMagicRedPCT = 14050,
      AttrElementMagicRedPCTTotal = 14051,
      AttrElementMagicRedPCTAdd = 14052,
      AttrElementMagicRedPCTExAdd = 14053,
      AttrElementMagicRedPCTPer = 14054,
      AttrElementMagicRedPCTExPer = 14055,
      AttrDmgA = 15000,
      AttrDmgATotal = 15001,
      AttrDmgAAdd = 15002,
      AttrDmgAExAdd = 15003,
      AttrDmgAPer = 15004,
      AttrDmgAExPer = 15005,
      AttrDmgRed = 15300,
      AttrDmgRedTotal = 15301,
      AttrDmgRedAdd = 15302,
      AttrDmgRedExAdd = 15303,
      AttrDmgRedPer = 15304,
      AttrDmgRedExPer = 15305,
      AttrDmgNear = 16000,
      AttrDmgNearTotal = 16001,
      AttrDmgNearAdd = 16002,
      AttrDmgNearExAdd = 16003,
      AttrDmgNearPer = 16004,
      AttrDmgNearExPer = 16005,
      AttrDmgRedNear = 16020,
      AttrDmgRedNearTotal = 16021,
      AttrDmgRedNearAdd = 16022,
      AttrDmgRedNearExAdd = 16023,
      AttrDmgRedNearPer = 16024,
      AttrDmgRedNearExPer = 16025,
      AttrDmgFar = 16100,
      AttrDmgFarTotal = 16101,
      AttrDmgFarAdd = 16102,
      AttrDmgFarExAdd = 16103,
      AttrDmgFarPer = 16104,
      AttrDmgFarExPer = 16105,
      AttrDmgRedFar = 16120,
      AttrDmgRedFarTotal = 16121,
      AttrDmgRedFarAdd = 16122,
      AttrDmgRedFarExAdd = 16123,
      AttrDmgRedFarPer = 16124,
      AttrDmgRedFarExPer = 16125,
      AttrDmgRedBullet = 16220,
      AttrDmgRedBulletTotal = 16221,
      AttrDmgRedBulletAdd = 16222,
      AttrDmgRedBulletExAdd = 16223,
      AttrDmgRedBulletPer = 16224,
      AttrDmgRedBulletExPer = 16225,
      AttrDmgRedMelee = 16320,
      AttrDmgRedMeleeTotal = 16321,
      AttrDmgRedMeleeAdd = 16322,
      AttrDmgRedMeleeExAdd = 16323,
      AttrDmgRedMeleePer = 16324,
      AttrDmgRedMeleeExPer = 16325,
      AttrDmgRedBuff = 16420,
      AttrDmgRedBuffTotal = 16421,
      AttrDmgRedBuffAdd = 16422,
      AttrDmgRedBuffExAdd = 16423,
      AttrDmgRedBuffPer = 16424,
      AttrDmgRedBuffExPer = 16425,
      AttrDmgRedVsNormalEnemy = 16720,
      AttrDmgRedVsNormalEnemyTotal = 16721,
      AttrDmgRedVsNormalEnemyAdd = 16722,
      AttrDmgRedVsNormalEnemyExAdd = 16723,
      AttrDmgRedVsNormalEnemyPer = 16724,
      AttrDmgRedVsNormalEnemyExPer = 16725,
      AttrDmgRedVsEliteEnemy = 16820,
      AttrDmgRedVsEliteEnemyTotal = 16821,
      AttrDmgRedVsEliteEnemyAdd = 16822,
      AttrDmgRedVsEliteEnemyExAdd = 16823,
      AttrDmgRedVsEliteEnemyPer = 16824,
      AttrDmgRedVsEliteEnemyExPer = 16825,
      AttrDmgRedHealthy = 17020,
      AttrDmgRedHealthyTotal = 17021,
      AttrDmgRedHealthyAdd = 17022,
      AttrDmgRedHealthyExAdd = 17023,
      AttrDmgRedHealthyPer = 17024,
      AttrDmgRedHealthyExPer = 17025,
      AttrDmgRedNonHealthy = 17120,
      AttrDmgRedNonHealthyTotal = 17121,
      AttrDmgRedNonHealthyAdd = 17122,
      AttrDmgRedNonHealthyExAdd = 17123,
      AttrDmgRedNonHealthyPer = 17124,
      AttrDmgRedNonHealthyExPer = 17125,
      AttrEntityLevel = 60010,

    }

    public static class AttrCreator
    {
        #region AttrTypeDict
    
        private static Dictionary<ELogicAttr, Type> _attrTypeDict = new Dictionary<ELogicAttr, Type>()
        {
                 { ELogicAttr.AttrUid, typeof(int) }, 
     { ELogicAttr.AttrSourceActorUid, typeof(int) }, 
     { ELogicAttr.AttrSourceAbilityConfigId, typeof(int) }, 
     { ELogicAttr.SourceAbilityType, typeof(int) }, 
     { ELogicAttr.AttrRot, typeof(Quaternion) }, 
     { ELogicAttr.AttrPosition, typeof(Vector3) }, 
     { ELogicAttr.AttrBaseSpeed, typeof(float) }, 
     { ELogicAttr.AttrAttackTargetUids, typeof(List<int>) }, 
     { ELogicAttr.AttrMoveTargetUid, typeof(int) }, 
     { ELogicAttr.AttrFaction, typeof(int) }, 
     { ELogicAttr.AttrUnselectable, typeof(int) }, 
     { ELogicAttr.AttrInvincible, typeof(int) }, 
     { ELogicAttr.AttrHp, typeof(int) }, 
     { ELogicAttr.AttrHpPer, typeof(int) }, 
     { ELogicAttr.AttrMaxHp, typeof(int) }, 
     { ELogicAttr.AttrMaxHpTotal, typeof(int) }, 
     { ELogicAttr.AttrMaxHpAdd, typeof(int) }, 
     { ELogicAttr.AttrMaxHpExAdd, typeof(int) }, 
     { ELogicAttr.AttrMaxHpPer, typeof(int) }, 
     { ELogicAttr.AttrMaxHpExPer, typeof(int) }, 
     { ELogicAttr.AttrMp, typeof(int) }, 
     { ELogicAttr.AttrMpPer, typeof(int) }, 
     { ELogicAttr.AttrMaxMp, typeof(int) }, 
     { ELogicAttr.AttrMaxMpTotal, typeof(int) }, 
     { ELogicAttr.AttrMaxMpAdd, typeof(int) }, 
     { ELogicAttr.AttrMaxMpExAdd, typeof(int) }, 
     { ELogicAttr.AttrMaxMpPer, typeof(int) }, 
     { ELogicAttr.AttrMaxMpExPer, typeof(int) }, 
     { ELogicAttr.AttrShield, typeof(int) }, 
     { ELogicAttr.AttrShieldPer, typeof(int) }, 
     { ELogicAttr.AttrAttack, typeof(int) }, 
     { ELogicAttr.AttrAttackTotal, typeof(int) }, 
     { ELogicAttr.AttrAttackAdd, typeof(int) }, 
     { ELogicAttr.AttrAttackExAdd, typeof(int) }, 
     { ELogicAttr.AttrAttackPer, typeof(int) }, 
     { ELogicAttr.AttrAttackExPer, typeof(int) }, 
     { ELogicAttr.AttrDefense, typeof(int) }, 
     { ELogicAttr.AttrDefenseTotal, typeof(int) }, 
     { ELogicAttr.AttrDefenseAdd, typeof(int) }, 
     { ELogicAttr.AttrDefenseExAdd, typeof(int) }, 
     { ELogicAttr.AttrDefensePer, typeof(int) }, 
     { ELogicAttr.AttrDefenseExPer, typeof(int) }, 
     { ELogicAttr.AttrIgnoreDefense, typeof(int) }, 
     { ELogicAttr.AttrIgnoreDefenseTotal, typeof(int) }, 
     { ELogicAttr.AttrIgnoreDefenseAdd, typeof(int) }, 
     { ELogicAttr.AttrIgnoreDefenseExAdd, typeof(int) }, 
     { ELogicAttr.AttrIgnoreDefensePer, typeof(int) }, 
     { ELogicAttr.AttrIgnoreDefenseExPer, typeof(int) }, 
     { ELogicAttr.AttrIgnoreDefensePCT, typeof(int) }, 
     { ELogicAttr.AttrIgnoreDefensePCTTotal, typeof(int) }, 
     { ELogicAttr.AttrIgnoreDefensePCTAdd, typeof(int) }, 
     { ELogicAttr.AttrIgnoreDefensePCTExAdd, typeof(int) }, 
     { ELogicAttr.AttrIgnoreDefensePCTPer, typeof(int) }, 
     { ELogicAttr.AttrIgnoreDefensePCTExPer, typeof(int) }, 
     { ELogicAttr.AttrCrit, typeof(int) }, 
     { ELogicAttr.AttrCritTotal, typeof(int) }, 
     { ELogicAttr.AttrCritAdd, typeof(int) }, 
     { ELogicAttr.AttrCritExAdd, typeof(int) }, 
     { ELogicAttr.AttrCritPer, typeof(int) }, 
     { ELogicAttr.AttrCritExPer, typeof(int) }, 
     { ELogicAttr.AttrCritDamage, typeof(int) }, 
     { ELogicAttr.AttrCritDamageTotal, typeof(int) }, 
     { ELogicAttr.AttrCritDamageAdd, typeof(int) }, 
     { ELogicAttr.AttrCritDamageExAdd, typeof(int) }, 
     { ELogicAttr.AttrCritDamagePer, typeof(int) }, 
     { ELogicAttr.AttrCritDamageExPer, typeof(int) }, 
     { ELogicAttr.AttrHealIntensity, typeof(int) }, 
     { ELogicAttr.AttrHealIntensityTotal, typeof(int) }, 
     { ELogicAttr.AttrHealIntensityAdd, typeof(int) }, 
     { ELogicAttr.AttrHealIntensityExAdd, typeof(int) }, 
     { ELogicAttr.AttrHealIntensityPer, typeof(int) }, 
     { ELogicAttr.AttrHealIntensityExPer, typeof(int) }, 
     { ELogicAttr.AttrHeal, typeof(int) }, 
     { ELogicAttr.AttrHealTotal, typeof(int) }, 
     { ELogicAttr.AttrHealAdd, typeof(int) }, 
     { ELogicAttr.AttrHealExAdd, typeof(int) }, 
     { ELogicAttr.AttrHealPer, typeof(int) }, 
     { ELogicAttr.AttrHealExPer, typeof(int) }, 
     { ELogicAttr.AttrHealed, typeof(int) }, 
     { ELogicAttr.AttrHealedTotal, typeof(int) }, 
     { ELogicAttr.AttrHealedAdd, typeof(int) }, 
     { ELogicAttr.AttrHealedExAdd, typeof(int) }, 
     { ELogicAttr.AttrHealedPer, typeof(int) }, 
     { ELogicAttr.AttrHealedExPer, typeof(int) }, 
     { ELogicAttr.AttrAttackSpeedPCT, typeof(int) }, 
     { ELogicAttr.AttrAttackSpeedPCTTotal, typeof(int) }, 
     { ELogicAttr.AttrAttackSpeedPCTAdd, typeof(int) }, 
     { ELogicAttr.AttrAttackSpeedPCTExAdd, typeof(int) }, 
     { ELogicAttr.AttrAttackSpeedPCTPer, typeof(int) }, 
     { ELogicAttr.AttrAttackSpeedPCTExPer, typeof(int) }, 
     { ELogicAttr.AttrElementPenPCT, typeof(int) }, 
     { ELogicAttr.AttrElementPenPCTTotal, typeof(int) }, 
     { ELogicAttr.AttrElementPenPCTAdd, typeof(int) }, 
     { ELogicAttr.AttrElementPenPCTExAdd, typeof(int) }, 
     { ELogicAttr.AttrElementPenPCTPer, typeof(int) }, 
     { ELogicAttr.AttrElementPenPCTExPer, typeof(int) }, 
     { ELogicAttr.AttrElementRedPCT, typeof(int) }, 
     { ELogicAttr.AttrElementRedPCTTotal, typeof(int) }, 
     { ELogicAttr.AttrElementRedPCTAdd, typeof(int) }, 
     { ELogicAttr.AttrElementRedPCTExAdd, typeof(int) }, 
     { ELogicAttr.AttrElementRedPCTPer, typeof(int) }, 
     { ELogicAttr.AttrElementRedPCTExPer, typeof(int) }, 
     { ELogicAttr.AttrElementPhysicalPenPCT, typeof(int) }, 
     { ELogicAttr.AttrElementPhysicalPenPCTTotal, typeof(int) }, 
     { ELogicAttr.AttrElementPhysicalPenPCTAdd, typeof(int) }, 
     { ELogicAttr.AttrElementPhysicalPenPCTExAdd, typeof(int) }, 
     { ELogicAttr.AttrElementPhysicalPenPCTPer, typeof(int) }, 
     { ELogicAttr.AttrElementPhysicalPenPCTExPer, typeof(int) }, 
     { ELogicAttr.AttrElementPhysicalRedPCT, typeof(int) }, 
     { ELogicAttr.AttrElementPhysicalRedPCTTotal, typeof(int) }, 
     { ELogicAttr.AttrElementPhysicalRedPCTAdd, typeof(int) }, 
     { ELogicAttr.AttrElementPhysicalRedPCTExAdd, typeof(int) }, 
     { ELogicAttr.AttrElementPhysicalRedPCTPer, typeof(int) }, 
     { ELogicAttr.AttrElementPhysicalRedPCTExPer, typeof(int) }, 
     { ELogicAttr.AttrElementMagicPenPCT, typeof(int) }, 
     { ELogicAttr.AttrElementMagicPenPCTTotal, typeof(int) }, 
     { ELogicAttr.AttrElementMagicPenPCTAdd, typeof(int) }, 
     { ELogicAttr.AttrElementMagicPenPCTExAdd, typeof(int) }, 
     { ELogicAttr.AttrElementMagicPenPCTPer, typeof(int) }, 
     { ELogicAttr.AttrElementMagicPenPCTExPer, typeof(int) }, 
     { ELogicAttr.AttrElementMagicRedPCT, typeof(int) }, 
     { ELogicAttr.AttrElementMagicRedPCTTotal, typeof(int) }, 
     { ELogicAttr.AttrElementMagicRedPCTAdd, typeof(int) }, 
     { ELogicAttr.AttrElementMagicRedPCTExAdd, typeof(int) }, 
     { ELogicAttr.AttrElementMagicRedPCTPer, typeof(int) }, 
     { ELogicAttr.AttrElementMagicRedPCTExPer, typeof(int) }, 
     { ELogicAttr.AttrDmgA, typeof(int) }, 
     { ELogicAttr.AttrDmgATotal, typeof(int) }, 
     { ELogicAttr.AttrDmgAAdd, typeof(int) }, 
     { ELogicAttr.AttrDmgAExAdd, typeof(int) }, 
     { ELogicAttr.AttrDmgAPer, typeof(int) }, 
     { ELogicAttr.AttrDmgAExPer, typeof(int) }, 
     { ELogicAttr.AttrDmgRed, typeof(int) }, 
     { ELogicAttr.AttrDmgRedTotal, typeof(int) }, 
     { ELogicAttr.AttrDmgRedAdd, typeof(int) }, 
     { ELogicAttr.AttrDmgRedExAdd, typeof(int) }, 
     { ELogicAttr.AttrDmgRedPer, typeof(int) }, 
     { ELogicAttr.AttrDmgRedExPer, typeof(int) }, 
     { ELogicAttr.AttrDmgNear, typeof(int) }, 
     { ELogicAttr.AttrDmgNearTotal, typeof(int) }, 
     { ELogicAttr.AttrDmgNearAdd, typeof(int) }, 
     { ELogicAttr.AttrDmgNearExAdd, typeof(int) }, 
     { ELogicAttr.AttrDmgNearPer, typeof(int) }, 
     { ELogicAttr.AttrDmgNearExPer, typeof(int) }, 
     { ELogicAttr.AttrDmgRedNear, typeof(int) }, 
     { ELogicAttr.AttrDmgRedNearTotal, typeof(int) }, 
     { ELogicAttr.AttrDmgRedNearAdd, typeof(int) }, 
     { ELogicAttr.AttrDmgRedNearExAdd, typeof(int) }, 
     { ELogicAttr.AttrDmgRedNearPer, typeof(int) }, 
     { ELogicAttr.AttrDmgRedNearExPer, typeof(int) }, 
     { ELogicAttr.AttrDmgFar, typeof(int) }, 
     { ELogicAttr.AttrDmgFarTotal, typeof(int) }, 
     { ELogicAttr.AttrDmgFarAdd, typeof(int) }, 
     { ELogicAttr.AttrDmgFarExAdd, typeof(int) }, 
     { ELogicAttr.AttrDmgFarPer, typeof(int) }, 
     { ELogicAttr.AttrDmgFarExPer, typeof(int) }, 
     { ELogicAttr.AttrDmgRedFar, typeof(int) }, 
     { ELogicAttr.AttrDmgRedFarTotal, typeof(int) }, 
     { ELogicAttr.AttrDmgRedFarAdd, typeof(int) }, 
     { ELogicAttr.AttrDmgRedFarExAdd, typeof(int) }, 
     { ELogicAttr.AttrDmgRedFarPer, typeof(int) }, 
     { ELogicAttr.AttrDmgRedFarExPer, typeof(int) }, 
     { ELogicAttr.AttrDmgRedBullet, typeof(int) }, 
     { ELogicAttr.AttrDmgRedBulletTotal, typeof(int) }, 
     { ELogicAttr.AttrDmgRedBulletAdd, typeof(int) }, 
     { ELogicAttr.AttrDmgRedBulletExAdd, typeof(int) }, 
     { ELogicAttr.AttrDmgRedBulletPer, typeof(int) }, 
     { ELogicAttr.AttrDmgRedBulletExPer, typeof(int) }, 
     { ELogicAttr.AttrDmgRedMelee, typeof(int) }, 
     { ELogicAttr.AttrDmgRedMeleeTotal, typeof(int) }, 
     { ELogicAttr.AttrDmgRedMeleeAdd, typeof(int) }, 
     { ELogicAttr.AttrDmgRedMeleeExAdd, typeof(int) }, 
     { ELogicAttr.AttrDmgRedMeleePer, typeof(int) }, 
     { ELogicAttr.AttrDmgRedMeleeExPer, typeof(int) }, 
     { ELogicAttr.AttrDmgRedBuff, typeof(int) }, 
     { ELogicAttr.AttrDmgRedBuffTotal, typeof(int) }, 
     { ELogicAttr.AttrDmgRedBuffAdd, typeof(int) }, 
     { ELogicAttr.AttrDmgRedBuffExAdd, typeof(int) }, 
     { ELogicAttr.AttrDmgRedBuffPer, typeof(int) }, 
     { ELogicAttr.AttrDmgRedBuffExPer, typeof(int) }, 
     { ELogicAttr.AttrDmgRedVsNormalEnemy, typeof(int) }, 
     { ELogicAttr.AttrDmgRedVsNormalEnemyTotal, typeof(int) }, 
     { ELogicAttr.AttrDmgRedVsNormalEnemyAdd, typeof(int) }, 
     { ELogicAttr.AttrDmgRedVsNormalEnemyExAdd, typeof(int) }, 
     { ELogicAttr.AttrDmgRedVsNormalEnemyPer, typeof(int) }, 
     { ELogicAttr.AttrDmgRedVsNormalEnemyExPer, typeof(int) }, 
     { ELogicAttr.AttrDmgRedVsEliteEnemy, typeof(int) }, 
     { ELogicAttr.AttrDmgRedVsEliteEnemyTotal, typeof(int) }, 
     { ELogicAttr.AttrDmgRedVsEliteEnemyAdd, typeof(int) }, 
     { ELogicAttr.AttrDmgRedVsEliteEnemyExAdd, typeof(int) }, 
     { ELogicAttr.AttrDmgRedVsEliteEnemyPer, typeof(int) }, 
     { ELogicAttr.AttrDmgRedVsEliteEnemyExPer, typeof(int) }, 
     { ELogicAttr.AttrDmgRedHealthy, typeof(int) }, 
     { ELogicAttr.AttrDmgRedHealthyTotal, typeof(int) }, 
     { ELogicAttr.AttrDmgRedHealthyAdd, typeof(int) }, 
     { ELogicAttr.AttrDmgRedHealthyExAdd, typeof(int) }, 
     { ELogicAttr.AttrDmgRedHealthyPer, typeof(int) }, 
     { ELogicAttr.AttrDmgRedHealthyExPer, typeof(int) }, 
     { ELogicAttr.AttrDmgRedNonHealthy, typeof(int) }, 
     { ELogicAttr.AttrDmgRedNonHealthyTotal, typeof(int) }, 
     { ELogicAttr.AttrDmgRedNonHealthyAdd, typeof(int) }, 
     { ELogicAttr.AttrDmgRedNonHealthyExAdd, typeof(int) }, 
     { ELogicAttr.AttrDmgRedNonHealthyPer, typeof(int) }, 
     { ELogicAttr.AttrDmgRedNonHealthyExPer, typeof(int) }, 
     { ELogicAttr.AttrEntityLevel, typeof(int) }, 

        };
    
        public static Type GetValueType(this ELogicAttr attrType)
        {
            if (_attrTypeDict.TryGetValue(attrType, out var type))
            {
                return _attrTypeDict[attrType];
            }
        
            return null;
        }
    
        #endregion
    
        #region AttrCreate
    
        public static IAttr Create(int attrType)
        {
            switch ((ELogicAttr)attrType)
            {
                
                case ELogicAttr.AttrUid :
                     return new Attr<int>((a, b) => a + b);
                case ELogicAttr.AttrSourceActorUid :
                     return new Attr<int>((a, b) => a + b);
                case ELogicAttr.AttrSourceAbilityConfigId :
                     return new Attr<int>((a, b) => a + b);
                case ELogicAttr.SourceAbilityType :
                     return new Attr<int>((a, b) => a + b);
                case ELogicAttr.AttrRot :
                     return new Attr<Quaternion>((a, b) => b * a);
                case ELogicAttr.AttrPosition :
                     return new Attr<Vector3>((a, b) => a + b);
                case ELogicAttr.AttrBaseSpeed :
                     return new Attr<float>((a, b) => a + b);
                case ELogicAttr.AttrAttackTargetUids :
                     return new Attr<List<int>>(null);
                case ELogicAttr.AttrMoveTargetUid :
                     return new Attr<int>((a, b) => a + b);
                case ELogicAttr.AttrFaction :
                     return new Attr<int>((a, b) => a + b);
                case ELogicAttr.AttrUnselectable :
                     return new Attr<int>((a, b) => a + b);
                case ELogicAttr.AttrInvincible :
                     return new Attr<int>((a, b) => a + b);
                case ELogicAttr.AttrHp :
                     return new Attr<int>((a, b) => a + b);
                case ELogicAttr.AttrHpPer :
                     return new Attr<int>((a, b) => a + b);
                case ELogicAttr.AttrMaxHp :
                     return new Attr<int>((a, b) => a + b);
                case ELogicAttr.AttrMaxHpTotal :
                     return new Attr<int>((a, b) => a + b);
                case ELogicAttr.AttrMaxHpAdd :
                     return new Attr<int>((a, b) => a + b);
                case ELogicAttr.AttrMaxHpExAdd :
                     return new Attr<int>((a, b) => a + b);
                case ELogicAttr.AttrMaxHpPer :
                     return new Attr<int>((a, b) => a + b);
                case ELogicAttr.AttrMaxHpExPer :
                     return new Attr<int>((a, b) => a + b);
                case ELogicAttr.AttrMp :
                     return new Attr<int>((a, b) => a + b);
                case ELogicAttr.AttrMpPer :
                     return new Attr<int>((a, b) => a + b);
                case ELogicAttr.AttrMaxMp :
                     return new Attr<int>((a, b) => a + b);
                case ELogicAttr.AttrMaxMpTotal :
                     return new Attr<int>((a, b) => a + b);
                case ELogicAttr.AttrMaxMpAdd :
                     return new Attr<int>((a, b) => a + b);
                case ELogicAttr.AttrMaxMpExAdd :
                     return new Attr<int>((a, b) => a + b);
                case ELogicAttr.AttrMaxMpPer :
                     return new Attr<int>((a, b) => a + b);
                case ELogicAttr.AttrMaxMpExPer :
                     return new Attr<int>((a, b) => a + b);
                case ELogicAttr.AttrShield :
                     return new Attr<int>((a, b) => a + b);
                case ELogicAttr.AttrShieldPer :
                     return new Attr<int>((a, b) => a + b);
                case ELogicAttr.AttrAttack :
                     return new Attr<int>((a, b) => a + b);
                case ELogicAttr.AttrAttackTotal :
                     return new Attr<int>((a, b) => a + b);
                case ELogicAttr.AttrAttackAdd :
                     return new Attr<int>((a, b) => a + b);
                case ELogicAttr.AttrAttackExAdd :
                     return new Attr<int>((a, b) => a + b);
                case ELogicAttr.AttrAttackPer :
                     return new Attr<int>((a, b) => a + b);
                case ELogicAttr.AttrAttackExPer :
                     return new Attr<int>((a, b) => a + b);
                case ELogicAttr.AttrDefense :
                     return new Attr<int>((a, b) => a + b);
                case ELogicAttr.AttrDefenseTotal :
                     return new Attr<int>((a, b) => a + b);
                case ELogicAttr.AttrDefenseAdd :
                     return new Attr<int>((a, b) => a + b);
                case ELogicAttr.AttrDefenseExAdd :
                     return new Attr<int>((a, b) => a + b);
                case ELogicAttr.AttrDefensePer :
                     return new Attr<int>((a, b) => a + b);
                case ELogicAttr.AttrDefenseExPer :
                     return new Attr<int>((a, b) => a + b);
                case ELogicAttr.AttrIgnoreDefense :
                     return new Attr<int>((a, b) => a + b);
                case ELogicAttr.AttrIgnoreDefenseTotal :
                     return new Attr<int>((a, b) => a + b);
                case ELogicAttr.AttrIgnoreDefenseAdd :
                     return new Attr<int>((a, b) => a + b);
                case ELogicAttr.AttrIgnoreDefenseExAdd :
                     return new Attr<int>((a, b) => a + b);
                case ELogicAttr.AttrIgnoreDefensePer :
                     return new Attr<int>((a, b) => a + b);
                case ELogicAttr.AttrIgnoreDefenseExPer :
                     return new Attr<int>((a, b) => a + b);
                case ELogicAttr.AttrIgnoreDefensePCT :
                     return new Attr<int>((a, b) => a + b);
                case ELogicAttr.AttrIgnoreDefensePCTTotal :
                     return new Attr<int>((a, b) => a + b);
                case ELogicAttr.AttrIgnoreDefensePCTAdd :
                     return new Attr<int>((a, b) => a + b);
                case ELogicAttr.AttrIgnoreDefensePCTExAdd :
                     return new Attr<int>((a, b) => a + b);
                case ELogicAttr.AttrIgnoreDefensePCTPer :
                     return new Attr<int>((a, b) => a + b);
                case ELogicAttr.AttrIgnoreDefensePCTExPer :
                     return new Attr<int>((a, b) => a + b);
                case ELogicAttr.AttrCrit :
                     return new Attr<int>((a, b) => a + b);
                case ELogicAttr.AttrCritTotal :
                     return new Attr<int>((a, b) => a + b);
                case ELogicAttr.AttrCritAdd :
                     return new Attr<int>((a, b) => a + b);
                case ELogicAttr.AttrCritExAdd :
                     return new Attr<int>((a, b) => a + b);
                case ELogicAttr.AttrCritPer :
                     return new Attr<int>((a, b) => a + b);
                case ELogicAttr.AttrCritExPer :
                     return new Attr<int>((a, b) => a + b);
                case ELogicAttr.AttrCritDamage :
                     return new Attr<int>((a, b) => a + b);
                case ELogicAttr.AttrCritDamageTotal :
                     return new Attr<int>((a, b) => a + b);
                case ELogicAttr.AttrCritDamageAdd :
                     return new Attr<int>((a, b) => a + b);
                case ELogicAttr.AttrCritDamageExAdd :
                     return new Attr<int>((a, b) => a + b);
                case ELogicAttr.AttrCritDamagePer :
                     return new Attr<int>((a, b) => a + b);
                case ELogicAttr.AttrCritDamageExPer :
                     return new Attr<int>((a, b) => a + b);
                case ELogicAttr.AttrHealIntensity :
                     return new Attr<int>((a, b) => a + b);
                case ELogicAttr.AttrHealIntensityTotal :
                     return new Attr<int>((a, b) => a + b);
                case ELogicAttr.AttrHealIntensityAdd :
                     return new Attr<int>((a, b) => a + b);
                case ELogicAttr.AttrHealIntensityExAdd :
                     return new Attr<int>((a, b) => a + b);
                case ELogicAttr.AttrHealIntensityPer :
                     return new Attr<int>((a, b) => a + b);
                case ELogicAttr.AttrHealIntensityExPer :
                     return new Attr<int>((a, b) => a + b);
                case ELogicAttr.AttrHeal :
                     return new Attr<int>((a, b) => a + b);
                case ELogicAttr.AttrHealTotal :
                     return new Attr<int>((a, b) => a + b);
                case ELogicAttr.AttrHealAdd :
                     return new Attr<int>((a, b) => a + b);
                case ELogicAttr.AttrHealExAdd :
                     return new Attr<int>((a, b) => a + b);
                case ELogicAttr.AttrHealPer :
                     return new Attr<int>((a, b) => a + b);
                case ELogicAttr.AttrHealExPer :
                     return new Attr<int>((a, b) => a + b);
                case ELogicAttr.AttrHealed :
                     return new Attr<int>((a, b) => a + b);
                case ELogicAttr.AttrHealedTotal :
                     return new Attr<int>((a, b) => a + b);
                case ELogicAttr.AttrHealedAdd :
                     return new Attr<int>((a, b) => a + b);
                case ELogicAttr.AttrHealedExAdd :
                     return new Attr<int>((a, b) => a + b);
                case ELogicAttr.AttrHealedPer :
                     return new Attr<int>((a, b) => a + b);
                case ELogicAttr.AttrHealedExPer :
                     return new Attr<int>((a, b) => a + b);
                case ELogicAttr.AttrAttackSpeedPCT :
                     return new Attr<int>((a, b) => a + b);
                case ELogicAttr.AttrAttackSpeedPCTTotal :
                     return new Attr<int>((a, b) => a + b);
                case ELogicAttr.AttrAttackSpeedPCTAdd :
                     return new Attr<int>((a, b) => a + b);
                case ELogicAttr.AttrAttackSpeedPCTExAdd :
                     return new Attr<int>((a, b) => a + b);
                case ELogicAttr.AttrAttackSpeedPCTPer :
                     return new Attr<int>((a, b) => a + b);
                case ELogicAttr.AttrAttackSpeedPCTExPer :
                     return new Attr<int>((a, b) => a + b);
                case ELogicAttr.AttrElementPenPCT :
                     return new Attr<int>((a, b) => a + b);
                case ELogicAttr.AttrElementPenPCTTotal :
                     return new Attr<int>((a, b) => a + b);
                case ELogicAttr.AttrElementPenPCTAdd :
                     return new Attr<int>((a, b) => a + b);
                case ELogicAttr.AttrElementPenPCTExAdd :
                     return new Attr<int>((a, b) => a + b);
                case ELogicAttr.AttrElementPenPCTPer :
                     return new Attr<int>((a, b) => a + b);
                case ELogicAttr.AttrElementPenPCTExPer :
                     return new Attr<int>((a, b) => a + b);
                case ELogicAttr.AttrElementRedPCT :
                     return new Attr<int>((a, b) => a + b);
                case ELogicAttr.AttrElementRedPCTTotal :
                     return new Attr<int>((a, b) => a + b);
                case ELogicAttr.AttrElementRedPCTAdd :
                     return new Attr<int>((a, b) => a + b);
                case ELogicAttr.AttrElementRedPCTExAdd :
                     return new Attr<int>((a, b) => a + b);
                case ELogicAttr.AttrElementRedPCTPer :
                     return new Attr<int>((a, b) => a + b);
                case ELogicAttr.AttrElementRedPCTExPer :
                     return new Attr<int>((a, b) => a + b);
                case ELogicAttr.AttrElementPhysicalPenPCT :
                     return new Attr<int>((a, b) => a + b);
                case ELogicAttr.AttrElementPhysicalPenPCTTotal :
                     return new Attr<int>((a, b) => a + b);
                case ELogicAttr.AttrElementPhysicalPenPCTAdd :
                     return new Attr<int>((a, b) => a + b);
                case ELogicAttr.AttrElementPhysicalPenPCTExAdd :
                     return new Attr<int>((a, b) => a + b);
                case ELogicAttr.AttrElementPhysicalPenPCTPer :
                     return new Attr<int>((a, b) => a + b);
                case ELogicAttr.AttrElementPhysicalPenPCTExPer :
                     return new Attr<int>((a, b) => a + b);
                case ELogicAttr.AttrElementPhysicalRedPCT :
                     return new Attr<int>((a, b) => a + b);
                case ELogicAttr.AttrElementPhysicalRedPCTTotal :
                     return new Attr<int>((a, b) => a + b);
                case ELogicAttr.AttrElementPhysicalRedPCTAdd :
                     return new Attr<int>((a, b) => a + b);
                case ELogicAttr.AttrElementPhysicalRedPCTExAdd :
                     return new Attr<int>((a, b) => a + b);
                case ELogicAttr.AttrElementPhysicalRedPCTPer :
                     return new Attr<int>((a, b) => a + b);
                case ELogicAttr.AttrElementPhysicalRedPCTExPer :
                     return new Attr<int>((a, b) => a + b);
                case ELogicAttr.AttrElementMagicPenPCT :
                     return new Attr<int>((a, b) => a + b);
                case ELogicAttr.AttrElementMagicPenPCTTotal :
                     return new Attr<int>((a, b) => a + b);
                case ELogicAttr.AttrElementMagicPenPCTAdd :
                     return new Attr<int>((a, b) => a + b);
                case ELogicAttr.AttrElementMagicPenPCTExAdd :
                     return new Attr<int>((a, b) => a + b);
                case ELogicAttr.AttrElementMagicPenPCTPer :
                     return new Attr<int>((a, b) => a + b);
                case ELogicAttr.AttrElementMagicPenPCTExPer :
                     return new Attr<int>((a, b) => a + b);
                case ELogicAttr.AttrElementMagicRedPCT :
                     return new Attr<int>((a, b) => a + b);
                case ELogicAttr.AttrElementMagicRedPCTTotal :
                     return new Attr<int>((a, b) => a + b);
                case ELogicAttr.AttrElementMagicRedPCTAdd :
                     return new Attr<int>((a, b) => a + b);
                case ELogicAttr.AttrElementMagicRedPCTExAdd :
                     return new Attr<int>((a, b) => a + b);
                case ELogicAttr.AttrElementMagicRedPCTPer :
                     return new Attr<int>((a, b) => a + b);
                case ELogicAttr.AttrElementMagicRedPCTExPer :
                     return new Attr<int>((a, b) => a + b);
                case ELogicAttr.AttrDmgA :
                     return new Attr<int>((a, b) => a + b);
                case ELogicAttr.AttrDmgATotal :
                     return new Attr<int>((a, b) => a + b);
                case ELogicAttr.AttrDmgAAdd :
                     return new Attr<int>((a, b) => a + b);
                case ELogicAttr.AttrDmgAExAdd :
                     return new Attr<int>((a, b) => a + b);
                case ELogicAttr.AttrDmgAPer :
                     return new Attr<int>((a, b) => a + b);
                case ELogicAttr.AttrDmgAExPer :
                     return new Attr<int>((a, b) => a + b);
                case ELogicAttr.AttrDmgRed :
                     return new Attr<int>((a, b) => a + b);
                case ELogicAttr.AttrDmgRedTotal :
                     return new Attr<int>((a, b) => a + b);
                case ELogicAttr.AttrDmgRedAdd :
                     return new Attr<int>((a, b) => a + b);
                case ELogicAttr.AttrDmgRedExAdd :
                     return new Attr<int>((a, b) => a + b);
                case ELogicAttr.AttrDmgRedPer :
                     return new Attr<int>((a, b) => a + b);
                case ELogicAttr.AttrDmgRedExPer :
                     return new Attr<int>((a, b) => a + b);
                case ELogicAttr.AttrDmgNear :
                     return new Attr<int>((a, b) => a + b);
                case ELogicAttr.AttrDmgNearTotal :
                     return new Attr<int>((a, b) => a + b);
                case ELogicAttr.AttrDmgNearAdd :
                     return new Attr<int>((a, b) => a + b);
                case ELogicAttr.AttrDmgNearExAdd :
                     return new Attr<int>((a, b) => a + b);
                case ELogicAttr.AttrDmgNearPer :
                     return new Attr<int>((a, b) => a + b);
                case ELogicAttr.AttrDmgNearExPer :
                     return new Attr<int>((a, b) => a + b);
                case ELogicAttr.AttrDmgRedNear :
                     return new Attr<int>((a, b) => a + b);
                case ELogicAttr.AttrDmgRedNearTotal :
                     return new Attr<int>((a, b) => a + b);
                case ELogicAttr.AttrDmgRedNearAdd :
                     return new Attr<int>((a, b) => a + b);
                case ELogicAttr.AttrDmgRedNearExAdd :
                     return new Attr<int>((a, b) => a + b);
                case ELogicAttr.AttrDmgRedNearPer :
                     return new Attr<int>((a, b) => a + b);
                case ELogicAttr.AttrDmgRedNearExPer :
                     return new Attr<int>((a, b) => a + b);
                case ELogicAttr.AttrDmgFar :
                     return new Attr<int>((a, b) => a + b);
                case ELogicAttr.AttrDmgFarTotal :
                     return new Attr<int>((a, b) => a + b);
                case ELogicAttr.AttrDmgFarAdd :
                     return new Attr<int>((a, b) => a + b);
                case ELogicAttr.AttrDmgFarExAdd :
                     return new Attr<int>((a, b) => a + b);
                case ELogicAttr.AttrDmgFarPer :
                     return new Attr<int>((a, b) => a + b);
                case ELogicAttr.AttrDmgFarExPer :
                     return new Attr<int>((a, b) => a + b);
                case ELogicAttr.AttrDmgRedFar :
                     return new Attr<int>((a, b) => a + b);
                case ELogicAttr.AttrDmgRedFarTotal :
                     return new Attr<int>((a, b) => a + b);
                case ELogicAttr.AttrDmgRedFarAdd :
                     return new Attr<int>((a, b) => a + b);
                case ELogicAttr.AttrDmgRedFarExAdd :
                     return new Attr<int>((a, b) => a + b);
                case ELogicAttr.AttrDmgRedFarPer :
                     return new Attr<int>((a, b) => a + b);
                case ELogicAttr.AttrDmgRedFarExPer :
                     return new Attr<int>((a, b) => a + b);
                case ELogicAttr.AttrDmgRedBullet :
                     return new Attr<int>((a, b) => a + b);
                case ELogicAttr.AttrDmgRedBulletTotal :
                     return new Attr<int>((a, b) => a + b);
                case ELogicAttr.AttrDmgRedBulletAdd :
                     return new Attr<int>((a, b) => a + b);
                case ELogicAttr.AttrDmgRedBulletExAdd :
                     return new Attr<int>((a, b) => a + b);
                case ELogicAttr.AttrDmgRedBulletPer :
                     return new Attr<int>((a, b) => a + b);
                case ELogicAttr.AttrDmgRedBulletExPer :
                     return new Attr<int>((a, b) => a + b);
                case ELogicAttr.AttrDmgRedMelee :
                     return new Attr<int>((a, b) => a + b);
                case ELogicAttr.AttrDmgRedMeleeTotal :
                     return new Attr<int>((a, b) => a + b);
                case ELogicAttr.AttrDmgRedMeleeAdd :
                     return new Attr<int>((a, b) => a + b);
                case ELogicAttr.AttrDmgRedMeleeExAdd :
                     return new Attr<int>((a, b) => a + b);
                case ELogicAttr.AttrDmgRedMeleePer :
                     return new Attr<int>((a, b) => a + b);
                case ELogicAttr.AttrDmgRedMeleeExPer :
                     return new Attr<int>((a, b) => a + b);
                case ELogicAttr.AttrDmgRedBuff :
                     return new Attr<int>((a, b) => a + b);
                case ELogicAttr.AttrDmgRedBuffTotal :
                     return new Attr<int>((a, b) => a + b);
                case ELogicAttr.AttrDmgRedBuffAdd :
                     return new Attr<int>((a, b) => a + b);
                case ELogicAttr.AttrDmgRedBuffExAdd :
                     return new Attr<int>((a, b) => a + b);
                case ELogicAttr.AttrDmgRedBuffPer :
                     return new Attr<int>((a, b) => a + b);
                case ELogicAttr.AttrDmgRedBuffExPer :
                     return new Attr<int>((a, b) => a + b);
                case ELogicAttr.AttrDmgRedVsNormalEnemy :
                     return new Attr<int>((a, b) => a + b);
                case ELogicAttr.AttrDmgRedVsNormalEnemyTotal :
                     return new Attr<int>((a, b) => a + b);
                case ELogicAttr.AttrDmgRedVsNormalEnemyAdd :
                     return new Attr<int>((a, b) => a + b);
                case ELogicAttr.AttrDmgRedVsNormalEnemyExAdd :
                     return new Attr<int>((a, b) => a + b);
                case ELogicAttr.AttrDmgRedVsNormalEnemyPer :
                     return new Attr<int>((a, b) => a + b);
                case ELogicAttr.AttrDmgRedVsNormalEnemyExPer :
                     return new Attr<int>((a, b) => a + b);
                case ELogicAttr.AttrDmgRedVsEliteEnemy :
                     return new Attr<int>((a, b) => a + b);
                case ELogicAttr.AttrDmgRedVsEliteEnemyTotal :
                     return new Attr<int>((a, b) => a + b);
                case ELogicAttr.AttrDmgRedVsEliteEnemyAdd :
                     return new Attr<int>((a, b) => a + b);
                case ELogicAttr.AttrDmgRedVsEliteEnemyExAdd :
                     return new Attr<int>((a, b) => a + b);
                case ELogicAttr.AttrDmgRedVsEliteEnemyPer :
                     return new Attr<int>((a, b) => a + b);
                case ELogicAttr.AttrDmgRedVsEliteEnemyExPer :
                     return new Attr<int>((a, b) => a + b);
                case ELogicAttr.AttrDmgRedHealthy :
                     return new Attr<int>((a, b) => a + b);
                case ELogicAttr.AttrDmgRedHealthyTotal :
                     return new Attr<int>((a, b) => a + b);
                case ELogicAttr.AttrDmgRedHealthyAdd :
                     return new Attr<int>((a, b) => a + b);
                case ELogicAttr.AttrDmgRedHealthyExAdd :
                     return new Attr<int>((a, b) => a + b);
                case ELogicAttr.AttrDmgRedHealthyPer :
                     return new Attr<int>((a, b) => a + b);
                case ELogicAttr.AttrDmgRedHealthyExPer :
                     return new Attr<int>((a, b) => a + b);
                case ELogicAttr.AttrDmgRedNonHealthy :
                     return new Attr<int>((a, b) => a + b);
                case ELogicAttr.AttrDmgRedNonHealthyTotal :
                     return new Attr<int>((a, b) => a + b);
                case ELogicAttr.AttrDmgRedNonHealthyAdd :
                     return new Attr<int>((a, b) => a + b);
                case ELogicAttr.AttrDmgRedNonHealthyExAdd :
                     return new Attr<int>((a, b) => a + b);
                case ELogicAttr.AttrDmgRedNonHealthyPer :
                     return new Attr<int>((a, b) => a + b);
                case ELogicAttr.AttrDmgRedNonHealthyExPer :
                     return new Attr<int>((a, b) => a + b);
                case ELogicAttr.AttrEntityLevel :
                     return new Attr<int>((a, b) => a + b);
            }

            return null;
        }
        
        #endregion
    }
}