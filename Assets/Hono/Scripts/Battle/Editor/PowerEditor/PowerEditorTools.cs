using System;
using System.Collections.Generic;
using System.IO;
using Editor.AbilityEditor.SimpleWindow;
using Hono.Scripts.Battle;
using Sirenix.OdinInspector;
using Sirenix.Utilities.Editor;
using UnityEditor;
using UnityEngine;

namespace Editor.AbilityEditor
{
    public static class PowerEditorTools
    {
        /// <summary>
        /// 获取指定路径下的所有Asset文件
        /// </summary>
        /// <param name="folderPath"></param>
        /// <returns></returns>
        public static string[] LoadFolder(string folderPath)
        {
            // 获取指定文件夹路径下所有 .asset 文件的 GUID 数组
            string[] assetGUIDs = AssetDatabase.FindAssets("t:object", new[] { folderPath });

            // 创建一个字符串数组来存储文件名
            string[] assetFileNames = new string[assetGUIDs.Length];

            for (int i = 0; i < assetGUIDs.Length; i++)
            {
                // 使用 GUID 获取每个资产的路径
                string assetPath = AssetDatabase.GUIDToAssetPath(assetGUIDs[i]);

                // 检查文件是否为 .asset 文件
                if (assetPath.EndsWith(".asset"))
                {
                    // 获取文件名（带扩展名）
                    assetFileNames[i] = assetPath;
                }
            }

            return assetFileNames;
        }
        
        
        /// <summary>
        /// 指定路径获取该路径下的文件及文件夹
        /// </summary>
        /// <param name="path"></param>
        /// <param name="folder"></param>
        /// <param name="files"></param>
        /// <param name="filePattern">文件类型模式匹配，默认asset文件</param>
        public static void GetPathAssetsAndFolders(string path, ref List<string> folder, ref List<string> files,
            string filePattern = "*.asset")
        {
            files.Clear();
            folder.Clear();

            string[] fileEntries = Directory.GetFiles(path, filePattern);
            files.AddRange(fileEntries);

            string[] subdirectoryEntries = Directory.GetDirectories(path);
            folder.AddRange(subdirectoryEntries);
            foreach (string subdirectory in subdirectoryEntries)
            {
                files.Remove(subdirectory);
            }
        }
    }
}