using Newtonsoft.Json.Linq;
using System.Collections.Generic;
namespace Hono.Scripts.Battle
{
    /// <summary>
    /// 自动生成的 DamageTableRow 类。
    /// </summary>
    public class DamageTableRow : ConfigTableRow
    {
        /// <summary>
/// 策划描述
/// </summary>
public string Desc { get; private set; }

/// <summary>
/// 伤害计算公式
/// </summary>
public string FormulaName { get; private set; }

/// <summary>
/// 伤害倍率万分比
/// </summary>
public int DamageRatio { get; private set; }

/// <summary>
/// 伤害类型(普通伤害，百分比伤害，Dot，治疗)
/// </summary>
public int DamageType { get; private set; }

/// <summary>
/// 元素类型（物理，法术）
/// </summary>
public int ElementType { get; private set; }

/// <summary>
/// 额外异常积蓄倍率(异常标签=异常积蓄值)
/// </summary>
public List<int> ElementsDamage { get; private set; }

/// <summary>
/// 禁用元素爆炸
/// </summary>
public bool DisableElementBoom { get; private set; }

/// <summary>
/// 伤害Tag
/// </summary>
public List<int> Tags { get; private set; }

/// <summary>
/// 冲击力
/// </summary>
public int ImpactValue { get; private set; }

/// <summary>
/// 加值表配置
/// </summary>
public List<int> AdditiveId { get; private set; }

/// <summary>
/// 乘值表配置
/// </summary>
public List<int> MultiplyId { get; private set; }

/// <summary>
/// 受击特效路径
/// </summary>
public string BeHitVFXPath { get; private set; }

        // 解析 JSON 对象
        public override void Parse(JObject rowJsonObject)
        {
            Id = rowJsonObject["Id"].Value<int>();
            Desc = rowJsonObject["Desc"].ToObject<string>();; FormulaName = rowJsonObject["FormulaName"].ToObject<string>();; DamageRatio = rowJsonObject["DamageRatio"].ToObject<int>();; DamageType = rowJsonObject["DamageType"].ToObject<int>();; ElementType = rowJsonObject["ElementType"].ToObject<int>();; ElementsDamage = rowJsonObject["ElementsDamage"].ToObject<List<int>>();; DisableElementBoom = rowJsonObject["DisableElementBoom"].ToObject<bool>();; Tags = rowJsonObject["Tags"].ToObject<List<int>>();; ImpactValue = rowJsonObject["ImpactValue"].ToObject<int>();; AdditiveId = rowJsonObject["AdditiveId"].ToObject<List<int>>();; MultiplyId = rowJsonObject["MultiplyId"].ToObject<List<int>>();; BeHitVFXPath = rowJsonObject["BeHitVFXPath"].ToObject<string>();
        }
    }
}