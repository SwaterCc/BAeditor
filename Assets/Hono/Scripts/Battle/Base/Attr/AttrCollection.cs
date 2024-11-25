#region

using System.Collections.Generic;

#endregion

namespace Hono.Scripts.Battle.Base
{
    public class AttrCollection
    {
        private Actor _actor;

        public AttrCollection(Actor actor)
        {
            _actor = actor;
        }

        private readonly Dictionary<EAttrType, Attr> _attrs = new(128);

        public bool HasAttr(EAttrType attrType)
        {
            return _attrs.ContainsKey(attrType);
        }

        public Attr GetAttr(EAttrType attrType)
        {
            if (!_attrs.TryGetValue(attrType, out Attr attr))
            {
                attr = AObjectPool<Attr>.Pool.Rent();
                _attrs.Add(attrType, attr);
            }

            return attr;
        }

        public void SetAttr(EAttrType attrType, int value, bool isCommand)
        {
            var attrTypeInt = attrType;

            if (!_attrs.TryGetValue(attrTypeInt, out var attr))
            {
                attr = AObjectPool<Attr>.Pool.Rent();
                _attrs.Add(attrTypeInt, attr);
            }

            attr.Set(value, isCommand);
        }

        /// <summary>
        /// 重新计算复合属性
        /// </summary>
        public void UpdateComplexAttr()
        {
            //遍历表，判定是否存在该属性，然后使用公式计算
        }

        private void calculateAttr(int headId)
        {
            /*var add = Self.GetAttr(headId + 2);
            var exAdd = Self.GetAttr(headId + 3);
            var per = Self.GetAttr(headId + 4);
            var finalPer = Mathf.Clamp(((10000 + per) / 10000f), 0f, float.MaxValue - 1);
            int final = (int)(add * finalPer + exAdd);
            Self.SetAttr(headId + 1, final, false);
            var total = Self.GetAttr(headId + 1);
            Self.SetAttr(headId, total, false);*/
        }
    }
}