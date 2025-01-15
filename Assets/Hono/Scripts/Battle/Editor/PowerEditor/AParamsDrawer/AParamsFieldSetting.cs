using System;
using System.Collections;
using System.Collections.Generic;
using Hono.Scripts.Battle.AbilitySystem;
using Hono.Scripts.Battle.Base;
using UnityEditor;
using UnityEngine;

namespace Editor.AbilityEditor
{
    public static class AParamsFieldSetting
    {
        /// <summary>
        /// 允许转换类型字典
        /// </summary>
        public static readonly Dictionary<Type, List<Type>> AllowCastConfig = new()
        {
            { typeof(RefFloat), new() { typeof(RefInt) } },
            { typeof(RefInt), new() { typeof(RefFloat) } },
            { typeof(object), new() { typeof(RefInt), typeof(RefFloat), typeof(RefBoolean), typeof(RefVector3) } },
            { typeof(IList), new() { typeof(List<int>), typeof(List<float>), typeof(List<bool>) } },
        };

        /// <summary>
        /// 实现的序列化窗口
        /// </summary>
        public static readonly Dictionary<Type, EditorWindow> SerializeWindow = new()
            { };

        public static string GetTypeName(Type type)
        {
            if (type == typeof(List<int>))
            {
                return "IntList";
            }
            if (type == typeof(List<float>))
            {
                return "FloatList";
            }
            if (type == typeof(List<bool>))
            {
                return "BoolList";
            }
            if (type == typeof(RefInt))
            {
                return "Int(Ref)";
            }
            if (type == typeof(RefFloat))
            {
                return "Float(Ref)";
            }
            if (type == typeof(RefBoolean))
            {
                return "Bool(Ref)";
            }
            if (type == typeof(RefVector3))
            {
                return "Vec3(Ref)";
            }
            return type.Name;
        }
        
        public static object GetDefaultValue(Type type)
        {
            object value;
            try
            {
                if (type == typeof(object))
                {
                    return null;
                }

                if (type.IsAbstract || type.IsInterface)
                {
                    return null;
                }

                if (type == typeof(string))
                {
                    value = "";
                }
                else
                {
                    value = Activator.CreateInstance(type);
                }
            }
            catch (Exception)
            {
                value = null;
                Debug.LogError($"{type} 该类型没有默认构造函数，无法创建默认对象");
            }

            return value;
        }
    }
}