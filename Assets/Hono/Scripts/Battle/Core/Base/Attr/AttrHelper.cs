using System;
using System.Collections.Generic;
using Hono.Scripts.Battle.Core;
using Hono.Scripts.Battle.Core.Base;
using Hono.Scripts.Battle.Tools;
using UnityEngine;

namespace Hono.Scripts.Battle.Base
{
    public class AttrHelper : Singleton<AttrHelper>
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

            /// <summary>
            /// 最终值下限
            /// </summary>
            private int _finalLower;

            /// <summary>
            /// 最终值上限
            /// </summary>
            private int _finalUpper;

            public bool Init(AttrTableRow attrRow)
            {
                if (!setField(ref _final, attrRow.AttrFinal))
                {
                    return false;
                }

                if (!setField(ref _total, attrRow.AttrTotal))
                {
                    return false;
                }

                if (!setField(ref _add, attrRow.AttrAdd))
                {
                    return false;
                }

                if (!setField(ref _addEx, attrRow.AttrExAdd))
                {
                    return false;
                }

                if (!setField(ref _sub, attrRow.AttrPer))
                {
                    return false;
                }

                if (!setField(ref _subEx, attrRow.AttrExPer))
                {
                    return false;
                }

                _finalLower = Mathf.Clamp(attrRow.AttrLowerLimit, Int32.MinValue, attrRow.AttrLowerLimit);
                _finalUpper = attrRow.AttrUpperLimit <= 0 ? Int32.MaxValue : attrRow.AttrUpperLimit;
                return true;
            }

            private bool setField(ref EAttrType field, int rowField)
            {
                if (!Enum.IsDefined(typeof(EAttrType), rowField))
                {
                    Debug.LogError($"attrTable id {rowField} 该属性不存在");
                    return false;
                }

                field = (EAttrType)rowField;
                return true;
            }

            public void UpdateLink(AttrCollection collection)
            {
                var add = collection.GetAttr(_add);
                var exAdd = collection.GetAttr(_addEx);
                var per = collection.GetAttr(_sub);
                var finalPer = Mathf.Clamp(((10000 + per) / 10000f), 0f, float.MaxValue - 1);
                var total = collection.GetAttr(_total);
                int final = total == 0 ? (int)(add * finalPer + exAdd) : total;
                final = Mathf.Clamp(final, _finalLower, _finalUpper);
                collection.SetAttr(_final, final);
            }
        }

        private static readonly Dictionary<EAttrType, AttrLink> AttrLinks = new();


        public void Init()
        {
            //初始化属性关联
            foreach (var attrRow in ConfigDataBase.Table<AttrTable>().GetAllRows())
            {
                if (!Enum.IsDefined(typeof(EAttrType), attrRow.Id))
                {
                    Debug.LogError($"attrTable id {attrRow.Id} 该属性不存在");
                    continue;
                }

                var link = new AttrLink();
                if (!link.Init(attrRow))
                {
                    continue;
                }

                AttrLinks.Add((EAttrType)attrRow.Id, link);
            }
        }

        public bool TryGetLink(EAttrType attrType, out AttrLink link)
        {
            return AttrLinks.TryGetValue(attrType, out link);
        }

        public void InitByTableRow(AttrCollection collection, EntityAttrBaseTableRow attrRow)
        {
            collection.SetAttr(EAttrType.AttrUnitLevel,                attrRow.AttrEntityLevel,              false);
            collection.SetAttr(EAttrType.AttrMoveSpeedPCTAdd,          attrRow.AttrMoveSpeedPCTAdd,          false);
            collection.SetAttr(EAttrType.AttrRotSpeedPCTAdd,           attrRow.AttrRotSpeedPCTAdd,           false);
            collection.SetAttr(EAttrType.AttrMaxHpAdd,                 attrRow.AttrMaxHpAdd,                 false);
            collection.SetAttr(EAttrType.AttrAttackAdd,                attrRow.AttrAttackAdd,                false);
            collection.SetAttr(EAttrType.AttrCritAdd,                  attrRow.AttrCritAdd,                  false);
            collection.SetAttr(EAttrType.AttrDefenseAdd,               attrRow.AttrDefenseAdd,               false);
            collection.SetAttr(EAttrType.AttrHealAdd,                  attrRow.AttrHealAdd,                  false);
            collection.SetAttr(EAttrType.AttrHealedAdd,                attrRow.AttrHealedAdd,                false);
            collection.SetAttr(EAttrType.AttrCritDamageAdd,            attrRow.AttrCritDamageAdd,            false);
            collection.SetAttr(EAttrType.AttrDmgAAdd,                  attrRow.AttrDmgAAdd,                  false);
            collection.SetAttr(EAttrType.AttrDmgRedAdd,                attrRow.AttrDmgRedAdd,                false);
            collection.SetAttr(EAttrType.AttrHealIntensityAdd,         attrRow.AttrHealIntensityAdd,         false);
            collection.SetAttr(EAttrType.AttrIgnoreDefenseAdd,         attrRow.AttrIgnoreDefenseAdd,         false);
            collection.SetAttr(EAttrType.AttrAttackSpeedPCTAdd,        attrRow.AttrAttackSpeedPCTAdd,        false);
            collection.SetAttr(EAttrType.AttrElementPenPCTAdd,         attrRow.AttrElementPenPCTAdd,         false);
            collection.SetAttr(EAttrType.AttrElementMagicRedPCTAdd,    attrRow.AttrElementMagicRedPCTAdd,    false);
            collection.SetAttr(EAttrType.AttrElementPhysicalPenPCTAdd, attrRow.AttrElementPhysicalPenPCTAdd, false);
            collection.SetAttr(EAttrType.AttrElementPhysicalRedPCTAdd, attrRow.AttrElementPhysicalRedPCTAdd, false);

            foreach (var linksValue in AttrLinks.Values)
            {
                linksValue.UpdateLink(collection);
            }
        }
    }
}