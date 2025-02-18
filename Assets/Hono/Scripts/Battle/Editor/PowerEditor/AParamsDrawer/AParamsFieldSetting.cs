using System;
using System.Collections;
using System.Collections.Generic;
using Hono.Scripts.Battle.AbilitySystem;
using Hono.Scripts.Battle.Base;
using Hono.Scripts.Battle.ObjectPool;
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
            { typeof(IList), new() { typeof(List<int>), typeof(List<float>), typeof(GList<bool>), typeof(GList<int>), typeof(GList<float>) } },
        };

        /// <summary>
        /// 实现的序列化窗口
        /// </summary>
        public static readonly Dictionary<Type, EditorWindow> SerializeWindow = new()
            { };

        public static string GetTypeName(Type type)
        {
            if (type.IsGenericType)
            {
                return GetGenericTypeName(type);
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
        
        public static string GetGenericTypeName(Type type)
        {
            if (type == null)
            {
                throw new ArgumentNullException(nameof(type));
            }

            // 如果类型是泛型
            if (type.IsGenericType)
            {
                // 获取泛型定义的名称（去掉后面的反引号和数字）
                string genericTypeName = type.Name.Split('`')[0];

                // 获取泛型参数
                Type[] genericArguments = type.GetGenericArguments();

                // 递归处理泛型参数
                string genericArgs = string.Join(", ", Array.ConvertAll(genericArguments, GetGenericTypeName));

                return $"{genericTypeName}<{genericArgs}>";
            }

            // 如果类型是数组
            if (type.IsArray)
            {
                return $"{GetGenericTypeName(type.GetElementType())}[{new string(',', type.GetArrayRank() - 1)}]";
            }

            // 返回类型的短名称
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

                if (type.IsEnum)
                {
                    return new RefInt();
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