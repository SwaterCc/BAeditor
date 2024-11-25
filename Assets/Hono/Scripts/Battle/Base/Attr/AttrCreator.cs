#region

#endregion

namespace Hono.Scripts.Battle
{
//Auto::AttrMaker
    public enum EAttrType
    {
        /// <summary>
        ///     属性类型 int
        /// </summary>
        AttrUid = 1,

        /// <summary>
        ///     属性类型 int
        /// </summary>
        AttrConfigId = 2,

        /// <summary>
        ///     属性类型 int
        /// </summary>
        AttrModelId = 3,

        /// <summary>
        ///     属性类型 int
        /// </summary>
        AttrIsSummoned = 4,

        /// <summary>
        ///     属性类型 int
        /// </summary>
        AttrActorState = 5,

        /// <summary>
        ///     属性类型 int
        /// </summary>
        AttrTopSourceActorUid = 10,

        /// <summary>
        ///     属性类型 int
        /// </summary>
        AttrSourceActorUid = 11,

        /// <summary>
        ///     属性类型 int
        /// </summary>
        AttrSourceAbilityId = 12,

        /// <summary>
        ///     属性类型 int
        /// </summary>
        SourceAbilityType = 13,

        /// <summary>
        ///     属性类型 Quaternion
        /// </summary>
        AttrRot = 14,

        /// <summary>
        ///     属性类型 Vector3
        /// </summary>
        AttrPosition = 15,

        /// <summary>
        ///     属性类型 float
        /// </summary>
        AttrBaseSpeed = 16,

        /// <summary>
        ///     属性类型 IntArray
        /// </summary>
        AttrAttackTargetUids = 17,

        /// <summary>
        ///     属性类型 int
        /// </summary>
        AttrMoveTargetUid = 18,

        /// <summary>
        ///     属性类型 int
        /// </summary>
        AttrFaction = 19,

        /// <summary>
        ///     属性类型 int
        /// </summary>
        AttrUnselectable = 20,

        /// <summary>
        ///     属性类型 int
        /// </summary>
        AttrInvincible = 21,

        /// <summary>
        ///     属性类型 int
        /// </summary>
        AttrHateTargetUid = 22,

        /// <summary>
        ///     属性类型 int
        /// </summary>
        AttrCantBeHatredTarget = 47,

        /// <summary>
        ///     属性类型 int
        /// </summary>
        AttrCantNormalSkill = 48,

        /// <summary>
        ///     属性类型 int
        /// </summary>
        AttrCantMove = 49,

        /// <summary>
        ///     属性类型 int
        /// </summary>
        AttrStunned = 50,

        /// <summary>
        ///     属性类型 int
        /// </summary>
        AttrSA = 51,

        /// <summary>
        ///     属性类型 int
        /// </summary>
        AttrIgnoreOtherMotion = 52,

        /// <summary>
        ///     属性类型 int
        /// </summary>
        AttrHp = 10000,

        /// <summary>
        ///     属性类型 int
        /// </summary>
        AttrHpPer = 10001,

        /// <summary>
        ///     属性类型 int
        /// </summary>
        AttrMaxHp = 10010,

        /// <summary>
        ///     属性类型 int
        /// </summary>
        AttrMaxHpTotal = 10011,

        /// <summary>
        ///     属性类型 int
        /// </summary>
        AttrMaxHpAdd = 10012,

        /// <summary>
        ///     属性类型 int
        /// </summary>
        AttrMaxHpExAdd = 10013,

        /// <summary>
        ///     属性类型 int
        /// </summary>
        AttrMaxHpPer = 10014,

        /// <summary>
        ///     属性类型 int
        /// </summary>
        AttrMaxHpExPer = 10015,

        /// <summary>
        ///     属性类型 int
        /// </summary>
        AttrMp = 10020,

        /// <summary>
        ///     属性类型 int
        /// </summary>
        AttrMpPer = 10021,

        /// <summary>
        ///     属性类型 int
        /// </summary>
        AttrMaxMp = 10030,

        /// <summary>
        ///     属性类型 int
        /// </summary>
        AttrMaxMpTotal = 10031,

        /// <summary>
        ///     属性类型 int
        /// </summary>
        AttrMaxMpAdd = 10032,

