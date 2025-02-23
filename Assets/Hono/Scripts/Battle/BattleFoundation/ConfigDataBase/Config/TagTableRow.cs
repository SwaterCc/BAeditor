using Newtonsoft.Json.Linq;
using System.Collections.Generic;
namespace Hono.Scripts.Battle
{
    /// <summary>
    /// 自动生成的 TagTableRow 类。
    /// </summary>
    public class TagTableRow : ConfigTableRow
    {
        /// <summary>
/// TAG名称
/// </summary>
public string TagName { get; private set; }

/// <summary>
/// TAG释义
/// </summary>
public string TagString { get; private set; }

/// <summary>
/// TAG类型，1=单位tag、2=技能BDTag
/// </summary>
public int TagType { get; private set; }

/// <summary>
/// 父TAGId
/// </summary>
public int ParentTag { get; private set; }

        // 解析 JSON 对象
        public override void Parse(JObject rowJsonObject)
        {
            Id = rowJsonObject["Id"].Value<int>();
            TagName = rowJsonObject["TagName"].ToObject<string>();; TagString = rowJsonObject["TagString"].ToObject<string>();; TagType = rowJsonObject["TagType"].ToObject<int>();; ParentTag = rowJsonObject["ParentTag"].ToObject<int>();
        }
    }
}