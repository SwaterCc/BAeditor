using System;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Hono.Scripts.Battle
{
    public class DebugMode : MonoBehaviour
    {
        private static DebugMode _instance;
        public static DebugMode Instance
        {
            get
            {
                if (_instance == null)
                {
                    // 尝试查找现有实例
                    _instance = FindObjectOfType<DebugMode>();

                    // 如果没有找到，则创建新的 GameObject 并添加该组件
                    if (_instance == null)
                    {
                        GameObject singletonObject = new GameObject(nameof(DebugMode));
                        _instance = singletonObject.AddComponent<DebugMode>();
                    }
                }

                return _instance;
            }
        }
        
        private bool _isAutoReload = false;
        public bool AutoReloadAsset => _isAutoReload;
        
        protected void Awake()
        {
            DontDestroyOnLoad(this.gameObject);

            // 检查是否已经存在另一个实例
            if (_instance != null && _instance != this)
            {
                Destroy(this.gameObject);
            }
            else
            {
                _instance = this;
            }
        }

        public void Init()
        {
            
        }

        private void OnGUI()
        {
            // 设置 GUI 元素的宽度和高度
            float elementWidth = 100f;
            float elementHeight = 20f;

            // 开始水平布局
            GUILayout.BeginHorizontal();

            // 绘制 Toggle 控件
            _isAutoReload = GUILayout.Toggle(_isAutoReload, "启用Asset自动加载", GUILayout.Width(elementWidth), GUILayout.Height(elementHeight));

            // 绘制 Button 控件
            if (GUILayout.Button("Reload All Table", GUILayout.Width(elementWidth), GUILayout.Height(elementHeight)))
            {
                ConfigManager.Instance.ReloadAll();
            }
            // 结束水平布局
            GUILayout.EndHorizontal();
        }
    }
}