        /// <summary>
        ///     属性类型 int
        /// </summary>
        AttrMaxMpExAdd = 10033,

        /// <summary>
        ///     属性类型 int
        /// </summary>
        AttrMaxMpPer = 10034,

        /// <summary>
        ///     属性类型 int
        /// </summary>
        AttrMaxMpExPer = 10035,

        /// <summary>
        ///     属性类型 int
        /// </summary>
        AttrShield = 10040,

        /// <summary>
        ///     属性类型 int
        /// </summary>
        AttrShieldPer = 10041,

        /// <summary>
        ///     属性类型 int
        /// </summary>
        AttrMoveSpeedPCT = 10100,

        /// <summary>
        ///     属性类型 int
        /// </summary>
        AttrMoveSpeedPCTTotal = 10101,

        /// <summary>
        ///     属性类型 int
        /// </summary>
        AttrMoveSpeedPCTAdd = 10102,

        /// <summary>
        ///     属性类型 int
        /// </summary>
        AttrMoveSpeedPCTExAdd = 10103,

        /// <summary>
        ///     属性类型 int
        /// </summary>
        AttrMoveSpeedPCTPer = 10104,

        /// <summary>
        ///     属性类型 int
        /// </summary>
        AttrMoveSpeedPCTExPer = 10105,

        /// <summary>
        ///     属性类型 int
        /// </summary>
        AttrMpRecAll = 10110,

        /// <summary>
        ///     属性类型 int
        /// </summary>
        AttrMpRecAllTotal = 10111,

        /// <summary>
        ///     属性类型 int
        /// </summary>
        AttrMpRecAllAdd = 10112,

        /// <summary>
        ///     属性类型 int
        /// </summary>
        AttrMpRecAllExAdd = 10113,

        /// <summary>
        ///     属性类型 int
        /// </summary>
        AttrMpRecAllPer = 10114,

        /// <summary>
        ///     属性类型 int
        /// </summary>
        AttrMpRecAllExPer = 10115,

        /// <summary>
        ///     属性类型 int
        /// </summary>
        AttrMpRecCast = 10120,

        /// <summary>
        ///     属性类型 int
        /// </summary>
        AttrMpRecCastTotal = 10121,

        /// <summary>
        ///     属性类型 int
        /// </summary>
        AttrMpRecCastAdd = 10122,

        /// <summary>
        ///     属性类型 int
        /// </summary>
        AttrMpRecCastExAdd = 10123,

        /// <summary>
        ///     属性类型 int
        /// </summary>
        AttrMpRecCastPer = 10124,

        /// <summary>
        ///     属性类型 int
        /// </summary>
        AttrMpRecCastExPer = 10125,

        /// <summary>
        ///     属性类型 int
        /// </summary>
        AttrMpRecBehit = 10130,

        /// <summary>
        ///     属性类型 int
        /// </summary>
        AttrMpRecBehitTotal = 10131,

        /// <summary>
        ///     属性类型 int
        /// </summary>
        AttrMpRecBehitAdd = 10132,

        /// <summary>
        ///     属性类型 int
        /// </summary>
        AttrMpRecBehitExAdd = 10133,

        /// <summary>
        ///     属性类型 int
        /// </summary>
        AttrMpRecBehitPer = 10134,

        /// <summary>
        ///     属性类型 int
        /// </summary>
        AttrMpRecBehitExPer = 10135,

        /// <summary>
        ///     属性类型 int
        /// </summary>
        AttrMpRecKilled = 10140,

        /// <summary>
        ///     属性类型 int
        /// </summary>
        AttrMpRecKilledTotal = 10141,

        /// <summary>
        ///     属性类型 int
        /// </summary>
        AttrMpRecKilledAdd = 10142,

        /// <summary>
        ///     属性类型 int
        /// </summary>
        AttrMpRecKilledExAdd = 10143,

        /// <summary>
        ///     属性类型 int
        /// </summary>
        AttrMpRecKilledPer = 10144,

        /// <summary>
        ///     属性类型 int
        /// </summary>
        AttrMpRecKilledExPer = 10145,

        /// <summary>
        ///     属性类型 int
        /// </summary>
        AttrAttack = 12100,

        /// <summary>
        ///     属性类型 int
        /// </summary>
        AttrAttackTotal = 12101,

