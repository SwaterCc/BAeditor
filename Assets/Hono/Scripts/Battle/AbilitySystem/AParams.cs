#region

using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using Hono.Scripts.Battle.Base;
using Hono.Scripts.Battle.Core;
using UnityEditor;

#endregion

namespace Hono.Scripts.Battle.AbilitySystem
{
    /// <summary>
    /// 编辑器数据
    /// </summary>
    [Serializable]
    public class AParams
    {
        /// <summary>
        /// 参数类型
        /// </summary>
        public EParamType paramType;

        /// <summary>
        /// 函数名
        /// </summary>
        public string funcName;

        /// <summary>
        /// 函数参数
        /// </summary>
        public List<AParams> funcParams;

        /// <summary>
        /// 变量名
        /// </summary>
        public string variableName;

        /// <summary>
        /// 属性类型
        /// </summary>
        public EAttrType attrType;

        /// <summary>
        /// 基础类型需要包装
        /// </summary>
        public object Value;

        /// <summary>
        /// 参数最终转换类型字符串
        /// </summary>
        public string paramCastType;

        public AParams() { }

        /// <summary>
        /// 拷贝构造函数
        /// </summary>
        /// <param name="aParams"></param>
        public AParams(in AParams aParams)
        {
            if (aParams == null)
                throw new ArgumentNullException(nameof(aParams));

            paramType = aParams.paramType;
            funcName = aParams.funcName;

            // 深拷贝 FuncParam 列表
            if (aParams.funcParams != null)
            {
                funcParams = new List<AParams>();
                foreach (var param in aParams.funcParams)
                {
                    funcParams.Add(new AParams(param));
                }
            }

            if (aParams.Value != null)
            {
                using (MemoryStream memoryStream = new MemoryStream())
                {
                    BinaryFormatter formatter = new();
                    formatter.Serialize(memoryStream, aParams.Value);
                    memoryStream.Seek(0, SeekOrigin.Begin);
                    Value = formatter.Deserialize(memoryStream);
                    ;
                }
            }

            paramCastType = aParams.paramCastType;
            variableName = aParams.variableName;
            attrType = aParams.attrType;
        }

        /// <summary>
        /// 复制
        /// </summary>
        public void CopyTo(AParams copy)
        {
            var temp = new AParams(copy);

            paramType = temp.paramType;
            funcName = temp.funcName;
            funcParams = temp.funcParams;
            Value = temp.Value;
            variableName = temp.variableName;
            attrType = temp.attrType;
            paramCastType = temp.paramCastType;
        }

        public override string ToString()
        {
            string desc = "";

            switch (paramType)
            {
                case EParamType.Simple:
                    if (Value == null)
                    {
                        desc = "null";
                        break;
                    }

                    if (Value.GetType().IsValueType || 
                        Value.GetType().BaseType == typeof(ARef) ||
                        Value is string)
                    {
                        desc = Value.ToString();
                        break;
                    }
                    
                    if (Value.GetType().IsClass)
                    {
                        if (Value is IList list)
                        {
                            for (var index = 0; index < list.Count; index++)
                            {
                                var obj = list[index];
                                desc += obj;
                                if (index != list.Count - 1)
                                {
                                    desc +=",";
                                }
                            }
                        }
                        else
                        {
                            desc = Value.GetType().Name;
                        }
                    }
                    break;
                case EParamType.Function:
                    if (string.IsNullOrEmpty(funcName))
                    {
                        return "函数未初始化";
                    }

                    desc = "" + funcName + " ( ";
                    for (var index = 0; index < funcParams.Count; index++)
                    {
                        var parameter = funcParams[index];
                        desc += parameter;
                        if (index != funcParams.Count - 1)
                        {
                            desc += ", ";
                        }
                    }

                    desc += " ) ";
                    break;
                case EParamType.Variable:
                    desc = string.IsNullOrEmpty(variableName) ? "未设置变量名" : "变量:" + variableName;
                    break;
                case EParamType.Attr:
                    desc = "属性:" + attrType;
                    break;
            }

            return desc;
        }

        #region Type缓存

        private static readonly Dictionary<string, Type> TypeCache = new();

        public Type GetParamType()
        {
            if (TypeCache.TryGetValue(paramCastType, out Type cachedType))
            {
                return cachedType;
            }

            Type type = Type.GetType(paramCastType);
            if (type != null)
            {
                TypeCache[paramCastType] = type;
            }

            return type;
        }

        #endregion
    }
}