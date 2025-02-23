#region

using System.Collections;
using System.Collections.Generic;
using Hono.Scripts.Battle.Base;
using Newtonsoft.Json.Linq;
using UnityEngine;

#endregion

namespace Hono.Scripts.Battle
{
    public abstract class ConfigTableRow
    {
        public int Id { get; protected set; }

        private readonly Dictionary<string, object> _rowKeyValue = new();

        //解析后的字段要额外调用这个方法存在字典中
        private void addRowItem(string key,object value)
        {
            switch (value)
            {
                case int iValue :
                    value = new RefInt(iValue);
                    break;
                case float fValue :
                    value = new RefFloat(fValue);
                    break;
                case bool bValue :
                    value = new RefBoolean(bValue);
                    break;
                case Vector3 v3Value :
                    value = new RefVector3(v3Value);
                    break;
            }

            _rowKeyValue.Add(key, value);
        }
        
        public object TryGetItem(string filedName, out object value)
        {
            return _rowKeyValue.TryGetValue(filedName, out value);
        }


        public abstract void Parse(JObject rowJsonObject);
    }
}