using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace Hono.Scripts.Battle
{
    public partial class MonsterGenerateTmpTable : ITableHelper
    {
        private readonly Dictionary<int, MonsterGenerateTmpRow> _tableData = new();

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
                        var row = Activator.CreateInstance<MonsterGenerateTmpRow>();
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

        private void addRow(int id, MonsterGenerateTmpRow row)
        {
            if (!_tableData.TryAdd(id, row))
            {
                Debug.LogError($"{typeof(MonsterGenerateTmpRow)} TryAdd {id} id重复");
            }
        }

        public MonsterGenerateTmpRow Get(int id)
        {
            return _tableData[id];
        }

        public bool TryGet(int id, out MonsterGenerateTmpRow data)
        {
            return _tableData.TryGetValue(id, out data);
        }

       
    }

    public partial class MonsterGenerateTmpTable
    {
        public class MonsterGenerateTmpRow : TableRow
        {
           
            /// <summary>
            /// 描述
            /// </summary>
            public string Desc { get; private set; }
            
            /// <summary>
            /// 延迟时间
            /// </summary>
            public float DelayTime { get; private set; }
            
            /// <summary>
            /// 创建间隔
            /// </summary>
            public float Interval { get; private set; }
            
            /// <summary>
            /// 怪物列表，按顺序刷新
            /// </summary>
            public IntTable MonsterInfos { get; private set; }
            
            /// <summary>
            /// 创建时添加的buff
            /// </summary>
            public IntTable ExBuffs { get; private set; }
            
            /// <summary>
            /// 额外的技能
            /// </summary>
            public IntTable ExSkills { get; private set; }
            
            /// <summary>
            /// 阵营覆盖
            /// </summary>
            public int FactionId { get; private set; }
            
            /// <summary>
            /// 额外Tag
            /// </summary>
            public IntArray ExTags { get; private set; }
            

            public MonsterGenerateTmpRow()
            {
                Parser = new MonsterGenerateTmpRowCSVParser(this);
            }

            private class MonsterGenerateTmpRowCSVParser : CSVParser
            {
                private MonsterGenerateTmpRow _row;

                public MonsterGenerateTmpRowCSVParser(MonsterGenerateTmpRow row) : base(row)
                {
                    _row = (MonsterGenerateTmpRow)base._row;
                }

                protected override void onParse(string[] line)
                {
                    
                    _row.Desc = parseString(line[1]);
            
                    _row.DelayTime = parseNumber(line[2]);
            
                    _row.Interval = parseNumber(line[3]);
            
                    _row.MonsterInfos = parseIntTable(line[4]);
            
                    _row.ExBuffs = parseIntTable(line[5]);
            
                    _row.ExSkills = parseIntTable(line[6]);
            
                    _row.FactionId = parseInt(line[7]);
            
                    _row.ExTags = parseIntArray(line[8]);
            
                }
            }
        }
    }
}