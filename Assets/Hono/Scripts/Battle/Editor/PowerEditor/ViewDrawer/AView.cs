using Hono.Scripts.Battle;
using Hono.Scripts.Battle.Core.AbilityFramework;
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
        public abstract void Init();
        public abstract void Draw();
        public abstract void Save();
    }

    public abstract class AView<T> : AView where T : ASerializableData
    {
        public T Data { get; set; }

        public override void Load(string path)
        {
            Data = AssetDatabase.LoadAssetAtPath<T>(path);
            if (Data == null)
            {
                Debug.LogError(path + " Get File is Null");
                HasError = true;
            }
        }

        public sealed override void Init()
        {
            onInit();
        }

        protected virtual void onInit() { }

        protected virtual void onSave(){}
        
        public override void Save()
        {
            if (HasError) return;
            onSave();
            EditorUtility.SetDirty(Data);
            AssetDatabase.SaveAssets();
            if (Application.isPlaying) {
	            AbilityReloadManager.Instance.CallReloadHandles(Data.id);
            }
        }
    }

    public class AViewDrawer<T> : OdinValueDrawer<T> where T : AView
    {
        private Vector2 _scrollViewPos = Vector2.zero;
        public bool EnableScrollView { get; set; }

        protected override void Initialize()
        {
            base.Initialize();
            ValueEntry.SmartValue.Init();
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