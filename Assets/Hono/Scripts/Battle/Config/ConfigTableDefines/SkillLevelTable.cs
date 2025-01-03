using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace Hono.Scripts.Battle
{
    public partial class SkillLevelTable : ITableHelper
    {
        private readonly Dictionary<int, SkillLevelRow> _tableData = new();

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
                        var row = Activator.CreateInstance<SkillLevelRow>();
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

        private void addRow(int id, SkillLevelRow row)
        {
            if (!_tableData.TryAdd(id, row))
            {
                Debug.LogError($"{typeof(SkillLevelRow)} TryAdd {id} id重复");
            }
        }

        public SkillLevelRow Get(int id)
        {
            return _tableData[id];
        }

        public bool TryGet(int id, out SkillLevelRow data)
        {
            return _tableData.TryGetValue(id, out data);
        }

         public Dictionary<int, SkillLevelRow> GetTable()
         {
             return _tableData;
         }
    }

    public partial class SkillLevelTable
    {
        public class SkillLevelRow : TableRow
        {
           
            /// <summary>
            /// 描述
            /// </summary>
            public string Desc { get; private set; }
            
            /// <summary>
            /// Cd成长值
            /// </summary>
            public float CdGrowth { get; private set; }
            
            /// <summary>
            /// 技能可释放区域半径成长值
            /// </summary>
            public float RangeGrowth { get; private set; }
            
            /// <summary>
            /// 最大命中数量成长
            /// </summary>
            public int HitCeilingGrowth { get; private set; }
            
            /// <summary>
            /// 矩形筛选范围成长值
            /// </summary>
            public NumberArray RectGrowth { get; private set; }
            
            /// <summary>
            /// 圆形筛选半径成长值
            /// </summary>
            public float RadiusGrowth { get; private set; }
            
            /// <summary>
            /// 自定义参数
            /// </summary>
            public IntArray SkillParams { get; private set; }
            

            public SkillLevelRow()
            {
                Parser = new SkillLevelRowCSVParser(this);
            }

            private class SkillLevelRowCSVParser : CSVParser
            {
                private SkillLevelRow _row;

                public SkillLevelRowCSVParser(SkillLevelRow row) : base(row)
                {
                    _row = (SkillLevelRow)base._row;
                }

                protected override void onParse(string[] line)
                {
                    
                    _row.Desc = parseString(line[1]);
            
                    _row.CdGrowth = parseNumber(line[2]);
            
                    _row.RangeGrowth = parseNumber(line[3]);
            
                    _row.HitCeilingGrowth = parseInt(line[4]);
            
                    _row.RectGrowth = parseNumberArray(line[5]);
            
                    _row.RadiusGrowth = parseNumber(line[6]);
            
                    _row.SkillParams = parseIntArray(line[7]);
            
                }
            }
        }
    }
}