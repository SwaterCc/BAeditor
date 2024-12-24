using System;
using System.Collections.Generic;
using Hono.Scripts.Battle.Base;
using UnityEditor;

namespace Editor.AbilityEditor
{
    public static class AParamsFieldConfig{
        /// <summary>
        /// 允许转换类型字典
        /// </summary>
        public static readonly Dictionary<Type, List<Type>> AllowCastConfig = new()
        {
            { typeof(RefFloat),new(){typeof(RefInt)} },
        };

        /// <summary>
        /// 实现的序列化窗口
        /// </summary>
        public static readonly Dictionary<Type, EditorWindow> SerializeWindow = new()
            { };
    }

}