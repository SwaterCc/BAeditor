using System.Collections.Generic;
using Hono.Scripts.Battle;
using Hono.Scripts.Battle.Tools;
using UnityEngine;
using UnityEditor;
using UnityEditor.AddressableAssets;
using UnityEditor.AddressableAssets.Settings;
using UnityEngine.AddressableAssets;


namespace Editor
{
    public class ResourceLabelCounter : EditorWindow
    {
        [MenuItem("Tools/统计资源label")]
        public static void CounterLabel()
        {
            var labelCountDict = AssetDatabase.LoadAssetAtPath<AssetLabelCounter>("Assets/BattleData/LabelCount.asset");
            labelCountDict.LabelCounts.Clear();
            
            // 查找指定路径下的所有资产GUID
            string[] assetGuids = AssetDatabase.FindAssets("", new[] { AbilityEditorPath.RootPath });

            // 遍历所有资产
            foreach (string guid in assetGuids)
            {
                var settings = AddressableAssetSettingsDefaultObject.Settings;
                if (settings == null) continue;

                var entry = settings.FindAssetEntry(guid);
                if (entry == null || entry.labels.Count == 0) continue;

                // 获取资产的标签并统计数量
                foreach (string label in entry.labels)
                {
                    if (!labelCountDict.LabelCounts.TryAdd(label, 1))
                    {
                        labelCountDict.LabelCounts[label]++;
                    }
                }
            }
        }
    }
}