        /// <summary>
        ///     属性类型 int
        /// </summary>
        AttrAttackAdd = 12102,

        /// <summary>
        ///     属性类型 int
        /// </summary>
        AttrAttackExAdd = 12103,

        /// <summary>
        ///     属性类型 int
        /// </summary>
        AttrAttackPer = 12104,

        /// <summary>
        ///     属性类型 int
        /// </summary>
        AttrAttackExPer = 12105,

        /// <summary>
        ///     属性类型 int
        /// </summary>
        AttrDefense = 12110,

        /// <summary>
        ///     属性类型 int
        /// </summary>
        AttrDefenseTotal = 12111,

        /// <summary>
        ///     属性类型 int
        /// </summary>
        AttrDefenseAdd = 12112,

        /// <summary>
        ///     属性类型 int
        /// </summary>
        AttrDefenseExAdd = 12113,

        /// <summary>
        ///     属性类型 int
        /// </summary>
        AttrDefensePer = 12114,

        /// <summary>
        ///     属性类型 int
        /// </summary>
        AttrDefenseExPer = 12115,

        /// <summary>
        ///     属性类型 int
        /// </summary>
        AttrIgnoreDefense = 12120,

        /// <summary>
        ///     属性类型 int
        /// </summary>
        AttrIgnoreDefenseTotal = 12121,

        /// <summary>
        ///     属性类型 int
        /// </summary>
        AttrIgnoreDefenseAdd = 12122,

        /// <summary>
        ///     属性类型 int
        /// </summary>
        AttrIgnoreDefenseExAdd = 12123,

        /// <summary>
        ///     属性类型 int
        /// </summary>
        AttrIgnoreDefensePer = 12124,

        /// <summary>
        ///     属性类型 int
        /// </summary>
        AttrIgnoreDefenseExPer = 12125,

        /// <summary>
        ///     属性类型 int
        /// </summary>
        AttrIgnoreDefensePCT = 12130,

        /// <summary>
        ///     属性类型 int
        /// </summary>
        AttrIgnoreDefensePCTTotal = 12131,

        /// <summary>
        ///     属性类型 int
        /// </summary>
        AttrIgnoreDefensePCTAdd = 12132,

        /// <summary>
        ///     属性类型 int
        /// </summary>
        AttrIgnoreDefensePCTExAdd = 12133,

        /// <summary>
        ///     属性类型 int
        /// </summary>
        AttrIgnoreDefensePCTPer = 12134,

        /// <summary>
        ///     属性类型 int
        /// </summary>
        AttrIgnoreDefensePCTExPer = 12135,

        /// <summary>
        ///     属性类型 int
        /// </summary>
        AttrCrit = 12140,

        /// <summary>
        ///     属性类型 int
        /// </summary>
        AttrCritTotal = 12141,

        /// <summary>
        ///     属性类型 int
        /// </summary>
        AttrCritAdd = 12142,

        /// <summary>
        ///     属性类型 int
        /// </summary>
        AttrCritExAdd = 12143,

        /// <summary>
        ///     属性类型 int
        /// </summary>
        AttrCritPer = 12144,

        /// <summary>
        ///     属性类型 int
        /// </summary>
        AttrCritExPer = 12145,

        /// <summary>
        ///     属性类型 int
        /// </summary>
        AttrCritDamage = 12150,

        /// <summary>
        ///     属性类型 int
        /// </summary>
        AttrCritDamageTotal = 12151,

        /// <summary>
        ///     属性类型 int
        /// </summary>
        AttrCritDamageAdd = 12152,

        /// <summary>
        ///     属性类型 int
        /// </summary>
        AttrCritDamageExAdd = 12153,

        /// <summary>
        ///     属性类型 int
        /// </summary>
        AttrCritDamagePer = 12154,

        /// <summary>
        ///     属性类型 int
        /// </summary>
        AttrCritDamageExPer = 12155,

        /// <summary>
        ///     属性类型 int
        /// </summary>
        AttrHealIntensity = 12200,

        /// <summary>
        ///     属性类型 int
        /// </summary>
        AttrHealIntensityTotal = 12201,

        /// <summary>
        ///     属性类型 int
        /// </summary>
        AttrHealIntensityAdd = 12202,

