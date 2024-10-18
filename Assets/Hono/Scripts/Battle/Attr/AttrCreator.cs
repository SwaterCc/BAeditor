using System;
using System.Collections.Generic;
using UnityEngine;

namespace Hono.Scripts.Battle {
//Auto::AttrMaker
	public enum ELogicAttr {
		/// <summary>
		/// 属性类型 int
		/// </summary>
		AttrUid = 1,

		/// <summary>
		/// 属性类型 int
		/// </summary>
		AttrConfigId = 2,

		/// <summary>
		/// 属性类型 int
		/// </summary>
		AttrModelId = 3,

		/// <summary>
		/// 属性类型 int
		/// </summary>
		AttrIsSummoned = 4,

		/// <summary>
		/// 属性类型 int
		/// </summary>
		AttrActorState = 5,

		/// <summary>
		/// 属性类型 int
		/// </summary>
		AttrTopSourceActorUid = 10,

		/// <summary>
		/// 属性类型 int
		/// </summary>
		AttrSourceActorUid = 11,

		/// <summary>
		/// 属性类型 int
		/// </summary>
		AttrSourceAbilityConfigId = 12,

		/// <summary>
		/// 属性类型 int
		/// </summary>
		SourceAbilityType = 13,

		/// <summary>
		/// 属性类型 Quaternion
		/// </summary>
		AttrRot = 14,

		/// <summary>
		/// 属性类型 Vector3
		/// </summary>
		AttrPosition = 15,

		/// <summary>
		/// 属性类型 float
		/// </summary>
		AttrBaseSpeed = 16,

		/// <summary>
		/// 属性类型 IntArray
		/// </summary>
		AttrAttackTargetUids = 17,

		/// <summary>
		/// 属性类型 int
		/// </summary>
		AttrMoveTargetUid = 18,

		/// <summary>
		/// 属性类型 int
		/// </summary>
		AttrFaction = 19,

		/// <summary>
		/// 属性类型 int
		/// </summary>
		AttrUnselectable = 20,

		/// <summary>
		/// 属性类型 int
		/// </summary>
		AttrInvincible = 21,

		/// <summary>
		/// 属性类型 int
		/// </summary>
		AttrHateTargetUid = 22,

		/// <summary>
		/// 属性类型 int
		/// </summary>
		AttrCantBeHatredTarget = 47,

		/// <summary>
		/// 属性类型 int
		/// </summary>
		AttrCantNormalSkill = 48,

		/// <summary>
		/// 属性类型 int
		/// </summary>
		AttrCantMove = 49,

		/// <summary>
		/// 属性类型 int
		/// </summary>
		AttrStunned = 50,

		/// <summary>
		/// 属性类型 int
		/// </summary>
		AttrSA = 51,

		/// <summary>
		/// 属性类型 int
		/// </summary>
		AttrIgnoreOtherMotion = 52,

		/// <summary>
		/// 属性类型 Vector3
		/// </summary>
		AttrOriginPos = 53,

		/// <summary>
		/// 属性类型 int
		/// </summary>
		AttrHp = 10000,

		/// <summary>
		/// 属性类型 int
		/// </summary>
		AttrHpPer = 10001,

		/// <summary>
		/// 属性类型 int
		/// </summary>
		AttrMaxHp = 10010,

		/// <summary>
		/// 属性类型 int
		/// </summary>
		AttrMaxHpTotal = 10011,

		/// <summary>
		/// 属性类型 int
		/// </summary>
		AttrMaxHpAdd = 10012,

		/// <summary>
		/// 属性类型 int
		/// </summary>
		AttrMaxHpExAdd = 10013,

		/// <summary>
		/// 属性类型 int
		/// </summary>
		AttrMaxHpPer = 10014,

		/// <summary>
		/// 属性类型 int
		/// </summary>
		AttrMaxHpExPer = 10015,

		/// <summary>
		/// 属性类型 int
		/// </summary>
		AttrMp = 10020,

		/// <summary>
		/// 属性类型 int
		/// </summary>
		AttrMpPer = 10021,

		/// <summary>
		/// 属性类型 int
		/// </summary>
		AttrMaxMp = 10030,

		/// <summary>
		/// 属性类型 int
		/// </summary>
		AttrMaxMpTotal = 10031,

		/// <summary>
		/// 属性类型 int
		/// </summary>
		AttrMaxMpAdd = 10032,

		/// <summary>
		/// 属性类型 int
		/// </summary>
		AttrMaxMpExAdd = 10033,

		/// <summary>
		/// 属性类型 int
		/// </summary>
		AttrMaxMpPer = 10034,

		/// <summary>
		/// 属性类型 int
		/// </summary>
		AttrMaxMpExPer = 10035,

		/// <summary>
		/// 属性类型 int
		/// </summary>
		AttrShield = 10040,

		/// <summary>
		/// 属性类型 int
		/// </summary>
		AttrShieldPer = 10041,

		/// <summary>
		/// 属性类型 int
		/// </summary>
		AttrMoveSpeedPCT = 10100,

		/// <summary>
		/// 属性类型 int
		/// </summary>
		AttrMoveSpeedPCTTotal = 10101,

		/// <summary>
		/// 属性类型 int
		/// </summary>
		AttrMoveSpeedPCTAdd = 10102,

		/// <summary>
		/// 属性类型 int
		/// </summary>
		AttrMoveSpeedPCTExAdd = 10103,

		/// <summary>
		/// 属性类型 int
		/// </summary>
		AttrMoveSpeedPCTPer = 10104,

		/// <summary>
		/// 属性类型 int
		/// </summary>
		AttrMoveSpeedPCTExPer = 10105,

		/// <summary>
		/// 属性类型 int
		/// </summary>
		AttrMpRecAll = 10110,

		/// <summary>
		/// 属性类型 int
		/// </summary>
		AttrMpRecAllTotal = 10111,

		/// <summary>
		/// 属性类型 int
		/// </summary>
		AttrMpRecAllAdd = 10112,

		/// <summary>
		/// 属性类型 int
		/// </summary>
		AttrMpRecAllExAdd = 10113,

		/// <summary>
		/// 属性类型 int
		/// </summary>
		AttrMpRecAllPer = 10114,

		/// <summary>
		/// 属性类型 int
		/// </summary>
		AttrMpRecAllExPer = 10115,

		/// <summary>
		/// 属性类型 int
		/// </summary>
		AttrMpRecCast = 10120,

		/// <summary>
		/// 属性类型 int
		/// </summary>
		AttrMpRecCastTotal = 10121,

		/// <summary>
		/// 属性类型 int
		/// </summary>
		AttrMpRecCastAdd = 10122,

		/// <summary>
		/// 属性类型 int
		/// </summary>
		AttrMpRecCastExAdd = 10123,

		/// <summary>
		/// 属性类型 int
		/// </summary>
		AttrMpRecCastPer = 10124,

		/// <summary>
		/// 属性类型 int
		/// </summary>
		AttrMpRecCastExPer = 10125,

		/// <summary>
		/// 属性类型 int
		/// </summary>
		AttrMpRecBehit = 10130,

		/// <summary>
		/// 属性类型 int
		/// </summary>
		AttrMpRecBehitTotal = 10131,

		/// <summary>
		/// 属性类型 int
		/// </summary>
		AttrMpRecBehitAdd = 10132,

		/// <summary>
		/// 属性类型 int
		/// </summary>
		AttrMpRecBehitExAdd = 10133,

		/// <summary>
		/// 属性类型 int
		/// </summary>
		AttrMpRecBehitPer = 10134,

		/// <summary>
		/// 属性类型 int
		/// </summary>
		AttrMpRecBehitExPer = 10135,

		/// <summary>
		/// 属性类型 int
		/// </summary>
		AttrMpRecKilled = 10140,

