using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace Hono.Scripts.Battle
{
    public partial class PawnLogicTable : ITableHelper
    {
        private readonly Dictionary<int, PawnLogicRow> _tableData = new();

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
                        var row = Activator.CreateInstance<PawnLogicRow>();
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

        private void addRow(int id, PawnLogicRow row)
        {
            if (!_tableData.TryAdd(id, row))
            {
                Debug.LogError($"{typeof(PawnLogicRow)} TryAdd {id} id重复");
            }
        }

        public PawnLogicRow Get(int id)
        {
            return _tableData[id];
        }

        public bool TryGet(int id, out PawnLogicRow data)
        {
            return _tableData.TryGetValue(id, out data);
        }

       
    }

    public partial class PawnLogicTable
    {
        public class PawnLogicRow : TableRow
        {
           
            /// <summary>
            /// 描述
            /// </summary>
            public string Desc { get; private set; }
            
            /// <summary>
            /// 角色头像
            /// </summary>
            public string RPGIcon { get; private set; }
            
            /// <summary>
            /// 模型表Id
            /// </summary>
            public int ModelId { get; private set; }
            
            /// <summary>
            /// 职业ID
            /// </summary>
            public int ActorClassId { get; private set; }
            
            /// <summary>
            /// 初始阵营
            /// </summary>
            public int Faction { get; private set; }
            
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
            public IntArray ownerOtherAbility { get; private set; }
            

            public PawnLogicRow()
            {
                Parser = new PawnLogicRowCSVParser(this);
            }

            private class PawnLogicRowCSVParser : CSVParser
            {
                private PawnLogicRow _row;

                public PawnLogicRowCSVParser(PawnLogicRow row) : base(row)
                {
                    _row = (PawnLogicRow)base._row;
                }

                protected override void onParse(string[] line)
                {
                    
                    _row.Desc = parseString(line[1]);
            
                    _row.RPGIcon = parseString(line[2]);
            
                    _row.ModelId = parseInt(line[3]);
            
                    _row.ActorClassId = parseInt(line[4]);
            
                    _row.Faction = parseInt(line[5]);
            
                    _row.AttrTemplateId = parseInt(line[6]);
            
                    _row.OwnerSkills = parseIntTable(line[7]);
            
                    _row.OwnerBuffs = parseIntArray(line[8]);
            
                    _row.ownerOtherAbility = parseIntArray(line[9]);
            
                }
            }
        }
    }
}