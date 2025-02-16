using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace Hono.Scripts.Battle
{
    public partial class ModelTable : ITableHelper
    {
        private readonly Dictionary<int, ModelRow> _tableData = new();

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
                        var row = Activator.CreateInstance<ModelRow>();
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

        private void addRow(int id, ModelRow row)
        {
            if (!_tableData.TryAdd(id, row))
            {
                Debug.LogError($"{typeof(ModelRow)} TryAdd {id} id重复");
            }
        }

        public ModelRow Get(int id)
        {
            return _tableData[id];
        }

        public bool TryGet(int id, out ModelRow data)
        {
            return _tableData.TryGetValue(id, out data);
        }

         public Dictionary<int, ModelRow> GetTable()
         {
             return _tableData;
         }
    }

    public partial class ModelTable
    {
        public class ModelRow : TableRow
        {
           
            /// <summary>
            /// ����
            /// </summary>
            public string Desc { get; private set; }
            
            /// <summary>
            /// UO��������
            /// </summary>
            public int UOProxyType { get; private set; }
            
            /// <summary>
            /// ����Layer��
            /// </summary>
            public int ProxyLayer { get; private set; }
            
            /// <summary>
            /// ������Դģ��Id
            /// </summary>
            public int ResReplTplId { get; private set; }
            
            /// <summary>
            /// ģ������
            /// </summary>
            public float ModelScale { get; private set; }
            
            /// <summary>
            /// P1(���Σ�������뾶��������ĳ�)
            /// </summary>
            public float P1 { get; private set; }
            
            /// <summary>
            /// �����壬������ĸ�
            /// </summary>
            public float P2 { get; private set; }
            
            /// <summary>
            /// ������Ŀ�
            /// </summary>
            public float P3 { get; private set; }
            
            /// <summary>
            /// ColliderCenter
            /// </summary>
            public NumberArray ColliderCenter { get; private set; }
            

            public ModelRow()
            {
                Parser = new ModelRowCSVParser(this);
            }

            private class ModelRowCSVParser : CSVParser
            {
                private ModelRow _row;

                public ModelRowCSVParser(ModelRow row) : base(row)
                {
                    _row = (ModelRow)base._row;
                }

                protected override void onParse(string[] line)
                {
                    
                    _row.Desc = parseString(line[1]);
            
                    _row.UOProxyType = parseInt(line[2]);
            
                    _row.ProxyLayer = parseInt(line[3]);
            
                    _row.ResReplTplId = parseInt(line[4]);
            
                    _row.ModelScale = parseNumber(line[5]);
            
                    _row.P1 = parseNumber(line[6]);
            
                    _row.P2 = parseNumber(line[7]);
            
                    _row.P3 = parseNumber(line[8]);
            
                    _row.ColliderCenter = parseNumberArray(line[9]);
            
                }
            }
        }
    }
}