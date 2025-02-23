using System.Collections.Generic;
using Newtonsoft.Json.Linq;

namespace Hono.Scripts.Battle
{
    public class DamageMultiplyTable : ConfigTable
    {
        private readonly Dictionary<int, DamageMultiplyTableRow> _rows = new Dictionary<int, DamageMultiplyTableRow>();

        // 解析 JSON 对象
        public override void ParseJsonObject(JObject tableJObject)
        {
            foreach (var rowJObject in tableJObject.Properties())
            {
                var row = new DamageMultiplyTableRow();
                row.Parse(rowJObject.Value as JObject);
                _rows[row.Id] = row;
            }
        }

        // 获取所有行（泛型版本）
        public IEnumerable<DamageMultiplyTableRow> GetAllRows()
        {
            return _rows.Values;
        }

        // 获取单行（基类版本）
        public override ConfigTableRow GetRowBase(int id)
        {
            return _rows.ContainsKey(id) ? _rows[id] : null;
        }

        // 获取单行（子类版本）
        public DamageMultiplyTableRow GetRow(int id)
        {
            return _rows.ContainsKey(id) ? _rows[id] : null;
        }

        public bool TryGetRow(int id,out DamageMultiplyTableRow row)
        {
            return _rows.TryGetValue(id,out row);
        }
    }
}