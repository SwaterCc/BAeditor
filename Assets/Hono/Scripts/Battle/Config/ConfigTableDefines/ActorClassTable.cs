using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace Hono.Scripts.Battle
{
    public partial class ActorClassTable : ITableHelper
    {
        private readonly Dictionary<int, ActorClassRow> _tableData = new();

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
                        var row = Activator.CreateInstance<ActorClassRow>();
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

        private void addRow(int id, ActorClassRow row)
        {
            if (!_tableData.TryAdd(id, row))
            {
                Debug.LogError($"{typeof(ActorClassRow)} TryAdd {id} id重复");
            }
        }

        public ActorClassRow Get(int id)
        {
            return _tableData[id];
        }

        public bool TryGet(int id, out ActorClassRow data)
        {
            return _tableData.TryGetValue(id, out data);
        }

       
    }

    public partial class ActorClassTable
    {
        public class ActorClassRow : TableRow
        {
           
            /// <summary>
            /// 职业类型
            /// </summary>
            public int ActorClassType { get; private set; }
            
            /// <summary>
            /// 职业技能
            /// </summary>
            public IntArray Skills { get; private set; }
            
            /// <summary>
            /// 职业Buff
            /// </summary>
            public IntArray Buffs { get; private set; }
            

            public ActorClassRow()
            {
                Parser = new ActorClassRowCSVParser(this);
            }

            private class ActorClassRowCSVParser : CSVParser
            {
                private ActorClassRow _row;

                public ActorClassRowCSVParser(ActorClassRow row) : base(row)
                {
                    _row = (ActorClassRow)base._row;
                }

                protected override void onParse(string[] line)
                {
                    
                    _row.ActorClassType = parseInt(line[1]);
            
                    _row.Skills = parseIntArray(line[2]);
            
                    _row.Buffs = parseIntArray(line[3]);
            
                }
            }
        }
    }
}