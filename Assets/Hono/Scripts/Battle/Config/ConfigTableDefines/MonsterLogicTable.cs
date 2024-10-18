using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace Hono.Scripts.Battle
{
    public partial class MonsterLogicTable : ITableHelper
    {
        private readonly Dictionary<int, MonsterLogicRow> _tableData = new();

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
                        var row = Activator.CreateInstance<MonsterLogicRow>();
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

        private void addRow(int id, MonsterLogicRow row)
        {
            if (!_tableData.TryAdd(id, row))
            {
                Debug.LogError($"{typeof(MonsterLogicRow)} TryAdd {id} id重复");
            }
        }

        public MonsterLogicRow Get(int id)
        {
            return _tableData[id];
        }

        public bool TryGet(int id, out MonsterLogicRow data)
        {
            return _tableData.TryGetValue(id, out data);
        }

       
    }

    public partial class MonsterLogicTable
    {
        public class MonsterLogicRow : TableRow
        {
           
            /// <summary>
            /// 名字
            /// </summary>
            public string Name { get; private set; }
            
            /// <summary>
            /// 描述
            /// </summary>
            public string Desc { get; private set; }
            
            /// <summary>
            /// 职业ID
            /// </summary>
            public int ActorClassId { get; private set; }
            
            /// <summary>
            /// 初始阵营
            /// </summary>
            public int Faction { get; private set; }
            
            /// <summary>
            /// 初始标签
            /// </summary>
            public IntArray TagList { get; private set; }
            
            /// <summary>
            /// 模型表Id
            /// </summary>
            public int ModelId { get; private set; }
            
            /// <summary>
            /// 初始化属性模板Id
            /// </summary>
            public int AttrTemplateId { get; private set; }
            
            /// <summary>
            /// 拥有技能
            /// </summary>
            public IntTable OwnerSkills { get; private set; }
            
            /// <summary>
            /// 拥有Buff
            /// </summary>
            public IntArray OwnerBuffs { get; private set; }
            
            /// <summary>
            /// 拥有的其他Ability
            /// </summary>
            public IntTable ownerOtherAbility { get; private set; }
            
            /// <summary>
            /// 不吃位移控制
            /// </summary>
            public int IgnoreOtherMotion { get; private set; }
            

            public MonsterLogicRow()
            {
                Parser = new MonsterLogicRowCSVParser(this);
            }

            private class MonsterLogicRowCSVParser : CSVParser
            {
                private MonsterLogicRow _row;

                public MonsterLogicRowCSVParser(MonsterLogicRow row) : base(row)
                {
                    _row = (MonsterLogicRow)base._row;
                }

                protected override void onParse(string[] line)
                {
                    
                    _row.Name = parseString(line[1]);
            
                    _row.Desc = parseString(line[2]);
            
                    _row.ActorClassId = parseInt(line[3]);
            
                    _row.Faction = parseInt(line[4]);
            
                    _row.TagList = parseIntArray(line[5]);
            
                    _row.ModelId = parseInt(line[6]);
            
                    _row.AttrTemplateId = parseInt(line[7]);
            
                    _row.OwnerSkills = parseIntTable(line[8]);
            
                    _row.OwnerBuffs = parseIntArray(line[9]);
            
                    _row.ownerOtherAbility = parseIntTable(line[10]);
            
                    _row.IgnoreOtherMotion = parseInt(line[11]);
            
                }
            }
        }
    }
}