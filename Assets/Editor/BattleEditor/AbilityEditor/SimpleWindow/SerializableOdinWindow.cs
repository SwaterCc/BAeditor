using System;
using Hono.Scripts.Battle;
using Sirenix.OdinInspector;
using Sirenix.OdinInspector.Editor;
using Sirenix.Serialization;
using Sirenix.Utilities;
using Sirenix.Utilities.Editor;
using UnityEngine;
using Object = System.Object;

namespace Editor.AbilityEditor.SimpleWindow
{
    public class SerializableOdinWindow : OdinEditorWindow
    {
        public static void Open(ref object serializableObject,Type type,string label = "创建序列化参数")
        {
            var window = GetWindow<SerializableOdinWindow>();
            window.position = GUIHelper.GetEditorWindowRect().AlignCenter(450, 500);
            window.titleContent = new GUIContent("创建序列化参数");
            window.init(ref serializableObject,type);
        }

        private Type _type;
        private void init(ref object serializableObject,Type type)
        {
            _type = type;
            if (!_type.IsSerializable)
            {
                Debug.LogError($"{_type} is not Serializable");
                return;
            }
            
            serializableObject ??= (_type.InstantiateDefault(true));
            Setting = serializableObject;
        }
        
        [OdinSerialize]
        [NonSerialized]
        [VerticalGroup("setting")]
        public object Setting;

        [VerticalGroup("clear")]
        [Button("重置")]
        public void clear()
        {
            Setting = (_type.InstantiateDefault(true));
        }

        [VerticalGroup("end")]
        [Button("保存")]
        public void Save()
        {
            Close();
        }
    }
}