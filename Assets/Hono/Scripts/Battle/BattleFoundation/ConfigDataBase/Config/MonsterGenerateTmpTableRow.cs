using Newtonsoft.Json.Linq;
using System.Collections.Generic;
namespace Hono.Scripts.Battle
{
    /// <summary>
    /// 自动生成的 MonsterGenerateTmpTableRow 类。
    /// </summary>
    public class MonsterGenerateTmpTableRow : ConfigTableRow
    {
        /// <summary>
/// 描述
/// </summary>
public string Desc { get; private set; }

/// <summary>
/// 怪物行为（0=原地不动，1主动寻找当前操控的队伍）
/// </summary>
public int MonsterBehave { get; private set; }

/// <summary>
/// 延迟时间
/// </summary>
public float DelayTime { get; private set; }

/// <summary>
/// 创建间隔
/// </summary>
public float Interval { get; private set; }

/// <summary>
/// 单次创建最大数量
/// </summary>
public int MaxCreationOnce { get; private set; }

/// <summary>
/// 怪物列表，按顺序刷新
/// </summary>
public List<List<int>> MonsterInfos { get; private set; }

/// <summary>
/// 创建时添加的buff
/// </summary>
public List<List<int>> ExBuffs { get; private set; }

/// <summary>
/// 额外的技能
/// </summary>
public List<List<int>> ExSkills { get; private set; }

/// <summary>
/// 阵营覆盖
/// </summary>
public int FactionId { get; private set; }

/// <summary>
/// 等级覆盖
/// </summary>
public int LevelOverride { get; private set; }

/// <summary>
/// 额外Tag
/// </summary>
public List<int> ExTags { get; private set; }

        // 解析 JSON 对象
        public override void Parse(JObject rowJsonObject)
        {
            Id = rowJsonObject["Id"].Value<int>();
            Desc = rowJsonObject["Desc"].ToObject<string>();; MonsterBehave = rowJsonObject["MonsterBehave"].ToObject<int>();; DelayTime = rowJsonObject["DelayTime"].ToObject<float>();; Interval = rowJsonObject["Interval"].ToObject<float>();; MaxCreationOnce = rowJsonObject["MaxCreationOnce"].ToObject<int>();; MonsterInfos = rowJsonObject["MonsterInfos"].ToObject<List<List<int>>>();; ExBuffs = rowJsonObject["ExBuffs"].ToObject<List<List<int>>>();; ExSkills = rowJsonObject["ExSkills"].ToObject<List<List<int>>>();; FactionId = rowJsonObject["FactionId"].ToObject<int>();; LevelOverride = rowJsonObject["LevelOverride"].ToObject<int>();; ExTags = rowJsonObject["ExTags"].ToObject<List<int>>();
        }
    }
}