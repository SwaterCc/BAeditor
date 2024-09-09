using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace Hono.Scripts.Battle
{
    public partial class ActorLogicTable : ITableHelper
    {
        private readonly Dictionary<int, ActorLogicRow> _tableData = new();

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
                        var row = Activator.CreateInstance<ActorLogicRow>();
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

        private void addRow(int id, ActorLogicRow row)
        {
            if (!_tableData.TryAdd(id, row))
            {
                Debug.LogError($"{typeof(ActorLogicRow)} TryAdd {id} id重复");
            }
        }

        public ActorLogicRow Get(int id)
        {
            return _tableData[id];
        }

        public bool TryGet(int id, out ActorLogicRow data)
        {
            return _tableData.TryGetValue(id, out data);
        }

       
    }

    public partial class ActorLogicTable
    {
        public class ActorLogicRow : TableRow
        {
           
            /// <summary>
            /// 逻辑类型(玩家养成角色0，怪物1，建筑物2，打击点3)
            /// </summary>
            public int LogicType { get; private set; }
            
            /// <summary>
            /// 职业ID
            /// </summary>
            public int ActorClassId { get; private set; }
            
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
            

            public ActorLogicRow()
            {
                Parser = new ActorLogicRowCSVParser(this);
            }

            private class ActorLogicRowCSVParser : CSVParser
            {
                private ActorLogicRow _row;

                public ActorLogicRowCSVParser(ActorLogicRow row) : base(row)
                {
                    _row = (ActorLogicRow)base._row;
                }

                protected override void onParse(string[] line)
                {
                    
                    _row.LogicType = parseInt(line[1]);
            
                    _row.ActorClassId = parseInt(line[2]);
            
                    _row.AttrTemplateId = parseInt(line[3]);
            
                    _row.OwnerSkills = parseIntTable(line[4]);
            
                    _row.OwnerBuffs = parseIntArray(line[5]);
            
                    _row.ownerOtherAbility = parseIntArray(line[6]);
            
                }
            }
        }
    }
}