		/// <summary>
		/// 属性类型 int
		/// </summary>
		AttrMpRecKilledTotal = 10141,

		/// <summary>
		/// 属性类型 int
		/// </summary>
		AttrMpRecKilledAdd = 10142,

		/// <summary>
		/// 属性类型 int
		/// </summary>
		AttrMpRecKilledExAdd = 10143,

		/// <summary>
		/// 属性类型 int
		/// </summary>
		AttrMpRecKilledPer = 10144,

		/// <summary>
		/// 属性类型 int
		/// </summary>
		AttrMpRecKilledExPer = 10145,

		/// <summary>
		/// 属性类型 int
		/// </summary>
		AttrAttack = 12100,

		/// <summary>
		/// 属性类型 int
		/// </summary>
		AttrAttackTotal = 12101,

		/// <summary>
		/// 属性类型 int
		/// </summary>
		AttrAttackAdd = 12102,

		/// <summary>
		/// 属性类型 int
		/// </summary>
		AttrAttackExAdd = 12103,

		/// <summary>
		/// 属性类型 int
		/// </summary>
		AttrAttackPer = 12104,

		/// <summary>
		/// 属性类型 int
		/// </summary>
		AttrAttackExPer = 12105,

		/// <summary>
		/// 属性类型 int
		/// </summary>
		AttrDefense = 12110,

		/// <summary>
		/// 属性类型 int
		/// </summary>
		AttrDefenseTotal = 12111,

		/// <summary>
		/// 属性类型 int
		/// </summary>
		AttrDefenseAdd = 12112,

		/// <summary>
		/// 属性类型 int
		/// </summary>
		AttrDefenseExAdd = 12113,

		/// <summary>
		/// 属性类型 int
		/// </summary>
		AttrDefensePer = 12114,

		/// <summary>
		/// 属性类型 int
		/// </summary>
		AttrDefenseExPer = 12115,

		/// <summary>
		/// 属性类型 int
		/// </summary>
		AttrIgnoreDefense = 12120,

		/// <summary>
		/// 属性类型 int
		/// </summary>
		AttrIgnoreDefenseTotal = 12121,

		/// <summary>
		/// 属性类型 int
		/// </summary>
		AttrIgnoreDefenseAdd = 12122,

		/// <summary>
		/// 属性类型 int
		/// </summary>
		AttrIgnoreDefenseExAdd = 12123,

		/// <summary>
		/// 属性类型 int
		/// </summary>
		AttrIgnoreDefensePer = 12124,

		/// <summary>
		/// 属性类型 int
		/// </summary>
		AttrIgnoreDefenseExPer = 12125,

		/// <summary>
		/// 属性类型 int
		/// </summary>
		AttrIgnoreDefensePCT = 12130,

		/// <summary>
		/// 属性类型 int
		/// </summary>
		AttrIgnoreDefensePCTTotal = 12131,

		/// <summary>
		/// 属性类型 int
		/// </summary>
		AttrIgnoreDefensePCTAdd = 12132,

		/// <summary>
		/// 属性类型 int
		/// </summary>
		AttrIgnoreDefensePCTExAdd = 12133,

		/// <summary>
		/// 属性类型 int
		/// </summary>
		AttrIgnoreDefensePCTPer = 12134,

		/// <summary>
		/// 属性类型 int
		/// </summary>
		AttrIgnoreDefensePCTExPer = 12135,

		/// <summary>
		/// 属性类型 int
		/// </summary>
		AttrCrit = 12140,

		/// <summary>
		/// 属性类型 int
		/// </summary>
		AttrCritTotal = 12141,

		/// <summary>
		/// 属性类型 int
		/// </summary>
		AttrCritAdd = 12142,

		/// <summary>
		/// 属性类型 int
		/// </summary>
		AttrCritExAdd = 12143,

		/// <summary>
		/// 属性类型 int
		/// </summary>
		AttrCritPer = 12144,

		/// <summary>
		/// 属性类型 int
		/// </summary>
		AttrCritExPer = 12145,

		/// <summary>
		/// 属性类型 int
		/// </summary>
		AttrCritDamage = 12150,

		/// <summary>
		/// 属性类型 int
		/// </summary>
		AttrCritDamageTotal = 12151,

		/// <summary>
		/// 属性类型 int
		/// </summary>
		AttrCritDamageAdd = 12152,

		/// <summary>
		/// 属性类型 int
		/// </summary>
		AttrCritDamageExAdd = 12153,

		/// <summary>
		/// 属性类型 int
		/// </summary>
		AttrCritDamagePer = 12154,

		/// <summary>
		/// 属性类型 int
		/// </summary>
		AttrCritDamageExPer = 12155,

		/// <summary>
		/// 属性类型 int
		/// </summary>
		AttrHealIntensity = 12200,

		/// <summary>
		/// 属性类型 int
		/// </summary>
		AttrHealIntensityTotal = 12201,

		/// <summary>
		/// 属性类型 int
		/// </summary>
		AttrHealIntensityAdd = 12202,

		/// <summary>
		/// 属性类型 int
		/// </summary>
		AttrHealIntensityExAdd = 12203,

		/// <summary>
		/// 属性类型 int
		/// </summary>
		AttrHealIntensityPer = 12204,

		/// <summary>
		/// 属性类型 int
		/// </summary>
		AttrHealIntensityExPer = 12205,

		/// <summary>
		/// 属性类型 int
		/// </summary>
		AttrHeal = 13010,

		/// <summary>
		/// 属性类型 int
		/// </summary>
		AttrHealTotal = 13011,

		/// <summary>
		/// 属性类型 int
		/// </summary>
		AttrHealAdd = 13012,

		/// <summary>
		/// 属性类型 int
		/// </summary>
		AttrHealExAdd = 13013,

		/// <summary>
		/// 属性类型 int
		/// </summary>
		AttrHealPer = 13014,

		/// <summary>
		/// 属性类型 int
		/// </summary>
		AttrHealExPer = 13015,

		/// <summary>
		/// 属性类型 int
		/// </summary>
		AttrHealed = 13020,

		/// <summary>
		/// 属性类型 int
		/// </summary>
		AttrHealedTotal = 13021,

		/// <summary>
		/// 属性类型 int
		/// </summary>
		AttrHealedAdd = 13022,

		/// <summary>
		/// 属性类型 int
		/// </summary>
		AttrHealedExAdd = 13023,

		/// <summary>
		/// 属性类型 int
		/// </summary>
		AttrHealedPer = 13024,

		/// <summary>
		/// 属性类型 int
		/// </summary>
		AttrHealedExPer = 13025,

		/// <summary>
		/// 属性类型 int
		/// </summary>
		AttrAttackSpeedPCT = 12500,

		/// <summary>
		/// 属性类型 int
		/// </summary>
		AttrAttackSpeedPCTTotal = 12501,

		/// <summary>
		/// 属性类型 int
		/// </summary>
		AttrAttackSpeedPCTAdd = 12502,

		/// <summary>
		/// 属性类型 int
		/// </summary>
		AttrAttackSpeedPCTExAdd = 12503,

		/// <summary>
		/// 属性类型 int
		/// </summary>
		AttrAttackSpeedPCTPer = 12504,

		/// <summary>
		/// 属性类型 int
		/// </summary>
		AttrAttackSpeedPCTExPer = 12505,

		/// <summary>
		/// 属性类型 int
		/// </summary>
		AttrSkillCDPCT = 12520,

		/// <summary>
		/// 属性类型 int
		/// </summary>
		AttrSkillCDPCTTotal = 12521,

		/// <summary>
		/// 属性类型 int
		/// </summary>
		AttrSkillCDPCTAdd = 12522,

		/// <summary>
		/// 属性类型 int
		/// </summary>
		AttrSkillCDPCTExAdd = 12523,

