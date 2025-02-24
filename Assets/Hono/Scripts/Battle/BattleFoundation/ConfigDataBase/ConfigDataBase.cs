#region

using System;
using System.Collections.Generic;
using System.IO;
using Cysharp.Threading.Tasks;
using Hono.Scripts.Battle.Core.Base;
using Hono.Scripts.Battle.Tools;
using Newtonsoft.Json.Linq;
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
        public static T Table<T>() where T : ConfigTable
        {
            return Instance.GetTable<T>();
        }

        private readonly Dictionary<Type, ConfigTable> _tables = new(32);
        
        public override async UniTask AsyncLoad()
        {
            _tables.Clear();
            var json = await Addressables.LoadAssetAsync<TextAsset>("Assets/BattleData/Json/BattleConfigs.json");
            var tableObjects = JObject.Parse(json.text);
            foreach (var tableJObject in tableObjects.Properties())
            {
                jsonItemParse(tableJObject);
            }
               
            Debug.Log("ConfigManager Init Finish！");
        }


        private void jsonItemParse(JProperty jProperty)
        {
            var className = $"{typeof(ConfigTable).Namespace}.{jProperty.Name}";
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
            var table = (ConfigTable)Activator.CreateInstance(classType);
            table.ParseJsonObject(jProperty.Value as JObject);
            _tables.Add(classType, table);
        }
        
        public async void ReloadAll()
        {
#if UNITY_EDITOR
            await AsyncLoad();
            Debug.Log("ReloadTable Finish");
#endif
        }

        private T GetTable<T>() where T : ConfigTable
        {
            if (_tables.TryGetValue(typeof(T), out var table))
            {
                return (T)table;
            }

            return null;
        }

        public bool TryGet<T>(int id, out T table) where T : ConfigTable
        {
            table = null;
            if (_tables.TryGetValue(typeof(T), out var iTable))
            {
                table = (T)iTable;
                return true;
            }

            return false;
        }
    }
}