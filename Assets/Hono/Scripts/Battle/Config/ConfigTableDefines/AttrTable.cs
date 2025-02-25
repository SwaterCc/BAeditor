using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace Hono.Scripts.Battle
{
    public partial class AttrTable : ITableHelper
    {
        private readonly Dictionary<int, AttrRow> _tableData = new();

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
                        var row = Activator.CreateInstance<AttrRow>();
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

        private void addRow(int id, AttrRow row)
        {
            if (!_tableData.TryAdd(id, row))
            {
                Debug.LogError($"{typeof(AttrRow)} TryAdd {id} id重复");
            }
        }

        public AttrRow Get(int id)
        {
            return _tableData[id];
        }

        public bool TryGet(int id, out AttrRow data)
        {
            return _tableData.TryGetValue(id, out data);
        }

         public Dictionary<int, AttrRow> GetTable()
         {
             return _tableData;
         }
    }

    public partial class AttrTable
    {
        public class AttrRow : TableRow
        {
           
            /// <summary>
            /// 属性名字
            /// </summary>
            public string EnumName { get; private set; }
            
            /// <summary>
            /// 属性名字
            /// </summary>
            public string Name { get; private set; }
            
            /// <summary>
            /// 属性使用方式（int or float or bool）
            /// </summary>
            public string Type { get; private set; }
            
            /// <summary>
            /// 属性下限
            /// </summary>
            public int AttrLowerLimit { get; private set; }
            
            /// <summary>
            /// 属性上限
            /// </summary>
            public int AttrUpperLimit { get; private set; }
            
            /// <summary>
            /// 最终值
            /// </summary>
            public int AttrFinal { get; private set; }
            
            /// <summary>
            /// 总值
            /// </summary>
            public int AttrTotal { get; private set; }
            
            /// <summary>
            /// 加值
            /// </summary>
            public int AttrAdd { get; private set; }
            
            /// <summary>
            /// 额外加值
            /// </summary>
            public int AttrExAdd { get; private set; }
            
            /// <summary>
            /// 乘值
            /// </summary>
            public int AttrPer { get; private set; }
            
            /// <summary>
            /// 额外乘值
            /// </summary>
            public int AttrExPer { get; private set; }
            
            /// <summary>
            /// 外显名称
            /// </summary>
            public string OfficialName { get; private set; }
            
            /// <summary>
            /// 属性说明文本
            /// </summary>
            public string AttrDes { get; private set; }
            

            public AttrRow()
            {
                Parser = new AttrRowCSVParser(this);
            }

            private class AttrRowCSVParser : CSVParser
            {
                private AttrRow _row;

                public AttrRowCSVParser(AttrRow row) : base(row)
                {
                    _row = (AttrRow)base._row;
                }

                protected override void onParse(string[] line)
                {
                    
                    _row.EnumName = parseString(line[1]);
            
                    _row.Name = parseString(line[2]);
            
                    _row.Type = parseString(line[3]);
            
                    _row.AttrLowerLimit = parseInt(line[4]);
            
                    _row.AttrUpperLimit = parseInt(line[5]);
            
                    _row.AttrFinal = parseInt(line[6]);
            
                    _row.AttrTotal = parseInt(line[7]);
            
                    _row.AttrAdd = parseInt(line[8]);
            
                    _row.AttrExAdd = parseInt(line[9]);
            
                    _row.AttrPer = parseInt(line[10]);
            
                    _row.AttrExPer = parseInt(line[11]);
            
                    _row.OfficialName = parseString(line[12]);
            
                    _row.AttrDes = parseString(line[13]);
            
                }
            }
        }
    }
}