using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Hono.Scripts.Battle;
using UnityEditor;
using UnityEngine;

namespace Editor.BattleEditor.CSVConfig
{
    public static class CSVReaderMakerInterface
    {
        [MenuItem("Tools/生成Config")]
        public static void GenConfig()
        {
            CSVReaderMaker maker = new CSVReaderMaker();
            maker.GenConfigReaders();
            Debug.Log("生成类完成");
        }
    }
    
    
    
    public class CSVReaderMaker
    {
        private string _classCodeTmp;
        private readonly string _replaceNameSpaceStr = "@nameSpace";
        private readonly string _replaceTableNameStr = "@tableName";
        private readonly string _replaceTableRowNameStr = "@tableRowName";
        private readonly string _replaceTableRowPropertyStr = "@tableRowProperty";
        private readonly string _replaceTableRowPropertyParseStr = "@tableRowParse";


        private readonly string _configPathRoot = "Assets/BattleData/CSV";
        private readonly string _genCShapePath = "Assets/Hono/Scripts/Battle/Config/ConfigTableDefines";
        private string[] _files;


        private readonly string _propertyTmp =
            @"
            /// <summary>
            /// @PropertyDesc
            /// </summary>
            public @PropertyType @PropertyName { get; private set; }
            ";

        private readonly string _parseFuncTmp = @"
                    _row.@PropertyName = @ParseFunc(line[@index]);
            ";

        private readonly Dictionary<string, string> _parseTypeDict = new Dictionary<string, string>()
        {
            { "int", "int" },
            { "number", "float" },
            { "float", "float" },
            { "bool", "bool" },
            { "string", "string" },
            { "intarray", "IntArray" },
            { "inttable", "IntTable" },
            { "numberarray", "NumberArray" },
            { "numbertable", "NumberTable" },
            { "floatarray", "NumberArray" },
            { "floattable", "NumberTable" },
            { "stringarray", "StringArray" },
        };
        
        private readonly Dictionary<string, string> _parseFuncDict = new Dictionary<string, string>()
        {
            { "int", "parseInt" },
            { "number", "parseNumber" },
            { "float", "parseNumber" },
            { "bool", "parseBool" },
            { "string", "parseString" },
            { "intarray", "parseIntArray" },
            { "inttable", "parseIntTable" },
            { "numberarray", "parseNumberArray" },
            { "numbertable", "parseNumberTable" },
            { "floatarray", "parseNumberArray" },
            { "floattable", "parseNumberTable" },
            { "stringarray", "parseStringArray" },
        };


        public CSVReaderMaker()
        {
            //_classCodeTmp = File.ReadAllText("Assets/Editor/BattleEditor/CSVConfig/CSVReaderCodeTmp");

            using (StreamReader reader = new StreamReader("Assets/Editor/BattleEditor/CSVConfig/CSVReaderCodeTmp", Encoding.Default))
            {
                _classCodeTmp = reader.ReadToEnd();
            }
            
            
            if (Directory.Exists(_configPathRoot))
            {
                // 获取文件夹中所有 .csv 文件的路径
                _files = Directory.GetFiles(_configPathRoot, "*.csv");
            }
            else
            {
                Debug.LogError("指定的文件夹不存在: " + _configPathRoot);
            }
        }

        public void GenConfigReaders()
        {
            foreach (var filePath in _files)
            {
                var tableName = Path.GetFileName(filePath).Split(".")[0];
                var classCode = genClassCode(tableName, filePath);
                if (string.IsNullOrEmpty(classCode))
                {
                    return;
                }

                genCShapeFile(tableName, classCode);
            }
        }


