using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace Hono.Scripts.Battle
{
    public partial class BuildingLogicTable : ITableHelper
    {
        private readonly Dictionary<int, BuildingLogicRow> _tableData = new();

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
                        var row = Activator.CreateInstance<BuildingLogicRow>();
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

        private void addRow(int id, BuildingLogicRow row)
        {
            if (!_tableData.TryAdd(id, row))
            {
                Debug.LogError($"{typeof(BuildingLogicRow)} TryAdd {id} id重复");
            }
        }

        public BuildingLogicRow Get(int id)
        {
            return _tableData[id];
        }

        public bool TryGet(int id, out BuildingLogicRow data)
        {
            return _tableData.TryGetValue(id, out data);
        }

       
    }

    public partial class BuildingLogicTable
    {
        public class BuildingLogicRow : TableRow
        {
           
            /// <summary>
            /// 描述
            /// </summary>
            public string Desc { get; private set; }
            
            /// <summary>
            /// 建筑Icon
            /// </summary>
            public string RPGIcon { get; private set; }
            
            /// <summary>
            /// 模型表Id
            /// </summary>
            public int Model { get; private set; }
            
            /// <summary>
            /// 初始阵营
            /// </summary>
            public int Faction { get; private set; }
            
            /// <summary>
            /// 初始化属性模板Id
            /// </summary>
            public int AttrTemplateId { get; private set; }
            
            /// <summary>
            /// 拥有Buff
            /// </summary>
            public IntArray OwnerBuffs { get; private set; }
            
            /// <summary>
            /// 拥有的其他Ability
            /// </summary>
            public IntArray ownerOtherAbility { get; private set; }
            

            public BuildingLogicRow()
            {
                Parser = new BuildingLogicRowCSVParser(this);
            }

            private class BuildingLogicRowCSVParser : CSVParser
            {
                private BuildingLogicRow _row;

                public BuildingLogicRowCSVParser(BuildingLogicRow row) : base(row)
                {
                    _row = (BuildingLogicRow)base._row;
                }

                protected override void onParse(string[] line)
                {
                    
                    _row.Desc = parseString(line[1]);
            
                    _row.RPGIcon = parseString(line[2]);
            
                    _row.Model = parseInt(line[3]);
            
                    _row.Faction = parseInt(line[4]);
            
                    _row.AttrTemplateId = parseInt(line[5]);
            
                    _row.OwnerBuffs = parseIntArray(line[6]);
            
                    _row.ownerOtherAbility = parseIntArray(line[7]);
            
                }
            }
        }
    }
}