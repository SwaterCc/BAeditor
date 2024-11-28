using Hono.Scripts.Battle;
using Sirenix.OdinInspector;
using Sirenix.OdinInspector.Editor;
using UnityEditor;
using UnityEngine;

namespace Editor.AbilityEditor
{
    public abstract class AView
    {
        public abstract void Load(string path);
        public abstract void Draw();
        public abstract void Save();
    }

    public abstract class AView<T> : AView where T : ASerializableData
    {
        private T _serializableData;
        public T Data => _serializableData;
        
        public sealed override void Load(string path)
        {
            _serializableData = PowerEditorTools.GetSerializeAsset<T>(path);
        }

        public override void Save()
        {
            EditorUtility.SetDirty(_serializableData);
            AssetDatabase.SaveAssets();
        }
    }

    public class AViewDrawer<T> : OdinValueDrawer<T> where T : AView
    {
        protected T View;

        protected override void Initialize()
        {
            base.Initialize();
            View = ValueEntry.SmartValue;
        }

        protected override void DrawPropertyLayout(GUIContent label)
        {
            View.Draw();
        }
    }
}