using Newtonsoft.Json.Linq;
using System.Collections.Generic;
namespace Hono.Scripts.Battle
{
    /// <summary>
    /// 自动生成的 EntityAttrBaseTableRow 类。
    /// </summary>
    public class EntityAttrBaseTableRow : ConfigTableRow
    {
        /// <summary>
/// 属性描述
/// </summary>
public string Desc { get; private set; }

/// <summary>
/// 默认初始等级，目标单位等级，为角色时该等级=主职业等级，为怪物时即正常等级。
/// </summary>
public int AttrEntityLevel { get; private set; }

/// <summary>
/// 最大HP
/// </summary>
public int AttrMaxHpAdd { get; private set; }

/// <summary>
/// 最大MP
/// </summary>
public int AttrMaxMpAdd { get; private set; }

/// <summary>
/// 攻击力
/// </summary>
public int AttrAttackAdd { get; private set; }

/// <summary>
/// 防御力
/// </summary>
public int AttrDefenseAdd { get; private set; }

/// <summary>
/// 转向速度万分比
/// </summary>
public int AttrRotSpeedPCTAdd { get; private set; }

/// <summary>
/// 移动速度万分比
/// </summary>
public int AttrMoveSpeedPCTAdd { get; private set; }

/// <summary>
/// 防御穿透
/// </summary>
public int AttrIgnoreDefenseAdd { get; private set; }

/// <summary>
/// 防御穿透万分比
/// </summary>
public int AttrIgnoreDefensePCTAdd { get; private set; }

/// <summary>
/// 暴击率加值万分比
/// </summary>
public int AttrCritAdd { get; private set; }

/// <summary>
/// 暴击伤害加值万分比
/// </summary>
public int AttrCritDamageAdd { get; private set; }

/// <summary>
/// 治疗强度
/// </summary>
public int AttrHealIntensityAdd { get; private set; }

/// <summary>
/// 治疗效果
/// </summary>
public int AttrHealAdd { get; private set; }

/// <summary>
/// 被治疗效果
/// </summary>
public int AttrHealedAdd { get; private set; }

/// <summary>
/// 攻击速度加成
/// </summary>
public int AttrAttackSpeedPCTAdd { get; private set; }

/// <summary>
/// 全属性抗性穿透
/// </summary>
public int AttrElementPenPCTAdd { get; private set; }

/// <summary>
/// 全属性抗性
/// </summary>
public int AttrElementRedPCTAdd { get; private set; }

/// <summary>
/// 物理属性抗性穿透
/// </summary>
public int AttrElementPhysicalPenPCTAdd { get; private set; }

/// <summary>
/// 物理属性抗性
/// </summary>
public int AttrElementPhysicalRedPCTAdd { get; private set; }

/// <summary>
/// 元素属性抗性穿透
/// </summary>
public int AttrElementMagicPenPCTAdd { get; private set; }

/// <summary>
/// 元素属性抗性
/// </summary>
public int AttrElementMagicRedPCTAdd { get; private set; }

/// <summary>
/// 伤害增加万分比%[x]
/// </summary>
public int AttrDmgAAdd { get; private set; }

/// <summary>
/// 直接减伤%[x]
/// </summary>
public int AttrDmgRedAdd { get; private set; }

/// <summary>
/// CDR万分比
/// </summary>
public int AttrSkillCDPCTAdd { get; private set; }

/// <summary>
/// 全局回MP效率加值
/// </summary>
public int AttrMpRecAllAdd { get; private set; }

/// <summary>
/// 全局回MP效率乘值
/// </summary>
public int AttrMpRecAllPer { get; private set; }

/// <summary>
/// 击杀回MP修正值
/// </summary>
public int AttrMpRecKilledAdd { get; private set; }

/// <summary>
/// 受击回MP万分比
/// </summary>
public int AttrMpRecBehitPer { get; private set; }

        // 解析 JSON 对象
        public override void Parse(JObject rowJsonObject)
        {
            Id = rowJsonObject["Id"].Value<int>();
            Desc = rowJsonObject["Desc"].ToObject<string>();; AttrEntityLevel = rowJsonObject["AttrEntityLevel"].ToObject<int>();; AttrMaxHpAdd = rowJsonObject["AttrMaxHpAdd"].ToObject<int>();; AttrMaxMpAdd = rowJsonObject["AttrMaxMpAdd"].ToObject<int>();; AttrAttackAdd = rowJsonObject["AttrAttackAdd"].ToObject<int>();; AttrDefenseAdd = rowJsonObject["AttrDefenseAdd"].ToObject<int>();; AttrRotSpeedPCTAdd = rowJsonObject["AttrRotSpeedPCTAdd"].ToObject<int>();; AttrMoveSpeedPCTAdd = rowJsonObject["AttrMoveSpeedPCTAdd"].ToObject<int>();; AttrIgnoreDefenseAdd = rowJsonObject["AttrIgnoreDefenseAdd"].ToObject<int>();; AttrIgnoreDefensePCTAdd = rowJsonObject["AttrIgnoreDefensePCTAdd"].ToObject<int>();; AttrCritAdd = rowJsonObject["AttrCritAdd"].ToObject<int>();; AttrCritDamageAdd = rowJsonObject["AttrCritDamageAdd"].ToObject<int>();; AttrHealIntensityAdd = rowJsonObject["AttrHealIntensityAdd"].ToObject<int>();; AttrHealAdd = rowJsonObject["AttrHealAdd"].ToObject<int>();; AttrHealedAdd = rowJsonObject["AttrHealedAdd"].ToObject<int>();; AttrAttackSpeedPCTAdd = rowJsonObject["AttrAttackSpeedPCTAdd"].ToObject<int>();; AttrElementPenPCTAdd = rowJsonObject["AttrElementPenPCTAdd"].ToObject<int>();; AttrElementRedPCTAdd = rowJsonObject["AttrElementRedPCTAdd"].ToObject<int>();; AttrElementPhysicalPenPCTAdd = rowJsonObject["AttrElementPhysicalPenPCTAdd"].ToObject<int>();; AttrElementPhysicalRedPCTAdd = rowJsonObject["AttrElementPhysicalRedPCTAdd"].ToObject<int>();; AttrElementMagicPenPCTAdd = rowJsonObject["AttrElementMagicPenPCTAdd"].ToObject<int>();; AttrElementMagicRedPCTAdd = rowJsonObject["AttrElementMagicRedPCTAdd"].ToObject<int>();; AttrDmgAAdd = rowJsonObject["AttrDmgAAdd"].ToObject<int>();; AttrDmgRedAdd = rowJsonObject["AttrDmgRedAdd"].ToObject<int>();; AttrSkillCDPCTAdd = rowJsonObject["AttrSkillCDPCTAdd"].ToObject<int>();; AttrMpRecAllAdd = rowJsonObject["AttrMpRecAllAdd"].ToObject<int>();; AttrMpRecAllPer = rowJsonObject["AttrMpRecAllPer"].ToObject<int>();; AttrMpRecKilledAdd = rowJsonObject["AttrMpRecKilledAdd"].ToObject<int>();; AttrMpRecBehitPer = rowJsonObject["AttrMpRecBehitPer"].ToObject<int>();
        }
    }
}