using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

namespace Hono.Scripts.Battle
{
    public enum EParameterType
    {
        Simple,
        Function,
        Variable,
        Attr,
    }

    /// <summary>
    /// 编辑器数据
    /// </summary>
    [Serializable]
    public class Parameter
    {
        /// <summary>
        /// 是否是函数
        /// </summary>
        public EParameterType ParameterType;

        public string FuncName;
        public List<Parameter> FuncParams;
        
        public object Value;
        
        public string VairableName;
        
        public EAbilityType AttrType;

        public Parameter() { }

        /// <summary>
        /// 拷贝构造函数
        /// </summary>
        /// <param name="parameter"></param>
        public Parameter(Parameter parameter)
        {
            if (parameter == null)
                throw new ArgumentNullException(nameof(parameter));

            ParameterType = parameter.ParameterType;
            FuncName = parameter.FuncName;

            // 深拷贝 FuncParam 列表
            if (parameter.FuncParams != null)
            {
                FuncParams = new List<Parameter>();
                foreach (var param in parameter.FuncParams)
                {
                    FuncParams.Add(new Parameter(param));
                }
            }
            Value = DeepCopy(parameter.Value);
            VairableName = parameter.VairableName;
            AttrType = parameter.AttrType;
        }

        /// <summary>
        /// 通过序列化实现深拷贝
        /// </summary>
        /// <param name="obj">要深拷贝的对象</param>
        /// <returns>深拷贝得到的新对象</returns>
        private object DeepCopy(object obj)
        {
            if (obj == null)
                return null;

            if (!IsSerializable(obj))
            {
                Debug.LogWarning("Parameter 尝试拷贝一个非可序列化对象失败，返回null");
                return null;
            }

            using (MemoryStream ms = new MemoryStream())
            {
                IFormatter formatter = new BinaryFormatter();
                formatter.Serialize(ms, obj);
                ms.Seek(0, SeekOrigin.Begin);
                return formatter.Deserialize(ms);
            }
        }

        /// <summary>
        /// 检查对象是否可序列化
        /// </summary>
        /// <param name="obj">要检查的对象</param>
        /// <returns>如果对象可序列化，则为 true；否则为 false。</returns>
        private static bool IsSerializable(object obj)
        {
            if (obj == null) return false;
            var type = obj.GetType();
            return type.IsSerializable || typeof(ISerializable).IsAssignableFrom(type);
        }
    }

    public static class ParameterHelper
    {
        public static void InitFunc(this Parameter parameter, string funcName = null) { }
    }
}