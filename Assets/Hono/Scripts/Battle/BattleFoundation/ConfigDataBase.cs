#region

using System;
using System.Collections.Generic;
using System.IO;
using Cysharp.Threading.Tasks;
using Hono.Scripts.Battle.Core.Base;
using Hono.Scripts.Battle.Tools;
using UnityEngine;
using UnityEngine.AddressableAssets;

#endregion

namespace Hono.Scripts.Battle
{
    /// <summary>
    ///     配置管理
    /// </summary>
    public class ConfigDataBase : BattleFoundation<ConfigDataBase>
    {
        public static T Table<T>() where T : class, ITableHelper
        {
            return Instance.GetTable<T>();
        }

        private readonly Dictionary<Type, ITableHelper> _tables = new(32);
        
        public override async UniTask AsyncLoad()
        {
            if (!BattleManager.Paths.paths.TryGetValue(EPathType.CSV, out var paths))
            {
                Debug.LogError("获取CSV路径失败");
                return;
            }
            
            var csvList = await Addressables.LoadAssetsAsync<TextAsset>("aCSV");
            foreach (var textAsset in csvList)
            {
                addCSVTable(textAsset);
            }
               
            Debug.Log("ConfigManager Init Finish！");
        }
        
        private void addCSVTable(TextAsset textAsset)
        {
            if (string.IsNullOrEmpty(textAsset.text))
            {
                Debug.Log($"Asset key empty");
                return;
            }

            var classType = Type.GetType(textAsset.name);
            if (classType == null)
            {
                Debug.LogError($"{textAsset.name} 转换type失败！");
                return;
            }

            if (_tables.ContainsKey(classType))
            {
                Debug.LogError($"表名冲突 {textAsset.name}");
                return;
            }

            var table = (ITableHelper)Activator.CreateInstance(classType);
            _tables.Add(classType, table);
            table.LoadCSV(textAsset.text);
        }
        
        public async void ReloadAll()
        {
#if UNITY_EDITOR
            _tables.Clear();
            await AsyncLoad();
            Debug.Log("ReloadTable Finish");
#endif
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