		/// <summary>
		/// 属性类型 int
		/// </summary>
		AttrSkillCDPCTPer = 12524,

		/// <summary>
		/// 属性类型 int
		/// </summary>
		AttrSkillCDPCTExPer = 12525,

		/// <summary>
		/// 属性类型 int
		/// </summary>
		AttrElementPenPCT = 14000,

		/// <summary>
		/// 属性类型 int
		/// </summary>
		AttrElementPenPCTTotal = 14001,

		/// <summary>
		/// 属性类型 int
		/// </summary>
		AttrElementPenPCTAdd = 14002,

		/// <summary>
		/// 属性类型 int
		/// </summary>
		AttrElementPenPCTExAdd = 14003,

		/// <summary>
		/// 属性类型 int
		/// </summary>
		AttrElementPenPCTPer = 14004,

		/// <summary>
		/// 属性类型 int
		/// </summary>
		AttrElementPenPCTExPer = 14005,

		/// <summary>
		/// 属性类型 int
		/// </summary>
		AttrElementRedPCT = 14010,

		/// <summary>
		/// 属性类型 int
		/// </summary>
		AttrElementRedPCTTotal = 14011,

		/// <summary>
		/// 属性类型 int
		/// </summary>
		AttrElementRedPCTAdd = 14012,

		/// <summary>
		/// 属性类型 int
		/// </summary>
		AttrElementRedPCTExAdd = 14013,

		/// <summary>
		/// 属性类型 int
		/// </summary>
		AttrElementRedPCTPer = 14014,

		/// <summary>
		/// 属性类型 int
		/// </summary>
		AttrElementRedPCTExPer = 14015,

		/// <summary>
		/// 属性类型 int
		/// </summary>
		AttrElementPhysicalPenPCT = 14020,

		/// <summary>
		/// 属性类型 int
		/// </summary>
		AttrElementPhysicalPenPCTTotal = 14021,

		/// <summary>
		/// 属性类型 int
		/// </summary>
		AttrElementPhysicalPenPCTAdd = 14022,

		/// <summary>
		/// 属性类型 int
		/// </summary>
		AttrElementPhysicalPenPCTExAdd = 14023,

		/// <summary>
		/// 属性类型 int
		/// </summary>
		AttrElementPhysicalPenPCTPer = 14024,

		/// <summary>
		/// 属性类型 int
		/// </summary>
		AttrElementPhysicalPenPCTExPer = 14025,

		/// <summary>
		/// 属性类型 int
		/// </summary>
		AttrElementPhysicalRedPCT = 14030,

		/// <summary>
		/// 属性类型 int
		/// </summary>
		AttrElementPhysicalRedPCTTotal = 14031,

		/// <summary>
		/// 属性类型 int
		/// </summary>
		AttrElementPhysicalRedPCTAdd = 14032,

		/// <summary>
		/// 属性类型 int
		/// </summary>
		AttrElementPhysicalRedPCTExAdd = 14033,

		/// <summary>
		/// 属性类型 int
		/// </summary>
		AttrElementPhysicalRedPCTPer = 14034,

		/// <summary>
		/// 属性类型 int
		/// </summary>
		AttrElementPhysicalRedPCTExPer = 14035,

		/// <summary>
		/// 属性类型 int
		/// </summary>
		AttrElementMagicPenPCT = 14040,

		/// <summary>
		/// 属性类型 int
		/// </summary>
		AttrElementMagicPenPCTTotal = 14041,

		/// <summary>
		/// 属性类型 int
		/// </summary>
		AttrElementMagicPenPCTAdd = 14042,

		/// <summary>
		/// 属性类型 int
		/// </summary>
		AttrElementMagicPenPCTExAdd = 14043,

		/// <summary>
		/// 属性类型 int
		/// </summary>
		AttrElementMagicPenPCTPer = 14044,

		/// <summary>
		/// 属性类型 int
		/// </summary>
		AttrElementMagicPenPCTExPer = 14045,

		/// <summary>
		/// 属性类型 int
		/// </summary>
		AttrElementMagicRedPCT = 14050,

		/// <summary>
		/// 属性类型 int
		/// </summary>
		AttrElementMagicRedPCTTotal = 14051,

		/// <summary>
		/// 属性类型 int
		/// </summary>
		AttrElementMagicRedPCTAdd = 14052,

		/// <summary>
		/// 属性类型 int
		/// </summary>
		AttrElementMagicRedPCTExAdd = 14053,

		/// <summary>
		/// 属性类型 int
		/// </summary>
		AttrElementMagicRedPCTPer = 14054,

		/// <summary>
		/// 属性类型 int
		/// </summary>
		AttrElementMagicRedPCTExPer = 14055,

		/// <summary>
		/// 属性类型 int
		/// </summary>
		AttrDmgA = 15000,

		/// <summary>
		/// 属性类型 int
		/// </summary>
		AttrDmgATotal = 15001,

		/// <summary>
		/// 属性类型 int
		/// </summary>
		AttrDmgAAdd = 15002,

		/// <summary>
		/// 属性类型 int
		/// </summary>
		AttrDmgAExAdd = 15003,

		/// <summary>
		/// 属性类型 int
		/// </summary>
		AttrDmgAPer = 15004,

		/// <summary>
		/// 属性类型 int
		/// </summary>
		AttrDmgAExPer = 15005,

		/// <summary>
		/// 属性类型 int
		/// </summary>
		AttrDmgRed = 15300,

		/// <summary>
		/// 属性类型 int
		/// </summary>
		AttrDmgRedTotal = 15301,

		/// <summary>
		/// 属性类型 int
		/// </summary>
		AttrDmgRedAdd = 15302,

		/// <summary>
		/// 属性类型 int
		/// </summary>
		AttrDmgRedExAdd = 15303,

		/// <summary>
		/// 属性类型 int
		/// </summary>
		AttrDmgRedPer = 15304,

		/// <summary>
		/// 属性类型 int
		/// </summary>
		AttrDmgRedExPer = 15305,

		/// <summary>
		/// 属性类型 int
		/// </summary>
		AttrDmgNear = 16000,

		/// <summary>
		/// 属性类型 int
		/// </summary>
		AttrDmgNearTotal = 16001,

		/// <summary>
		/// 属性类型 int
		/// </summary>
		AttrDmgNearAdd = 16002,

		/// <summary>
		/// 属性类型 int
		/// </summary>
		AttrDmgNearExAdd = 16003,

		/// <summary>
		/// 属性类型 int
		/// </summary>
		AttrDmgNearPer = 16004,

		/// <summary>
		/// 属性类型 int
		/// </summary>
		AttrDmgNearExPer = 16005,

		/// <summary>
		/// 属性类型 int
		/// </summary>
		AttrDmgRedNear = 16020,

		/// <summary>
		/// 属性类型 int
		/// </summary>
		AttrDmgRedNearTotal = 16021,

		/// <summary>
		/// 属性类型 int
		/// </summary>
		AttrDmgRedNearAdd = 16022,

		/// <summary>
		/// 属性类型 int
		/// </summary>
		AttrDmgRedNearExAdd = 16023,

		/// <summary>
		/// 属性类型 int
		/// </summary>
		AttrDmgRedNearPer = 16024,

		/// <summary>
		/// 属性类型 int
		/// </summary>
		AttrDmgRedNearExPer = 16025,

		/// <summary>
		/// 属性类型 int
		/// </summary>
		AttrDmgFar = 16100,

		/// <summary>
		/// 属性类型 int
		/// </summary>
		AttrDmgFarTotal = 16101,

		/// <summary>
		/// 属性类型 int
		/// </summary>
		AttrDmgFarAdd = 16102,

		/// <summary>
		/// 属性类型 int
		/// </summary>
		AttrDmgFarExAdd = 16103,

