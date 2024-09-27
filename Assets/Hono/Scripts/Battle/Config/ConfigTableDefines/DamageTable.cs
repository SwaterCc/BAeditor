using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace Hono.Scripts.Battle
{
    public partial class DamageTable : ITableHelper
    {
        private readonly Dictionary<int, DamageRow> _tableData = new();

        public bool LoadCSV(string csvFile)
        {
            try
            {
                using (StringReader  reader = new StringReader (csvFile))
                {
                    int lineCount = 0;
                    while (reader.ReadLine() is { } line)
                    {
                        if (++lineCount <= 3)
                        {
                            continue;
                        }
                        var row = Activator.CreateInstance<DamageRow>();
                        row.Parser.Parse(line);
                        addRow(row.Id, row);
                    }
                }
            }
            catch (Exception e)
            {
                Debug.LogError(e);
                return false;
            }

            return true;
        }

        public TableRow GetTableRow(int id)
        {
            if (TryGet(id, out var row))
            {
                return row;
            }

            return null;
        }

        private void addRow(int id, DamageRow row)
        {
            if (!_tableData.TryAdd(id, row))
            {
                Debug.LogError($"{typeof(DamageRow)} TryAdd {id} id重复");
            }
        }

        public DamageRow Get(int id)
        {
            return _tableData[id];
        }

        public bool TryGet(int id, out DamageRow data)
        {
            return _tableData.TryGetValue(id, out data);
        }

       
    }

    public partial class DamageTable
    {
        public class DamageRow : TableRow
        {
           
            /// <summary>
            /// 策划描述
            /// </summary>
            public string Desc { get; private set; }
            
            /// <summary>
            /// 伤害计算公式
            /// </summary>
            public string FormulaName { get; private set; }
            
            /// <summary>
            /// 伤害倍率万分比
            /// </summary>
            public int DamageRatio { get; private set; }
            
            /// <summary>
            /// 伤害类型(普通伤害，百分比伤害，Dot，治疗)
            /// </summary>
            public int DamageType { get; private set; }
            
            /// <summary>
            /// 元素类型（物理，法术）
            /// </summary>
            public int ElementType { get; private set; }
            
            /// <summary>
            /// 冲击力
            /// </summary>
            public int ImpactValue { get; private set; }
            
            /// <summary>
            /// 加值表配置
            /// </summary>
            public IntArray AdditiveId { get; private set; }
            
            /// <summary>
            /// 乘值表配置
            /// </summary>
            public IntArray MultiplyId { get; private set; }
            

            public DamageRow()
            {
                Parser = new DamageRowCSVParser(this);
            }

            private class DamageRowCSVParser : CSVParser
            {
                private DamageRow _row;

                public DamageRowCSVParser(DamageRow row) : base(row)
                {
                    _row = (DamageRow)base._row;
                }

                protected override void onParse(string[] line)
                {
                    
                    _row.Desc = parseString(line[1]);
            
                    _row.FormulaName = parseString(line[2]);
            
                    _row.DamageRatio = parseInt(line[3]);
            
                    _row.DamageType = parseInt(line[4]);
            
                    _row.ElementType = parseInt(line[5]);
            
                    _row.ImpactValue = parseInt(line[6]);
            
                    _row.AdditiveId = parseIntArray(line[7]);
            
                    _row.MultiplyId = parseIntArray(line[8]);
            
                }
            }
        }
    }
}