        /// <summary>
        ///     属性类型 int
        /// </summary>
        AttrHealIntensityExAdd = 12203,

        /// <summary>
        ///     属性类型 int
        /// </summary>
        AttrHealIntensityPer = 12204,

        /// <summary>
        ///     属性类型 int
        /// </summary>
        AttrHealIntensityExPer = 12205,

        /// <summary>
        ///     属性类型 int
        /// </summary>
        AttrHeal = 13010,

        /// <summary>
        ///     属性类型 int
        /// </summary>
        AttrHealTotal = 13011,

        /// <summary>
        ///     属性类型 int
        /// </summary>
        AttrHealAdd = 13012,

        /// <summary>
        ///     属性类型 int
        /// </summary>
        AttrHealExAdd = 13013,

        /// <summary>
        ///     属性类型 int
        /// </summary>
        AttrHealPer = 13014,

        /// <summary>
        ///     属性类型 int
        /// </summary>
        AttrHealExPer = 13015,

        /// <summary>
        ///     属性类型 int
        /// </summary>
        AttrHealed = 13020,

        /// <summary>
        ///     属性类型 int
        /// </summary>
        AttrHealedTotal = 13021,

        /// <summary>
        ///     属性类型 int
        /// </summary>
        AttrHealedAdd = 13022,

        /// <summary>
        ///     属性类型 int
        /// </summary>
        AttrHealedExAdd = 13023,

        /// <summary>
        ///     属性类型 int
        /// </summary>
        AttrHealedPer = 13024,

        /// <summary>
        ///     属性类型 int
        /// </summary>
        AttrHealedExPer = 13025,

        /// <summary>
        ///     属性类型 int
        /// </summary>
        AttrAttackSpeedPCT = 12500,

        /// <summary>
        ///     属性类型 int
        /// </summary>
        AttrAttackSpeedPCTTotal = 12501,

        /// <summary>
        ///     属性类型 int
        /// </summary>
        AttrAttackSpeedPCTAdd = 12502,

        /// <summary>
        ///     属性类型 int
        /// </summary>
        AttrAttackSpeedPCTExAdd = 12503,

        /// <summary>
        ///     属性类型 int
        /// </summary>
        AttrAttackSpeedPCTPer = 12504,

        /// <summary>
        ///     属性类型 int
        /// </summary>
        AttrAttackSpeedPCTExPer = 12505,

        /// <summary>
        ///     属性类型 int
        /// </summary>
        AttrSkillCDPCT = 12520,

        /// <summary>
        ///     属性类型 int
        /// </summary>
        AttrSkillCDPCTTotal = 12521,

        /// <summary>
        ///     属性类型 int
        /// </summary>
        AttrSkillCDPCTAdd = 12522,

        /// <summary>
        ///     属性类型 int
        /// </summary>
        AttrSkillCDPCTExAdd = 12523,

        /// <summary>
        ///     属性类型 int
        /// </summary>
        AttrSkillCDPCTPer = 12524,

        /// <summary>
        ///     属性类型 int
        /// </summary>
        AttrSkillCDPCTExPer = 12525,

        /// <summary>
        ///     属性类型 int
        /// </summary>
        AttrElementPenPCT = 14000,

        /// <summary>
        ///     属性类型 int
        /// </summary>
        AttrElementPenPCTTotal = 14001,

        /// <summary>
        ///     属性类型 int
        /// </summary>
        AttrElementPenPCTAdd = 14002,

        /// <summary>
        ///     属性类型 int
        /// </summary>
        AttrElementPenPCTExAdd = 14003,

        /// <summary>
        ///     属性类型 int
        /// </summary>
        AttrElementPenPCTPer = 14004,

        /// <summary>
        ///     属性类型 int
        /// </summary>
        AttrElementPenPCTExPer = 14005,

        /// <summary>
        ///     属性类型 int
        /// </summary>
        AttrElementRedPCT = 14010,

        /// <summary>
        ///     属性类型 int
        /// </summary>
        AttrElementRedPCTTotal = 14011,

        /// <summary>
        ///     属性类型 int
        /// </summary>
        AttrElementRedPCTAdd = 14012,

        /// <summary>
        ///     属性类型 int
        /// </summary>
        AttrElementRedPCTExAdd = 14013,