        private string genClassCode(string tableName, string filePath)
        {
            var classCode = string.Copy(_classCodeTmp);
            var nameSpace = typeof(ITableHelper).Namespace;

            classCode = classCode.Replace(_replaceNameSpaceStr, nameSpace);
            classCode = classCode.Replace(_replaceTableNameStr, tableName);
            var rowName = tableName.Replace("Table", "Row");
            classCode = classCode.Replace(_replaceTableRowNameStr, rowName);

            try
            {
                string descLine = null;
                string propertyNames = null;
                string propertyTypes = null;
                //读取字段类型
                using (StreamReader reader = new StreamReader(filePath))
                {
                    descLine = reader.ReadLine();
                    propertyNames = reader.ReadLine();
                    propertyTypes = reader.ReadLine();
                }

                var propertyNameRows = propertyNames.Split(",");
                var propertyTypeRows = propertyTypes.Split(",");
                var descRows = descLine.Split(",");
                if (propertyNameRows.Length != propertyTypeRows.Length)
                {
                    throw new Exception("配置格式不对，字段名和字段类型长度不一致");
                }

                var proStr = makeProperty(descRows, propertyNameRows, propertyTypeRows);
                classCode = classCode.Replace(_replaceTableRowPropertyStr, proStr);
                var parseStr = makePropertyParse(propertyNameRows, propertyTypeRows);
                classCode = classCode.Replace(_replaceTableRowPropertyParseStr, parseStr);
            }
            catch (Exception e)
            {
                classCode = null;
                Debug.LogError(e);
            }


            return classCode;
        }

        private string makeProperty(string[] desc, string[] propertyNames, string[] propertyTypes)
        {
            string propertyStr = "";

            for (var index = 0; index < propertyNames.Length; index++)
            {
                if(index == 0) continue;
                var propertyItemStr = string.Copy(_propertyTmp);
                var propertyName = propertyNames[index];
                var propertyType = propertyTypes[index];
                var propertyDesc = desc[index];
                if (string.IsNullOrEmpty(propertyName))
                {
                    throw new Exception("变量名存在空字符串");
                }

                if (string.IsNullOrEmpty(propertyType))
                {
                    throw new Exception("变量类型存在空字符串");
                }

                if (!_parseTypeDict.TryGetValue(propertyType.ToLower(), out var rightType))
                {
                    throw new Exception($"变量类型 {propertyType} 不存在");
                }
                propertyItemStr = propertyItemStr.Replace("@PropertyType", rightType);
                propertyItemStr = propertyItemStr.Replace("@PropertyName", propertyName);
                propertyItemStr = propertyItemStr.Replace("@PropertyDesc",
                    string.IsNullOrEmpty(propertyDesc) ? "无描述" : propertyDesc);
                propertyStr += propertyItemStr;
            }

            return propertyStr;
        }

        private string makePropertyParse(string[] propertyNames, string[] propertyTypes)
        {
            string parseStr = "";

            for (int i = 0; i < propertyNames.Length; i++)
            {
                if(i == 0) continue;
                var propertyParseStr = string.Copy(_parseFuncTmp);
                //这次不用检测了
                var propertyName = propertyNames[i];
                var propertyType = propertyTypes[i];

                if (!_parseFuncDict.TryGetValue(propertyType.ToLower(), out var rightType))
                {
                    throw new Exception($"变量Parse函数 {propertyType} 不存在");
                }

                propertyParseStr = propertyParseStr.Replace("@ParseFunc", rightType);
                propertyParseStr = propertyParseStr.Replace("@PropertyName", propertyName);
                propertyParseStr = propertyParseStr.Replace("@index", i.ToString());
                parseStr += propertyParseStr;
            }

            return parseStr;
        }
        
        private void genCShapeFile(string tableName, string code)
        {
            string fileName = tableName + ".cs";
            string filePath = Path.Combine(_genCShapePath, fileName);

            // 检查目录是否存在，如果不存在则创建
            if (!Directory.Exists(_genCShapePath))
            {
                Directory.CreateDirectory(_genCShapePath);
            }
            
            using (StreamWriter writer = new StreamWriter(filePath, false, Encoding.UTF8))
            {
                writer.Write(code);
            }
        }
    }
}