		/// <summary>
		/// 属性类型 int
		/// </summary>
		AttrDmgFarPer = 16104,

		/// <summary>
		/// 属性类型 int
		/// </summary>
		AttrDmgFarExPer = 16105,

		/// <summary>
		/// 属性类型 int
		/// </summary>
		AttrDmgRedFar = 16120,

		/// <summary>
		/// 属性类型 int
		/// </summary>
		AttrDmgRedFarTotal = 16121,

		/// <summary>
		/// 属性类型 int
		/// </summary>
		AttrDmgRedFarAdd = 16122,

		/// <summary>
		/// 属性类型 int
		/// </summary>
		AttrDmgRedFarExAdd = 16123,

		/// <summary>
		/// 属性类型 int
		/// </summary>
		AttrDmgRedFarPer = 16124,

		/// <summary>
		/// 属性类型 int
		/// </summary>
		AttrDmgRedFarExPer = 16125,

		/// <summary>
		/// 属性类型 int
		/// </summary>
		AttrDmgRedBullet = 16220,

		/// <summary>
		/// 属性类型 int
		/// </summary>
		AttrDmgRedBulletTotal = 16221,

		/// <summary>
		/// 属性类型 int
		/// </summary>
		AttrDmgRedBulletAdd = 16222,

		/// <summary>
		/// 属性类型 int
		/// </summary>
		AttrDmgRedBulletExAdd = 16223,

		/// <summary>
		/// 属性类型 int
		/// </summary>
		AttrDmgRedBulletPer = 16224,

		/// <summary>
		/// 属性类型 int
		/// </summary>
		AttrDmgRedBulletExPer = 16225,

		/// <summary>
		/// 属性类型 int
		/// </summary>
		AttrDmgRedMelee = 16320,

		/// <summary>
		/// 属性类型 int
		/// </summary>
		AttrDmgRedMeleeTotal = 16321,

		/// <summary>
		/// 属性类型 int
		/// </summary>
		AttrDmgRedMeleeAdd = 16322,

		/// <summary>
		/// 属性类型 int
		/// </summary>
		AttrDmgRedMeleeExAdd = 16323,

		/// <summary>
		/// 属性类型 int
		/// </summary>
		AttrDmgRedMeleePer = 16324,

		/// <summary>
		/// 属性类型 int
		/// </summary>
		AttrDmgRedMeleeExPer = 16325,

		/// <summary>
		/// 属性类型 int
		/// </summary>
		AttrDmgRedBuff = 16420,

		/// <summary>
		/// 属性类型 int
		/// </summary>
		AttrDmgRedBuffTotal = 16421,

		/// <summary>
		/// 属性类型 int
		/// </summary>
		AttrDmgRedBuffAdd = 16422,

		/// <summary>
		/// 属性类型 int
		/// </summary>
		AttrDmgRedBuffExAdd = 16423,

		/// <summary>
		/// 属性类型 int
		/// </summary>
		AttrDmgRedBuffPer = 16424,

		/// <summary>
		/// 属性类型 int
		/// </summary>
		AttrDmgRedBuffExPer = 16425,

		/// <summary>
		/// 属性类型 int
		/// </summary>
		AttrDmgRedVsNormalEnemy = 16720,

		/// <summary>
		/// 属性类型 int
		/// </summary>
		AttrDmgRedVsNormalEnemyTotal = 16721,

		/// <summary>
		/// 属性类型 int
		/// </summary>
		AttrDmgRedVsNormalEnemyAdd = 16722,

		/// <summary>
		/// 属性类型 int
		/// </summary>
		AttrDmgRedVsNormalEnemyExAdd = 16723,

		/// <summary>
		/// 属性类型 int
		/// </summary>
		AttrDmgRedVsNormalEnemyPer = 16724,

		/// <summary>
		/// 属性类型 int
		/// </summary>
		AttrDmgRedVsNormalEnemyExPer = 16725,

		/// <summary>
		/// 属性类型 int
		/// </summary>
		AttrDmgRedVsEliteEnemy = 16820,

		/// <summary>
		/// 属性类型 int
		/// </summary>
		AttrDmgRedVsEliteEnemyTotal = 16821,

		/// <summary>
		/// 属性类型 int
		/// </summary>
		AttrDmgRedVsEliteEnemyAdd = 16822,

		/// <summary>
		/// 属性类型 int
		/// </summary>
		AttrDmgRedVsEliteEnemyExAdd = 16823,

		/// <summary>
		/// 属性类型 int
		/// </summary>
		AttrDmgRedVsEliteEnemyPer = 16824,

		/// <summary>
		/// 属性类型 int
		/// </summary>
		AttrDmgRedVsEliteEnemyExPer = 16825,

		/// <summary>
		/// 属性类型 int
		/// </summary>
		AttrDmgRedHealthy = 17020,

		/// <summary>
		/// 属性类型 int
		/// </summary>
		AttrDmgRedHealthyTotal = 17021,

		/// <summary>
		/// 属性类型 int
		/// </summary>
		AttrDmgRedHealthyAdd = 17022,

		/// <summary>
		/// 属性类型 int
		/// </summary>
		AttrDmgRedHealthyExAdd = 17023,

		/// <summary>
		/// 属性类型 int
		/// </summary>
		AttrDmgRedHealthyPer = 17024,

		/// <summary>
		/// 属性类型 int
		/// </summary>
		AttrDmgRedHealthyExPer = 17025,

		/// <summary>
		/// 属性类型 int
		/// </summary>
		AttrDmgRedNonHealthy = 17120,

		/// <summary>
		/// 属性类型 int
		/// </summary>
		AttrDmgRedNonHealthyTotal = 17121,

		/// <summary>
		/// 属性类型 int
		/// </summary>
		AttrDmgRedNonHealthyAdd = 17122,

		/// <summary>
		/// 属性类型 int
		/// </summary>
		AttrDmgRedNonHealthyExAdd = 17123,

		/// <summary>
		/// 属性类型 int
		/// </summary>
		AttrDmgRedNonHealthyPer = 17124,

		/// <summary>
		/// 属性类型 int
		/// </summary>
		AttrDmgRedNonHealthyExPer = 17125,

		/// <summary>
		/// 属性类型 int
		/// </summary>
		AttrEntityLevel = 60010,
	}

	public static class AttrCreator {
		#region AttrTypeDict

