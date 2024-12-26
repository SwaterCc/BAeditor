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
        public static void Open(object serializableObject)
        {
            var window = GetWindow<SerializableOdinWindow>();
            window.position = GUIHelper.GetEditorWindowRect().AlignCenter(450, 500);
            window.titleContent = new GUIContent("创建序列化参数");
            window.Data = serializableObject;
        }
        
        [OdinSerialize]
        [NonSerialized]
        public object Data;
        
       
        [Button("确 定")]
        public void Save()
        {
            Close();
        }
    }
}