using Newtonsoft.Json.Linq;
using System.Collections.Generic;
namespace Hono.Scripts.Battle
{
    /// <summary>
    /// 自动生成的 CombatEnergyTableRow 类。
    /// </summary>
    public class CombatEnergyTableRow : ConfigTableRow
    {
        /// <summary>
/// 能量类型描述
/// </summary>
public string Desc { get; private set; }

/// <summary>
/// 能量初始值
/// </summary>
public int InitValue { get; private set; }

/// <summary>
/// 能量自然增长值每秒
/// </summary>
public int IdleGetValue { get; private set; }

/// <summary>
/// 受击获取值
/// </summary>
public int BehitGetValue { get; private set; }

/// <summary>
/// 攻击获取值
/// </summary>
public int AttackGetValue { get; private set; }

/// <summary>
/// 能量上限
/// </summary>
public int EnergyMax { get; private set; }

        // 解析 JSON 对象
        public override void Parse(JObject rowJsonObject)
        {
            Id = rowJsonObject["Id"].Value<int>();
            Desc = rowJsonObject["Desc"].ToObject<string>();; InitValue = rowJsonObject["InitValue"].ToObject<int>();; IdleGetValue = rowJsonObject["IdleGetValue"].ToObject<int>();; BehitGetValue = rowJsonObject["BehitGetValue"].ToObject<int>();; AttackGetValue = rowJsonObject["AttackGetValue"].ToObject<int>();; EnergyMax = rowJsonObject["EnergyMax"].ToObject<int>();
        }
    }
}