        /// <summary>
        ///     属性类型 int
        /// </summary>
        AttrElementRedPCTPer = 14014,

        /// <summary>
        ///     属性类型 int
        /// </summary>
        AttrElementRedPCTExPer = 14015,

        /// <summary>
        ///     属性类型 int
        /// </summary>
        AttrElementPhysicalPenPCT = 14020,

        /// <summary>
        ///     属性类型 int
        /// </summary>
        AttrElementPhysicalPenPCTTotal = 14021,

        /// <summary>
        ///     属性类型 int
        /// </summary>
        AttrElementPhysicalPenPCTAdd = 14022,

        /// <summary>
        ///     属性类型 int
        /// </summary>
        AttrElementPhysicalPenPCTExAdd = 14023,

        /// <summary>
        ///     属性类型 int
        /// </summary>
        AttrElementPhysicalPenPCTPer = 14024,

        /// <summary>
        ///     属性类型 int
        /// </summary>
        AttrElementPhysicalPenPCTExPer = 14025,

        /// <summary>
        ///     属性类型 int
        /// </summary>
        AttrElementPhysicalRedPCT = 14030,

        /// <summary>
        ///     属性类型 int
        /// </summary>
        AttrElementPhysicalRedPCTTotal = 14031,

        /// <summary>
        ///     属性类型 int
        /// </summary>
        AttrElementPhysicalRedPCTAdd = 14032,

        /// <summary>
        ///     属性类型 int
        /// </summary>
        AttrElementPhysicalRedPCTExAdd = 14033,

        /// <summary>
        ///     属性类型 int
        /// </summary>
        AttrElementPhysicalRedPCTPer = 14034,

        /// <summary>
        ///     属性类型 int
        /// </summary>
        AttrElementPhysicalRedPCTExPer = 14035,

        /// <summary>
        ///     属性类型 int
        /// </summary>
        AttrElementMagicPenPCT = 14040,

        /// <summary>
        ///     属性类型 int
        /// </summary>
        AttrElementMagicPenPCTTotal = 14041,

        /// <summary>
        ///     属性类型 int
        /// </summary>
        AttrElementMagicPenPCTAdd = 14042,

        /// <summary>
        ///     属性类型 int
        /// </summary>
        AttrElementMagicPenPCTExAdd = 14043,

        /// <summary>
        ///     属性类型 int
        /// </summary>
        AttrElementMagicPenPCTPer = 14044,

        /// <summary>
        ///     属性类型 int
        /// </summary>
        AttrElementMagicPenPCTExPer = 14045,

        /// <summary>
        ///     属性类型 int
        /// </summary>
        AttrElementMagicRedPCT = 14050,

        /// <summary>
        ///     属性类型 int
        /// </summary>
        AttrElementMagicRedPCTTotal = 14051,

        /// <summary>
        ///     属性类型 int
        /// </summary>
        AttrElementMagicRedPCTAdd = 14052,

        /// <summary>
        ///     属性类型 int
        /// </summary>
        AttrElementMagicRedPCTExAdd = 14053,

        /// <summary>
        ///     属性类型 int
        /// </summary>
        AttrElementMagicRedPCTPer = 14054,

        /// <summary>
        ///     属性类型 int
        /// </summary>
        AttrElementMagicRedPCTExPer = 14055,

        /// <summary>
        ///     属性类型 int
        /// </summary>
        AttrDmgA = 15000,

        /// <summary>
        ///     属性类型 int
        /// </summary>
        AttrDmgATotal = 15001,

        /// <summary>
        ///     属性类型 int
        /// </summary>
        AttrDmgAAdd = 15002,

        /// <summary>
        ///     属性类型 int
        /// </summary>
        AttrDmgAExAdd = 15003,

        /// <summary>
        ///     属性类型 int
        /// </summary>
        AttrDmgAPer = 15004,

        /// <summary>
        ///     属性类型 int
        /// </summary>
        AttrDmgAExPer = 15005,

        /// <summary>
        ///     属性类型 int
        /// </summary>
        AttrDmgRed = 15300,

        /// <summary>
        ///     属性类型 int
        /// </summary>
        AttrDmgRedTotal = 15301,

        /// <summary>
        ///     属性类型 int
        /// </summary>
        AttrDmgRedAdd = 15302,

