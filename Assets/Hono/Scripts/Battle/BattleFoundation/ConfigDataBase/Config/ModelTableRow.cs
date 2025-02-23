using Newtonsoft.Json.Linq;
using System.Collections.Generic;
namespace Hono.Scripts.Battle
{
    /// <summary>
    /// 自动生成的 ModelTableRow 类。
    /// </summary>
    public class ModelTableRow : ConfigTableRow
    {
        /// <summary>
/// 描述
/// </summary>
public string Desc { get; private set; }

/// <summary>
/// UO代理类型
/// </summary>
public string UOProxyType { get; private set; }

/// <summary>
/// 代理Layer层
/// </summary>
public string ProxyLayer { get; private set; }

/// <summary>
/// 基础资源模板Id
/// </summary>
public int ResReplTplId { get; private set; }

/// <summary>
/// 模型缩放
/// </summary>
public float ModelScale { get; private set; }

/// <summary>
/// P1(球形，胶囊体半径，立方体的长)
/// </summary>
public float P1 { get; private set; }

/// <summary>
/// 胶囊体，立方体的高
/// </summary>
public float P2 { get; private set; }

/// <summary>
/// 立方体的宽
/// </summary>
public float P3 { get; private set; }

/// <summary>
/// ColliderCenter
/// </summary>
public List<float> ColliderCenter { get; private set; }

        // 解析 JSON 对象
        public override void Parse(JObject rowJsonObject)
        {
            Id = rowJsonObject["Id"].Value<int>();
            Desc = rowJsonObject["Desc"].ToObject<string>();; UOProxyType = rowJsonObject["UOProxyType"].ToObject<string>();; ProxyLayer = rowJsonObject["ProxyLayer"].ToObject<string>();; ResReplTplId = rowJsonObject["ResReplTplId"].ToObject<int>();; ModelScale = rowJsonObject["ModelScale"].ToObject<float>();; P1 = rowJsonObject["P1"].ToObject<float>();; P2 = rowJsonObject["P2"].ToObject<float>();; P3 = rowJsonObject["P3"].ToObject<float>();; ColliderCenter = rowJsonObject["ColliderCenter"].ToObject<List<float>>();
        }
    }
}