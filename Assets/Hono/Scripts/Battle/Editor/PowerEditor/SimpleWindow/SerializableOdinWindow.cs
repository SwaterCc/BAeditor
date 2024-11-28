using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using Hono.Scripts.Battle;
using Sirenix.OdinInspector;
using Sirenix.OdinInspector.Editor;
using Sirenix.Serialization;
using Sirenix.Utilities;
using Sirenix.Utilities.Editor;
using UnityEngine;
using Object = System.Object;

namespace Editor.AbilityEditor
{
    public class SerializableOdinWindow : OdinEditorWindow
    {
        public static void Open(object serializableObject, Type type, Action<object> onSave)
        {
            var window = GetWindow<SerializableOdinWindow>();
            window.position = GUIHelper.GetEditorWindowRect().AlignCenter(450, 500);
            window.titleContent = new GUIContent("创建序列化参数");
            window.init(serializableObject, type, onSave);
        }

        private Action<object> _onSave;
        private Type _type;
        private void init(object serializableObject,Type type, Action<object> onSave)
        {
            _type = type;
            
            if (serializableObject == null)
            {
                Debug.LogError($"{_type} is null");
                return;
            }
            
            if (!_type.IsSerializable)
            {
                Debug.LogError($"{_type} is not Serializable");
                return;
            }

            _onSave = onSave;
            Setting = serializableObject;
        }

        private object DeepCopy(object serializableObject)
        {
            using (var ms = new MemoryStream())
            {
                var formatter = new BinaryFormatter();
                formatter.Serialize(ms, serializableObject);
                ms.Position = 0;
                var copy = formatter.Deserialize(ms);
                
                return copy;
            }
        }
        
        [OdinSerialize]
        [NonSerialized]
        [VerticalGroup("setting")]
        public object Setting;
        

        [VerticalGroup("end")]
        [Button("保存")]
        public void Save()
        {
            _onSave.Invoke(Setting);
            Close();
        }
    }
}