        /// <summary>
        ///     属性类型 int
        /// </summary>
        AttrDmgRedExAdd = 15303,

        /// <summary>
        ///     属性类型 int
        /// </summary>
        AttrDmgRedPer = 15304,

        /// <summary>
        ///     属性类型 int
        /// </summary>
        AttrDmgRedExPer = 15305,

        /// <summary>
        ///     属性类型 int
        /// </summary>
        AttrDmgNear = 16000,

        /// <summary>
        ///     属性类型 int
        /// </summary>
        AttrDmgNearTotal = 16001,

        /// <summary>
        ///     属性类型 int
        /// </summary>
        AttrDmgNearAdd = 16002,

        /// <summary>
        ///     属性类型 int
        /// </summary>
        AttrDmgNearExAdd = 16003,

        /// <summary>
        ///     属性类型 int
        /// </summary>
        AttrDmgNearPer = 16004,

        /// <summary>
        ///     属性类型 int
        /// </summary>
        AttrDmgNearExPer = 16005,

        /// <summary>
        ///     属性类型 int
        /// </summary>
        AttrDmgRedNear = 16020,

        /// <summary>
        ///     属性类型 int
        /// </summary>
        AttrDmgRedNearTotal = 16021,

        /// <summary>
        ///     属性类型 int
        /// </summary>
        AttrDmgRedNearAdd = 16022,

        /// <summary>
        ///     属性类型 int
        /// </summary>
        AttrDmgRedNearExAdd = 16023,

        /// <summary>
        ///     属性类型 int
        /// </summary>
        AttrDmgRedNearPer = 16024,

        /// <summary>
        ///     属性类型 int
        /// </summary>
        AttrDmgRedNearExPer = 16025,

        /// <summary>
        ///     属性类型 int
        /// </summary>
        AttrDmgFar = 16100,

        /// <summary>
        ///     属性类型 int
        /// </summary>
        AttrDmgFarTotal = 16101,

        /// <summary>
        ///     属性类型 int
        /// </summary>
        AttrDmgFarAdd = 16102,

        /// <summary>
        ///     属性类型 int
        /// </summary>
        AttrDmgFarExAdd = 16103,

        /// <summary>
        ///     属性类型 int
        /// </summary>
        AttrDmgFarPer = 16104,

        /// <summary>
        ///     属性类型 int
        /// </summary>
        AttrDmgFarExPer = 16105,

        /// <summary>
        ///     属性类型 int
        /// </summary>
        AttrDmgRedFar = 16120,

        /// <summary>
        ///     属性类型 int
        /// </summary>
        AttrDmgRedFarTotal = 16121,

        /// <summary>
        ///     属性类型 int
        /// </summary>
        AttrDmgRedFarAdd = 16122,

        /// <summary>
        ///     属性类型 int
        /// </summary>
        AttrDmgRedFarExAdd = 16123,

        /// <summary>
        ///     属性类型 int
        /// </summary>
        AttrDmgRedFarPer = 16124,

        /// <summary>
        ///     属性类型 int
        /// </summary>
        AttrDmgRedFarExPer = 16125,

        /// <summary>
        ///     属性类型 int
        /// </summary>
        AttrDmgRedBullet = 16220,

        /// <summary>
        ///     属性类型 int
        /// </summary>
        AttrDmgRedBulletTotal = 16221,

        /// <summary>
        ///     属性类型 int
        /// </summary>
        AttrDmgRedBulletAdd = 16222,

        /// <summary>
        ///     属性类型 int
        /// </summary>
        AttrDmgRedBulletExAdd = 16223,

        /// <summary>
        ///     属性类型 int
        /// </summary>
        AttrDmgRedBulletPer = 16224,

        /// <summary>
        ///     属性类型 int
        /// </summary>
        AttrDmgRedBulletExPer = 16225,

        /// <summary>
        ///     属性类型 int
        /// </summary>
        AttrDmgRedMelee = 16320,

        /// <summary>
        ///     属性类型 int
        /// </summary>
        AttrDmgRedMeleeTotal = 16321,

        /// <summary>
        ///     属性类型 int
        /// </summary>
        AttrDmgRedMeleeAdd = 16322,

        /// <summary>
        ///     属性类型 int
        /// </summary>
        AttrDmgRedMeleeExAdd = 16323,

