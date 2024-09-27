using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace Hono.Scripts.Battle
{
    public partial class HitBoxLogicTable : ITableHelper
    {
        private readonly Dictionary<int, HitBoxLogicRow> _tableData = new();

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
                        var row = Activator.CreateInstance<HitBoxLogicRow>();
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

        private void addRow(int id, HitBoxLogicRow row)
        {
            if (!_tableData.TryAdd(id, row))
            {
                Debug.LogError($"{typeof(HitBoxLogicRow)} TryAdd {id} id重复");
            }
        }

        public HitBoxLogicRow Get(int id)
        {
            return _tableData[id];
        }

        public bool TryGet(int id, out HitBoxLogicRow data)
        {
            return _tableData.TryGetValue(id, out data);
        }

       
    }

    public partial class HitBoxLogicTable
    {
        public class HitBoxLogicRow : TableRow
        {
           
            /// <summary>
            /// 描述
            /// </summary>
            public string Desc { get; private set; }
            
            /// <summary>
            /// 模型表Id
            /// </summary>
            public int Model { get; private set; }
            
            /// <summary>
            /// 拥有的其他Ability
            /// </summary>
            public IntArray ownerOtherAbility { get; private set; }
            

            public HitBoxLogicRow()
            {
                Parser = new HitBoxLogicRowCSVParser(this);
            }

            private class HitBoxLogicRowCSVParser : CSVParser
            {
                private HitBoxLogicRow _row;

                public HitBoxLogicRowCSVParser(HitBoxLogicRow row) : base(row)
                {
                    _row = (HitBoxLogicRow)base._row;
                }

                protected override void onParse(string[] line)
                {
                    
                    _row.Desc = parseString(line[1]);
            
                    _row.Model = parseInt(line[2]);
            
                    _row.ownerOtherAbility = parseIntArray(line[3]);
            
                }
            }
        }
    }
}