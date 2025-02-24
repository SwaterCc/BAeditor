using Newtonsoft.Json.Linq;
using System.Collections.Generic;
namespace Hono.Scripts.Battle
{
    /// <summary>
    /// 自动生成的 DamageMultiplyTableRow 类。
    /// </summary>
    public class DamageMultiplyTableRow : ConfigTableRow
    {
        /// <summary>
/// 备注
/// </summary>
public string Desc { get; private set; }

/// <summary>
/// Apply增伤数值万分比
/// </summary>
public List<int> DamageValue { get; private set; }

/// <summary>
/// Apply数值的方法，可以在lua中自定义计算方法，通常保持默认即可
/// </summary>
public string ApplyFuncName { get; private set; }

/// <summary>
/// 增伤条件ID列表
/// </summary>
public List<int> ConditionIds { get; private set; }

/// <summary>
/// 增伤条件参数表
/// </summary>
public List<List<int>> ConditionParams { get; private set; }

        // 解析 JSON 对象
        public override void Parse(JObject rowJsonObject)
        {
            Id = rowJsonObject["Id"].Value<int>();
            Desc = rowJsonObject["Desc"].ToObject<string>();; DamageValue = rowJsonObject["DamageValue"].ToObject<List<int>>();; ApplyFuncName = rowJsonObject["ApplyFuncName"].ToObject<string>();; ConditionIds = rowJsonObject["ConditionIds"].ToObject<List<int>>();; ConditionParams = rowJsonObject["ConditionParams"].ToObject<List<List<int>>>();
        }
    }
}