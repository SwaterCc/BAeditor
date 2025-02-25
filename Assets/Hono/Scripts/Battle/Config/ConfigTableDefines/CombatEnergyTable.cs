using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace Hono.Scripts.Battle
{
    public partial class CombatEnergyTable : ITableHelper
    {
        private readonly Dictionary<int, CombatEnergyRow> _tableData = new();

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
                        var row = Activator.CreateInstance<CombatEnergyRow>();
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

        private void addRow(int id, CombatEnergyRow row)
        {
            if (!_tableData.TryAdd(id, row))
            {
                Debug.LogError($"{typeof(CombatEnergyRow)} TryAdd {id} id重复");
            }
        }

        public CombatEnergyRow Get(int id)
        {
            return _tableData[id];
        }

        public bool TryGet(int id, out CombatEnergyRow data)
        {
            return _tableData.TryGetValue(id, out data);
        }

         public Dictionary<int, CombatEnergyRow> GetTable()
         {
             return _tableData;
         }
    }

    public partial class CombatEnergyTable
    {
        public class CombatEnergyRow : TableRow
        {
           
            /// <summary>
            /// 能量类型描述
            /// </summary>
            public string Desc { get; private set; }
            
            /// <summary>
            /// 能量初始值
            /// </summary>
            public int InitValue { get; private set; }
            
            /// <summary>
            /// 能量自然增长值每秒
            /// </summary>
            public int IdleGetValue { get; private set; }
            
            /// <summary>
            /// 受击获取值
            /// </summary>
            public int BehitGetValue { get; private set; }
            
            /// <summary>
            /// 攻击获取值
            /// </summary>
            public int AttackGetValue { get; private set; }
            
            /// <summary>
            /// 能量上限
            /// </summary>
            public int EnergyMax { get; private set; }
            

            public CombatEnergyRow()
            {
                Parser = new CombatEnergyRowCSVParser(this);
            }

            private class CombatEnergyRowCSVParser : CSVParser
            {
                private CombatEnergyRow _row;

                public CombatEnergyRowCSVParser(CombatEnergyRow row) : base(row)
                {
                    _row = (CombatEnergyRow)base._row;
                }

                protected override void onParse(string[] line)
                {
                    
                    _row.Desc = parseString(line[1]);
            
                    _row.InitValue = parseInt(line[2]);
            
                    _row.IdleGetValue = parseInt(line[3]);
            
                    _row.BehitGetValue = parseInt(line[4]);
            
                    _row.AttackGetValue = parseInt(line[5]);
            
                    _row.EnergyMax = parseInt(line[6]);
            
                }
            }
        }
    }
}