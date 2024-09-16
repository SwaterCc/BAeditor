using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

namespace Hono.Scripts.Battle
{
    /// <summary>
    /// 编辑器数据
    /// </summary>
    [Serializable]
    public class Parameter
    {
        /// <summary>
        /// 是否是函数
        /// </summary>
        public bool IsFunc;

        public string FuncName;
        public List<Parameter> FuncParam;

        /// <summary>
        /// 是否是基础值
        /// </summary>
        public bool IsBaseValue;
        public object Value;

        /// <summary>
        /// 是否是自定义变量
        /// </summary>
        public bool IsVairable;

        public string VairableName;

        /// <summary>
        /// 是否是属性
        /// </summary>
        public bool IsAttr;

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

            IsFunc = parameter.IsFunc;
            FuncName = parameter.FuncName;

            // 深拷贝 FuncParam 列表
            if (parameter.FuncParam != null)
            {
                FuncParam = new List<Parameter>();
                foreach (var param in parameter.FuncParam)
                {
                    FuncParam.Add(new Parameter(param));
                }
            }

            IsBaseValue = parameter.IsBaseValue;
            Value = DeepCopy(parameter.Value);

            IsVairable = parameter.IsVairable;
            VairableName = parameter.VairableName;
            IsAttr = parameter.IsAttr;
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
}