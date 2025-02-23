using Sirenix.OdinInspector.Editor;
using UnityEditor;
using UnityEngine;

namespace Hono.Scripts.Battle.Core.Base
{
    // 定义专属特性
    public class PrefabPathAttribute : System.Attribute { }

// 自定义绘制器
    public class PrefabPathDrawer : OdinAttributeDrawer<PrefabPathAttribute, string>
    {
        protected override void DrawPropertyLayout(GUIContent label)
        {
            // 当前路径值
            string currentPath = this.ValueEntry.SmartValue;

            // 通过路径加载预制体
            GameObject prefab = !string.IsNullOrEmpty(currentPath) ? AssetDatabase.LoadAssetAtPath<GameObject>(currentPath) : null;

            // 创建对象字段
            EditorGUI.BeginChangeCheck();
            GameObject newPrefab = (GameObject)EditorGUILayout.ObjectField(
                label,
                prefab,
                typeof(GameObject),
                false); // 限制只能选择预制体

            if (EditorGUI.EndChangeCheck())
            {
                if (newPrefab != null)
                {
                    // 获取完整资源路径
                    string path = AssetDatabase.GetAssetPath(newPrefab);

                    // 验证是否为预制体
                    if (PrefabUtility.GetPrefabAssetType(newPrefab) == PrefabAssetType.NotAPrefab)
                    {
                        Debug.LogError("请选择有效的预制体");
                        return;
                    }

                    this.ValueEntry.SmartValue = path;
                }
                else
                {
                    this.ValueEntry.SmartValue = string.Empty;
                }
            }

            // 显示路径辅助信息
            if (!string.IsNullOrEmpty(currentPath))
            {
                EditorGUILayout.HelpBox($"当前路径: {currentPath}", MessageType.None);
            }
        }
    }
}