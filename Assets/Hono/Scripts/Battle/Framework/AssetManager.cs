using System;
using System.Collections.Generic;
using Hono.Scripts.Battle.Tools;
using UnityEngine;

namespace Hono.Scripts.Battle
{
    public class DataHelper
    {
        private Dictionary<int, ScriptableObject> _datas = new();

        public void AddData(int id, ScriptableObject data)
        {
            _datas.TryAdd(id, data);
        }

        public T Get<T>(int id) where T : ScriptableObject
        {
            return (T)_datas[id];
        }

        public bool TryGetData<T>(int id, out T data) where T : ScriptableObject
        {
            data = null;
            if (_datas.TryGetValue(id, out var scriptableObject) && scriptableObject is T parseData)
            {
                data = parseData;
                return true;
            }
            return false;
        }
    }


    /// <summary>
    /// Demo2使用的数据类，用于加载Asset
    /// </summary>
    public class AssetManager : Singleton<AssetManager>
    {
        private Dictionary<Type, DataHelper> _assetDict = new Dictionary<Type, DataHelper>();

        public void Init()
        {
            //加载路径下的所有Asset并存储
        }

        public T GetData<T>(int id) where T : ScriptableObject
        {
            if (_assetDict.TryGetValue(typeof(T), out var dataHelper))
            {
                if (dataHelper.TryGetData<T>(id, out var row))
                {
                    return row;
                }
            }
            return null;
        }
    }
}