using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace Hono.Scripts.Battle
{
    public partial class DamageMultiplyTable : ITableHelper
    {
        private readonly Dictionary<int, DamageMultiplyRow> _tableData = new();

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
                        var row = Activator.CreateInstance<DamageMultiplyRow>();
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

        private void addRow(int id, DamageMultiplyRow row)
        {
            if (!_tableData.TryAdd(id, row))
            {
                Debug.LogError($"{typeof(DamageMultiplyRow)} TryAdd {id} id重复");
            }
        }

        public DamageMultiplyRow Get(int id)
        {
            return _tableData[id];
        }

        public bool TryGet(int id, out DamageMultiplyRow data)
        {
            return _tableData.TryGetValue(id, out data);
        }

       
    }

    public partial class DamageMultiplyTable
    {
        public class DamageMultiplyRow : TableRow
        {
           
            /// <summary>
            /// Apply增伤数值万分比
            /// </summary>
            public IntArray DamageValue { get; private set; }
            
            /// <summary>
            /// Apply数值的方法，可以在lua中自定义计算方法，通常保持默认即可
            /// </summary>
            public string ApplyFuncName { get; private set; }
            
            /// <summary>
            /// 增伤条件ID列表
            /// </summary>
            public IntArray ConditionIds { get; private set; }
            
            /// <summary>
            /// 增伤条件参数表
            /// </summary>
            public IntTable ConditionParams { get; private set; }
            

            public DamageMultiplyRow()
            {
                Parser = new DamageMultiplyRowCSVParser(this);
            }

            private class DamageMultiplyRowCSVParser : CSVParser
            {
                private DamageMultiplyRow _row;

                public DamageMultiplyRowCSVParser(DamageMultiplyRow row) : base(row)
                {
                    _row = (DamageMultiplyRow)base._row;
                }

                protected override void onParse(string[] line)
                {
                    
                    _row.DamageValue = parseIntArray(line[1]);
            
                    _row.ApplyFuncName = parseString(line[2]);
            
                    _row.ConditionIds = parseIntArray(line[3]);
            
                    _row.ConditionParams = parseIntTable(line[4]);
            
                }
            }
        }
    }
}