        /// <summary>
        ///     属性类型 int
        /// </summary>
        AttrDmgRedMeleePer = 16324,

        /// <summary>
        ///     属性类型 int
        /// </summary>
        AttrDmgRedMeleeExPer = 16325,

        /// <summary>
        ///     属性类型 int
        /// </summary>
        AttrDmgRedBuff = 16420,

        /// <summary>
        ///     属性类型 int
        /// </summary>
        AttrDmgRedBuffTotal = 16421,

        /// <summary>
        ///     属性类型 int
        /// </summary>
        AttrDmgRedBuffAdd = 16422,

        /// <summary>
        ///     属性类型 int
        /// </summary>
        AttrDmgRedBuffExAdd = 16423,

        /// <summary>
        ///     属性类型 int
        /// </summary>
        AttrDmgRedBuffPer = 16424,

        /// <summary>
        ///     属性类型 int
        /// </summary>
        AttrDmgRedBuffExPer = 16425,

        /// <summary>
        ///     属性类型 int
        /// </summary>
        AttrDmgRedVsNormalEnemy = 16720,

        /// <summary>
        ///     属性类型 int
        /// </summary>
        AttrDmgRedVsNormalEnemyTotal = 16721,

        /// <summary>
        ///     属性类型 int
        /// </summary>
        AttrDmgRedVsNormalEnemyAdd = 16722,

        /// <summary>
        ///     属性类型 int
        /// </summary>
        AttrDmgRedVsNormalEnemyExAdd = 16723,

        /// <summary>
        ///     属性类型 int
        /// </summary>
        AttrDmgRedVsNormalEnemyPer = 16724,

        /// <summary>
        ///     属性类型 int
        /// </summary>
        AttrDmgRedVsNormalEnemyExPer = 16725,

        /// <summary>
        ///     属性类型 int
        /// </summary>
        AttrDmgRedVsEliteEnemy = 16820,

        /// <summary>
        ///     属性类型 int
        /// </summary>
        AttrDmgRedVsEliteEnemyTotal = 16821,

        /// <summary>
        ///     属性类型 int
        /// </summary>
        AttrDmgRedVsEliteEnemyAdd = 16822,

        /// <summary>
        ///     属性类型 int
        /// </summary>
        AttrDmgRedVsEliteEnemyExAdd = 16823,

        /// <summary>
        ///     属性类型 int
        /// </summary>
        AttrDmgRedVsEliteEnemyPer = 16824,

        /// <summary>
        ///     属性类型 int
        /// </summary>
        AttrDmgRedVsEliteEnemyExPer = 16825,

        /// <summary>
        ///     属性类型 int
        /// </summary>
        AttrDmgRedHealthy = 17020,

        /// <summary>
        ///     属性类型 int
        /// </summary>
        AttrDmgRedHealthyTotal = 17021,

        /// <summary>
        ///     属性类型 int
        /// </summary>
        AttrDmgRedHealthyAdd = 17022,

        /// <summary>
        ///     属性类型 int
        /// </summary>
        AttrDmgRedHealthyExAdd = 17023,

        /// <summary>
        ///     属性类型 int
        /// </summary>
        AttrDmgRedHealthyPer = 17024,

        /// <summary>
        ///     属性类型 int
        /// </summary>
        AttrDmgRedHealthyExPer = 17025,

        /// <summary>
        ///     属性类型 int
        /// </summary>
        AttrDmgRedNonHealthy = 17120,

        /// <summary>
        ///     属性类型 int
        /// </summary>
        AttrDmgRedNonHealthyTotal = 17121,

        /// <summary>
        ///     属性类型 int
        /// </summary>
        AttrDmgRedNonHealthyAdd = 17122,

        /// <summary>
        ///     属性类型 int
        /// </summary>
        AttrDmgRedNonHealthyExAdd = 17123,

        /// <summary>
        ///     属性类型 int
        /// </summary>
        AttrDmgRedNonHealthyPer = 17124,

        /// <summary>
        ///     属性类型 int
        /// </summary>
        AttrDmgRedNonHealthyExPer = 17125,

        /// <summary>
        ///     属性类型 int
        /// </summary>
        AttrEntityLevel = 60010,
    }
}