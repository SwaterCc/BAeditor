using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace Hono.Scripts.Battle
{
    public partial class DamageAdditiveTable : ITableHelper
    {
        private readonly Dictionary<int, DamageAdditiveRow> _tableData = new();

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
                        var row = Activator.CreateInstance<DamageAdditiveRow>();
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

        private void addRow(int id, DamageAdditiveRow row)
        {
            if (!_tableData.TryAdd(id, row))
            {
                Debug.LogError($"{typeof(DamageAdditiveRow)} TryAdd {id} id重复");
            }
        }

        public DamageAdditiveRow Get(int id)
        {
            return _tableData[id];
        }

        public bool TryGet(int id, out DamageAdditiveRow data)
        {
            return _tableData.TryGetValue(id, out data);
        }

       
    }

    public partial class DamageAdditiveTable
    {
        public class DamageAdditiveRow : TableRow
        {
           
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
            
            /// <summary>
            /// Apply增伤数值万分比
            /// </summary>
            public IntArray DamageValue { get; private set; }
            

            public DamageAdditiveRow()
            {
                Parser = new DamageAdditiveRowCSVParser(this);
            }

            private class DamageAdditiveRowCSVParser : CSVParser
            {
                private DamageAdditiveRow _row;

                public DamageAdditiveRowCSVParser(DamageAdditiveRow row) : base(row)
                {
                    _row = (DamageAdditiveRow)base._row;
                }

                protected override void onParse(string[] line)
                {
                    
                    _row.ApplyFuncName = parseString(line[1]);
            
                    _row.ConditionIds = parseIntArray(line[2]);
            
                    _row.ConditionParams = parseIntTable(line[3]);
            
                    _row.DamageValue = parseIntArray(line[4]);
            
                }
            }
        }
    }
}