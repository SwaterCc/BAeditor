using System.Linq;

namespace Hono.Scripts.Battle.Tools.Editor
{
    using UnityEngine;
    using UnityEditor;
    using UnityEditor.SceneManagement;

    public class SceneSwitcherWindow : EditorWindow
    {
        private Vector2 scrollPos;
        private string[] scenePaths;

        // 添加菜单项以打开窗口
        [MenuItem("Tools/Scene Switcher")]
        public static void ShowWindow()
        {
            GetWindow<SceneSwitcherWindow>("Scene Switcher");
        }

        private void OnEnable()
        {
            // 获取所有场景路径
            scenePaths = AssetDatabase.FindAssets("t:Scene")
                .Select(AssetDatabase.GUIDToAssetPath)
                .ToArray();
        }

        private void OnGUI()
        {
            GUILayout.Label("Scenes", EditorStyles.boldLabel);
            scrollPos = GUILayout.BeginScrollView(scrollPos);

            foreach (var scenePath in scenePaths)
            {
                if (GUILayout.Button(scenePath))
                {
                    OpenScene(scenePath);
                }
            }

            GUILayout.EndScrollView();
        }

        private void OpenScene(string scenePath)
        {
            // 打开场景
            if (EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo())
            {
                EditorSceneManager.OpenScene(scenePath);
            }
        }
    }
}