		private static Dictionary<ELogicAttr, Type> _attrTypeDict = new Dictionary<ELogicAttr, Type>() {
			{ ELogicAttr.AttrUid, typeof(int) },
			{ ELogicAttr.AttrConfigId, typeof(int) },
			{ ELogicAttr.AttrModelId, typeof(int) },
			{ ELogicAttr.AttrIsSummoned, typeof(int) },
			{ ELogicAttr.AttrActorState, typeof(int) },
			{ ELogicAttr.AttrTopSourceActorUid, typeof(int) },
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
			{ ELogicAttr.AttrHateTargetUid, typeof(int) },
			{ ELogicAttr.AttrCantBeHatredTarget, typeof(int) },
			{ ELogicAttr.AttrCantNormalSkill, typeof(int) },
			{ ELogicAttr.AttrCantMove, typeof(int) },
			{ ELogicAttr.AttrStunned, typeof(int) },
			{ ELogicAttr.AttrSA, typeof(int) },
			{ ELogicAttr.AttrIgnoreOtherMotion, typeof(int) },
			{ ELogicAttr.AttrOriginPos, typeof(Vector3) },
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
			{ ELogicAttr.AttrMoveSpeedPCT, typeof(int) },
			{ ELogicAttr.AttrMoveSpeedPCTTotal, typeof(int) },
			{ ELogicAttr.AttrMoveSpeedPCTAdd, typeof(int) },
			{ ELogicAttr.AttrMoveSpeedPCTExAdd, typeof(int) },
			{ ELogicAttr.AttrMoveSpeedPCTPer, typeof(int) },
			{ ELogicAttr.AttrMoveSpeedPCTExPer, typeof(int) },
			{ ELogicAttr.AttrMpRecAll, typeof(int) },
			{ ELogicAttr.AttrMpRecAllTotal, typeof(int) },
			{ ELogicAttr.AttrMpRecAllAdd, typeof(int) },
			{ ELogicAttr.AttrMpRecAllExAdd, typeof(int) },
			{ ELogicAttr.AttrMpRecAllPer, typeof(int) },
			{ ELogicAttr.AttrMpRecAllExPer, typeof(int) },
			{ ELogicAttr.AttrMpRecCast, typeof(int) },
			{ ELogicAttr.AttrMpRecCastTotal, typeof(int) },
			{ ELogicAttr.AttrMpRecCastAdd, typeof(int) },
			{ ELogicAttr.AttrMpRecCastExAdd, typeof(int) },
			{ ELogicAttr.AttrMpRecCastPer, typeof(int) },
			{ ELogicAttr.AttrMpRecCastExPer, typeof(int) },
			{ ELogicAttr.AttrMpRecBehit, typeof(int) },
			{ ELogicAttr.AttrMpRecBehitTotal, typeof(int) },
			{ ELogicAttr.AttrMpRecBehitAdd, typeof(int) },
			{ ELogicAttr.AttrMpRecBehitExAdd, typeof(int) },
			{ ELogicAttr.AttrMpRecBehitPer, typeof(int) },
			{ ELogicAttr.AttrMpRecBehitExPer, typeof(int) },
			{ ELogicAttr.AttrMpRecKilled, typeof(int) },
			{ ELogicAttr.AttrMpRecKilledTotal, typeof(int) },
			{ ELogicAttr.AttrMpRecKilledAdd, typeof(int) },
			{ ELogicAttr.AttrMpRecKilledExAdd, typeof(int) },
			{ ELogicAttr.AttrMpRecKilledPer, typeof(int) },
			{ ELogicAttr.AttrMpRecKilledExPer, typeof(int) },
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
			{ ELogicAttr.AttrSkillCDPCT, typeof(int) },
			{ ELogicAttr.AttrSkillCDPCTTotal, typeof(int) },
			{ ELogicAttr.AttrSkillCDPCTAdd, typeof(int) },
			{ ELogicAttr.AttrSkillCDPCTExAdd, typeof(int) },
			{ ELogicAttr.AttrSkillCDPCTPer, typeof(int) },
			{ ELogicAttr.AttrSkillCDPCTExPer, typeof(int) },
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

		public static Type GetValueType(this ELogicAttr attrType) {
			if (_attrTypeDict.TryGetValue(attrType, out var type)) {
				return _attrTypeDict[attrType];
			}

			return null;
		}

		#endregion

		#region AttrCreate

		public static IAttr Create(int attrType) {
			switch ((ELogicAttr)attrType) {
				case ELogicAttr.AttrUid:
					return new Attr<int>((a, b) => a + b);
				case ELogicAttr.AttrConfigId:
					return new Attr<int>((a, b) => a + b);
				case ELogicAttr.AttrModelId:
					return new Attr<int>((a, b) => a + b);
				case ELogicAttr.AttrIsSummoned:
					return new Attr<int>((a, b) => a + b);
				case ELogicAttr.AttrActorState:
					return new Attr<int>((a, b) => a + b);
				case ELogicAttr.AttrTopSourceActorUid:
					return new Attr<int>((a, b) => a + b);
				case ELogicAttr.AttrSourceActorUid:
					return new Attr<int>((a, b) => a + b);
				case ELogicAttr.AttrSourceAbilityConfigId:
					return new Attr<int>((a, b) => a + b);
				case ELogicAttr.SourceAbilityType:
					return new Attr<int>((a, b) => a + b);
				case ELogicAttr.AttrRot:
					return new Attr<Quaternion>((a, b) => b * a);
				case ELogicAttr.AttrPosition:
					return new Attr<Vector3>((a, b) => a + b);
				case ELogicAttr.AttrBaseSpeed:
					return new Attr<float>((a, b) => a + b);
				case ELogicAttr.AttrAttackTargetUids:
					return new Attr<List<int>>(null);
				case ELogicAttr.AttrMoveTargetUid:
					return new Attr<int>((a, b) => a + b);
				case ELogicAttr.AttrFaction:
					return new Attr<int>((a, b) => a + b);
				case ELogicAttr.AttrUnselectable:
					return new Attr<int>((a, b) => a + b);
				case ELogicAttr.AttrInvincible:
					return new Attr<int>((a, b) => a + b);
				case ELogicAttr.AttrHateTargetUid:
					return new Attr<int>((a, b) => a + b);
				case ELogicAttr.AttrCantBeHatredTarget:
					return new Attr<int>((a, b) => a + b);
				case ELogicAttr.AttrCantNormalSkill:
					return new Attr<int>((a, b) => a + b);
				case ELogicAttr.AttrCantMove:
					return new Attr<int>((a, b) => a + b);
				case ELogicAttr.AttrStunned:
					return new Attr<int>((a, b) => a + b);
				case ELogicAttr.AttrSA:
					return new Attr<int>((a, b) => a + b);
				case ELogicAttr.AttrIgnoreOtherMotion:
					return new Attr<int>((a, b) => a + b);
				case ELogicAttr.AttrOriginPos:
					return new Attr<Vector3>((a, b) => a + b);
				case ELogicAttr.AttrHp:
					return new Attr<int>((a, b) => a + b);
				case ELogicAttr.AttrHpPer:
					return new Attr<int>((a, b) => a + b);
				case ELogicAttr.AttrMaxHp:
					return new Attr<int>((a, b) => a + b);
				case ELogicAttr.AttrMaxHpTotal:
					return new Attr<int>((a, b) => a + b);
				case ELogicAttr.AttrMaxHpAdd:
					return new Attr<int>((a, b) => a + b);
				case ELogicAttr.AttrMaxHpExAdd:
					return new Attr<int>((a, b) => a + b);
				case ELogicAttr.AttrMaxHpPer:
					return new Attr<int>((a, b) => a + b);
				case ELogicAttr.AttrMaxHpExPer:
					return new Attr<int>((a, b) => a + b);
				case ELogicAttr.AttrMp:
					return new Attr<int>((a, b) => a + b);
				case ELogicAttr.AttrMpPer:
					return new Attr<int>((a, b) => a + b);
				case ELogicAttr.AttrMaxMp:
					return new Attr<int>((a, b) => a + b);
				case ELogicAttr.AttrMaxMpTotal:
					return new Attr<int>((a, b) => a + b);
				case ELogicAttr.AttrMaxMpAdd:
					return new Attr<int>((a, b) => a + b);
				case ELogicAttr.AttrMaxMpExAdd:
					return new Attr<int>((a, b) => a + b);
				case ELogicAttr.AttrMaxMpPer:
					return new Attr<int>((a, b) => a + b);
				case ELogicAttr.AttrMaxMpExPer:
					return new Attr<int>((a, b) => a + b);
				case ELogicAttr.AttrShield:
					return new Attr<int>((a, b) => a + b);
				case ELogicAttr.AttrShieldPer:
					return new Attr<int>((a, b) => a + b);
				case ELogicAttr.AttrMoveSpeedPCT:
					return new Attr<int>((a, b) => a + b);
				case ELogicAttr.AttrMoveSpeedPCTTotal:
					return new Attr<int>((a, b) => a + b);
				case ELogicAttr.AttrMoveSpeedPCTAdd:
					return new Attr<int>((a, b) => a + b);
				case ELogicAttr.AttrMoveSpeedPCTExAdd:
					return new Attr<int>((a, b) => a + b);
				case ELogicAttr.AttrMoveSpeedPCTPer:
					return new Attr<int>((a, b) => a + b);
				case ELogicAttr.AttrMoveSpeedPCTExPer:
					return new Attr<int>((a, b) => a + b);
				case ELogicAttr.AttrMpRecAll:
					return new Attr<int>((a, b) => a + b);
				case ELogicAttr.AttrMpRecAllTotal:
					return new Attr<int>((a, b) => a + b);
				case ELogicAttr.AttrMpRecAllAdd:
					return new Attr<int>((a, b) => a + b);
				case ELogicAttr.AttrMpRecAllExAdd:
					return new Attr<int>((a, b) => a + b);
				case ELogicAttr.AttrMpRecAllPer:
					return new Attr<int>((a, b) => a + b);
				case ELogicAttr.AttrMpRecAllExPer:
					return new Attr<int>((a, b) => a + b);
				case ELogicAttr.AttrMpRecCast:
					return new Attr<int>((a, b) => a + b);
				case ELogicAttr.AttrMpRecCastTotal:
					return new Attr<int>((a, b) => a + b);
				case ELogicAttr.AttrMpRecCastAdd:
					return new Attr<int>((a, b) => a + b);
				case ELogicAttr.AttrMpRecCastExAdd:
					return new Attr<int>((a, b) => a + b);
				case ELogicAttr.AttrMpRecCastPer:
					return new Attr<int>((a, b) => a + b);
				case ELogicAttr.AttrMpRecCastExPer:
					return new Attr<int>((a, b) => a + b);
				case ELogicAttr.AttrMpRecBehit:
					return new Attr<int>((a, b) => a + b);
				case ELogicAttr.AttrMpRecBehitTotal:
					return new Attr<int>((a, b) => a + b);
				case ELogicAttr.AttrMpRecBehitAdd:
					return new Attr<int>((a, b) => a + b);
				case ELogicAttr.AttrMpRecBehitExAdd:
					return new Attr<int>((a, b) => a + b);
				case ELogicAttr.AttrMpRecBehitPer:
					return new Attr<int>((a, b) => a + b);
				case ELogicAttr.AttrMpRecBehitExPer:
					return new Attr<int>((a, b) => a + b);
				case ELogicAttr.AttrMpRecKilled:
					return new Attr<int>((a, b) => a + b);
				case ELogicAttr.AttrMpRecKilledTotal:
					return new Attr<int>((a, b) => a + b);
				case ELogicAttr.AttrMpRecKilledAdd:
					return new Attr<int>((a, b) => a + b);
				case ELogicAttr.AttrMpRecKilledExAdd:
					return new Attr<int>((a, b) => a + b);
				case ELogicAttr.AttrMpRecKilledPer:
					return new Attr<int>((a, b) => a + b);
				case ELogicAttr.AttrMpRecKilledExPer:
					return new Attr<int>((a, b) => a + b);
				case ELogicAttr.AttrAttack:
					return new Attr<int>((a, b) => a + b);
				case ELogicAttr.AttrAttackTotal:
					return new Attr<int>((a, b) => a + b);
				case ELogicAttr.AttrAttackAdd:
					return new Attr<int>((a, b) => a + b);
				case ELogicAttr.AttrAttackExAdd:
					return new Attr<int>((a, b) => a + b);
				case ELogicAttr.AttrAttackPer:
					return new Attr<int>((a, b) => a + b);
				case ELogicAttr.AttrAttackExPer:
					return new Attr<int>((a, b) => a + b);
				case ELogicAttr.AttrDefense:
					return new Attr<int>((a, b) => a + b);
				case ELogicAttr.AttrDefenseTotal:
					return new Attr<int>((a, b) => a + b);
				case ELogicAttr.AttrDefenseAdd:
					return new Attr<int>((a, b) => a + b);
				case ELogicAttr.AttrDefenseExAdd:
					return new Attr<int>((a, b) => a + b);
				case ELogicAttr.AttrDefensePer:
					return new Attr<int>((a, b) => a + b);
				case ELogicAttr.AttrDefenseExPer:
					return new Attr<int>((a, b) => a + b);
				case ELogicAttr.AttrIgnoreDefense:
					return new Attr<int>((a, b) => a + b);
				case ELogicAttr.AttrIgnoreDefenseTotal:
					return new Attr<int>((a, b) => a + b);
				case ELogicAttr.AttrIgnoreDefenseAdd:
					return new Attr<int>((a, b) => a + b);
				case ELogicAttr.AttrIgnoreDefenseExAdd:
					return new Attr<int>((a, b) => a + b);
				case ELogicAttr.AttrIgnoreDefensePer:
					return new Attr<int>((a, b) => a + b);
				case ELogicAttr.AttrIgnoreDefenseExPer:
					return new Attr<int>((a, b) => a + b);
				case ELogicAttr.AttrIgnoreDefensePCT:
					return new Attr<int>((a, b) => a + b);
				case ELogicAttr.AttrIgnoreDefensePCTTotal:
					return new Attr<int>((a, b) => a + b);
				case ELogicAttr.AttrIgnoreDefensePCTAdd:
					return new Attr<int>((a, b) => a + b);
				case ELogicAttr.AttrIgnoreDefensePCTExAdd:
					return new Attr<int>((a, b) => a + b);
				case ELogicAttr.AttrIgnoreDefensePCTPer:
					return new Attr<int>((a, b) => a + b);
				case ELogicAttr.AttrIgnoreDefensePCTExPer:
					return new Attr<int>((a, b) => a + b);
				case ELogicAttr.AttrCrit:
					return new Attr<int>((a, b) => a + b);
				case ELogicAttr.AttrCritTotal:
					return new Attr<int>((a, b) => a + b);
				case ELogicAttr.AttrCritAdd:
					return new Attr<int>((a, b) => a + b);
				case ELogicAttr.AttrCritExAdd:
					return new Attr<int>((a, b) => a + b);
				case ELogicAttr.AttrCritPer:
					return new Attr<int>((a, b) => a + b);
				case ELogicAttr.AttrCritExPer:
					return new Attr<int>((a, b) => a + b);
				case ELogicAttr.AttrCritDamage:
					return new Attr<int>((a, b) => a + b);
				case ELogicAttr.AttrCritDamageTotal:
					return new Attr<int>((a, b) => a + b);
				case ELogicAttr.AttrCritDamageAdd:
					return new Attr<int>((a, b) => a + b);
				case ELogicAttr.AttrCritDamageExAdd:
					return new Attr<int>((a, b) => a + b);
				case ELogicAttr.AttrCritDamagePer:
					return new Attr<int>((a, b) => a + b);
				case ELogicAttr.AttrCritDamageExPer:
					return new Attr<int>((a, b) => a + b);
				case ELogicAttr.AttrHealIntensity:
					return new Attr<int>((a, b) => a + b);
				case ELogicAttr.AttrHealIntensityTotal:
					return new Attr<int>((a, b) => a + b);
				case ELogicAttr.AttrHealIntensityAdd:
					return new Attr<int>((a, b) => a + b);
				case ELogicAttr.AttrHealIntensityExAdd:
					return new Attr<int>((a, b) => a + b);
				case ELogicAttr.AttrHealIntensityPer:
					return new Attr<int>((a, b) => a + b);
				case ELogicAttr.AttrHealIntensityExPer:
					return new Attr<int>((a, b) => a + b);
				case ELogicAttr.AttrHeal:
					return new Attr<int>((a, b) => a + b);
				case ELogicAttr.AttrHealTotal:
					return new Attr<int>((a, b) => a + b);
				case ELogicAttr.AttrHealAdd:
					return new Attr<int>((a, b) => a + b);
				case ELogicAttr.AttrHealExAdd:
					return new Attr<int>((a, b) => a + b);
				case ELogicAttr.AttrHealPer:
					return new Attr<int>((a, b) => a + b);
				case ELogicAttr.AttrHealExPer:
					return new Attr<int>((a, b) => a + b);
				case ELogicAttr.AttrHealed:
					return new Attr<int>((a, b) => a + b);
				case ELogicAttr.AttrHealedTotal:
					return new Attr<int>((a, b) => a + b);
				case ELogicAttr.AttrHealedAdd:
					return new Attr<int>((a, b) => a + b);
				case ELogicAttr.AttrHealedExAdd:
					return new Attr<int>((a, b) => a + b);
				case ELogicAttr.AttrHealedPer:
					return new Attr<int>((a, b) => a + b);
				case ELogicAttr.AttrHealedExPer:
					return new Attr<int>((a, b) => a + b);
				case ELogicAttr.AttrAttackSpeedPCT:
					return new Attr<int>((a, b) => a + b);
				case ELogicAttr.AttrAttackSpeedPCTTotal:
					return new Attr<int>((a, b) => a + b);
				case ELogicAttr.AttrAttackSpeedPCTAdd:
					return new Attr<int>((a, b) => a + b);
				case ELogicAttr.AttrAttackSpeedPCTExAdd:
					return new Attr<int>((a, b) => a + b);
				case ELogicAttr.AttrAttackSpeedPCTPer:
					return new Attr<int>((a, b) => a + b);
				case ELogicAttr.AttrAttackSpeedPCTExPer:
					return new Attr<int>((a, b) => a + b);
				case ELogicAttr.AttrSkillCDPCT:
					return new Attr<int>((a, b) => a + b);
				case ELogicAttr.AttrSkillCDPCTTotal:
					return new Attr<int>((a, b) => a + b);
				case ELogicAttr.AttrSkillCDPCTAdd:
					return new Attr<int>((a, b) => a + b);
				case ELogicAttr.AttrSkillCDPCTExAdd:
					return new Attr<int>((a, b) => a + b);
				case ELogicAttr.AttrSkillCDPCTPer:
					return new Attr<int>((a, b) => a + b);
				case ELogicAttr.AttrSkillCDPCTExPer:
					return new Attr<int>((a, b) => a + b);
				case ELogicAttr.AttrElementPenPCT:
					return new Attr<int>((a, b) => a + b);
				case ELogicAttr.AttrElementPenPCTTotal:
					return new Attr<int>((a, b) => a + b);
				case ELogicAttr.AttrElementPenPCTAdd:
					return new Attr<int>((a, b) => a + b);
				case ELogicAttr.AttrElementPenPCTExAdd:
					return new Attr<int>((a, b) => a + b);
				case ELogicAttr.AttrElementPenPCTPer:
					return new Attr<int>((a, b) => a + b);
				case ELogicAttr.AttrElementPenPCTExPer:
					return new Attr<int>((a, b) => a + b);
				case ELogicAttr.AttrElementRedPCT:
					return new Attr<int>((a, b) => a + b);
				case ELogicAttr.AttrElementRedPCTTotal:
					return new Attr<int>((a, b) => a + b);
				case ELogicAttr.AttrElementRedPCTAdd:
					return new Attr<int>((a, b) => a + b);
				case ELogicAttr.AttrElementRedPCTExAdd:
					return new Attr<int>((a, b) => a + b);
				case ELogicAttr.AttrElementRedPCTPer:
					return new Attr<int>((a, b) => a + b);
				case ELogicAttr.AttrElementRedPCTExPer:
					return new Attr<int>((a, b) => a + b);
				case ELogicAttr.AttrElementPhysicalPenPCT:
					return new Attr<int>((a, b) => a + b);
				case ELogicAttr.AttrElementPhysicalPenPCTTotal:
					return new Attr<int>((a, b) => a + b);
				case ELogicAttr.AttrElementPhysicalPenPCTAdd:
					return new Attr<int>((a, b) => a + b);
				case ELogicAttr.AttrElementPhysicalPenPCTExAdd:
					return new Attr<int>((a, b) => a + b);
				case ELogicAttr.AttrElementPhysicalPenPCTPer:
					return new Attr<int>((a, b) => a + b);
				case ELogicAttr.AttrElementPhysicalPenPCTExPer:
					return new Attr<int>((a, b) => a + b);
				case ELogicAttr.AttrElementPhysicalRedPCT:
					return new Attr<int>((a, b) => a + b);
				case ELogicAttr.AttrElementPhysicalRedPCTTotal:
					return new Attr<int>((a, b) => a + b);
				case ELogicAttr.AttrElementPhysicalRedPCTAdd:
					return new Attr<int>((a, b) => a + b);
				case ELogicAttr.AttrElementPhysicalRedPCTExAdd:
					return new Attr<int>((a, b) => a + b);
				case ELogicAttr.AttrElementPhysicalRedPCTPer:
					return new Attr<int>((a, b) => a + b);
				case ELogicAttr.AttrElementPhysicalRedPCTExPer:
					return new Attr<int>((a, b) => a + b);
				case ELogicAttr.AttrElementMagicPenPCT:
					return new Attr<int>((a, b) => a + b);
				case ELogicAttr.AttrElementMagicPenPCTTotal:
					return new Attr<int>((a, b) => a + b);
				case ELogicAttr.AttrElementMagicPenPCTAdd:
					return new Attr<int>((a, b) => a + b);
				case ELogicAttr.AttrElementMagicPenPCTExAdd:
					return new Attr<int>((a, b) => a + b);
				case ELogicAttr.AttrElementMagicPenPCTPer:
					return new Attr<int>((a, b) => a + b);
				case ELogicAttr.AttrElementMagicPenPCTExPer:
					return new Attr<int>((a, b) => a + b);
				case ELogicAttr.AttrElementMagicRedPCT:
					return new Attr<int>((a, b) => a + b);
				case ELogicAttr.AttrElementMagicRedPCTTotal:
					return new Attr<int>((a, b) => a + b);
				case ELogicAttr.AttrElementMagicRedPCTAdd:
					return new Attr<int>((a, b) => a + b);
				case ELogicAttr.AttrElementMagicRedPCTExAdd:
					return new Attr<int>((a, b) => a + b);
				case ELogicAttr.AttrElementMagicRedPCTPer:
					return new Attr<int>((a, b) => a + b);
				case ELogicAttr.AttrElementMagicRedPCTExPer:
					return new Attr<int>((a, b) => a + b);
				case ELogicAttr.AttrDmgA:
					return new Attr<int>((a, b) => a + b);
				case ELogicAttr.AttrDmgATotal:
					return new Attr<int>((a, b) => a + b);
				case ELogicAttr.AttrDmgAAdd:
					return new Attr<int>((a, b) => a + b);
				case ELogicAttr.AttrDmgAExAdd:
					return new Attr<int>((a, b) => a + b);
				case ELogicAttr.AttrDmgAPer:
					return new Attr<int>((a, b) => a + b);
				case ELogicAttr.AttrDmgAExPer:
					return new Attr<int>((a, b) => a + b);
				case ELogicAttr.AttrDmgRed:
					return new Attr<int>((a, b) => a + b);
				case ELogicAttr.AttrDmgRedTotal:
					return new Attr<int>((a, b) => a + b);
				case ELogicAttr.AttrDmgRedAdd:
					return new Attr<int>((a, b) => a + b);
				case ELogicAttr.AttrDmgRedExAdd:
					return new Attr<int>((a, b) => a + b);
				case ELogicAttr.AttrDmgRedPer:
					return new Attr<int>((a, b) => a + b);
				case ELogicAttr.AttrDmgRedExPer:
					return new Attr<int>((a, b) => a + b);
				case ELogicAttr.AttrDmgNear:
					return new Attr<int>((a, b) => a + b);
				case ELogicAttr.AttrDmgNearTotal:
					return new Attr<int>((a, b) => a + b);
				case ELogicAttr.AttrDmgNearAdd:
					return new Attr<int>((a, b) => a + b);
				case ELogicAttr.AttrDmgNearExAdd:
					return new Attr<int>((a, b) => a + b);
				case ELogicAttr.AttrDmgNearPer:
					return new Attr<int>((a, b) => a + b);
				case ELogicAttr.AttrDmgNearExPer:
					return new Attr<int>((a, b) => a + b);
				case ELogicAttr.AttrDmgRedNear:
					return new Attr<int>((a, b) => a + b);
				case ELogicAttr.AttrDmgRedNearTotal:
					return new Attr<int>((a, b) => a + b);
				case ELogicAttr.AttrDmgRedNearAdd:
					return new Attr<int>((a, b) => a + b);
				case ELogicAttr.AttrDmgRedNearExAdd:
					return new Attr<int>((a, b) => a + b);
				case ELogicAttr.AttrDmgRedNearPer:
					return new Attr<int>((a, b) => a + b);
				case ELogicAttr.AttrDmgRedNearExPer:
					return new Attr<int>((a, b) => a + b);
				case ELogicAttr.AttrDmgFar:
					return new Attr<int>((a, b) => a + b);
				case ELogicAttr.AttrDmgFarTotal:
					return new Attr<int>((a, b) => a + b);
				case ELogicAttr.AttrDmgFarAdd:
					return new Attr<int>((a, b) => a + b);
				case ELogicAttr.AttrDmgFarExAdd:
					return new Attr<int>((a, b) => a + b);
				case ELogicAttr.AttrDmgFarPer:
					return new Attr<int>((a, b) => a + b);
				case ELogicAttr.AttrDmgFarExPer:
					return new Attr<int>((a, b) => a + b);
				case ELogicAttr.AttrDmgRedFar:
					return new Attr<int>((a, b) => a + b);
				case ELogicAttr.AttrDmgRedFarTotal:
					return new Attr<int>((a, b) => a + b);
				case ELogicAttr.AttrDmgRedFarAdd:
					return new Attr<int>((a, b) => a + b);
				case ELogicAttr.AttrDmgRedFarExAdd:
					return new Attr<int>((a, b) => a + b);
				case ELogicAttr.AttrDmgRedFarPer:
					return new Attr<int>((a, b) => a + b);
				case ELogicAttr.AttrDmgRedFarExPer:
					return new Attr<int>((a, b) => a + b);
				case ELogicAttr.AttrDmgRedBullet:
					return new Attr<int>((a, b) => a + b);
				case ELogicAttr.AttrDmgRedBulletTotal:
					return new Attr<int>((a, b) => a + b);
				case ELogicAttr.AttrDmgRedBulletAdd:
					return new Attr<int>((a, b) => a + b);
				case ELogicAttr.AttrDmgRedBulletExAdd:
					return new Attr<int>((a, b) => a + b);
				case ELogicAttr.AttrDmgRedBulletPer:
					return new Attr<int>((a, b) => a + b);
				case ELogicAttr.AttrDmgRedBulletExPer:
					return new Attr<int>((a, b) => a + b);
				case ELogicAttr.AttrDmgRedMelee:
					return new Attr<int>((a, b) => a + b);
				case ELogicAttr.AttrDmgRedMeleeTotal:
					return new Attr<int>((a, b) => a + b);
				case ELogicAttr.AttrDmgRedMeleeAdd:
					return new Attr<int>((a, b) => a + b);
				case ELogicAttr.AttrDmgRedMeleeExAdd:
					return new Attr<int>((a, b) => a + b);
				case ELogicAttr.AttrDmgRedMeleePer:
					return new Attr<int>((a, b) => a + b);
				case ELogicAttr.AttrDmgRedMeleeExPer:
					return new Attr<int>((a, b) => a + b);
				case ELogicAttr.AttrDmgRedBuff:
					return new Attr<int>((a, b) => a + b);
				case ELogicAttr.AttrDmgRedBuffTotal:
					return new Attr<int>((a, b) => a + b);
				case ELogicAttr.AttrDmgRedBuffAdd:
					return new Attr<int>((a, b) => a + b);
				case ELogicAttr.AttrDmgRedBuffExAdd:
					return new Attr<int>((a, b) => a + b);
				case ELogicAttr.AttrDmgRedBuffPer:
					return new Attr<int>((a, b) => a + b);
				case ELogicAttr.AttrDmgRedBuffExPer:
					return new Attr<int>((a, b) => a + b);
				case ELogicAttr.AttrDmgRedVsNormalEnemy:
					return new Attr<int>((a, b) => a + b);
				case ELogicAttr.AttrDmgRedVsNormalEnemyTotal:
					return new Attr<int>((a, b) => a + b);
				case ELogicAttr.AttrDmgRedVsNormalEnemyAdd:
					return new Attr<int>((a, b) => a + b);
				case ELogicAttr.AttrDmgRedVsNormalEnemyExAdd:
					return new Attr<int>((a, b) => a + b);
				case ELogicAttr.AttrDmgRedVsNormalEnemyPer:
					return new Attr<int>((a, b) => a + b);
				case ELogicAttr.AttrDmgRedVsNormalEnemyExPer:
					return new Attr<int>((a, b) => a + b);
				case ELogicAttr.AttrDmgRedVsEliteEnemy:
					return new Attr<int>((a, b) => a + b);
				case ELogicAttr.AttrDmgRedVsEliteEnemyTotal:
					return new Attr<int>((a, b) => a + b);
				case ELogicAttr.AttrDmgRedVsEliteEnemyAdd:
					return new Attr<int>((a, b) => a + b);
				case ELogicAttr.AttrDmgRedVsEliteEnemyExAdd:
					return new Attr<int>((a, b) => a + b);
				case ELogicAttr.AttrDmgRedVsEliteEnemyPer:
					return new Attr<int>((a, b) => a + b);
				case ELogicAttr.AttrDmgRedVsEliteEnemyExPer:
					return new Attr<int>((a, b) => a + b);
				case ELogicAttr.AttrDmgRedHealthy:
					return new Attr<int>((a, b) => a + b);
				case ELogicAttr.AttrDmgRedHealthyTotal:
					return new Attr<int>((a, b) => a + b);
				case ELogicAttr.AttrDmgRedHealthyAdd:
					return new Attr<int>((a, b) => a + b);
				case ELogicAttr.AttrDmgRedHealthyExAdd:
					return new Attr<int>((a, b) => a + b);
				case ELogicAttr.AttrDmgRedHealthyPer:
					return new Attr<int>((a, b) => a + b);
				case ELogicAttr.AttrDmgRedHealthyExPer:
					return new Attr<int>((a, b) => a + b);
				case ELogicAttr.AttrDmgRedNonHealthy:
					return new Attr<int>((a, b) => a + b);
				case ELogicAttr.AttrDmgRedNonHealthyTotal:
					return new Attr<int>((a, b) => a + b);
				case ELogicAttr.AttrDmgRedNonHealthyAdd:
					return new Attr<int>((a, b) => a + b);
				case ELogicAttr.AttrDmgRedNonHealthyExAdd:
					return new Attr<int>((a, b) => a + b);
				case ELogicAttr.AttrDmgRedNonHealthyPer:
					return new Attr<int>((a, b) => a + b);
				case ELogicAttr.AttrDmgRedNonHealthyExPer:
					return new Attr<int>((a, b) => a + b);
				case ELogicAttr.AttrEntityLevel:
					return new Attr<int>((a, b) => a + b);
			}

			return null;
		}

		#endregion
	}
}