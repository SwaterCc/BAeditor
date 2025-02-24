using Newtonsoft.Json.Linq;
using System.Collections.Generic;
namespace Hono.Scripts.Battle
{
    /// <summary>
    /// 自动生成的 BattleSceneTableRow 类。
    /// </summary>
    public class BattleSceneTableRow : ConfigTableRow
    {
        /// <summary>
/// 描述
/// </summary>
public string Desc { get; private set; }

/// <summary>
/// 场景路径
/// </summary>
public string ScenePath { get; private set; }

/// <summary>
/// 最大队伍数量
/// </summary>
public int TeamCount { get; private set; }

/// <summary>
/// 战斗类型(1普通，2战争，3肉鸽)
/// </summary>
public int BattleType { get; private set; }

        // 解析 JSON 对象
        public override void Parse(JObject rowJsonObject)
        {
            Id = rowJsonObject["Id"].Value<int>();
            Desc = rowJsonObject["Desc"].ToObject<string>();; ScenePath = rowJsonObject["ScenePath"].ToObject<string>();; TeamCount = rowJsonObject["TeamCount"].ToObject<int>();; BattleType = rowJsonObject["BattleType"].ToObject<int>();
        }
    }
}