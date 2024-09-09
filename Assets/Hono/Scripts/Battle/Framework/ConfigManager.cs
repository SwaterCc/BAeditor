using Hono.Scripts.Battle.Tools;
using System;
using System.Collections.Generic;
using System.IO;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Hono.Scripts.Battle
{
    /// <summary>
    /// 配置管理
    /// </summary>
    public class ConfigManager : Singleton<ConfigManager>
    {
        private Dictionary<Type, ITableHelper> _tables = new Dictionary<Type, ITableHelper>();

        public bool IsLoadFinish { get; private set; }

        public void Init()
        {
            IsLoadFinish = false;
            loadConfigs();
        }


        public void Reload(string className)
        {
#if UNITY_EDITOR
#endif
        }

        public void ReloadAll()
        {
#if UNITY_EDITOR
            IsLoadFinish = false;
            _tables.Clear();
            loadConfigs();
#endif
        }

        private async void loadConfigs()
        {
            string folderPath = DataPath.CSVRoot;

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
                Debug.LogError("指定的文件夹不存在: " + folderPath);
            }
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

        public T GetTable<T>(int id) where T : class, ITableHelper
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