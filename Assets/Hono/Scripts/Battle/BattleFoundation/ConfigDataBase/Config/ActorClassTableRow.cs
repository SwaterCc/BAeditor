using Newtonsoft.Json.Linq;
using System.Collections.Generic;
namespace Hono.Scripts.Battle
{
    /// <summary>
    /// 自动生成的 ActorClassTableRow 类。
    /// </summary>
    public class ActorClassTableRow : ConfigTableRow
    {
        /// <summary>
/// 职业名字
/// </summary>
public string ClassName { get; private set; }

/// <summary>
/// 职业描述
/// </summary>
public string Desc { get; private set; }

/// <summary>
/// 职业类型
/// </summary>
public int ActorClassType { get; private set; }

/// <summary>
/// 职业技能
/// </summary>
public List<List<int>> Skills { get; private set; }

/// <summary>
/// 副职业技能
/// </summary>
public List<List<int>> SubClassSkill { get; private set; }

/// <summary>
/// 职业Buff
/// </summary>
public List<int> Buffs { get; private set; }

        // 解析 JSON 对象
        public override void Parse(JObject rowJsonObject)
        {
            Id = rowJsonObject["Id"].Value<int>();
            ClassName = rowJsonObject["ClassName"].ToObject<string>();; Desc = rowJsonObject["Desc"].ToObject<string>();; ActorClassType = rowJsonObject["ActorClassType"].ToObject<int>();; Skills = rowJsonObject["Skills"].ToObject<List<List<int>>>();; SubClassSkill = rowJsonObject["SubClassSkill"].ToObject<List<List<int>>>();; Buffs = rowJsonObject["Buffs"].ToObject<List<int>>();
        }
    }
}