using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace Hono.Scripts.Battle
{
    public partial class ModelTable : ITableHelper
    {
        private readonly Dictionary<int, ModelRow> _tableData = new();

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
                        var row = Activator.CreateInstance<ModelRow>();
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

        private void addRow(int id, ModelRow row)
        {
            if (!_tableData.TryAdd(id, row))
            {
                Debug.LogError($"{typeof(ModelRow)} TryAdd {id} id重复");
            }
        }

        public ModelRow Get(int id)
        {
            return _tableData[id];
        }

        public bool TryGet(int id, out ModelRow data)
        {
            return _tableData.TryGetValue(id, out data);
        }

       
    }

    public partial class ModelTable
    {
        public class ModelRow : TableRow
        {
           
            /// <summary>
            /// 描述
            /// </summary>
            public string Desc { get; private set; }
            
            /// <summary>
            /// 模型半径
            /// </summary>
            public float ModelRadius { get; private set; }
            
            /// <summary>
            /// 模型路径
            /// </summary>
            public string ModelPath { get; private set; }
            
            /// <summary>
            /// 动画模板
            /// </summary>
            public string AnimTemplateId { get; private set; }
            

            public ModelRow()
            {
                Parser = new ModelRowCSVParser(this);
            }

            private class ModelRowCSVParser : CSVParser
            {
                private ModelRow _row;

                public ModelRowCSVParser(ModelRow row) : base(row)
                {
                    _row = (ModelRow)base._row;
                }

                protected override void onParse(string[] line)
                {
                    
                    _row.Desc = parseString(line[1]);
            
                    _row.ModelRadius = parseNumber(line[2]);
            
                    _row.ModelPath = parseString(line[3]);
            
                    _row.AnimTemplateId = parseString(line[4]);
            
                }
            }
        }
    }
}