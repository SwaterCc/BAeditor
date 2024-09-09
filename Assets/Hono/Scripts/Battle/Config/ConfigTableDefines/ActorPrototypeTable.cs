﻿using System;
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
                Debug.LogError($"{typeof(ActorPrototypeRow)} TryAdd {id} id重复");
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
            /// 原型描述
            /// </summary>
            public string Desc { get; private set; }
            
            /// <summary>
            /// 逻辑ID(ActorLogicTableId)
            /// </summary>
            public int LogicConfigId { get; private set; }
            
            /// <summary>
            /// 表现ID(ActorShowTableId)
            /// </summary>
            public int ShowConfigId { get; private set; }
            
            /// <summary>
            /// Actor类型（人物0，怪物1，建筑2，打击盒3，Npc4）
            /// </summary>
            public int ActorType { get; private set; }
            
            /// <summary>
            /// 角色头像
            /// </summary>
            public string RPGIcon { get; private set; }
            

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
                    
                    _row.Desc = parseString(line[1]);
            
                    _row.LogicConfigId = parseInt(line[2]);
            
                    _row.ShowConfigId = parseInt(line[3]);
            
                    _row.ActorType = parseInt(line[4]);
            
                    _row.RPGIcon = parseString(line[5]);
            
                }
            }
        }
    }
}