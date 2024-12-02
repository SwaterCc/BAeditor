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
        public abstract void Init(OdinDrawer odinDrawer);
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

        public sealed override void Init(OdinDrawer odinDrawer)
        {
            onInit(odinDrawer);
        }

        protected virtual void onInit(OdinDrawer odinDrawer) { }

        public override void Save()
        {
            if (HasError) return;
            EditorUtility.SetDirty(_serializableData);
            AssetDatabase.SaveAssets();
        }
    }

    public class AViewDrawer<T> : OdinValueDrawer<T> where T : AView
    {
        private Vector2 _scrollViewPos = Vector2.zero;
        public bool EnableScrollView { get; set; }

        protected override void Initialize()
        {
            base.Initialize();
            ValueEntry.SmartValue.Init(this);
        }

        protected override void DrawPropertyLayout(GUIContent label)
        {
            if (EnableScrollView)
            {
                _scrollViewPos = GUILayout.BeginScrollView(_scrollViewPos, false, true);
            }

            if (!ValueEntry.SmartValue.HasError)
            {
                ValueEntry.SmartValue.Draw();
            }
            else
            {
                EditorGUILayout.LabelField("加载出错");
            }

            if (EnableScrollView)
            {
                GUILayout.EndScrollView();
            }
        }
    }
}