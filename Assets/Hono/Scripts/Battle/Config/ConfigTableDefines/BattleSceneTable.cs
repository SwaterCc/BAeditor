using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace Hono.Scripts.Battle
{
    public partial class BattleSceneTable : ITableHelper
    {
        private readonly Dictionary<int, BattleSceneRow> _tableData = new();

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
                        var row = Activator.CreateInstance<BattleSceneRow>();
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

        private void addRow(int id, BattleSceneRow row)
        {
            if (!_tableData.TryAdd(id, row))
            {
                Debug.LogError($"{typeof(BattleSceneRow)} TryAdd {id} id重复");
            }
        }

        public BattleSceneRow Get(int id)
        {
            return _tableData[id];
        }

        public bool TryGet(int id, out BattleSceneRow data)
        {
            return _tableData.TryGetValue(id, out data);
        }

       
    }

    public partial class BattleSceneTable
    {
        public class BattleSceneRow : TableRow
        {
           
            /// <summary>
            /// 描述
            /// </summary>
            public string Desc { get; private set; }
            
            /// <summary>
            /// 场景路径
            /// </summary>
            public string ScenePath { get; private set; }
            
            /// <summary>
            /// 最大队伍数量
            /// </summary>
            public int TeamCount { get; private set; }
            
            /// <summary>
            /// 战斗类型
            /// </summary>
            public int BattleType { get; private set; }
            

            public BattleSceneRow()
            {
                Parser = new BattleSceneRowCSVParser(this);
            }

            private class BattleSceneRowCSVParser : CSVParser
            {
                private BattleSceneRow _row;

                public BattleSceneRowCSVParser(BattleSceneRow row) : base(row)
                {
                    _row = (BattleSceneRow)base._row;
                }

                protected override void onParse(string[] line)
                {
                    
                    _row.Desc = parseString(line[1]);
            
                    _row.ScenePath = parseString(line[2]);
            
                    _row.TeamCount = parseInt(line[3]);
            
                    _row.BattleType = parseInt(line[4]);
            
                }
            }
        }
    }
}