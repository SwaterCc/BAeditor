using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace Hono.Scripts.Battle
{
    public partial class ActorPrototypeTable : ITableHelper
    {
        private readonly Dictionary<int, ActorPrototypeRow> _tableData = new();

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
                        var row = Activator.CreateInstance<ActorPrototypeRow>();
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

        private void addRow(int id, ActorPrototypeRow row)
        {
            if (!_tableData.TryAdd(id, row))
            {
                Debug.LogError($"{typeof(ActorPrototypeRow)} TryAdd {id} id÷ÿ∏¥");
            }
        }

        public ActorPrototypeRow Get(int id)
        {
            return _tableData[id];
        }

        public bool TryGet(int id, out ActorPrototypeRow data)
        {
            return _tableData.TryGetValue(id, out data);
        }

       
    }

    public partial class ActorPrototypeTable
    {
        public class ActorPrototypeRow : TableRow
        {
           
            /// <summary>
            /// ???????ID
            /// </summary>
            public int LogicConfigId { get; private set; }
            
            /// <summary>
            /// ????????ID
            /// </summary>
            public int ShowConfigId { get; private set; }
            
            /// <summary>
            /// Actor????
            /// </summary>
            public int ActorType { get; private set; }
            

            public ActorPrototypeRow()
            {
                Parser = new ActorPrototypeRowCSVParser(this);
            }

            private class ActorPrototypeRowCSVParser : CSVParser
            {
                private ActorPrototypeRow _row;

                public ActorPrototypeRowCSVParser(ActorPrototypeRow row) : base(row)
                {
                    _row = (ActorPrototypeRow)base._row;
                }

                protected override void onParse(string[] line)
                {
                    
            _row.LogicConfigId = parseInt(line[1]);
            
            _row.ShowConfigId = parseInt(line[2]);
            
            _row.ActorType = parseInt(line[3]);
            
                }
            }
        }
    }
}