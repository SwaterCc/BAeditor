using System.Collections.Generic;
using Hono.Scripts.Battle.Tools;
using UnityEngine;

namespace Hono.Scripts.Battle.Base
{
    public class AttrHelper : Singleton<AttrHelper>,IBattleFrameworkInit
    {
        public class AttrLink
        {
            /// <summary>
            /// 属性分量最终值，只读
            /// </summary>
            private EAttrType _final;
            /// <summary>
            /// 属性分量最终修订值，当为0时取公式结果，有值时 _final = _total
            /// </summary>
            private EAttrType _total;
            /// <summary>
            /// 加值
            /// </summary>
            private EAttrType _add;
            /// <summary>
            /// 额外加值
            /// </summary>
            private EAttrType _addEx;
            /// <summary>
            /// 乘值
            /// </summary>
            private EAttrType _sub;
            /// <summary>
            /// 额外乘值
            /// </summary>
            private EAttrType _subEx;
           
            public void UpdateLink(AttrCollection collection)
            {
                var add = collection.GetAttr(_add);
                var exAdd = collection.GetAttr(_addEx);
                var per = collection.GetAttr(_sub);
                var finalPer = Mathf.Clamp(((10000 + per) / 10000f), 0f, float.MaxValue - 1);
                var total = collection.GetAttr(_total);
                int final = total == 0 ? (int)(add * finalPer + exAdd) : total;
                collection.SetAttr(_final, final);
            }
        }

        private static readonly Dictionary<EAttrType, AttrLink> _attrLinks = new();
        
        
        public void Init()
        {
            //初始化属性关联
            foreach (var attrRow in ConfigManager.Table<AttrTable>().GetTable())
            {
                
            }
        }

        public bool TryGetLink(EAttrType attrType,out AttrLink link)
        {
            return _attrLinks.TryGetValue(attrType, out link);
        }
        
        public void InitByTableRow(AttrCollection collection,EntityAttrBaseTable.EntityAttrBaseRow row)
        {
           
        }

        
    }
}