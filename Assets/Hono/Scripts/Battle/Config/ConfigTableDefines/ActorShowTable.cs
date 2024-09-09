using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace Hono.Scripts.Battle
{
    public partial class ActorShowTable : ITableHelper
    {
        private readonly Dictionary<int, ActorShowRow> _tableData = new();

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
                        var row = Activator.CreateInstance<ActorShowRow>();
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

        private void addRow(int id, ActorShowRow row)
        {
            if (!_tableData.TryAdd(id, row))
            {
                Debug.LogError($"{typeof(ActorShowRow)} TryAdd {id} id重复");
            }
        }

        public ActorShowRow Get(int id)
        {
            return _tableData[id];
        }

        public bool TryGet(int id, out ActorShowRow data)
        {
            return _tableData.TryGetValue(id, out data);
        }

       
    }

    public partial class ActorShowTable
    {
        public class ActorShowRow : TableRow
        {
           
            /// <summary>
            /// 描述
            /// </summary>
            public string Desc { get; private set; }
            
            /// <summary>
            /// 表现层类型(测试胶囊体0，帧动画1)
            /// </summary>
            public int ShowType { get; private set; }
            
            /// <summary>
            /// 模型路径
            /// </summary>
            public string ModelPath { get; private set; }
            
            /// <summary>
            /// 动画模板
            /// </summary>
            public string AnimTemplateId { get; private set; }
            

            public ActorShowRow()
            {
                Parser = new ActorShowRowCSVParser(this);
            }

            private class ActorShowRowCSVParser : CSVParser
            {
                private ActorShowRow _row;

                public ActorShowRowCSVParser(ActorShowRow row) : base(row)
                {
                    _row = (ActorShowRow)base._row;
                }

                protected override void onParse(string[] line)
                {
                    
                    _row.Desc = parseString(line[1]);
            
                    _row.ShowType = parseInt(line[2]);
            
                    _row.ModelPath = parseString(line[3]);
            
                    _row.AnimTemplateId = parseString(line[4]);
            
                }
            }
        }
    }
}