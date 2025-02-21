using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace Hono.Scripts.Battle
{
    public partial class TagTable : ITableHelper
    {
        private readonly Dictionary<int, TagRow> _tableData = new();

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
                        var row = Activator.CreateInstance<TagRow>();
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

        private void addRow(int id, TagRow row)
        {
            if (!_tableData.TryAdd(id, row))
            {
                Debug.LogError($"{typeof(TagRow)} TryAdd {id} id重复");
            }
        }

        public TagRow Get(int id)
        {
            return _tableData[id];
        }

        public bool TryGet(int id, out TagRow data)
        {
            return _tableData.TryGetValue(id, out data);
        }

         public Dictionary<int, TagRow> GetTable()
         {
             return _tableData;
         }
    }

    public partial class TagTable
    {
        public class TagRow : TableRow
        {
           
            /// <summary>
            /// TAG名称
            /// </summary>
            public string TagName { get; private set; }
            
            /// <summary>
            /// TAG释义
            /// </summary>
            public string TagString { get; private set; }
            
            /// <summary>
            /// TAG类型，1=单位tag、2=技能BDTag
            /// </summary>
            public int TagType { get; private set; }
            
            /// <summary>
            /// 父TAGId
            /// </summary>
            public int ParentTag { get; private set; }
            

            public TagRow()
            {
                Parser = new TagRowCSVParser(this);
            }

            private class TagRowCSVParser : CSVParser
            {
                private TagRow _row;

                public TagRowCSVParser(TagRow row) : base(row)
                {
                    _row = (TagRow)base._row;
                }

                protected override void onParse(string[] line)
                {
                    
                    _row.TagName = parseString(line[1]);
            
                    _row.TagString = parseString(line[2]);
            
                    _row.TagType = parseInt(line[3]);
            
                    _row.ParentTag = parseInt(line[4]);
            
                }
            }
        }
    }
}