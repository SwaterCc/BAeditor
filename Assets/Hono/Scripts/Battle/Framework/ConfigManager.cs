#region

using System;
using System.Collections.Generic;
using System.IO;
using Cysharp.Threading.Tasks;
using Hono.Scripts.Battle.Tools;
using UnityEngine;
using UnityEngine.AddressableAssets;

#endregion

namespace Hono.Scripts.Battle
{
    /// <summary>
    ///     配置管理
    /// </summary>
    public class ConfigManager : Singleton<ConfigManager>, IBattleFrameworkAsyncInit
    {
        public static T Table<T>() where T : class, ITableHelper
        {
            return Instance.GetTable<T>();
        }

        private Dictionary<Type, ITableHelper> _tables = new(32);

        public bool IsLoadFinish { get; private set; }

        public async UniTask AsyncInit()
        {
#if UNITY_EDITOR
            await EditorLoad();
#else
			await ReleaseLoad();
#endif
        }

        private async UniTask EditorLoad()
        {
            string folderPath = BattleConstValue.CSVRoot;

            if (Directory.Exists(folderPath))
            {
                // 获取文件夹中所有 .csv 文件的路径
                string[] csvFiles = Directory.GetFiles(folderPath, "*.csv");

                List<UniTask> tasks = new List<UniTask>();
                try
                {
                    foreach (string fullPath in csvFiles)
                    {
                        string fileName = Path.GetFileName(fullPath);
                        string assetPath = folderPath + "/" + fileName;
                        string className = fileName.Split(".")[0];
                        className = $"{typeof(ITableHelper).Namespace}.{className}";
                        tasks.Add(loadConfigAsset(className, assetPath));
                    }

                    await UniTask.WhenAll(tasks);
                }
                catch (Exception e)
                {
                    Debug.LogError(e);
                }

                IsLoadFinish = true;
            }
            else
            {
                throw new Exception("指定的文件夹不存在: " + folderPath);
            }
        }

        private async UniTask ReleaseLoad()
        {
            if (!BattleManager.Paths.paths.TryGetValue(EPathType.CSV, out var paths))
            {
                Debug.LogError("获取CSV路径失败");
                return;
            }

            List<UniTask> tasks = new();
            foreach (var path in paths)
            {
                string fileNameWithExtension = Path.GetFileName(path);
                string className = Path.GetFileNameWithoutExtension(fileNameWithExtension);
                className = $"{typeof(ITableHelper).Namespace}.{className}";
                tasks.Add(loadConfigAsset(className, path));
            }

            try
            {
                await UniTask.WhenAll(tasks);
                Debug.Log("ConfigManager Init Finish！");
            }
            catch (Exception e)
            {
                Debug.LogError(e);
            }

            IsLoadFinish = true;
        }

        public async void ReloadAll()
        {
#if UNITY_EDITOR
            IsLoadFinish = false;
            _tables.Clear();
            await AsyncInit();
            Debug.Log("ReloadTable Finish");
#endif
        }

        private async UniTask loadConfigAsset(string className, string assetPath)
        {
            var sRow = await Addressables.LoadAssetAsync<TextAsset>(assetPath)
                .ToUniTask();

            if (string.IsNullOrEmpty(sRow.text))
            {
                Debug.Log($"Asset key empty");
                return;
            }

            var classType = Type.GetType(className);
            if (classType == null)
            {
                Debug.LogError($"{className} 转换type失败！");
                return;
            }

            if (_tables.ContainsKey(classType))
            {
                Debug.LogError($"表名冲突 {className}");
                return;
            }

            var table = (ITableHelper)Activator.CreateInstance(classType);
            _tables.Add(classType, table);
            table.LoadCSV(sRow.text);
            Addressables.Release(sRow);
        }

        public T GetTable<T>() where T : class, ITableHelper
        {
            if (_tables.TryGetValue(typeof(T), out var table))
            {
                return (T)table;
            }

            return null;
        }

        public bool TryGet<T>(int id, out T table) where T : class, ITableHelper
        {
            table = null;
            if (_tables.TryGetValue(typeof(T), out var iTable))
            {
                table = (T)iTable;
                return true;
            }

            return false;
        }


        public TTableRowName GetData<TTableName, TTableRowName>(int id) where TTableName : class, ITableHelper
            where TTableRowName : TableRow
        {
            if (_tables.TryGetValue(typeof(TTableName), out var table))
            {
                return (TTableRowName)(table.GetTableRow(id));
            }

            return null;
        }
    }
}