using Newtonsoft.Json.Linq;
using System.Collections.Generic;
namespace Hono.Scripts.Battle
{
    /// <summary>
    /// 自动生成的 AttrTableRow 类。
    /// </summary>
    public class AttrTableRow : ConfigTableRow
    {
        /// <summary>
/// 属性名字
/// </summary>
public string EnumName { get; private set; }

/// <summary>
/// 属性名字
/// </summary>
public string Name { get; private set; }

/// <summary>
/// 属性使用方式（int or float or bool）
/// </summary>
public string Type { get; private set; }

/// <summary>
/// 属性下限
/// </summary>
public int AttrLowerLimit { get; private set; }

/// <summary>
/// 属性上限
/// </summary>
public int AttrUpperLimit { get; private set; }

/// <summary>
/// 最终值
/// </summary>
public int AttrFinal { get; private set; }

/// <summary>
/// 总值
/// </summary>
public int AttrTotal { get; private set; }

/// <summary>
/// 加值
/// </summary>
public int AttrAdd { get; private set; }

/// <summary>
/// 额外加值
/// </summary>
public int AttrExAdd { get; private set; }

/// <summary>
/// 乘值
/// </summary>
public int AttrPer { get; private set; }

/// <summary>
/// 额外乘值
/// </summary>
public int AttrExPer { get; private set; }

/// <summary>
/// 外显名称
/// </summary>
public string OfficialName { get; private set; }

/// <summary>
/// 属性说明文本
/// </summary>
public string AttrDes { get; private set; }

        // 解析 JSON 对象
        public override void Parse(JObject rowJsonObject)
        {
            Id = rowJsonObject["Id"].Value<int>();
            EnumName = rowJsonObject["EnumName"].ToObject<string>();; Name = rowJsonObject["Name"].ToObject<string>();; Type = rowJsonObject["Type"].ToObject<string>();; AttrLowerLimit = rowJsonObject["AttrLowerLimit"].ToObject<int>();; AttrUpperLimit = rowJsonObject["AttrUpperLimit"].ToObject<int>();; AttrFinal = rowJsonObject["AttrFinal"].ToObject<int>();; AttrTotal = rowJsonObject["AttrTotal"].ToObject<int>();; AttrAdd = rowJsonObject["AttrAdd"].ToObject<int>();; AttrExAdd = rowJsonObject["AttrExAdd"].ToObject<int>();; AttrPer = rowJsonObject["AttrPer"].ToObject<int>();; AttrExPer = rowJsonObject["AttrExPer"].ToObject<int>();; OfficialName = rowJsonObject["OfficialName"].ToObject<string>();; AttrDes = rowJsonObject["AttrDes"].ToObject<string>();
        }
    }
}