﻿using System;
using System.IO;
using System.Text;
using UnityEditor;
using UnityEngine;

namespace Hono.Scripts.Battle
{
    public static class AttrMaker
    {
        [MenuItem("Tools/生成AttrCreator")]
        public static void Build()
        {
            var attrFileProcess = new AttrFileProcess();
            attrFileProcess.Process();
        }

        public class AttrFileProcess
        {
            private string _classTempStr;

            private string _intAttrTemp = @"
                case ELogicAttr.@AttrName :
                     return new Attr<RefInt>((a, b) => a + b);";

            private string _floatAttrTemp = @"
                case ELogicAttr.@AttrName :
                     return new Attr<RefFloat>((a, b) => a + b);";

            private string _boolAttrTemp = @"
                case ELogicAttr.@AttrName :
                     return new Attr<RefBool>(null);";

            private string _v3AttrTemp = @"
                case ELogicAttr.@AttrName :
                     return new Attr<RefVector3>((a, b) => a + b);";

            private string _q4AttrTemp = @"
                case ELogicAttr.@AttrName :
                     return new Attr<RefQuaternion>((a, b) => b * a);";

            private string _intArrayAttrTemp = @"
                case ELogicAttr.@AttrName :
                     return new Attr<List<int>>(null);";

            private string _floatArrayAttrTemp = @"
                case ELogicAttr.@AttrName :
                     return new Attr<List<float>>(null);";

            private string _flagAttrEnum = "@EnumAttrDefine";
            private string _flagCase = "@Case";

            string _attrEnumStr = "";
            string _attrCase = "";
            private string _genCShapePath = "Assets/Hono/Scripts/Battle/Attr";

            public AttrFileProcess()
            {
                using (StreamReader reader =
                       new StreamReader($"{BattleEditorPath.EditorRootPath}/AttrMaker/AttrCreatorTemplate", Encoding.Default))
                {
                    _classTempStr = reader.ReadToEnd();
                }
            }

            public void Process()
            {
                
                using (StreamReader reader = new StreamReader($"{BattleEditorPath.EditorRootPath}/AttrMaker/AttrMakerDefine",
                           Encoding.Default))
                {
                    string line;
                    while ((line = reader.ReadLine()) != null)
                    {
                        if (string.IsNullOrEmpty(line))
                        {
                            continue;
                        }

                        var attrInfo = line.Split(" ", StringSplitOptions.RemoveEmptyEntries);

                        if (attrInfo.Length < 3)
                        {
                            continue;
                        }

                        var define = "      "+attrInfo[1] + " = " + attrInfo[2] +",\n";
                        _attrEnumStr += define;

                        if (attrInfo[0].ToLower() == "int")
                        {
                            string caseStr = string.Copy(_intAttrTemp);
                            caseStr = caseStr.Replace("@AttrName", attrInfo[1]);
                            _attrCase += caseStr;
                        }

                        if (attrInfo[0].ToLower() == "float")
                        {
                            string caseStr = string.Copy(_floatAttrTemp);
                            caseStr = caseStr.Replace("@AttrName", attrInfo[1]);
                            _attrCase += caseStr;
                        }

                        if (attrInfo[0].ToLower() == "bool")
                        {
                            string caseStr = string.Copy(_boolAttrTemp);
                            caseStr = caseStr.Replace("@AttrName", attrInfo[1]);
                            _attrCase += caseStr;
                        }

                        if (attrInfo[0].ToLower() == "vector3")
                        {
                            string caseStr = string.Copy(_v3AttrTemp);
                            caseStr = caseStr.Replace("@AttrName", attrInfo[1]);
                            _attrCase += caseStr;
                        }

                        if (attrInfo[0].ToLower() == "quaternion")
                        {
                            string caseStr = string.Copy(_q4AttrTemp);
                            caseStr = caseStr.Replace("@AttrName", attrInfo[1]);
                            _attrCase += caseStr;
                        }

                        if (attrInfo[0].ToLower() == "intarray")
                        {
                            string caseStr = string.Copy(_intArrayAttrTemp);
                            caseStr = caseStr.Replace("@AttrName", attrInfo[1]);
                            _attrCase += caseStr;
                        }

                        if (attrInfo[0].ToLower() == "floatarray")
                        {
                            string caseStr = string.Copy(_floatArrayAttrTemp);
                            caseStr = caseStr.Replace("@AttrName", attrInfo[1]);
                            _attrCase += caseStr;
                        }
                    }
                }

                genFlie();
            }

            private void genFlie()
            {
                var classCode = string.Copy(_classTempStr);

                classCode = classCode.Replace(_flagAttrEnum, _attrEnumStr);
                classCode = classCode.Replace(_flagCase, _attrCase);


                string fileName = "AttrCreator.cs";
                string filePath = Path.Combine(_genCShapePath, fileName);

                // 检查目录是否存在，如果不存在则创建
                if (!Directory.Exists(_genCShapePath))
                {
                    Directory.CreateDirectory(_genCShapePath);
                }

                using (StreamWriter writer = new StreamWriter(filePath, false, Encoding.UTF8))
                {
                    writer.Write(classCode);
                }
                
                Debug.Log("AttrCreator 生成完成");
            }
        }
    }
}