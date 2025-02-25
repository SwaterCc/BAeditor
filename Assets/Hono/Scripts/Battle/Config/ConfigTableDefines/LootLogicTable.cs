using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace Hono.Scripts.Battle
{
    public partial class LootLogicTable : ITableHelper
    {
        private readonly Dictionary<int, LootLogicRow> _tableData = new();

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
                        var row = Activator.CreateInstance<LootLogicRow>();
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

        private void addRow(int id, LootLogicRow row)
        {
            if (!_tableData.TryAdd(id, row))
            {
                Debug.LogError($"{typeof(LootLogicRow)} TryAdd {id} id重复");
            }
        }

        public LootLogicRow Get(int id)
        {
            return _tableData[id];
        }

        public bool TryGet(int id, out LootLogicRow data)
        {
            return _tableData.TryGetValue(id, out data);
        }

         public Dictionary<int, LootLogicRow> GetTable()
         {
             return _tableData;
         }
    }

    public partial class LootLogicTable
    {
        public class LootLogicRow : TableRow
        {
           
            /// <summary>
            /// 描述
            /// </summary>
            public string Desc { get; private set; }
            
            /// <summary>
            /// 功能类型
            /// </summary>
            public int LootFunctionType { get; private set; }
            
            /// <summary>
            /// 参数
            /// </summary>
            public IntArray LootParam { get; private set; }
            

            public LootLogicRow()
            {
                Parser = new LootLogicRowCSVParser(this);
            }

            private class LootLogicRowCSVParser : CSVParser
            {
                private LootLogicRow _row;

                public LootLogicRowCSVParser(LootLogicRow row) : base(row)
                {
                    _row = (LootLogicRow)base._row;
                }

                protected override void onParse(string[] line)
                {
                    
                    _row.Desc = parseString(line[1]);
            
                    _row.LootFunctionType = parseInt(line[2]);
            
                    _row.LootParam = parseIntArray(line[3]);
            
                }
            }
        }
    }
}