using Hono.Scripts.Battle;
using Sirenix.OdinInspector;
using Sirenix.OdinInspector.Editor;
using UnityEditor;
using UnityEngine;

namespace Editor.AbilityEditor
{
    public abstract class AView
    {
        public bool HasError { get; protected set; }
        public abstract void Load(string path);
        public abstract void Draw();
        public abstract void Save();
    }

    public abstract class AView<T> : AView where T : ASerializableData
    {
        private T _serializableData;
        public T Data => _serializableData;

        public override void Load(string path)
        {
            _serializableData = AssetDatabase.LoadAssetAtPath<T>(path);
            if (_serializableData == null)
            {
                Debug.LogError(path + " Get File is Null");
                HasError = true;
            }
        }

        public override void Save()
        {
            if (HasError) return;
            EditorUtility.SetDirty(_serializableData);
            AssetDatabase.SaveAssets();
        }
    }

    public class AViewDrawer<T> : OdinValueDrawer<T> where T : AView
    {
        protected override void DrawPropertyLayout(GUIContent label)
        {
            if (!ValueEntry.SmartValue.HasError)
            {
                ValueEntry.SmartValue.Draw();
            }
            else
            {
                EditorGUILayout.LabelField("加载出错");
            }
        }
    }
}